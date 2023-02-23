using FluentValidation;
using Microsoft.Extensions.Options;

namespace FreelanceStormer.Validators
{
    public static class OptionsValidationExtension
    {
        public static OptionsBuilder<TOptions> ValidateFluently<TOptions>(
            this OptionsBuilder<TOptions> optionsBuilder) 
                where TOptions : class
        {
            optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
                sp => new FluentValidationOptions<TOptions>(
                    optionsBuilder.Name, 
                    sp.GetRequiredService<IValidator<TOptions>>()));
            
            return optionsBuilder;
        }
    }

    public class FluentValidationOptions<TOptions> : IValidateOptions<TOptions> 
        where TOptions : class
    {
        public string Name { get; set; }

        private readonly IValidator<TOptions> _validator;

        public FluentValidationOptions(
            string name, 
            IValidator<TOptions> validator)
        {
            Name = name;
            _validator = validator;
        }

        public ValidateOptionsResult Validate(string? name, TOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            var validationResults = _validator.Validate(options);

            if (validationResults.IsValid)
            {
                return ValidateOptionsResult.Success;
            }

            string typeName = options.GetType().Name;

            var errors = validationResults.Errors.Select(
                err => $"Validation failed for property '{err.PropertyName}' with error '{err.ErrorMessage}'");

            return ValidateOptionsResult.Fail(errors);
        }
    }
}
