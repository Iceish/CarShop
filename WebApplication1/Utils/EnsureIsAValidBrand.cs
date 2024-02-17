using System.ComponentModel.DataAnnotations;

namespace Server.Utils;

public class EnsureIsAValidBrand : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        IList<string> validBrands = new List<string> { "Toyota", "Ford", "Chevrolet", "Nissan" };
        if (!validBrands.Contains(value))
        {
            return new ValidationResult("Invalid brand");
        }
        return ValidationResult.Success;
    }

}