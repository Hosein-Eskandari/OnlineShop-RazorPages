using BlogManagement.Application.Contracts.Article;
using BlogManagement.Application.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Blog.Articles;

public class EditModel : PageModel
{
    private readonly IArticleApplication _articleApplication;
    private readonly IArticleCategoryApplication _articleCategoryApplication;

    public SelectList ArticleCategories;
    public EditArticle Command;

    public EditModel(IArticleCategoryApplication categoryApplication, IArticleApplication articleApplication)
    {
        _articleCategoryApplication = categoryApplication;
        _articleApplication = articleApplication;
    }

    public void OnGet(long id)
    {
        ArticleCategories = new SelectList(_articleCategoryApplication.GetArticleCategories(), "Id", "Name");
        Command = _articleApplication.GetDetails(id);
    }

    public IActionResult OnPost(EditArticle command)
    {
        var result = _articleApplication.Edit(command);
        return RedirectToPage("./Index");
    }
}