using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using BlogManagement.Application.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleCategoryAgg;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EFCore.Repository;

public class ArticleCategoryRepository : RepositoryBase<long, ArticleCategory>, IArticleCategoryRepository
{
    private readonly BlogContext _blogContext;

    public ArticleCategoryRepository(BlogContext context) : base(context)
    {
        _blogContext = context;
    }

    public EditArticleCategory GetDetails(long id)
    {
        return _blogContext.ArticleCategories
            .Select(x => new EditArticleCategory
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                KeyWords = x.KeyWords,
                MetaDescription = x.MetaDescription,
                ShowOrder = x.ShowOrder,
                CanonicalAddress = x.CanonicalAddress
            })
            .FirstOrDefault(x => x.Id == id);
    }

    public List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel categorySearchModel)
    {
        var query = _blogContext.ArticleCategories
            .Include(x => x.Articles)
            .Select(x => new ArticleCategoryViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Picture = x.Picture,
                ShowOrder = x.ShowOrder,
                CreationDate = x.CreationDate.ToFarsiFull(),
                ArticleCounts = x.Articles.Count
            });

        if (!string.IsNullOrWhiteSpace(categorySearchModel.Name))
            query = query.Where(x => x.Name == categorySearchModel.Name);

        var result = query.OrderByDescending(x => x.ShowOrder).ToList();

        return result;
    }

    public string GetCategorySlugById(long id)
    {
        return _blogContext.ArticleCategories.Select(x => new { x.Id, x.Slug })
            .FirstOrDefault(x => x.Id == id)?.Slug;
    }

    public List<ArticleCategoryViewModel> GetArticleCategories()
    {
        return _blogContext.ArticleCategories.Select(x => new ArticleCategoryViewModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToList();
        ;
    }
}