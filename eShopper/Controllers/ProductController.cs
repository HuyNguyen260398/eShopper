using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eShopper.Models;
using PagedList;

namespace eShopper.Controllers
{
    public class ProductController : Controller
    {
        eShopperEntities db = new eShopperEntities();
        // GET: Product
        public ActionResult Index(int? page_no, string search, int? category, int? brand)
        {
            var categoryList = db.Categories.ToList();
            ViewBag.Categories = categoryList;

            var brandList = db.Brands.ToList();
            ViewBag.Brands = brandList;

            var products = db.Products.AsQueryable();

            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Product_Name.Contains(search));
            }

            if(category != null)
            {
                products = products.Where(p => p.Product_Category == category);
            }

            if (brand != null)
            {
                products = products.Where(p => p.Product_Brand == brand);
            }

            return View(products.OrderBy(p => p.Product_Name).ToPagedList(page_no ?? 1, 9));
        }

        public ActionResult Details(int id)
        {
            var categoryList = db.Categories.ToList();
            ViewBag.Categories = categoryList;

            var brandList = db.Brands.ToList();
            ViewBag.Brands = brandList;

            var reviews = db.Reviews.Where(r => r.Review_Product == id).ToList();
            ViewBag.Total = reviews.Count();

            var product = db.Products.SingleOrDefault(p => p.Product_ID == id);
            Session["P_ID"] = id;

            return View(product);
        }
        
        public ActionResult Reviews(int p_id)
        {
            var reviews = db.Reviews.Where(r => r.Review_Product == p_id).ToList();
            return PartialView(reviews);
        }

        [HttpGet]
        public ActionResult AddReview()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReview(Review review)
        {
            if (ModelState.IsValid)
            {
                Review new_review = new Review();
                new_review.Review_Name = review.Review_Name;
                new_review.Review_Email = review.Review_Email;
                new_review.Review_Detail = review.Review_Detail;
                new_review.Review_Datetime = DateTime.Now;
                new_review.Review_Product = Convert.ToInt32(Session["P_ID"]);
                db.Reviews.Add(new_review);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = Convert.ToInt32(Session["P_ID"]) });
        }
    }
}