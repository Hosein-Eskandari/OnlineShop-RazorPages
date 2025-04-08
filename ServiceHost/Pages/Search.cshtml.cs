using System.Collections.Generic;
using _01_LampshadeQuery.Contracts.Article;
using _01_LampshadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages;

public class SearchModel : PageModel
{
    private readonly IArticleQuery _articleQuery;

    private readonly IProductQuery _productQuery;
    public List<ArticleQueryModel> Articles;
    public List<ProductQueryModel> Products;
    public string Value;

    public SearchModel(IProductQuery productQuery, IArticleQuery articleQuery)
    {
        _productQuery = productQuery;
        _articleQuery = articleQuery;
    }


    public void OnGet(string value)
    {
        Value = value;
        Products = _productQuery.Search(value);
    }


    public void OnGetArticleSearch(string value)
    {
        Value = value;
        Articles = _articleQuery.Search(value);

    }
}