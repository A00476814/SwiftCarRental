using SwiftCarRental.Models;
using System.ComponentModel.DataAnnotations;

namespace SwiftCarRental.Models
{
    public class ExpirationDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var paymentDetails = (PaymentDetails)validationContext.ObjectInstance;
            var expirationDate = paymentDetails.ExpirationDate;

            DateTime expDate;
            if (DateTime.TryParseExact(expirationDate, "MM/yyyy", null, System.Globalization.DateTimeStyles.None, out expDate))
            {
                if (expDate > DateTime.Now)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Expiration date should be greater than the current date");
                }
            }
            else
            {
                return new ValidationResult("Invalid expiration date format");
            }
        }
    }
}
