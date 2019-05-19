using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eShopper.Models
{
    public class Admin
    {
        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username Required!")]
        public string Admin_Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Admin_Password { get; set; }
    }
}