using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductPicture;

namespace ServiceHost.Areas.Administration.Pages.Shop.ProductPictures;

public class IndexModel : PageModel
{
    private readonly IProductApplication _productApplication;

    private readonly IProductPictureApplication _productPictureApplication;
    public List<ProductPictureViewModel> ProductPictures;

    public SelectList Products;

    public ProductPictureSearchModel SearchModel;

    public IndexModel(IProductPictureApplication productPictureApplication, IProductApplication productApplication)
    {
        _productPictureApplication = productPictureApplication;
        _productApplication = productApplication;
    }

    [TempData] public string Message { get; set; }

    [TempData] public string IsSuccedded { get; set; }

    public void OnGet(ProductPictureSearchModel searchModel)
    {
        ProductPictures = _productPictureApplication.Search(searchModel);
        Products = new SelectList(_productApplication.GetProducts(),
            "Id", "Name");
    }

    public IActionResult OnGetCreate()
    {
        var command = new CreateProductPicture
        {
            Products = _productApplication.GetProducts()
        };
        return Partial("./Create", command);
    }

    public JsonResult OnPostCreate(CreateProductPicture command)
    {
        var result = _productPictureApplication.Create(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var productPicture = _productPictureApplication.GetDetails(id);
        productPicture.Products = _productApplication.GetProducts();
        return Partial("./Edit", productPicture);
    }

    public JsonResult OnPostEdit(EditProductPicture command)
    {
        var result = _productPictureApplication.Edit(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetRemove(long id)
    {
        var result = _productPictureApplication.Remove(id);
        if (result.IsSuccedded)
        {
            Message = result.Message;
            IsSuccedded = result.IsSuccedded.ToString();
            return RedirectToPage("./Index");
        }

        Message = result.Message;
        IsSuccedded = result.IsSuccedded.ToString();
        return RedirectToPage("./Index");
    }

    public IActionResult OnGetRestore(long id)
    {
        var result = _productPictureApplication.Restore(id);
        if (result.IsSuccedded)
        {
            Message = result.Message;
            IsSuccedded = result.IsSuccedded.ToString();
            return RedirectToPage("./Index");
        }

        Message = result.Message;
        IsSuccedded = result.IsSuccedded.ToString();
        return RedirectToPage("./Index");
    }
}