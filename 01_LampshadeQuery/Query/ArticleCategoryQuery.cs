using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Article;
using _01_LampshadeQuery.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace _01_LampshadeQuery.Query;

public class ArticleCategoryQuery : IArticleCategoryQuery
{
    private readonly BlogContext _blogContext;

    public ArticleCategoryQuery(BlogContext blogContext)
    {
        _blogContext = blogContext;
    }

    public ArticleCategoryQueryModel GetArticleCategory(string slug)
    {
        var result = _blogContext.ArticleCategories
            .Include(x => x.Articles)
            .Select(x => new ArticleCategoryQueryModel
            {
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                MetaDescription = x.MetaDescription,
                KeyWords = x.KeyWords,
                CanonicalAddress = x.CanonicalAddress,
                Articles = MapArticles(x.Articles),
                ArticlesCount = x.Articles.Count
            })
            .AsNoTracking()
            .FirstOrDefault(x => x.Slug == slug);

        if (!string.IsNullOrWhiteSpace(result.KeyWords)) result.KeywordList = result.KeyWords.Split(",").ToList();

        return result;
    }

    public List<ArticleCategoryQueryModel> GetArticleCategories()
    {
        var result = _blogContext.ArticleCategories
            .Include(x => x.Articles)
            .Select(x => new ArticleCategoryQueryModel
            {
                Name = x.Name,
                Slug = x.Slug,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                KeyWords = x.KeyWords,
                MetaDescription = x.MetaDescription,
                CanonicalAddress = x.CanonicalAddress,
                Description = x.Description,
                ArticlesCount = x.Articles.Count
            });

        return result.ToList();
    }

    private static List<ArticleQueryModel> MapArticles(List<Article> articles)
    {
        var result = articles.Select(x => new ArticleQueryModel
        {
            Title = x.Title,
            Slug = x.Slug,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            ShortDescription = x.ShortDescription.Substring(0, Math.Min(x.ShortDescription.Length, 115)) + "...",
            MetaDescription = x.MetaDescription,
            Keywords = x.Keywords,
            //CanonicalAddress = x.CanonicalAddress,
            //CategorySlug = x.Category.Slug,
            //CategoryName = x.Category.Name,
            //CategoryId = x.Category.Id,
            PublishDate = x.PublishDate.ToFarsiFull()
        }).ToList();

        return result;
    }
}