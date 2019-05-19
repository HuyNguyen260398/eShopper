using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eShopper.Models;
using PagedList;

namespace eShopper.Controllers
{
    public class CustomerController : Controller
    {
        eShopperEntities db = new eShopperEntities();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User customer)
        {
            bool Status = false;
            string message = "";

            if (ModelState.IsValid)
            {
                
                var checkUsername = db.Users.Where(u => u.User_Username.Equals(customer.User_Username)).FirstOrDefault();

                if (checkUsername != null)
                {
                    ModelState.AddModelError("UsernameExist", "Username is already exist!");
                    return View(customer);
                }

                var checkEmail = db.Users.Where(u => u.User_Email.Equals(customer.User_Email)).FirstOrDefault();

                if (checkEmail != null)
                {
                    ModelState.AddModelError("EmailExist", "Email is already exist!");
                    return View(customer);
                }

                customer.User_Role = true;

                db.Users.Add(customer);
                db.SaveChanges();

                message = "success";
                
            }
            else
            {
                message = "Invalid Request!";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;

            return View(customer);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User customer)
        {
            var checkUser = db.Users.SingleOrDefault(u => u.User_Email == customer.User_Email);

            if(checkUser != null)
            {
                if (customer.User_Password.Equals(checkUser.User_Password))
                {
                    Session["C_ID"] = checkUser.User_ID.ToString();
                    Session["C_Username"] = checkUser.User_Username.ToString();
                    Session["Role"] = checkUser.User_Role.ToString();

                    return RedirectToAction("IndexCustomer", "Index");
                }
                else
                {
                    ModelState.AddModelError("PasswordError", "Password is not correct!");
                    return View(customer);
                }
            }
            else
            {
                ModelState.AddModelError("EmailError", "Email is not correct!");
                return View(customer);
            }
        }

        public ActionResult Logout()
        {
            Session["C_ID"] = null;
            Session["C_Username"] = null;
            Session["Role"] = null;
            return RedirectToAction("login");
        }

        public ActionResult ViewAccount()
        {
            int id = Convert.ToInt32(Session["C_ID"]);
            var customer= db.Users.SingleOrDefault(u => u.User_ID == id);
            return View(customer);
        }

        [HttpGet]
        public ActionResult EditAccount()
        {
            int id = Convert.ToInt32(Session["C_ID"]);
            var customer = db.Users.SingleOrDefault(u => u.User_ID == id);
            ViewBag.Customer = customer;
            return View(customer);
        }

        [HttpPost]
        public ActionResult EditAccount(User customer)
        {
            int id = Convert.ToInt32(Session["C_ID"]);
            var c = db.Users.SingleOrDefault(a => a.User_ID == id);
            ViewBag.Customer = customer;

            if (customer.User_Firstname != null)
            {
                c.User_Firstname = customer.User_Firstname;
            }
            if (customer.User_Lastname != null)
            {
                c.User_Lastname = customer.User_Lastname;
            }
            if (customer.User_Phone != null)
            {
                c.User_Phone = customer.User_Phone;
            }
            if (customer.User_Address != null)
            {
                c.User_Address = customer.User_Address;
            }

            c.User_Password = customer.User_Password;
            c.ConfirmPassword = customer.User_Password;

            db.SaveChanges();

            ViewBag.Message = "Update info successfully!";

            return View(customer);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            int id = Convert.ToInt32(Session["C_ID"]);
            var customer = db.Users.SingleOrDefault(u => u.User_ID == id);
            ViewBag.Customer = customer;
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(User customer, string old_pass)
        {
            int id = Convert.ToInt32(Session["C_ID"]);
            var c = db.Users.SingleOrDefault(u => u.User_ID == id);
            ViewBag.Customer = c;

            if (old_pass != "")
            {
                if (string.Compare(old_pass, c.User_Password) == 0)
                {
                    if (ModelState.IsValid)
                    {
                        c.User_Password = customer.User_Password;
                        c.ConfirmPassword = customer.User_Password;

                        db.SaveChanges();

                        ViewBag.Message = "Change password successfully!";

                        return PartialView();
                    }
                }
                else
                {
                    ModelState.AddModelError("IncorrectPass", "Curent Password is incorrect!");
                    return PartialView(customer);
                }
            }
            else
            {
                ModelState.AddModelError("Blank", "Current Password Cannot Be Blank!");
                return PartialView(customer);
            }
            return PartialView();
        }

        public ActionResult OrderList(int? page_no, DateTime? date, string ddl1, string ddl2, string ddl3)
        {
            List<Order> list = new List<Order>();

            List<SelectListItem> list1 = new List<SelectListItem>();
            list1.Add(new SelectListItem { Text = "Payment Method", Value = "refresh" });
            list1.Add(new SelectListItem { Text = "Cash", Value = "Cash" });
            list1.Add(new SelectListItem { Text = "Transfer", Value = "Transfer" });
            list1.Add(new SelectListItem { Text = "Paypal", Value = "Paypal" });

            ViewBag.PM = list1;

            List<SelectListItem> list2 = new List<SelectListItem>();
            list2.Add(new SelectListItem { Text = "Payment Status", Value = "refresh" });
            list2.Add(new SelectListItem { Text = "Waiting", Value = "Waiting" });
            list2.Add(new SelectListItem { Text = "Paid", Value = "Paid" });

            ViewBag.PS = list2;

            List<SelectListItem> list3 = new List<SelectListItem>();
            list3.Add(new SelectListItem { Text = "Order Status", Value = "refresh" });
            list3.Add(new SelectListItem { Text = "Pending", Value = "Pending" });
            list3.Add(new SelectListItem { Text = "Delivered", Value = "Delivered" });
            list3.Add(new SelectListItem { Text = "Canceled", Value = "Canceled" });

            ViewBag.OS = list3;

            var id = Convert.ToInt32(Session["C_ID"]);
            var orders = db.Orders.Where(o => o.Order_Customer == id).AsQueryable();

            if(!String.IsNullOrEmpty(date.ToString()))
            {
                foreach(var item in orders)
                {
                    if(item.Order_Date.Value.Date == date)
                    {
                        list.Add(item);
                    }
                }

                orders = list.AsQueryable();
            }

            if (ddl1 != null && ddl1 != "refresh")
            {
                orders = orders.Where(o => o.Payment_Method.Equals(ddl1));
            }

            if (ddl2 != null && ddl2 != "refresh")
            {
                orders = orders.Where(o => o.Payment_Status.Equals(ddl2));
            }

            if (ddl3 != null && ddl3 != "refresh")
            {
                orders = orders.Where(o => o.Order_Status.Equals(ddl3));
            }

            return View(orders.OrderByDescending(o => o.Order_Date).ToPagedList(page_no ?? 1, 5));
        }

        public ActionResult OrderDetails(int id)
        {
            var order = db.Orders.SingleOrDefault(o => o.Order_ID == id);

            var orderDetails = db.Order_Detail.Where(d => d.Order_ID == id).ToList();
            ViewBag.Details = orderDetails;

            return View(order);
        }
        
        public ActionResult CancelOrder(int id)
        {
            var order = db.Orders.SingleOrDefault(o => o.Order_ID == id);

            order.Order_Status = "Canceled";
            db.SaveChanges();

            return RedirectToAction("OrderDetails", new { id = order.Order_ID});

        }
    }
}