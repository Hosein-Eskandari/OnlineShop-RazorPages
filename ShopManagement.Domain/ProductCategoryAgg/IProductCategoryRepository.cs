using System.Collections.Generic;
using _0_Framework.Domain;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ShopManagement.Domain.ProductCategoryAgg;

public interface IProductCategoryRepository : IRepository<long, ProductCategory>
{
    EditProductCategory GetDetails(long id);

    List<ProductCategoryViewModel> Search(ProductCategorySearchModel search);

    string GetSlugById(long id);


    List<ProductCategoryViewModel> GetProductCategories();
}