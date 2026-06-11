using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarMagazine2025_42.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}