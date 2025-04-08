using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Domain.ArticleAgg;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EFCore.Repository;

public class ArticleRepository : RepositoryBase<long, Article>, IArticleRepository
{
    private readonly BlogContext _blogContext;

    public ArticleRepository(BlogContext context) : base(context)
    {
        _blogContext = context;
    }


    public EditArticle GetDetails(long id)
    {
        return _blogContext.Articles.Select(x => new EditArticle
        {
            Id = x.Id,
            Title = x.Title,
            Slug = x.Slug,
            ShortDescription = x.ShortDescription,
            Description = x.Description,
            CanonicalAddress = x.CanonicalAddress,
            CategoryId = x.CategoryId,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            PublishDate = x.PublishDate.ToFileName()
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<ArticleViewModel> Search(ArticleSearchModel searchModel)
    {
        var query = _blogContext.Articles
            .Include(x => x.Category)
            .Select(x => new ArticleViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Category = x.Category.Name,
                Picture = x.Picture,
                PublishDate = x.PublishDate.ToFarsiFull(),
                ShortDescription = x.ShortDescription.Substring(0, Math.Min(x.ShortDescription.Length, 50)) + "..."
            });

        if (!string.IsNullOrWhiteSpace(searchModel.Title)) query = query.Where(x => x.Title == searchModel.Title);

        if (searchModel.CategoryId > 0) query = query.Where(x => x.CategoryId == searchModel.CategoryId);

        return query.OrderByDescending(x => x.Id).ToList();
    }

    public Article GetWithCategory(long id)
    {
        return _blogContext.Articles.Include(x => x.Category)
            .FirstOrDefault(x => x.Id == id);
    }
}