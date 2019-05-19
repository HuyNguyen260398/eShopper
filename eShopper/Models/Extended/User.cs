using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eShopper.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }

    public class UserMetadata
    {
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required!")]
        public string User_Email { get; set; }

        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required!")]
        public string User_Username { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
        [MinLength(10, ErrorMessage = "Minimum 10 characters are required!")]
        public string User_Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("User_Password", ErrorMessage = "Passwords are not match!")]
        public string ConfirmPassword { get; set; }
    }
}