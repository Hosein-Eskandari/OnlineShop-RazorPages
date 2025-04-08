using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Comment;
using _01_LampshadeQuery.Contracts.Product;
using CommentManagement.Infrastructure.EFCore;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query;

public class ProductQuery : IProductQuery
{
    private readonly CommentContext _commentContext;
    private readonly DiscountContext _discountContext;
    private readonly InventoryContext _inventoryContext;
    private readonly ShopContext _shopContext;

    public ProductQuery(ShopContext context, InventoryContext inventoryContext,
        DiscountContext discountContext, CommentContext commentContext)
    {
        _shopContext = context;
        _inventoryContext = inventoryContext;
        _discountContext = discountContext;
        _commentContext = commentContext;
    }


    public List<ProductQueryModel> Search(string value)
    {
        var inventory = _inventoryContext.Inventory.Select(x =>
            new { x.ProductId, x.UnitPrice }).ToList();

        var discounts = _discountContext.CostumerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.ProductId, x.DiscountRate, x.EndDate }).ToList();

        var query = _shopContext.Products
            .Include(x => x.Category)
            .Select(x => new ProductQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Category = x.Category.Name,
                CategorySlug = x.Category.Slug,
                ShortDescription = x.ShortDescription
            }).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(value))
            query = query.Where(x => x.Name.Contains(value) || x.ShortDescription.Contains(value) ||
                                     x.Category.Contains(value));

        var products = query.ToList().OrderByDescending(x => x.Id).ToList();

        if (products != null)
            foreach (var product in products)
            {
                var productInventory = inventory.FirstOrDefault(x =>
                    x.ProductId == product.Id);

                if (productInventory != null)
                {
                    var price = productInventory.UnitPrice;
                    product.Price = price.ToMoney();

                    var productDiscount = discounts.FirstOrDefault(x =>
                        x.ProductId == product.Id);

                    if (productDiscount != null)
                    {
                        var discountRate = productDiscount.DiscountRate;

                        var discountAmount = Math.Round(price * discountRate / 100);

                        product.DiscountRate = discountRate;

                        product.DiscountExpireDate = productDiscount.EndDate.ToDiscountFormat();

                        product.HasDiscount = discountRate > 0;

                        product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }
            }


        return products;
    }

    public List<ProductQueryModel> GetLatestArrivals()
    {
        var inventory = _inventoryContext.Inventory
            .Select(x => new { x.ProductId, x.UnitPrice }).ToList();

        var discounts = _discountContext.CostumerDiscounts
            .Select(x => new { x.ProductId, x.DiscountRate }).ToList();


        var products = _shopContext.Products
            .Include(x => x.Category)
            .Select(x => new ProductQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Category = x.Category.Name,
                CategorySlug = x.Category.Slug
            }).AsNoTracking().OrderByDescending(x => x.Id).Take(8).ToList();


        foreach (var product in products)
        {
            var productInventory = inventory.FirstOrDefault(x =>
                x.ProductId == product.Id);

            if (productInventory != null)
            {
                var price = productInventory.UnitPrice;
                product.Price = price.ToMoney();

                var discount = discounts.FirstOrDefault(x =>
                    x.ProductId == product.Id);

                if (discount != null)
                {
                    var discountRate = discount.DiscountRate;

                    product.HasDiscount = discountRate > 0;

                    var discountAmount = Math.Round(price * discountRate / 100);

                    product.DiscountRate = discountRate;

                    product.PriceWithDiscount = (price - discountAmount).ToMoney();
                }
            }
        }

        return products;
    }

    public ProductQueryModel GetDetails(string slug)
    {
        var inventory = _inventoryContext.Inventory
            .Select(x => new { x.ProductId, x.UnitPrice, x.InStock }).ToList();

        var a = DateTime.Now;

        var discounts = _discountContext.CostumerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.ProductId, x.DiscountRate, x.EndDate }).ToList();


        var product = _shopContext.Products
            .Include(x => x.Category)
            .Include(x => x.ProductPictures)
            .Select(x => new ProductQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                CategorySlug = x.Category.Slug,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Category = x.Category.Name,
                Code = x.Code,
                Description = x.Description,
                ShortDescription = x.ShortDescription,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                Pictures = MapProductPictures(x.ProductPictures)
            }).AsNoTracking().FirstOrDefault(x => x.Slug == slug);

        if (product == null) return new ProductQueryModel();

        var productInventory = inventory.FirstOrDefault(x =>
            x.ProductId == product.Id);

        if (productInventory != null)
        {
            var price = productInventory.UnitPrice;
            product.Price = price.ToMoney();
            product.DoublePrice = price;

            var discount = discounts.FirstOrDefault(x =>
                x.ProductId == product.Id);
            product.InStock = productInventory.InStock;

            if (discount != null)
            {
                var discountRate = discount.DiscountRate;

                product.HasDiscount = discount.EndDate > DateTime.Now;

                product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();

                var discountAmount = Math.Round(price * discountRate / 100);

                product.DiscountRate = discountRate;

                product.PriceWithDiscount = (price - discountAmount).ToMoney();
            }

            var comments = _commentContext.Comments
                .Where(x => !x.IsCanceled)
                .Where(x => x.IsConfirmed)
                .Where(x => x.Type == CommentType.Product)
                .Where(x => x.OwnerRecordId == product.Id)
                .Select(x => new CommentQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Message = x.Message,
                    Website = x.Website,
                    CreationDate = x.CreationDate.ToFarsiFull()
                })
                .OrderByDescending(x => x.Id).ToList();

            if (comments.Count > 0) product.Comments = comments;
        }

        return product;
    }

    public List<CartItem> CheckInventoryStatus(List<CartItem>? cartItems)
    {
        var inventory = _inventoryContext.Inventory.ToList();

        foreach (var cartItem in cartItems)
            if (inventory.Any(x => x.ProductId == cartItem.Id && x.InStock))
            {
                var itemInventory = inventory
                    .Find(x => x.ProductId == cartItem.Id);
                if (itemInventory != null) cartItem.IsInStock = itemInventory.CalculateCurrentCount() >= cartItem.Count;
            }

        return cartItems;
    }

    //private static List<CommentQueryModel> MapComments(ICollection<Comment> comments)
    //{
    //    return comments
    //        .Where(x => !x.IsCanceled)
    //        .Where(x => x.IsConfirmed)
    //        .Select(x => new CommentQueryModel
    //        {
    //            Id = x.Id,
    //            Name = x.Name,
    //            Message = x.Message
    //        }).OrderByDescending(x => x.Id).ToList();
    //}

    private static List<ProductPictureQueryModel> MapProductPictures(ICollection<ProductPicture> productPictures)
    {
        return productPictures.Select(x => new ProductPictureQueryModel
        {
            ProductId = x.ProductId,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            IsRemoved = x.IsRemoved
        }).Where(x => !x.IsRemoved).ToList();
    }

    private static List<ProductQueryModel> MapProducts(ICollection<Product> products)
    {
        return products.Select(product => new ProductQueryModel
        {
            Id = product.Id,
            Name = product.Name,
            Slug = product.Slug,
            Picture = product.Picture,
            PictureAlt = product.PictureAlt,
            PictureTitle = product.PictureTitle,
            Category = product.Category.Name
        }).ToList();
    }
}