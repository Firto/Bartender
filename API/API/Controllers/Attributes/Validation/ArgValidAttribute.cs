using API.Services.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Attributes.Validation
{
    public class ArgValidAttribute : RequiredAttribute
    {
        public string[] FuncIdsAtributes { get; private set; }

        public ArgValidAttribute(params string[] funcIds)
            => FuncIdsAtributes = funcIds;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            validationContext.GetService<IValidatingService>().Validate(FuncIdsAtributes, value, validationContext.DisplayName);
            return ValidationResult.Success;
        }


    }
}
