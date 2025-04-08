using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace _0_Framework.Application;

public class FileExtensionLimitationAttribute : ValidationAttribute, IClientModelValidator
{
    private readonly string[] _validExtensions;

    public FileExtensionLimitationAttribute(string[] validExtensions)
    {
        _validExtensions = validExtensions;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        //context.Attributes.Add("data-val", "true");
        context.Attributes.Add("data-val-fileExtensionLimit", ErrorMessage);
    }

    public override bool IsValid(object value)
    {
        var file = value as IFormFile;
        if (file == null) return true;

        var fileExtension = Path.GetExtension(file.FileName);

        return _validExtensions.Contains(fileExtension);
    }
}