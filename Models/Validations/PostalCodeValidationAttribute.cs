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
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly userDBContext _userDbContext;

        public PostalCodeValidationAttribute(IHttpContextAccessor httpContextAccessor, userDBContext userDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _userDbContext = userDbContext;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var paymentDetails = (PaymentDetails)validationContext.ObjectInstance;
            var postalCode = paymentDetails.PostalCode;

            var userEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var currentUser = _userDbContext.Users.FirstOrDefault(x => x.Email == userEmail);
            var country = currentUser.Nation;

            if (country == "Canada" && !IsValidCanadianPostalCode(postalCode))
            {
                return new ValidationResult("Invalid Canadian postal code");
            }
            else if (country == "USA" && !IsValidUSZipCode(postalCode))
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
            var regex = new System.Text.RegularExpressions.Regex(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$");
            return regex.IsMatch(postalCode);
        }

        private bool IsValidUSZipCode(string postalCode)
        {
            // US zip codes are in the format 12345 or 12345-1234.
            var regex = new System.Text.RegularExpressions.Regex(@"^\d{5}(?:[-\s]\d{4})?$");
            return regex.IsMatch(postalCode);
        }
    }
}
