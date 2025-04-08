using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository;

public class ProductPictureRepository : RepositoryBase<long, ProductPicture>, IProductPictureRepository
{
    private readonly ShopContext _context;

    public ProductPictureRepository(ShopContext context) : base(context)
    {
        _context = context;
    }

    public EditProductPicture GetDetails(long id)
    {
        var result = _context.ProductPictures
            .Select(x => new EditProductPicture
            {
                Id = x.Id,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                ProductId = x.ProductId
            }).FirstOrDefault(x => x.Id == id);

        return result;
    }

    public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel)
    {
        var query = _context.ProductPictures
            .Include(x => x.Product)
            .Select(x => new ProductPictureViewModel
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Product = x.Product.Name,
                CreationDate = x.CreationDate.ToFarsiFull(),
                IsRemoved = x.IsRemoved,
                Picture = x.Picture
            });

        if (searchModel.ProductId != 0) query = query.Where(x => x.ProductId == searchModel.ProductId);

        return query.OrderByDescending(x => x.Id).ToList();
    }

    public ProductPicture GetWithProductAndCategory(long id)
    {
        return _context.ProductPictures.Include(x => x.Product)
            .ThenInclude(x => x.Category)
            .FirstOrDefault(x => x.Id == id);
    }
}