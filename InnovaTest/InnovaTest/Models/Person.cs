using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Web.Mvc;

namespace InnovaTest.Core.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [DisplayName("Profile Picture")]
        public string? ImagePath { get; set; }

        // This property will be used for the file upload form
        // but it will not be saved in the database.
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }

}
