using _01_LampshadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages;

public class ProductCategoryModel : PageModel
{
    private readonly IProductCategoryQuery _productCategoryQuery;
    public ProductCategoryQueryModel Category;

    public ProductCategoryModel(IProductCategoryQuery productCategoryQuery)
    {
        _productCategoryQuery = productCategoryQuery;
    }


    public void OnGet(string id)
    {
        Category = _productCategoryQuery.GetProductCategoryWithProductsBy(id);
    }
}