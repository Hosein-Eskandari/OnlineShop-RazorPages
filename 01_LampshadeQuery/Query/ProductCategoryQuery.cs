using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Product;
using _01_LampshadeQuery.Contracts.ProductCategory;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query;

public class ProductCategoryQuery : IProductCategoryQuery
{
    private readonly DiscountContext _discountContext;
    private readonly InventoryContext _inventoryContext;
    private readonly ShopContext _shopContext;

    public ProductCategoryQuery(ShopContext context, InventoryContext inventoryContext,
        DiscountContext discountContext)
    {
        _shopContext = context;
        _inventoryContext = inventoryContext;
        _discountContext = discountContext;
    }

    public List<ProductCategoryQueryModel> GetProductCategories()
    {
        return _shopContext.ProductCategories.Select(x => new ProductCategoryQueryModel
        {
            Id = x.Id,
            Name = x.Name,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Slug = x.Slug
        }).AsNoTracking().ToList();
    }

    public List<ProductCategoryQueryModel> GetProductCategoriesWithProducts()
    {
        var inventory = _inventoryContext.Inventory.Select(x =>
            new { x.ProductId, x.UnitPrice }).ToList();

        var discounts = _discountContext.CostumerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.ProductId, x.DiscountRate })
            .ToList();

        var categories = _shopContext.ProductCategories
            .Include(x => x.Products)
            .ThenInclude(x => x.Category)
            .Select(x => new ProductCategoryQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Products = MapProducts(x.Products)
            }).AsNoTracking().ToList();

        foreach (var category in categories)
        foreach (var product in category.Products)
        {
            var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);

            if (productInventory != null)
            {
                var price = productInventory.UnitPrice;
                product.Price = price.ToMoney();

                var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);

                if (discount != null)
                {
                    var discountRate = discount.DiscountRate;

                    product.DiscountRate = discountRate;

                    product.HasDiscount = discountRate > 0;

                    var discountAmount = Math.Round(price * discountRate / 100);

                    product.PriceWithDiscount = (price - discountAmount).ToMoney();
                }
            }
        }

        return categories;
    }

    public ProductCategoryQueryModel GetProductCategoryWithProductsBy(string slug)
    {
        var inventory = _inventoryContext.Inventory.Select(x =>
            new { x.ProductId, x.UnitPrice }).ToList();

        var discounts = _discountContext.CostumerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.ProductId, x.DiscountRate, x.EndDate }).ToList();

        var category = _shopContext.ProductCategories
            .Include(x => x.Products)
            .ThenInclude(x => x.Category)
            .Select(x => new ProductCategoryQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                MetaDescription = x.MetaDescription,
                Keywords = x.KeyWords,
                Products = MapProducts(x.Products)
            }).AsNoTracking().FirstOrDefault(x => x.Slug == slug);

        if (category != null)
            foreach (var product in category.Products)
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


        return category;
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