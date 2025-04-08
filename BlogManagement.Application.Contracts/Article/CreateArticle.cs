using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;

namespace BlogManagement.Application.Contracts.Article;

public class CreateArticle
{
    private const int MaximumFileSize = 5 * 1024 * 1024;

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(500, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Title { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(600, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Slug { get; set; }


    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(2000, ErrorMessage = ValidationMessages.OutOfRange)]
    public string ShortDescription { get; set; }

    //[Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Description { get; set; }

    [FileExtensionLimitation(new[] { ".jpeg", ".jpg", ".png" }, ErrorMessage = ValidationMessages.InvalidFileFormat)]
    [MaxFileSize(MaximumFileSize, ErrorMessage = ValidationMessages.ValidationFileSize)]
    public IFormFile Picture { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(1000, ErrorMessage = ValidationMessages.OutOfRange)]
    public string PictureAlt { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(1000, ErrorMessage = ValidationMessages.OutOfRange)]
    public string PictureTitle { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(100, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Keywords { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(150, ErrorMessage = ValidationMessages.OutOfRange)]
    public string MetaDescription { get; set; }

    public string CanonicalAddress { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string PublishDate { get; set; }

    [Range(1, long.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
    public long CategoryId { get; set; }
}