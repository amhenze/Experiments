using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using WebApplication2._0.Options;

namespace WebApplication2._0.Validators
{
    public class RootValidator : AbstractValidator<RootFolderOptions> ,IValidateOptions<RootFolderOptions>
    {
        public RootValidator()
        {
            RuleFor(x => x.Path)
                .NotEmpty()
                .Must(x => Directory.Exists(x))
                .WithMessage("Root directory not exist");
        }

        public ValidateOptionsResult Validate(string name, RootFolderOptions options)
        {
            ValidationResult result = Validate(options);
            if (result.IsValid)
            {
                return ValidateOptionsResult.Success;
            }
            return ValidateOptionsResult.Fail(result.Errors.Select(x => x.ErrorMessage));
        }
    }
}
