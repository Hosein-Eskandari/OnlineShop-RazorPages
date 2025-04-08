using System.Collections.Generic;
using _01_LampshadeQuery.Contracts.Article;
using _01_LampshadeQuery.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages;

public class ArticleCategoryModel : PageModel
{
    private readonly IArticleCategoryQuery _articleCategoryQuery;
    private readonly IArticleQuery _articleQuery;
    public List<ArticleCategoryQueryModel> ArticleCategories;
    public ArticleCategoryQueryModel ArticleCategory;
    public List<ArticleQueryModel> LatestArticles;

    public ArticleCategoryModel(IArticleCategoryQuery articleCategoryQuery, IArticleQuery articleQuery)
    {
        _articleCategoryQuery = articleCategoryQuery;
        _articleQuery = articleQuery;
    }

    public void OnGet(string id)
    {
        ArticleCategory = _articleCategoryQuery.GetArticleCategory(id);
        ArticleCategories = _articleCategoryQuery.GetArticleCategories();

        LatestArticles = _articleQuery.LatestArticles();
    }
}