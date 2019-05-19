using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;   

namespace eShopper.Models
{
    public class Cart
    {
        eShopperEntities db = new eShopperEntities();
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public double ProductPrice { get; set; }
        public string ProductCategory { get; set; }
        public string ProductBrand { get; set; }
        public string ProductCondition { get; set; }
        public string ProductAvailability { get; set; }
        public int Quantity { get; set; }
        public double Subtotal {
            get { return Quantity * ProductPrice; }
        }
        public Cart(int ProID, int quantity)
        {
            ProductId = ProID;
            Product product = db.Products.Single(p => p.Product_ID == ProductId);
            ProductName = product.Product_Name;
            ProductImage = product.Product_Image;
            ProductCategory = product.Category.Category_Name;
            ProductBrand = product.Brand.Brand_Name;
            ProductCondition = product.Product_Condition;
            ProductAvailability = product.Product_Availability;
            ProductPrice = double.Parse(product.Product_Price.ToString());
            Quantity = quantity;

        }
    }
}