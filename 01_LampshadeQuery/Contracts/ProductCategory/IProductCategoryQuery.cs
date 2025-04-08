using System.Collections.Generic;

namespace _01_LampshadeQuery.Contracts.ProductCategory;

public interface IProductCategoryQuery
{
    List<ProductCategoryQueryModel> GetProductCategories();

    List<ProductCategoryQueryModel> GetProductCategoriesWithProducts();

    ProductCategoryQueryModel GetProductCategoryWithProductsBy(string slug);
}