using System.Collections.Generic;
using _0_Framework.Application;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Application.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Blog.Articles;

[IgnoreAntiforgeryToken(Order = 2000)]
public class CreateModel : PageModel
{
    private readonly IArticleApplication _articleApplication;

    private readonly IArticleCategoryApplication _articleCategoryApplication;
    private readonly IFileUploader _fileUploader;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public SelectList ArticleCategories;
    public CreateArticle Command;

    public CreateModel(IArticleCategoryApplication articleCategoryApplication,
        IArticleApplication articleApplication,
        IWebHostEnvironment webHostEnvironment,
        IFileUploader fileUploader)
    {
        _articleCategoryApplication = articleCategoryApplication;
        _articleApplication = articleApplication;
        _webHostEnvironment = webHostEnvironment;
        _fileUploader = fileUploader;
    }

    public void OnGet()
    {
        ArticleCategories = new SelectList(_articleCategoryApplication.GetArticleCategories(), "Id", "Name");
    }

    public IActionResult OnPost(CreateArticle command)
    {
        if (!ModelState.IsValid) return Page();

        var result = _articleApplication.Create(command);

        return RedirectToPage("./Index");
    }

    public IActionResult OnPostUploadImage(List<IFormFile> upload)
    {
        var files = upload;
        var filePath = "";
        foreach (var photo in Request.Form.Files)
        {
            var path = "ArticleImage";

            var pictureName = _fileUploader.Upload(photo, path);
            filePath = "/UploadedFiles/" + pictureName;
        }

        return new JsonResult(new { Url = filePath });
    }


    //public async Task<JsonResult> OnPostUploadImage([FromForm] IFormFile upload)
    //{
    //    if (upload.Length <= 0) return null;

    //    //your custom code logic here

    //    //1)check if the file is image

    //    //2)check if the file is too large


    //    //etc


    //    var path = "ArticleImage";

    //    var pictureName = _fileUploader.Upload(upload, path);

    //    //var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

    //    //save file under wwwroot/CKEditorImages folder

    //    //var filePath = Path.Combine(
    //    //    Directory.GetCurrentDirectory(), "wwwroot\\CKEditorImages",
    //    //    fileName);

    //    //await using (var stream = System.IO.File.Register(filePath))
    //    //{
    //    //    await upload.CopyToAsync(stream);
    //    //}

    //    var url = $"{"/ArticleImage/"}{pictureName}";

    //    var success = new uploadsuccess
    //    {
    //        Uploaded = 1,
    //        FileName = pictureName,
    //        Url = url
    //    };

    //    return new JsonResult(success);
    //}
}