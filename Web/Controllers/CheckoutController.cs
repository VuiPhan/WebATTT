using Web.Models.Data;
using Web.Models.DB;
using Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class CheckoutController : Controller
    {
        ShopBanDoTheThao db = new ShopBanDoTheThao();

        // GET: Checkout
        public ActionResult Index()
        {
            var model = new CheckoutVM();
            List<Cart> list = Session["GioHang"] as List<Cart>;
            List<CartVM> c = new List<CartVM>();

            if (list != null)
            {
                foreach (var item in list)
                {
                    CartVM cartVM = new CartVM();
                    var Product = db.Products.SingleOrDefault(x => x.IDProduct == item.IDProduct);
                    cartVM.IDProduct = item.IDProduct;
                    cartVM.Image0 = Product.Image0;
                    cartVM.NameProduct = Product.NameProduct;
                    cartVM.Amount = item.Amount;
                    cartVM.Price = item.Price;
                    cartVM.TotalMoney = item.Price * item.Amount;
                    c.Add(cartVM);
                }
            }
            model.carts = c;
            ViewBag.Total = Total();

            return View(model);
        }
        public decimal? Total()
        {
            List<Cart> list = Session["GioHang"] as List<Cart>;
            if (list == null)
            {
                return 0;
            }
            decimal? TongTien = 0;
            foreach (var item in list)
            {
                TongTien += item.Price * item.Amount;
            }
            return TongTien;
        }

        public ActionResult _BillDetails()
        {
            Member model = new Member();
            Member x = (Member)Session["Account"];

            model.IDMember = x.IDMember;
            model.UserName = x.UserName;
            model.PassWord = x.PassWord;
            model.FullName = x.FullName;

            if (x.Email != null)
            {
                model.Email = x.Email;
                model.Address = x.Address;
                model.PhoneNumber = x.PhoneNumber;
            }

            return PartialView(model);
        }

        public ActionResult BillDetails(Member model)
        {
            var khachHang = db.Members.SingleOrDefault(x => x.IDMember==model.IDMember);
            try
            {
                khachHang.UserName = model.UserName;
                khachHang.PassWord = model.PassWord;
                khachHang.Email = model.Email;
                khachHang.FullName = model.FullName;
                khachHang.Address = model.Address;
                khachHang.PhoneNumber = model.PhoneNumber;

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            Session["Account"] = (from x in db.Members
                                  where x.IDMember == model.IDMember
                                  select new Member
                                  {
                                      IDMember = x.IDMember,
                                      UserName = x.UserName,
                                      PassWord = x.PassWord,
                                      FullName = x.FullName,
                                      Address = x.Address,
                                      Email = x.Email,
                                      PhoneNumber = x.PhoneNumber,
                                      Avatar =x.Avatar
                                  });

            return RedirectToAction("Index");
        }

        public ActionResult Confirmation(int id)
        {
            List<CartVM> cartVMs = new List<CartVM>();
            var model = new OrderVM();
            var hoaDon = db.Orders.Where(x=>x.IDOrder== id).FirstOrDefault();

            var list = (from x in db.DetailOrders
                        where x.IDOrder == id
                        select new
                        {
                            x.IDOrderDetail,
                            x.IDOrder,
                            x.IDProduct,
                            x.NameProduct,
                            x.Price,
                            x.Amount
                        });
            var khachHang = db.Members.SingleOrDefault(x => x.IDMember == hoaDon.IDMember);
            
            model.IDOrder = id;

            model.FullName = khachHang.FullName;
            model.Email = khachHang.Email;
            model.Address = khachHang.Address;
            model.PhoneNumber = khachHang.PhoneNumber;
            model.OrderedDate = hoaDon.OrderedDate;
            model.TotalMoney = hoaDon.TotalMoney;
            
            foreach(var item in list)
            {
                var product = db.Products.SingleOrDefault(x => x.IDProduct == item.IDProduct);
                CartVM c = new CartVM();
                c.IDProduct = product.IDProduct;
                c.Amount = item.Amount;
                c.Price = item.Price;
                c.NameProduct = product.NameProduct;
                c.Image0 = product.Image0;
                c.TotalMoney = item.Price * item.Amount;
                cartVMs.Add(c);
            }
            model.carts = cartVMs;

            return View(model);
        }

        public ActionResult _Confirmation(int id)
        {
            var order = db.Orders.SingleOrDefault(x => x.IDOrder == id);
            order.Status = 1;
            db.SaveChanges();
            return RedirectToAction("Confirmation", new { id = id });
        }

        public ActionResult Checkout(int? id)
        {            
            var hoaDon = new Order();
            if(id != null)
            {
                var khachHang = db.Members.SingleOrDefault(x => x.IDMember == id);
                hoaDon.IDMember = khachHang.IDMember;
                hoaDon.TotalMoney = Total();
                hoaDon.OrderedDate = DateTime.Now;
                db.Orders.Add(hoaDon);
                db.SaveChanges();
            }

            var MaHD = db.Orders.Select(x => x.IDOrder).Max();
            List<Cart> list = Session["GioHang"] as List<Cart>;

            if (list != null)
            {
                foreach (var item in list)
                {
                    var chiTietHoaDon = new DetailOrder();
                    //var product = db.Products.SingleOrDefault(x=>x.i)
                    chiTietHoaDon.IDOrder = MaHD;
                    chiTietHoaDon.IDProduct = item.IDProduct;
                    chiTietHoaDon.Amount = item.Amount;
                    chiTietHoaDon.Price = item.Price;
                    db.DetailOrders.Add(chiTietHoaDon);
                    db.SaveChanges();
                }
            }
            Session["GioHang"] = null;
            return RedirectToAction("Confirmation", new {id = MaHD });
        }
    }
}