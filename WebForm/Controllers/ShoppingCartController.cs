using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebForm.Models;

namespace WebForm.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart

        finalEntities2 db = new finalEntities2();
        Cart a= new Cart();

        public Cart getCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if(cart == null || Session["Cart"] == null) 
            {       
                cart = new Cart();
                Session["Cart"] = cart;
            }

            return cart;
        }

        

        public ActionResult AddtoCart(string id)
        {
            var pro = db.phones.SingleOrDefault(x => x.id.Equals(id));
            if(pro != null)
            {
                getCart().add(pro);
            }
            return RedirectToAction("ShowToCart", "ShoppingCart");
        }

        public ActionResult ShowToCart()
        {
            if (Session["Cart"] == null)
            {
                //return RedirectToAction("ShowToCart", "ShoppingCart");
            }

            Cart cart = Session["Cart"] as Cart;
            return View(cart);
        }

        public ActionResult update_Quantity_Cart(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            string id_pro = form["ID_Product"];
            int quantity = int.Parse(form["Quantity"]);
            cart.Update_Quantity_Shopping(id_pro, quantity);

            return RedirectToAction("ShowToCart", "ShoppingCart");
        }

        public ActionResult RemoveCart(string id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.remove_cart(id);
            return RedirectToAction("ShowToCart", "ShoppingCart");
        }
    
        public PartialViewResult BagCart()
        {
            int total_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if(cart != null)
            
                total_item = cart.Total_Quantity_in_Cart();
                ViewBag.QuantityCart = total_item;
                return PartialView("BagCart");
            
        }
    
        public ActionResult Shopping_success()
        {
            return View();
        }

   

        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                var User = Session["agent"] as WebForm.Models.agent;

                /* Cart cart = Session["Cart"] as Cart;
                 P_Order _order = new P_Order();
                 agent _agent = new agent();
                 _agent.name = form["NameAgent"];
                 _agent.Email = form["EmailAgent"];
                 _agent.Pass = "123456";
                 _agent.Repass = "123456";
                 db.agents.Add(_agent);

                 db.SaveChanges();
                */

                Cart cart = Session["Cart"] as Cart;
                P_Order _order = new P_Order();

                _order.id_agent = User.id;
                _order.date = DateTime.Now;
                _order.Address_Description = form["Address_Delivery"];
                _order.payment = form["payment"];
                _order.d_status = 0;
                _order.descrip = "Not confirm!!! Please wait";
                db.P_Order.Add(_order);


                db.SaveChanges();

                foreach (var item in cart.Items)
                {
                    Order_detail detail = new Order_detail();
                    detail.order_id = _order.id;
                    detail.id_phone = item.shopping.id;
                    detail.p_name = item.shopping.name;
                    detail.quantity = item.shop_quantity;
                    detail.price = item.shopping.price;
                    detail.total = item.shop_quantity * item.shopping.price;
                    db.Order_detail.Add(detail);
                }



                db.SaveChanges();


                //send email
                var strSanPham = "";
                var thanhtien = "";
                var tongtien = "";
                foreach(var sp in cart.Items)
                {
                    strSanPham += "<tr>";
                    strSanPham += "<td>"+ sp.shopping.name + "</td>";
                    strSanPham += "<td>" + sp.shop_quantity + "</td>";
                    strSanPham += "<td>" + sp.shop_quantity * sp.shopping.price + "</td>";
                    strSanPham += "</tr>";
                    thanhtien += sp.shop_quantity * sp.shopping.price;
                }
                tongtien = thanhtien;

                string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                contentCustomer = contentCustomer.Replace("{{MaDon}}",_order.id.ToString());
                contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", User.name);
                contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", _order.Address_Description);
                contentCustomer = contentCustomer.Replace("{{NgayDat}}", _order.date.ToString());
                contentCustomer = contentCustomer.Replace("{{TongTien}}", tongtien);
                WebForm.Common.Common.SendEmail("KNKD Shop", "Order #" + _order.id, contentCustomer.ToString(), User.Email);
                cart.ClearCart();
                return RedirectToAction("Shopping_success", "ShoppingCart");
            }
            
            catch
            {
                return Content("Error. Please check the information ...");
            }
        }

        
    }
}