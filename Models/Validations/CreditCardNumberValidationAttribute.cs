using SwiftCarRental.Models;
using System.ComponentModel.DataAnnotations;

namespace SwiftCarRental.Models
{
    public class CreditCardNumberValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var paymentDetails = (PaymentDetails)validationContext.ObjectInstance;
            var creditCardType = paymentDetails.CreditCardType;
            var creditCardNumber = paymentDetails.CreditCardNumber;

            if (creditCardType == "MasterCard" && creditCardNumber.Length == 16 && creditCardNumber.StartsWith("51") || creditCardNumber.StartsWith("52") || creditCardNumber.StartsWith("53") || creditCardNumber.StartsWith("54") || creditCardNumber.StartsWith("55"))
            {
                return ValidationResult.Success;
            }
            else if (creditCardType == "Visa" && creditCardNumber.Length == 16 && creditCardNumber.StartsWith("4"))
            {
                return ValidationResult.Success;
            }
            else if (creditCardType == "American Express" && creditCardNumber.Length == 15 && (creditCardNumber.StartsWith("34") || creditCardNumber.StartsWith("37")))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Invalid credit card number for the selected type");
            }
        }
    }
}
