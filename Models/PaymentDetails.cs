using System;
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
        [RegularExpression(@"^(0[1-9]|1[0-2])\/(20(1[6-9]|2[0-9]|3[0-3]))$", ErrorMessage = "Expiration date should be in MM/YYYY format and between 01/2016 and 12/2033")]
        [ExpirationDateValidation]
        public string ExpirationDate { get; set; }

        [Required]
        [Display(Name = "CVV")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV should consist of three digits")]
        public string CVV { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        [PostalCodeValidation]
        public string PostalCode { get; set; }
    }





}
