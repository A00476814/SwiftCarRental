﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SwiftCarRental.Models
{
    public class PaymentDetails
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        [Display(Name = "Credit Card Type")]
        public string CreditCardType { get; set; }

        [Required]
        [Display(Name = "Credit Card Number")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Credit card number should consist of digits only")]
        [CreditCardNumberValidation]
        public string CreditCardNumber { get; set; }

        [Required]
        [Display(Name = "Expiration Date")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/(20(1[6-9]|[2-3][0-3]))$", ErrorMessage = "Expiration date should be in MM/YYYY format and between 01/2016 and 12/2033")]
        [ExpirationDateValidation]
        public string ExpirationDate { get; set; }

        [Required]
        [Display(Name = "CVV")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV should consist of three digits")]
        public string CVV { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
    }


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
