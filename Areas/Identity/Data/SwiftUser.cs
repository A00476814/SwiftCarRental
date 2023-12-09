using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SwiftCarRental.Areas.Identity.Data;

// Add profile data for application users by adding properties to the SwiftUser class
public class SwiftUser : IdentityUser
{

    public string FirstName { get;set;}
    public string LastName { get;set;}
    public string LicenceNo { get;set;}
    public DateTime DateOfBirth { get;set;} 
    public string Country { get;set;}

    public string Phone { get;set;}
}

