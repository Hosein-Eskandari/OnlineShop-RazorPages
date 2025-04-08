using System.Collections.Generic;
using _0_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Configuration.Permissions;

namespace ServiceHost.Areas.Administration.Pages.Shop.ProductCategories;

//[Authorize(Roles = "1, 3")]
public class IndexModel : PageModel

{
    private readonly IProductCategoryApplication _productCategoryApplication;
    public List<ProductCategoryViewModel> ProductCategories;

    public ProductCategorySearchModel SearchModel;

    public IndexModel(IProductCategoryApplication productCategoryApplication)
    {
        _productCategoryApplication = productCategoryApplication;
    }

    [TempData] public string ErrorMsg { get; set; }

    //[NeedsPermission(ShopPermissions.ListProductCategories)]
    public void OnGet(ProductCategorySearchModel searchModel)
    {
        ProductCategories = _productCategoryApplication.Search(searchModel);
    }

    public IActionResult OnGetCreate()
    {
        return Partial("./Create", new CreateProductCategory());
    }

    [NeedsPermission(ShopPermissions.CreateProductCategory)]
    public IActionResult OnPostCreate(CreateProductCategory command)
    {
        //if (ModelState.IsValid)
        //{
        var result = _productCategoryApplication.Create(command);
        return new JsonResult(result);
        //}

        //else
        //{
        //    ErrorMsg = "Œÿ« œ— À»  «ÿ·«⁄« ";
        //    ViewData["error"] = "Œÿ«";

        //    return RedirectToPage("./Index");
        //}
    }

    public IActionResult OnGetEdit(long id)
    {
        var productCategory = _productCategoryApplication.GetDetails(id);
        return Partial("./Edit", productCategory);
    }

    //[NeedsPermission(ShopPermissions.EditProductCategory)]
    public JsonResult OnPostEdit(EditProductCategory command)
    {
        var result = _productCategoryApplication.Edit(command);
        return new JsonResult(result);
    }
}