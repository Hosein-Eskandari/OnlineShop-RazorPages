using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Application.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Blog.Articles;

public class IndexModel : PageModel

{
    private readonly IArticleApplication _articleApplication;
    private readonly IArticleCategoryApplication _articleCategoryApplication;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public SelectList ArticleCategories;
    public List<ArticleViewModel> Articles;


    public ArticleSearchModel SearchModel;

    public IndexModel(IArticleApplication articleApplication, IArticleCategoryApplication articleCategoryApplication,
        IWebHostEnvironment webHostEnvironment)
    {
        _articleApplication = articleApplication;
        _articleCategoryApplication = articleCategoryApplication;
        _webHostEnvironment = webHostEnvironment;
    }

    [TempData] public string ErrorMsg { get; set; }

    public void OnGet(ArticleSearchModel searchModel)
    {
        ArticleCategories = new SelectList(_articleCategoryApplication
            .GetArticleCategories(), "Id", "Name");
        Articles = _articleApplication.Search(searchModel);
    }

    public async Task<JsonResult> OnPostUploadImage([FromForm] IFormFile upload)
    {
        if (upload.Length <= 0) return null;

        //your custom code logic here

        //1)check if the file is image

        //2)check if the file is too large

        //etc

        var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

        //save file under wwwroot/CKEditorImages folder

        var filePath = Path.Combine(
            Directory.GetCurrentDirectory(), "wwwroot/CKEditorImages",
            fileName);

        using (var stream = System.IO.File.Create(filePath))
        {
            await upload.CopyToAsync(stream);
        }

        var url = $"{"/CKEditorImages/"}{fileName}";

        var success = new uploadsuccess
        {
            Uploaded = 1,
            FileName = fileName,
            Url = url
        };

        return new JsonResult(success);
    }
}

public class uploadsuccess
{
    public int Uploaded { get; set; }
    public string FileName { get; set; }
    public string Url { get; set; }
}