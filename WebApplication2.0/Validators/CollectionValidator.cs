using FluentValidation;
using Microsoft.Extensions.Options;
using WebApplication2._0.Controllers;
using WebApplication2._0.Models;
using WebApplication2._0.Options;

namespace WebApplication2._0.Validators
{
    public class CollectionValidator : AbstractValidator<CollectionModel>
    {
        private readonly ILogger<CollectionValidator> _logger;
        public CollectionValidator(ILogger<CollectionValidator> logger,IOptions<RootFolderOptions> options)
        {
            _logger = logger;
            RuleFor(x => x.CollectionName)
                .NotEmpty()
                .Must(x => Directory.Exists(Path.Combine(options.Value.Path, x)))
                .WithMessage("Directory not exist");
        }

    }
}
