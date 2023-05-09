using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebForm.Models;

namespace WebForm.Controllers
{
    public class HomeController : Controller
    {
        private finalEntities2 db = new finalEntities2();

        public ActionResult Index()
        {
            ViewBag.ListPhone = db.phones.ToList();
            
            return View();
        }

        //HTTP GET /Home/SignIn
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(string email, string password)
        {
            var user1 = email;
            var pass1 = password;
            var check = db.agents.SingleOrDefault(x => x.Email.Equals(user1) && x.Pass.Equals(pass1));

            if (check != null)
            {
                Session["agent"] = check;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.LoginFail = "Invalid Email or Password";
                return View("SignIn");
            }
        }

        public ActionResult SignOut()
        {
            Session["agent"] = null;

            return RedirectToAction("Index", "Home");
        }


        public ActionResult profile()
        {
            
            ViewBag.ListProduct = db.P_Order.ToList();
            //var pro = db.P_Order.SingleOrDefault(x => x.id ==id);
            ViewBag.Detail = db.Order_detail.ToList();

            return View();
        }

        public ActionResult update_status(FormCollection form)
        {
            Cart cart = new Cart();
            Session["Cart"] = cart;

            int id_pro = int.Parse(form["ID_Product"]);
            
            cart.update_status(id_pro, 2,"Finished");

            return RedirectToAction("profile", "Home");
        }

        public ActionResult remove_status(FormCollection form)
        {
            Cart cart = new Cart();
            Session["Cart"] = cart;

            int id_pro = int.Parse(form["ID_Product"]);

            cart.remove_status(id_pro);

            return RedirectToAction("profile", "Home");
        }
    }
}