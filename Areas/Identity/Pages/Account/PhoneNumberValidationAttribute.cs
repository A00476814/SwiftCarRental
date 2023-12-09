using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
            if (countryValue == "United States" && !IsValidUSPhoneNumber(value as string))
            {
                return new ValidationResult(ErrorMessage ?? $"Invalid phone number for {countryValue}. The format should be XXX-XXX-XXXX.");
            }

            // For example, let's say you want to require a valid format for Canadian numbers
            if (countryValue == "Canada" && !IsValidCanadianPhoneNumber(value as string))
            {
                return new ValidationResult(ErrorMessage ?? $"Invalid phone number for {countryValue}. This is not a valid Canadian Phone number ");
            }

            return ValidationResult.Success;
        }

        // Custom validation logic for US phone numbers
        private bool IsValidUSPhoneNumber(string phoneNumber)
        {
            // USA phone number format: XXX-XXX-XXXX
            var usaPhoneNumberRegex = @"^\d{10}$";

            if (!System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, usaPhoneNumberRegex))
            {
                return false;
            }

            // Extract the first three digits (area code)
            var areaCode = phoneNumber.Substring(0, 3);

            // List of valid Canadian area codes
            var validAreaCodes = new string[] { "251", "256", "334", "907", "480", "520", "602", "623", "928", "479", "501", "870", "209", "213", "310", "323", "408", "415", "424", "510", "530", "559", "562", "619", "626", "650", "661", "707", "714", "760", "805", "818", "831", "858", "909", "916", "925", "949", "951", "303", "719", "720", "970", "203", "860", "302", "202", "689", "239", "305", "321", "352", "386", "407", "561", "727", "754", "772", "786", "813", "850", "863", "904", "941", "954", "762", "229", "404", "478", "678", "706", "770", "912", "808", "208", "730", "779", "217", "224", "309", "312", "447", "618", "630", "708", "773", "815", "847", "219", "260", "317", "574", "765", "812", "319", "515", "563", "641", "712", "316", "620", "785", "913", "270", "502", "606", "859", "225", "318", "337", "504", "985", "207", "240", "301", "410", "443", "339", "351", "413", "508", "617", "774", "781", "857", "978", "231", "248", "269", "313", "517", "586", "616", "734", "810", "906", "947", "989", "218", "320", "507", "612", "651", "763", "952", "228", "601", "662", "769", "314", "417", "573", "636", "660", "816", "406", "308", "402", "702", "775", "603", "201", "551", "609", "732", "848", "856", "862", "908", "973", "505", "575", "212", "315", "347", "516", "518", "585", "607", "631", "646", "716", "718", "845", "914", "917", "252", "336", "704", "828", "910", "919", "980", "701", "216", "234", "330", "419", "440", "513", "567", "614", "740", "937", "405", "580", "918", "503", "541", "971", "215", "267", "412", "484", "570", "610", "717", "724", "814", "878", "401", "803", "843", "864", "605", "423", "615", "731", "865", "901", "931", "210", "214", "254", "281", "325", "361", "409", "430", "432", "469", "512", "682", "713", "806", "817", "830", "832", "903", "915", "936", "940", "956", "972", "979", "435", "801", "802", "276", "434", "540", "571", "703", "757", "804", "206", "253", "360", "425", "509", "681", "304", "262", "414", "608", "715", "920", "307" }
;

            return validAreaCodes.Contains(areaCode);
        }

        // Custom validation logic for Canadian phone numbers
        private bool IsValidCanadianPhoneNumber(string phoneNumber)
        {
            // Canadian phone number format: XXX-XXX-XXXX
            var canadianPhoneNumberRegex = @"^\d{10}$";

            if (!System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, canadianPhoneNumberRegex))
            {
                return false;
            }

            // Extract the first three digits (area code)
            var areaCode = phoneNumber.Substring(0, 3);
            
            // List of valid Canadian area codes
            var validAreaCodes = new string[] { "403", "604", "204", "902", "416", "613", "418", "514", "306" };

            return validAreaCodes.Contains(areaCode);
        }
    }
}
//251, 256, 334, 907, 480, 520, 602, 623, 928, 479, 501, 870, 209, 213, 310, 323, 408, 415, 424, 510, 530, 559, 562, 619, 626, 650, 661, 707, 714, 760, 805, 818, 831, 858, 909, 916, 925, 949, 951, 303, 719, 720, 970, 203, 860, 302, 202, 689, 239, 305, 321, 352, 386, 407, 561, 727, 754, 772, 786, 813, 850, 863, 904, 941, 954, 762, 229, 404, 478, 678, 706, 770, 912, 808, 208, 730, 779, 217, 224, 309, 312, 447, 618, 630, 708, 773, 815, 847, 219, 260, 317, 574, 765, 812, 319, 515, 563, 641, 712, 316, 620, 785, 913, 270, 502, 606, 859, 225, 318, 337, 504, 985, 207, 240, 301, 410, 443, 339, 351, 413, 508, 617, 774, 781, 857, 978, 231, 248, 269, 313, 517, 586, 616, 734, 810, 906, 947, 989, 218, 320, 507, 612, 651, 763, 952, 228, 601, 662, 769, 314, 417, 573, 636, 660, 816, 406, 308, 402, 702, 775, 603, 201, 551, 609, 732, 848, 856, 862, 908, 973, 505, 575, 212, 315, 347, 516, 518, 585, 607, 631, 646, 716, 718, 845, 914, 917, 252, 336, 704, 828, 910, 919, 980, 701, 216, 234, 330, 419, 440, 513, 567, 614, 740, 937, 405, 580, 918, 503, 541, 971, 215, 267, 412, 484, 570, 610, 717, 724, 814, 878, 401, 803, 843, 864, 605, 423, 615, 731, 865, 901, 931, 210, 214, 254, 281, 325, 361, 409, 430, 432, 469, 512, 682, 713, 806, 817, 830, 832, 903, 915, 936, 940, 956, 972, 979, 435, 801, 802, 276, 434, 540, 571, 703, 757, 804, 206, 253, 360, 425, 509, 681, 304, 262, 414, 608, 715, 920, 307
