using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eShopper.Models;

namespace eShopper.Controllers
{
    public class ReportController : Controller
    {
        eShopperEntities db = new eShopperEntities();
        // GET: Report
        public ActionResult Index()
        {
            List<Order> o_list = new List<Order>();
            List<int> values = new List<int>();
            var orders = db.Orders.AsQueryable();

            for (int i = 1; i < 13; i++)
            {
                foreach (var item in orders)
                {
                    if (item.Order_Date.Value.Month == i)
                    {
                        o_list.Add(item);
                    }
                }
                values.Add(Convert.ToInt32(o_list.Sum(o => o.Order_Payment)));
            }

            ViewBag.Values = values.ToList();

            return View();
        }

        public ActionResult MonthReport(string month)
        {
            var items = db.vOrderDetails.AsQueryable();
            var products = db.Products.AsQueryable();
            List<vOrderDetail> item_list = new List<vOrderDetail>();
            List<double> values_sales = new List<double>();
            List<double> values_revenues = new List<double>();
            List<string> labels = new List<string>();

            List<SelectListItem> months = new List<SelectListItem>();
            months.Add(new SelectListItem { Text = "January", Value = "1" });
            months.Add(new SelectListItem { Text = "February", Value = "2" });
            months.Add(new SelectListItem { Text = "March", Value = "3" });
            months.Add(new SelectListItem { Text = "April", Value = "4" });
            months.Add(new SelectListItem { Text = "May", Value = "5" });
            months.Add(new SelectListItem { Text = "June", Value = "6" });
            months.Add(new SelectListItem { Text = "July", Value = "7" });
            months.Add(new SelectListItem { Text = "August", Value = "8" });
            months.Add(new SelectListItem { Text = "September", Value = "9" });
            months.Add(new SelectListItem { Text = "October", Value = "10" });
            months.Add(new SelectListItem { Text = "November", Value = "11" });
            months.Add(new SelectListItem { Text = "December", Value = "12" });

            if (month == null)
            {
                month = "1";
            }

            foreach (var item in items)
            {
                if (item.Order_Date.Value.Month == Convert.ToInt32(month))
                {
                    item_list.Add(item);
                }
            }

            foreach (var p in products)
            {
                var count = item_list.Where(a => a.Product_ID == p.Product_ID);

                values_sales.Add(Convert.ToDouble(count.Sum(a => a.Quantity)));
                values_revenues.Add(Convert.ToDouble(count.Sum(a => a.Quantity) * p.Product_Price));
            }

            foreach (var item in products)
            {
                labels.Add(item.Product_Name);
            }

            ViewBag.Months = months;
            ViewBag.ValuesSales = values_sales.ToList();
            ViewBag.ValuesRevenues = values_revenues.ToList();
            ViewBag.Labels = labels.ToList();

            return View();
        }

        public ActionResult DateReport(DateTime? from, DateTime? to)
        {
            var items = db.vOrderDetails.AsQueryable();
            var products = db.Products.AsQueryable();
            List<double> values_sales = new List<double>();
            List<double> values_revenues = new List<double>();
            List<string> labels = new List<string>();

            if (from != null && to != null)
            {
                items = items.Where(a => a.Order_Date >= from && a.Order_Date <= to);
            }
            
            foreach (var p in products)
            {
                var count = items.Where(a => a.Product_ID == p.Product_ID);

                values_sales.Add(Convert.ToDouble(count.Sum(a => a.Quantity)));
                values_revenues.Add(Convert.ToDouble(count.Sum(a => a.Quantity) * p.Product_Price));
            }

            foreach (var item in products)
            {
                labels.Add(item.Product_Name);
            }
            
            ViewBag.ValuesSales = values_sales.ToList();
            ViewBag.ValuesRevenues = values_revenues.ToList();
            ViewBag.Labels = labels.ToList();

            return View();
        }

        public ActionResult YearReport(string year)
        {
            var items = db.vOrderDetails.AsQueryable();
            var products = db.Products.AsQueryable();
            List<vOrderDetail> item_list = new List<vOrderDetail>();
            List<double> values_sales = new List<double>();
            List<double> values_revenues = new List<double>();
            List<string> labels = new List<string>();

            List<SelectListItem> years = new List<SelectListItem>();
            years.Add(new SelectListItem { Text = "2018", Value = "2018" });
            years.Add(new SelectListItem { Text = "2019", Value = "2019" });
            years.Add(new SelectListItem { Text = "2020", Value = "2020" });

            if (year == null)
            {
                year = "2019";
            }

            foreach (var item in items)
            {
                if (item.Order_Date.Value.Year == Convert.ToInt32(year))
                {
                    item_list.Add(item);
                }
            }

            foreach (var p in products)
            {
                var count = item_list.Where(a => a.Product_ID == p.Product_ID);

                values_sales.Add(Convert.ToDouble(count.Sum(a => a.Quantity)));
                values_revenues.Add(Convert.ToDouble(count.Sum(a => a.Quantity) * p.Product_Price));
            }

            foreach (var item in products)
            {
                labels.Add(item.Product_Name);
            }

            ViewBag.Years = years;
            ViewBag.ValuesSales = values_sales.ToList();
            ViewBag.ValuesRevenues = values_revenues.ToList();
            ViewBag.Labels = labels.ToList();

            return View();
        }
    }
}