using System;
using _0_Framework.Domain;
using BlogManagement.Domain.ArticleCategoryAgg;

namespace BlogManagement.Domain.ArticleAgg;

public class Article : EntityBase
{
    public Article(string title, string slug, string shortDescription, string description,
        string picture, string pictureAlt, string pictureTitle, string keywords, string metaDescription,
        string canonicalAddress, DateTime publishDate, long categoryId)
    {
        Title = title;
        Slug = slug;
        ShortDescription = shortDescription;
        Description = description;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        Keywords = keywords;
        MetaDescription = metaDescription;
        CanonicalAddress = canonicalAddress;
        PublishDate = publishDate;
        CategoryId = categoryId;
    }

    public string Title { get; private set; }

    public string Slug { get; private set; }

    public string ShortDescription { get; private set; }

    public string Description { get; private set; }

    public string Picture { get; private set; }

    public string PictureAlt { get; private set; }

    public string PictureTitle { get; private set; }

    public string Keywords { get; private set; }

    public string MetaDescription { get; private set; }

    public string CanonicalAddress { get; private set; }

    public DateTime PublishDate { get; private set; }

    public long CategoryId { get; private set; }

    public ArticleCategory Category { get; }

    public void Edit(string title, string slug, string shortDescription, string description,
        string picture, string pictureAlt, string pictureTitle, string keywords, string metaDescription,
        string canonicalAddress, DateTime publishDate, long categoryId)
    {
        Title = title;
        Slug = slug;
        ShortDescription = shortDescription;
        Description = description;
        if (!string.IsNullOrWhiteSpace(picture)) Picture = picture;

        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        Keywords = keywords;
        MetaDescription = metaDescription;
        CanonicalAddress = canonicalAddress;
        PublishDate = publishDate;
        CategoryId = categoryId;
    }
}