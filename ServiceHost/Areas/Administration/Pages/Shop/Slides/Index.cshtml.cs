using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.Slide;

namespace ServiceHost.Areas.Administration.Pages.Shop.Slides;

public class IndexModel : PageModel
{
    private readonly ISlideApplication _slideApplication;

    public List<SlideViewModel> Slides;

    public IndexModel(ISlideApplication slideApplication)
    {
        _slideApplication = slideApplication;
    }

    [TempData] public string Message { get; set; }

    [TempData] public string IsSuccedded { get; set; }


    public void OnGet()
    {
        Slides = _slideApplication.GetList();
    }

    public IActionResult OnGetCreate()
    {
        var command = new CreateSlide();
        return Partial("./Create", command);
    }

    public JsonResult OnPostCreate(CreateSlide command)
    {
        var result = _slideApplication.Create(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var slide = _slideApplication.GetDetails(id);
        return Partial("./Edit", slide);
    }

    public JsonResult OnPostEdit(EditSlide command)
    {
        var result = _slideApplication.Edit(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetRemove(long id)
    {
        var result = _slideApplication.Remove(id);
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
        var result = _slideApplication.Restore(id);
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