using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eShopper.Models;

namespace eShopper.Controllers
{
    public class CartController : Controller
    {
        eShopperEntities db = new eShopperEntities();

        #region Create Cart

        // Get Cart
        public List<Cart> getCart()
        {
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart == null)
            {
                listCart = new List<Cart>();
                Session["Cart"] = listCart;
            }
            return listCart;
        }

        // Add Cart
        public ActionResult AddCart(int id, string strURL, FormCollection f)
        {
            int quantity;

            if (f["txtQuantity"] != null)
            {
                quantity = int.Parse(f["txtQuantity"].ToString());
            }
            else
            {
                quantity = 1;
            }

            Product product = db.Products.SingleOrDefault(p => p.Product_ID == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Cart> listCart = getCart();
            Cart item = listCart.Find(c => c.ProductId == id);
            if (item == null)
            {
                item = new Cart(id, quantity);
                listCart.Add(item);
                return Redirect(strURL);
            }
            else
            {
                item.Quantity += quantity;
                return Redirect(strURL);
            }
        }

        // Update Cart
        public ActionResult UpdateCart(int id, FormCollection f)
        {
            Product product = db.Products.SingleOrDefault(p => p.Product_ID == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Cart> listCart = getCart();
            Cart item = listCart.SingleOrDefault(i => i.ProductId == id);
            if (item != null)
            {
                item.Quantity = int.Parse(f["txtQuantity"].ToString());
            }
            return RedirectToAction("Cart");
        }

        // Delete Cart
        public ActionResult DeleteCart(int id)
        {
            Product product = db.Products.SingleOrDefault(p => p.Product_ID == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Cart> listCart = getCart();
            Cart item = listCart.SingleOrDefault(i => i.ProductId == id);
            if (item != null)
            {
                listCart.RemoveAll(c => c.ProductId == id);
            }
            return RedirectToAction("Cart");
        }

        // Cart Action
        public ActionResult Cart()
        {
            List<Cart> listCart = getCart();
            Session["Payment"] = TotalMoney();
            return View(listCart);
        }

        // Get Total Amount
        private int TotalAmount()
        {
            int totalAmount = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart != null)
            {
                totalAmount = listCart.Sum(q => q.Quantity);
            }
            return totalAmount;
        }

        // Get Total Money
        private double TotalMoney()
        {
            double totalMoney = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart != null)
            {
                totalMoney = listCart.Sum(q => q.Subtotal);
            }
            return totalMoney;
        }

        // Create Cart Partial
        public ActionResult CartPartial()
        {
            if (TotalAmount() == 0)
            {
                return PartialView();
            }
            ViewBag.TotalAmount = TotalAmount();
            ViewBag.TotalMoney = TotalMoney();
            return PartialView();
        }

        // Update Quantity
        public ActionResult UpdateQuantity()
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Cart> listCart = getCart();
            return View(listCart);
        }

        #endregion

        #region CheckOut

        [HttpGet]
        public ActionResult CheckOut()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Payment Method", Value = "refresh" });
            list.Add(new SelectListItem { Text = "Cash", Value = "Cash" });
            list.Add(new SelectListItem { Text = "Transfer", Value = "Transfer" });
            list.Add(new SelectListItem { Text = "Paypal", Value = "Paypal" });

            ViewBag.Method = list;

            List<Cart> listCart = getCart();

            if (Session["C_ID"] != null)
            {
                int id = Convert.ToInt32(Session["C_ID"]);
                var cus = db.Users.SingleOrDefault(c => c.User_ID == id);
                ViewBag.Customer = cus;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Customer");
            }
        }

        [HttpPost]
        public ActionResult Checkout(OrderInfo info, string method)
        {
            string message = "";

            int id = Convert.ToInt32(Session["C_ID"]);
            var cus = db.Users.SingleOrDefault(c => c.User_ID == id);
            ViewBag.Customer = cus;

            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Payment Method", Value = "refresh" });
            list.Add(new SelectListItem { Text = "Cash", Value = "Cash" });
            list.Add(new SelectListItem { Text = "Transfer", Value = "Transfer" });
            list.Add(new SelectListItem { Text = "Paypal", Value = "Paypal" });

            ViewBag.Method = list;

            List<Cart> listCart = getCart();
            ViewBag.Cart = listCart;

            Order order = new Order();

            if (ModelState.IsValid)
            {
                if(method == "refresh")
                {
                    message = "Please select a payment method";
                    ViewBag.Message1 = message;
                    return View(info);
                }

                if (method == "Cash")
                {
                    order.Payment_Status = "Waiting";
                }
                else
                {
                    order.Payment_Status = "Paid";
                }

                order.Order_Date = DateTime.Now;
                order.Order_Payment = TotalMoney();
                order.Order_Status = "Pending";
                order.Payment_Method = method;
                
                order.Order_Description = info.All;
                order.Order_Customer = Convert.ToInt32(Session["C_ID"]);

                db.Orders.Add(order);
                db.SaveChanges();

                // Add Order Items
                foreach (var item in listCart)
                {
                    Order_Detail orderDetail = new Order_Detail();
                    orderDetail.Order_ID = order.Order_ID;
                    orderDetail.Product_ID = item.ProductId;
                    orderDetail.Quantity = item.Quantity;
                    db.Order_Detail.Add(orderDetail);
                }
                db.SaveChanges();

                ViewBag.Message2 = "Order successfully!";

                return View(info);
            }

            return View(info);
        }
        #endregion

        #region Compare List

        public ActionResult CompareList()
        {
            List<Cart> compareList = getCompareList();
            return View(compareList);
        }

        public List<Cart> getCompareList()
        {
            List<Cart> compareList = Session["CompareList"] as List<Cart>;
            if (compareList == null)
            {
                compareList = new List<Cart>();
                Session["CompareList"] = compareList;
            }
            return compareList;
        }

        public ActionResult AddCompareList(int id, string strURL)
        {
            int quantity = 1;

            Product product = db.Products.SingleOrDefault(p => p.Product_ID == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Cart> compareList = getCompareList();
            Cart item = compareList.Find(c => c.ProductId == id);
            if (item == null)
            {
                item = new Cart(id, quantity);
                compareList.Add(item);
                return Redirect(strURL);
            }
            else
            {
                return Redirect(strURL);
            }
        }

        public ActionResult RemoveCompareList(int id)
        {
            Product product = db.Products.SingleOrDefault(p => p.Product_ID == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Cart> compareList = getCompareList();
            Cart item = compareList.SingleOrDefault(i => i.ProductId == id);
            if (item != null)
            {
                compareList.RemoveAll(c => c.ProductId == id);
            }
            return RedirectToAction("CompareList");
        }

        public ActionResult CompareListPartial()
        {
            int totalAmount = 0;
            List<Cart> compareList = Session["CompareList"] as List<Cart>;

            if (compareList != null)
            {
                totalAmount = compareList.Sum(q => q.Quantity);
            }

            if (totalAmount == 0)
            {
                return PartialView();
            }

            ViewBag.TotalAmount = totalAmount;
            return PartialView();
        }

        #endregion

        #region Wish List

        public ActionResult WishList()
        {
            var categoryList = db.Categories.ToList();
            ViewBag.Categories = categoryList;

            var brandList = db.Brands.ToList();
            ViewBag.Brands = brandList;

            List<Cart> wishList = getWishList();
            return View(wishList);
        }

        public List<Cart> getWishList()
        {
            List<Cart> wishList = Session["WishList"] as List<Cart>;
            if (wishList == null)
            {
                wishList = new List<Cart>();
                Session["WishList"] = wishList;
            }
            return wishList;
        }

        public ActionResult AddWishList(int id, string strURL)
        {
            int quantity = 1;

            Product product = db.Products.SingleOrDefault(p => p.Product_ID == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Cart> wishList = getWishList();
            Cart item = wishList.Find(c => c.ProductId == id);
            if (item == null)
            {
                item = new Cart(id, quantity);
                wishList.Add(item);
                return Redirect(strURL);
            }
            else
            {
                return Redirect(strURL);
            }
        }

        public ActionResult RemoveWishList(int id)
        {
            Product product = db.Products.SingleOrDefault(p => p.Product_ID == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Cart> wishList = getWishList();
            Cart item = wishList.SingleOrDefault(i => i.ProductId == id);
            if (item != null)
            {
                wishList.RemoveAll(c => c.ProductId == id);
            }
            return RedirectToAction("WishList");
        }

        public ActionResult WishListPartial()
        {
            int totalAmount = 0;
            List<Cart> wishList = Session["WishList"] as List<Cart>;

            if (wishList != null)
            {
                totalAmount = wishList.Sum(q => q.Quantity);
            }

            if (totalAmount == 0)
            {
                return PartialView();
            }

            ViewBag.TotalAmount = totalAmount;
            return PartialView();
        }

        #endregion
    }
}