using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;
using ShopManagement.Application.Contracts.Product;

namespace ShopManagement.Application.Contracts.ProductPicture;

public class CreateProductPicture
{
    //[Required(ErrorMessage = ValidationMessages.IsRequired)]
    //[StringLength(1000, ErrorMessage = ValidationMessages.OutOfRange)]
    private const int MaximumFileSize = 2 * 1024 * 1024;

    [Range(1, 100000, ErrorMessage = ValidationMessages.IsRequired)]
    public long ProductId { get; set; }

    [FileExtensionLimitation(new[] { ".jpeg", ".jpg", ".png" }, ErrorMessage = ValidationMessages.InvalidFileFormat)]
    [MaxFileSize(MaximumFileSize, ErrorMessage = ValidationMessages.ValidationFileSize)]
    public IFormFile Picture { get; set; }


    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(500, ErrorMessage = ValidationMessages.OutOfRange)]
    public string PictureAlt { get; set; }


    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(500, ErrorMessage = ValidationMessages.OutOfRange)]
    public string PictureTitle { get; set; }

    public List<ProductViewModel> Products { get; set; }
}