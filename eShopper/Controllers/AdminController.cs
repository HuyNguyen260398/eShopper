using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eShopper.Models;

namespace eShopper.Controllers
{
    public class AdminController : Controller
    {
        eShopperEntities db = new eShopperEntities();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Admin admin)
        {
            if (ModelState.IsValid)
            {

                var checkUser = db.Users.SingleOrDefault(u => u.User_Username == admin.Admin_Username);

                if (checkUser != null)
                {
                    if (admin.Admin_Password.Equals(checkUser.User_Password))
                    {
                        Session["A_ID"] = checkUser.User_ID.ToString();
                        Session["A_Username"] = checkUser.User_Username.ToString();
                        Session["Role"] = checkUser.User_Role.ToString();

                        return RedirectToAction("IndexAdmin", "Index");
                    }
                    else
                    {
                        ModelState.AddModelError("PasswordError", "Password is not correct!");
                        return View(admin);
                    }
                }
                else
                {
                    ModelState.AddModelError("EmailError", "Username is not correct!");
                    return View(admin);
                }
            }
            else
            {
                return View(admin);
            }
        }

        public ActionResult Logout()
        {
            Session["A_ID"] = null;
            Session["A_Username"] = null;
            Session["Role"] = null;

            return RedirectToAction("Login");
        }
    }
}