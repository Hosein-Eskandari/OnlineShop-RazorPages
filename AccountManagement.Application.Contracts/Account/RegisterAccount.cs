using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using AccountManagement.Application.Contracts.Role;
using Microsoft.AspNetCore.Http;
//using AccountManagement.Application.Contracts.Role;

namespace AccountManagement.Application.Contracts.Account;

public class RegisterAccount
{
    private const int MaximumFileSize = 3 * 1024 * 1024;

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [MaxLength(100, ErrorMessage = ValidationMessages.OutOfRange)]
    public string FullName { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [MaxLength(100, ErrorMessage = ValidationMessages.OutOfRange)]
    public string UserName { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [MaxLength(1000, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Password { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [MaxLength(20, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Mobile { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.NotInRange)]
    public long RoleId { get; set; }

    [FileExtensionLimitation(new[] { ".jpeg", ".jpg", ".png" }, ErrorMessage = ValidationMessages.InvalidFileFormat)]
    [MaxFileSize(MaximumFileSize, ErrorMessage = ValidationMessages.ValidationFileSize)]

    public IFormFile ProfilePhoto { get; set; }

    public List<RoleViewModel> Roles { get; set; }
}