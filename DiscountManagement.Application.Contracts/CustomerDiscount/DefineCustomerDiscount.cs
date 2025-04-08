using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;

namespace DiscountManagement.Application.Contracts.CustomerDiscount;

public class DefineCustomerDiscount
{
    [Range(1, 100000, ErrorMessage = ValidationMessages.IsRequired)]
    [Required(ErrorMessage = ValidationMessages.IsRequired, AllowEmptyStrings = false)]
    public long ProductId { get; set; }

    //[Range(0.1, 99.99, ErrorMessage = ValidationMessages.NotInRange)]
    [Required(ErrorMessage = ValidationMessages.IsRequired, AllowEmptyStrings = false)]
    public double DiscountRate { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired, AllowEmptyStrings = false)]
    public string StartDate { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired, AllowEmptyStrings = false)]
    public string EndDate { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired, AllowEmptyStrings = false)]
    public string Reason { get; set; }

    public List<ProductViewModel> Products { get; set; }
}