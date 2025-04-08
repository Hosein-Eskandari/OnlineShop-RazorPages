using System.Collections.Generic;
using DiscountManagement.Application.Contracts.CustomerDiscount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;

namespace ServiceHost.Areas.Administration.Pages.Discounts.CustomerDiscounts;

public class IndexModel : PageModel
{
    private readonly ICustomerDiscountApplication _customerDiscountApplication;

    private readonly IProductApplication _productApplication;

    public List<CustomerDiscountViewModel> CustomerDiscounts;

    public SelectList Products;

    public CustomerDiscountSearchModel SearchModel;

    public IndexModel(IProductApplication productApplication, ICustomerDiscountApplication customerDiscountApplication)
    {
        _productApplication = productApplication;
        _customerDiscountApplication = customerDiscountApplication;
        CustomerDiscounts = new List<CustomerDiscountViewModel>();
    }

    [TempData] public string Message { get; set; }

    [TempData] public string IsSuccedded { get; set; }

    public void OnGet(CustomerDiscountSearchModel searchModel)
    {
        CustomerDiscounts = _customerDiscountApplication.Search(searchModel);
        Products = new SelectList(_productApplication.GetProducts(),
            "Id", "Name");
    }

    public IActionResult OnGetCreate()
    {
        var command = new DefineCustomerDiscount
        {
            Products = _productApplication.GetProducts()
        };
        return Partial("./Create", command);
    }

    public JsonResult OnPostCreate(DefineCustomerDiscount command)
    {
        var result = _customerDiscountApplication.Define(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var customerDiscount = _customerDiscountApplication.GetDetails(id);
        customerDiscount.Products = _productApplication.GetProducts();
        return Partial("./Edit", customerDiscount);
    }

    public JsonResult OnPostEdit(EditCustomerDiscount command)
    {
        var result = _customerDiscountApplication.Edit(command);
        return new JsonResult(result);
    }

    //public IActionResult OnGetInStock(long id)
    //{
    //    var result = _productApplication.InStock(id);
    //    if (result.IsSucceeded)
    //    {
    //        Message = result.Message;
    //        IsSucceeded = result.IsSucceeded.ToString();
    //        return RedirectToPage("./Index");

    //    }
    //    Message = result.Message;
    //    IsSucceeded = result.IsSucceeded.ToString();
    //    return RedirectToPage("./Index");

    //}

    //public IActionResult OnGetNotInStock(long id)
    //{
    //    var result = _productApplication.NotInStock(id);
    //    if (result.IsSucceeded)
    //    {
    //        Message = result.Message;
    //        IsSucceeded = result.IsSucceeded.ToString();
    //        return RedirectToPage("./Index");

    //    }
    //    Message = result.Message;
    //    IsSucceeded = result.IsSucceeded.ToString();
    //    return RedirectToPage("./Index");

    //}
}