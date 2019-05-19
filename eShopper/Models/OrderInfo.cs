using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace eShopper.Models
{
    public class OrderInfo
    {
        [Display(Name = "Title")]
        [Required( AllowEmptyStrings = false , ErrorMessage = "Title can not be blank!")]
        public string Title { get; set; }

        [Display(Name = "Firstname")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name can not be blank!")]
        public string Firstname { get; set; }

        [Display(Name = "Lastname")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name can not be blank!")]
        public string LastName { get; set; }

        [Display(Name = "Phone")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone can not be blank!")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email can not be blank!")]
        public string Email { get; set; }

        [Display(Name = "Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Address can not be blank!")]
        public string Address { get; set; }

        public string All
        {
            get
            {
                return String.Concat(Title, ", ", Firstname, " ", LastName, ", ", Phone, ", ", Email, ", ", Address);
            }
        }
    }
}