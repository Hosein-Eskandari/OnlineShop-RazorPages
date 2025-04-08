using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;

namespace ShopManagement.Application.Contracts.Slide;

public class CreateSlide
{
    //[Required(ErrorMessage = ValidationMessages.IsRequired)]
    //[StringLength(1000, ErrorMessage = ValidationMessages.OutOfRange)]

    private const int MaximumFileSize = 2 * 1024 * 1024;

    [FileExtensionLimitation(new[] { ".jpeg", ".jpg", ".png" }, ErrorMessage = ValidationMessages.InvalidFileFormat)]
    [MaxFileSize(MaximumFileSize, ErrorMessage = ValidationMessages.ValidationFileSize)]
    public IFormFile Picture { get; set; }


    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(500, ErrorMessage = ValidationMessages.OutOfRange)]
    public string PictureTitle { get; set; }


    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(500, ErrorMessage = ValidationMessages.OutOfRange)]
    public string PictureAlt { get; set; }


    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(255, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Heading { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(255, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Title { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(255, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Text { get; set; }


    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(255, ErrorMessage = ValidationMessages.OutOfRange)]
    public string BtnText { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(1000, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Link { get; set; }
}