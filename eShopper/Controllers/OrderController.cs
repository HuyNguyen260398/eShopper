using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eShopper.Models;
using PagedList;

namespace eShopper.Controllers
{
    public class OrderController : Controller
    {
        eShopperEntities db = new eShopperEntities();
        // GET: Order
        public ActionResult Index(int? page_no, string name, DateTime? date, string ddl1, string ddl2, string ddl3)
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
            
            var orders = db.Orders.AsQueryable();

            if (!String.IsNullOrEmpty(name))
            {
                orders = orders.Where(o => o.User.User_Lastname.Contains(name));
            }

            if (!String.IsNullOrEmpty(date.ToString()))
            {
                foreach (var item in orders)
                {
                    if (item.Order_Date.Value.Date == date)
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

        public ActionResult DeliverOrder(int id)
        {
            var order = db.Orders.SingleOrDefault(o => o.Order_ID == id);

            order.Order_Status = "Delivered";
            db.SaveChanges();

            return RedirectToAction("OrderDetails", new { id = order.Order_ID });

        }

        public ActionResult Customer(int? page_no, string name, string username)
        {
            var customers = db.Users.Where(c => c.User_Role == true).AsQueryable();

            if (!String.IsNullOrEmpty(name))
            {
                customers = customers.Where(c => c.User_Lastname.Contains(name));
            }

            if (!String.IsNullOrEmpty(username))
            {
                customers = customers.Where(c => c.User_Username.Contains(username));
            }

            return View(customers.OrderBy(c => c.User_Lastname).ToPagedList(page_no ?? 1, 5));
        }

        public ActionResult Activities(int? page_no, int id)
        {
            var activities = db.Orders.Where(a => a.Order_Customer == id);

            ViewBag.Customer = db.Users.SingleOrDefault(u => u.User_ID == id).User_Lastname;

            return View(activities.OrderByDescending(a => a.Order_Date).ToPagedList(page_no ?? 1, 5));
        }

        public ActionResult CustomerOrderDetails(int id)
        {
            var order = db.Orders.SingleOrDefault(o => o.Order_ID == id);

            var orderDetails = db.Order_Detail.Where(d => d.Order_ID == id).ToList();
            ViewBag.Details = orderDetails;

            return View(order);
        }
    }
}