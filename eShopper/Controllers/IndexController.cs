using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eShopper.Models;
using PagedList;

namespace eShopper.Controllers
{
    public class IndexController : Controller
    {
        eShopperEntities db = new eShopperEntities();
        // GET: Index
        public ActionResult IndexCustomer(int? page_no)
        {
            var productList = db.Products.ToList();

            var categoryList = db.Categories.ToList();
            ViewBag.Categories = categoryList;
            Session["Shop"] = categoryList;

            var brandList = db.Brands.ToList();
            ViewBag.Brands = brandList;

            return View(productList.OrderBy(p => p.Product_Name).ToPagedList(page_no ?? 1, 9));
        }

        public ActionResult IndexAdmin()
        {
            ViewBag.BCount = db.Brands.ToList().Count();
            ViewBag.CCount = db.Categories.ToList().Count();
            ViewBag.PCount = db.Products.ToList().Count();
            ViewBag.UCount = db.Users.Where(u => u.User_Role == true).ToList().Count();
            ViewBag.OCount = db.Orders.ToList().Count();
            ViewBag.TotalRevenue = String.Format("{0:0,0}", db.Orders.Sum(o => o.Order_Payment));

            return View();
        }
    }
}