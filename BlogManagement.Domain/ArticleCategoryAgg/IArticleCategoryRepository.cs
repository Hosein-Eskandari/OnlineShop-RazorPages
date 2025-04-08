using System.Collections.Generic;
using _0_Framework.Domain;
using BlogManagement.Application.Contracts.ArticleCategory;

namespace BlogManagement.Domain.ArticleCategoryAgg;

public interface IArticleCategoryRepository : IRepository<long, ArticleCategory>
{
    EditArticleCategory GetDetails(long id);

    List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel categorySearchModel);
    string GetCategorySlugById(long id);

    List<ArticleCategoryViewModel> GetArticleCategories();
}