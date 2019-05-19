using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace eShopper.Models
{
    [MetadataType(typeof(ProductMetadata))]

    public partial class Product
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }

    public class ProductMetadata
    {
        [Display(Name = "Upload Image")]
        public string Product_Image { get; set; }
    }
}