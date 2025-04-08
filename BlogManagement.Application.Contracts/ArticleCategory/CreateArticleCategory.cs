using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;

namespace BlogManagement.Application.Contracts.ArticleCategory;

public class CreateArticleCategory
{
    private const int MaximumFileSize = 5 * 1024 * 1024;

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(500, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Name { get; set; }

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
    [StringLength(2000, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Description { get; set; }


    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
    public int ShowOrder { get; set; }


    [StringLength(600, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Slug { get; set; }


    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(100, ErrorMessage = ValidationMessages.OutOfRange)]
    public string KeyWords { get; set; }


    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(150, ErrorMessage = ValidationMessages.OutOfRange)]
    public string MetaDescription { get; set; }


    [StringLength(1000, ErrorMessage = ValidationMessages.OutOfRange)]
    public string CanonicalAddress { get; set; }
}