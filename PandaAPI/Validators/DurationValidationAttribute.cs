using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PandaAPI.Validators
{
    public class DurationValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string duration)
            {
                var regex = new Regex(@"^([1-9]h)?([0-5][0-9]m)?$");
                if (duration != string.Empty && regex.IsMatch(duration))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Invalid duration format. Valid formats are '1h', '15m', '1h15m'.");
        }
    }
}