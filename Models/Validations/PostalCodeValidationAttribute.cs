using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SwiftCarRental.Areas.Identity.Data;
using SwiftCarRental.Data;
using SwiftCarRental.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SwiftCarRental.Models
{
    public class PostalCodeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var paymentDetails = (PaymentDetails)validationContext.ObjectInstance;
            var postalCode = paymentDetails.PostalCode;

            var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
            var userDbContext = (userDBContext)validationContext.GetService(typeof(userDBContext));

            var userEmail = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var currentUser = userDbContext.Users.FirstOrDefault(x => x.Email == userEmail);
            var country = currentUser.Country;

            if ((country == "Canada"  || country == "CAN") && !IsValidCanadianPostalCode(postalCode))
            {
                return new ValidationResult("Invalid Canadian postal code");
            }
            else if ((country == "USA" || country == "US" || country == "United States") && !IsValidUSZipCode(postalCode))
            {
                return new ValidationResult("Invalid US zip code");
            }
            else
            {
                return ValidationResult.Success;
            }
        }

        private bool IsValidCanadianPostalCode(string postalCode)
        {
            // Canadian postal codes are in the format A1A 1A1, where A is a letter and 1 is a digit.
            var regex = new System.Text.RegularExpressions.Regex(@"^[A-Za-z]\d[A-Za-z] ?\d[A-Za-z]\d$");
            return regex.IsMatch(postalCode);
        }

        private bool IsValidUSZipCode(string postalCode)
        {
            // US zip codes are in the format 12345 or 12345-1234.

            if (postalCode.Length != 5) { 
                if(postalCode.Length != 10)return false;
            }
            var regex = new System.Text.RegularExpressions.Regex(@"^\d{5}(-\d{4})?$");
            return regex.IsMatch(postalCode);
        }
    }
}
