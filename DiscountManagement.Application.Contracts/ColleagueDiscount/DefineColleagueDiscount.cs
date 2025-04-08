using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;

namespace DiscountManagement.Application.Contracts.ColleagueDiscount;

public class DefineColleagueDiscount
{
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [Range(1, 100000, ErrorMessage = ValidationMessages.IsRequired)]

    public long ProductId { get; set; }


    [Range(0.1, 99.99, ErrorMessage = ValidationMessages.NotInRange)]
    [Required(ErrorMessage = ValidationMessages.IsRequired, AllowEmptyStrings = false)]
    public double DiscountRate { get; set; }

    public List<ProductViewModel> Products { get; set; }
}