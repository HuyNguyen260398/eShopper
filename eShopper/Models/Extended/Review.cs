using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eShopper.Models
{
    [MetadataType(typeof(ReviewMetadata))]

    public partial class Review
    {

    }

    public class ReviewMetadata
    {
        [Display(Name = "Reviewer's Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your name!")]
        public string Review_Name { get; set; }

        [Display(Name = "Reviewer's Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email!")]
        public string Review_Email { get; set; }

        [Display(Name = "Reviewer's comment")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please leave your comment here!")]
        public string Review_Detail { get; set; }
    }
}