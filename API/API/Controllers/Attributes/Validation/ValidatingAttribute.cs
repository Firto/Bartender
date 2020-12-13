using API.Services.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Attributes.Validation
{
    public class MyValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            validationContext.GetService<IValidatingService>().Validate(validationContext.ObjectType, value);
            return ValidationResult.Success;
        }
    }
}
