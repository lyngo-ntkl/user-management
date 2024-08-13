using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserManagement.Application.Common
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class PasswordAttribute : ValidationAttribute
    {
        private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[~`!@#$%^&*()-_+=\?{}[\]:;',.])[a-zA-Z\d~`!@#$%^&*()-_+=\?{}[\]:;',.]{16,}$";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || !Regex.IsMatch((string)value, PasswordPattern))
            {
                return new ValidationResult("Invalid password. Password contains at least 16 characters including at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character.");
            }
            return ValidationResult.Success;
        }
    }
}
