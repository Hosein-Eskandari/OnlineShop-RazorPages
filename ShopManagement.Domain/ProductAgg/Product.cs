using System.Collections.Generic;
using _0_Framework.Domain;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Domain.ProductAgg;

public class Product : EntityBase
{
    protected Product()
    {
    }

    public Product(string name, string code,
        string shortDescription, string description, string picture,
        string pictureAlt, string pictureTitle, long categoryId, string slug,
        string keywords, string metaDescription)
    {
        Name = name;
        Code = code;
        ShortDescription = shortDescription;
        Description = description;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        CategoryId = categoryId;
        Slug = slug;
        Keywords = keywords;
        MetaDescription = metaDescription;
        ProductPictures = new List<ProductPicture>();
    }

    public string Name { get; private set; }

    public string Slug { get; private set; }

    public string Code { get; private set; }

    public string ShortDescription { get; private set; }

    public string Description { get; private set; }

    public string Picture { get; private set; }

    public string PictureAlt { get; private set; }

    public string PictureTitle { get; private set; }

    public string Keywords { get; private set; }


    public string MetaDescription { get; private set; }

    public long CategoryId { get; private set; }

    public ProductCategory Category { get; }

    public ICollection<ProductPicture> ProductPictures { get; private set; }

    public void Edit(string name, string code,
        string shortDescription, string description, string picture,
        string pictureAlt, string pictureTitle, long categoryId, string slug,
        string keywords, string metaDescription)
    {
        Name = name;
        Code = code;
        ShortDescription = shortDescription;
        Description = description;
        if (!string.IsNullOrWhiteSpace(picture)) Picture = picture;

        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        CategoryId = categoryId;
        Slug = slug;
        Keywords = keywords;
        MetaDescription = metaDescription;
    }
}