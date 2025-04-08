using System.Collections.Generic;
using _0_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Configuration.Permissions;

namespace ServiceHost.Areas.Administration.Pages.Shop.Products;

public class IndexModel : PageModel
{
    private readonly IProductApplication _productApplication;
    private readonly IProductCategoryApplication _productCategoryApplication;

    public SelectList ProductCategories;
    public List<ProductViewModel> Products;

    public ProductSearchModel SearchModel;

    public IndexModel(IProductApplication productApplication, IProductCategoryApplication productCategoryApplication)
    {
        _productApplication = productApplication;
        _productCategoryApplication = productCategoryApplication;
    }

    [TempData] public string Message { get; set; }

    [TempData] public string IsSuccedded { get; set; }

    [NeedsPermission(ShopPermissions.ListProducts)]
    public void OnGet(ProductSearchModel searchModel)
    {
        Products = _productApplication.Search(searchModel);
        ProductCategories = new SelectList(_productCategoryApplication.GetProductCategories(),
            "Id", "Name");
    }

    public IActionResult OnGetCreate()
    {
        var command = new CreateProduct
        {
            Categories = _productCategoryApplication.GetProductCategories()
        };
        return Partial("./Create", command);
    }

    //[NeedsPermission(ShopPermissions.CreateProduct)]
    public JsonResult OnPostCreate(CreateProduct command)
    {
        var result = _productApplication.Create(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var product = _productApplication.GetDetails(id);
        product.Categories = _productCategoryApplication.GetProductCategories();
        return Partial("./Edit", product);
    }

    [NeedsPermission(ShopPermissions.EditProduct)]
    public JsonResult OnPostEdit(EditProduct command)
    {
        var result = _productApplication.Edit(command);
        return new JsonResult(result);
    }
}