using System.Collections.Generic;
using BlogManagement.Application.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages.Blog.ArticleCategories;

public class IndexModel : PageModel

{
    private readonly IArticleCategoryApplication _articleCategoryApplication;
    public List<ArticleCategoryViewModel> ArticleCategories;

    public ArticleCategorySearchModel CategorySearchModel;

    public IndexModel(IArticleCategoryApplication articleCategoryApplication)
    {
        _articleCategoryApplication = articleCategoryApplication;
    }

    [TempData] public string ErrorMsg { get; set; }

    public void OnGet(ArticleCategorySearchModel categorySearchModel)
    {
        ArticleCategories = _articleCategoryApplication.Search(categorySearchModel);
    }

    public IActionResult OnGetCreate()
    {
        return Partial("./Create", new CreateArticleCategory());
    }

    public IActionResult OnPostCreate(CreateArticleCategory command)
    {
        //if (ModelState.IsValid)
        //{
        var result = _articleCategoryApplication.Create(command);
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
        var articleCategory = _articleCategoryApplication.GetDetails(id);
        return Partial("./Edit", articleCategory);
    }

    public JsonResult OnPostEdit(EditArticleCategory command)
    {
        var result = _articleCategoryApplication.Edit(command);
        return new JsonResult(result);
    }
}