using System.Collections.Generic;
using _0_Framework.Application;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;

namespace BlogManagement.Application;

public class ArticleApplication : IArticleApplication
{
    private readonly IArticleCategoryRepository _articleCategoryRepository;

    private readonly IArticleRepository _articleRepository;
    private readonly IFileUploader _fileUploader;

    public ArticleApplication(IArticleRepository articleRepository, IFileUploader fileUploader,
        IArticleCategoryRepository articleCategoryRepository)
    {
        _articleRepository = articleRepository;
        _fileUploader = fileUploader;
        _articleCategoryRepository = articleCategoryRepository;
    }

    public OperationResult Create(CreateArticle command)
    {
        var operation = new OperationResult();
        if (_articleRepository.Exists(x => x.Title == command.Title))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var categorySlug = _articleCategoryRepository.GetCategorySlugById(command.CategoryId);

        var slug = command.Slug.Slugify();
        var path = $"ArticleCategory/{categorySlug}/{slug}";

        var pictureName = _fileUploader.Upload(command.Picture, path);

        var publishDate = command.PublishDate.ToGeorgianDateTime();

        var article = new Article(command.Title, slug, command.ShortDescription,
            command.Description, pictureName, command.PictureAlt, command.PictureTitle,
            command.Keywords, command.MetaDescription, command.CanonicalAddress, publishDate, command.CategoryId);

        _articleRepository.Create(article);
        _articleRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Edit(EditArticle command)
    {
        var operation = new OperationResult();
        var article = _articleRepository.GetWithCategory(command.Id);

        if (article == null) return operation.Failed(ApplicationMessages.RecordNotFound);


        if (_articleRepository.Exists(x => x.Title == command.Title && x.Id != command.Id))
            operation.Failed(ApplicationMessages.DuplicatedRecord);

        var categorySlug = article.Category.Slug;

        var slug = command.Slug.Slugify();
        var path = $"ArticleCategory/{categorySlug}/{slug}";

        var pictureName = _fileUploader.Upload(command.Picture, path);

        var publishDate = command.PublishDate.ToGeorgianDateTime();


        article.Edit(command.Title, slug, command.ShortDescription,
            command.Description, pictureName, command.PictureAlt, command.PictureTitle,
            command.Keywords, command.MetaDescription, command.CanonicalAddress, publishDate, command.CategoryId);

        _articleRepository.SaveChanges();
        return operation.Succeeded();
    }

    public EditArticle GetDetails(long id)
    {
        return _articleRepository.GetDetails(id);
    }

    public List<ArticleViewModel> Search(ArticleSearchModel searchModel)
    {
        return _articleRepository.Search(searchModel);
    }
}