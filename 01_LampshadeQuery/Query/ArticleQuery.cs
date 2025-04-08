using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Article;
using _01_LampshadeQuery.Contracts.Comment;
using BlogManagement.Infrastructure.EFCore;
using CommentManagement.Domain.CommentAgg;
using CommentManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace _01_LampshadeQuery.Query;

public class ArticleQuery : IArticleQuery
{
    private readonly BlogContext _blogContext;
    private readonly CommentContext _commentContext;

    public ArticleQuery(BlogContext blogContext, CommentContext commentContext)
    {
        _blogContext = blogContext;
        _commentContext = commentContext;
    }

    public List<ArticleQueryModel> LatestArticles()
    {
        var articles = _blogContext.Articles
            .Include(x => x.Category)
            .Where(x => x.PublishDate <= DateTime.Now)
            .Select(x => new ArticleQueryModel
            {
                Title = x.Title,
                Slug = x.Slug,
                ShortDescription = x.ShortDescription.Substring(0, Math.Min(x.ShortDescription.Length, 100)) + "...",
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                PublishDate = x.PublishDate.ToFarsiFull()
            }).Take(6).ToList();

        return articles;
    }

    public ArticleQueryModel GetArticleDetails(string slug)
    {

        var article = _blogContext.Articles
            .Include(x => x.Category)
            .Where(x => x.PublishDate <= DateTime.Now)
            .Select(x => new ArticleQueryModel
            {
                Id = x.Id,
                Title = x.Title,
                CategoryId = x.CategoryId,
                Slug = x.Slug,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                CategoryName = x.Category.Name,
                CategorySlug = x.Category.Slug,
                PublishDate = x.PublishDate.ToFarsiFull(),
                CanonicalAddress = x.CanonicalAddress
            }).First(x => x.Slug == slug);

        if (!string.IsNullOrWhiteSpace(article.Keywords)) article.KeywordList = article.Keywords.Split(",").ToList();

        var comments = _commentContext.Comments
            .Where(x => !x.IsCanceled)
            .Where(x => x.IsConfirmed)
            .Where(x => x.Type == CommentType.Article)
            .Where(x => x.OwnerRecordId == article.Id)
            .Select(x => new CommentQueryModel
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                Message = x.Message,
                Website = x.Website,
                CreationDate = x.CreationDate.ToFarsiFull(),
                Children = MapChildren(x.Children)
            })
            .OrderByDescending(x => x.Id).ToList();

        foreach (var comment in comments)
            if (comment.ParentId > 0)
                comment.ParentName = comments.FirstOrDefault(x => x.Id == comment.ParentId)?.Name;

        if (comments.Count > 0) article.Comments = comments;
        return article;
    }

    public List<ArticleQueryModel> Search(string value)
    {
        var query = _blogContext.Articles
            .Include(x => x.Category)
            .Select(x => new ArticleQueryModel
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                CategoryName = x.Category.Name,
                CategorySlug = x.Category.Slug,
                ShortDescription = x.ShortDescription,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription
            }).AsNoTracking();


        if (!string.IsNullOrWhiteSpace(value))
            query = query.Where(x => x.Title.Contains(value) || x.ShortDescription.Contains(value) ||
                                     x.CategoryName.Contains(value) || x.Keywords.Contains(value));


        var articles = query.ToList().OrderByDescending(x => x.Id).ToList();


        return articles;
    }

    private static List<CommentQueryModel> MapChildren(ICollection<Comment> children)
    {
        var result = children.Select(x => new CommentQueryModel
        {
            Id = x.Id,
            ParentId = x.ParentId,
            Name = x.Name,
            Message = x.Message,
            Website = x.Website,
            CreationDate = x.CreationDate.ToFarsiFull()
        }).ToList();

        return result;
    }
}