using System;
using System.ComponentModel.DataAnnotations;

namespace SwiftCarRental.Areas.Identity.Pages.Account
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PhoneNumberValidationAttribute : ValidationAttribute
    {
        private readonly string _countryPropertyName;

        public PhoneNumberValidationAttribute(string countryPropertyName)
        {
            _countryPropertyName = countryPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var countryProperty = validationContext.ObjectType.GetProperty(_countryPropertyName);
            if (countryProperty == null)
            {
                throw new ArgumentException($"Property {_countryPropertyName} not found on {validationContext.ObjectType}");
            }

            var countryValue = countryProperty.GetValue(validationContext.ObjectInstance, null) as string;

            // Perform validation based on the countryValue and value (phone number)
            // Add your custom validation logic here

            // For example, let's say you want to require a valid format for US numbers
            if (countryValue == "US" && !IsValidUSPhoneNumber(value as string))
            {
                return new ValidationResult(ErrorMessage ?? $"Invalid phone number for {countryValue}. The format should be XXX-XXX-XXXX.");
            }

            // For example, let's say you want to require a valid format for Canadian numbers
            if (countryValue == "CAN" && !IsValidCanadianPhoneNumber(value as string))
            {
                return new ValidationResult(ErrorMessage ?? $"Invalid phone number for {countryValue}. The format should be XXX-XXX-XXXX.");
            }

            return ValidationResult.Success;
        }

        // Custom validation logic for US phone numbers
        private bool IsValidUSPhoneNumber(string phoneNumber)
        {
            // US phone number format: XXX-XXX-XXXX
            var usPhoneNumberRegex = @"^\d{3}-\d{3}-\d{4}$";

            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, usPhoneNumberRegex);
        }

        // Custom validation logic for Canadian phone numbers
        private bool IsValidCanadianPhoneNumber(string phoneNumber)
        {
            // Canadian phone number format: XXX-XXX-XXXX
            var canadianPhoneNumberRegex = @"^\d{3}-\d{3}-\d{4}$";

            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, canadianPhoneNumberRegex);
        }
    }
}
