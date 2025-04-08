using _01_LampshadeQuery.Contracts.Product;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Infrastructure.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages;

public class ProductDetailsModel : PageModel
{
    public const string ErrorMessage = "فیلد کامنت نمیتواند خالی باشد";
    private readonly ICommentApplication _commentApplication;


    private readonly IProductQuery _productQuery;

    public AddComment Command;
    public ProductQueryModel Product;

    public ProductDetailsModel(IProductQuery productQuery, ICommentApplication commentApplication)
    {
        _productQuery = productQuery;
        _commentApplication = commentApplication;
    }

    [TempData] public string Message { get; set; }

    public void OnGet(string id)
    {
        Product = _productQuery.GetDetails(id);
    }

    public IActionResult OnPost(AddComment command, string productSlug)
    {
        //if (!ModelState.IsValid)
        //{
        //Message = ErrorMessage;

        //return RedirectToPage("./ProductDetails", new { Id = productSlug });
        //}

        command.Type = CommentType.Product;
        var result = _commentApplication.Add(command);
        return RedirectToPage("./ProductDetails", new { Id = productSlug });
    }
}