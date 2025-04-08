using System.Collections.Generic;
using DiscountManagement.Application.Contracts.ColleagueDiscount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;

namespace ServiceHost.Areas.Administration.Pages.Discounts.ColleagueDiscounts;

public class IndexModel : PageModel
{
    private readonly IColleagueDiscountApplication _colleagueDiscountApplication;

    private readonly IProductApplication _productApplication;

    public List<ColleagueDiscountViewModel> ColleagueDiscounts;

    public SelectList Products;

    public ColleagueDiscountSearchModel SearchModel;

    public IndexModel(IProductApplication productApplication,
        IColleagueDiscountApplication colleagueDiscountApplication)
    {
        _productApplication = productApplication;
        _colleagueDiscountApplication = colleagueDiscountApplication;
        ColleagueDiscounts = new List<ColleagueDiscountViewModel>();
    }

    [TempData] public string Message { get; set; }

    [TempData] public string IsSuccedded { get; set; }

    public void OnGet(ColleagueDiscountSearchModel searchModel)
    {
        ColleagueDiscounts = _colleagueDiscountApplication.Search(searchModel);
        Products = new SelectList(_productApplication.GetProducts(),
            "Id", "Name");
    }

    public IActionResult OnGetCreate()
    {
        var command = new DefineColleagueDiscount
        {
            Products = _productApplication.GetProducts()
        };
        return Partial("./Create", command);
    }

    public JsonResult OnPostCreate(DefineColleagueDiscount command)
    {
        var result = _colleagueDiscountApplication.Define(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var colleagueDiscount = _colleagueDiscountApplication.GetDetails(id);
        colleagueDiscount.Products = _productApplication.GetProducts();
        return Partial("./Edit", colleagueDiscount);
    }

    public JsonResult OnPostEdit(EditColleagueDiscount command)
    {
        var result = _colleagueDiscountApplication.Edit(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetRemove(long id)
    {
        var result = _colleagueDiscountApplication.Remove(id);
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
        var result = _colleagueDiscountApplication.Restore(id);
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