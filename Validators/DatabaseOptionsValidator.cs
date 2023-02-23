using FluentValidation;
using FreelanceStormer.Utils;

namespace FreelanceStormer.Validators
{
    public class DatabaseOptionsValidator : AbstractValidator<DatabaseOptions>
    {
        public DatabaseOptionsValidator() 
        {
            RuleFor(o => o.FreelanceStormerDbContext)
                .NotEmpty();

            RuleFor(o => o.Test)
                .InclusiveBetween(1, 10)
                .WithMessage("Test must be a value between 1 and 10");
        }
    }
}
