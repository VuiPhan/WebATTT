using Web.Models.Data;
using Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;

namespace Web.Controllers
{
    public class CheckoutController : Controller
    {
        readonly ShopBanDoTheThao db = new ShopBanDoTheThao();

        // GET: Checkout
        public ActionResult Index()
        {
            var model = new CheckoutVM();
            List<CartVM> c = new List<CartVM>();

            if (Session["Checkout"] != null)
            {
                var y = (CheckoutVM)Session["Checkout"];
                model.IDMember = 16;
                model.FullName = y.FullName;
                model.Email = y.Email;
                model.Address = y.Address;
                model.PhoneNumber = y.PhoneNumber;
            }
            else if (Session["Account"] != null)
            {
                var y = (Member)Session["Account"];
                model.IDMember = y.IDMember;
                model.UserName = y.UserName;
                model.PassWord = y.PassWord;
                model.FullName = y.FullName;

                if (y.Email != null)
                {
                    model.Email = y.Email;
                    model.Address = y.Address;
                    model.PhoneNumber = y.PhoneNumber;
                }
            }            
                    
            if (Session["GioHang"] is List<Cart>list)
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
            if (!(Session["GioHang"] is List<Cart> list))
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
      
        public ActionResult BillDetails(CheckoutVM model)
        {            
            Session["Checkout"] = model;
            return RedirectToAction("Index");
        }

        public ActionResult Confirmation(int id)
        {
            List<CartVM> cartVMs = new List<CartVM>();
            var hoaDon = db.Orders.Where(x => x.IDOrder == id).FirstOrDefault();

            var model = new OrderVM
            {
                IDOrder = id,
                FullName = hoaDon.FullName,
                Email = hoaDon.Email,
                Address = hoaDon.Address,
                PhoneNumber = hoaDon.PhoneNumber,
                OrderedDate = hoaDon.OrderedDate,
                TotalMoney = hoaDon.TotalMoney,
                ConfirmDate = hoaDon.ConfirmDate,
                DeliveredDate = hoaDon.DeliveredDate,
                DeliveryDate = hoaDon.DeliveryDate
            };

            ///
            switch (hoaDon.Status)
            {
                case 1: model.Status = "Chưa xác nhận";
                    break;
                case 2:
                    model.Status = "Chờ lấy hàng";
                    break;
                case 3:
                    model.Status = "Đang giao";
                    break;
                case 4:
                    model.Status = "Đã giao thành công";
                    break;
                case 5:
                    model.Status = "Trả hàng/ hoàn tiền";
                    break;
                case 0:
                    model.Status = "Đã hủy";
                    break;
            }

            var list = db.DetailOrders.Where(x => x.IDOrder == id).ToList();

            foreach (var item in list)
            {
                var product = db.Products.SingleOrDefault(x => x.IDProduct == item.IDProduct);
                CartVM c = new CartVM
                {
                    IDProduct = product.IDProduct,
                    Amount = item.Amount,
                    Price = item.Price,
                    NameProduct = product.NameProduct,
                    Image0 = product.Image0,
                    TotalMoney = item.Price * item.Amount
                };

                cartVMs.Add(c);
            }
            model.carts = cartVMs;

            return View(model);
        }

        public void SendMail(int MaHoaDon, string Ten, string Email)
        {
            var senderEmail = new MailAddress("oixanhh@gmail.com", "Karma Shop");
            var receiverEmail = new MailAddress(Email, Ten);
            var password = "oixanhla123";
            var sub = "New Order from Karma Shop";
            var body = "Chào " + Ten + ", bạn vừa có một đơn hàng từ Karma Shop. " +
                "Đây là là vận đơn của bạn:  " + MaHoaDon + " truy cập vào https://localhost:44369/Checkout/Confirmation/" + MaHoaDon +
                " để theo dõi tình trạng";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
            }
            smtp.Dispose();
        }
        public ActionResult Checkout(int? id)
        {
            var y = (CheckoutVM)Session["Checkout"];
            var z = (Member)Session["Account"];

            var hoaDon = new Order
            {
                IDMember = id,
                TotalMoney = Total(),
                OrderedDate = DateTime.Now,
                Status = 1
            };
            if (y != null)
            {
                hoaDon.Address = y.Address;
                hoaDon.Email = y.Email;
                hoaDon.FullName = y.FullName;
                hoaDon.PhoneNumber = y.PhoneNumber;
            }
            else
            {
                hoaDon.Address = z.Address;
                hoaDon.Email = z.Email;
                hoaDon.FullName = z.FullName;
                hoaDon.PhoneNumber = z.PhoneNumber;
            }
            db.Orders.Add(hoaDon);
            db.SaveChanges();

            var MaHD = db.Orders.Select(x => x.IDOrder).Max();

            if (Session["GioHang"] is List<Cart> list)
            {
                foreach (var item in list)
                {
                    var chiTietHoaDon = new DetailOrder
                    {
                        //var product = db.Products.SingleOrDefault(x=>x.i)
                        IDOrder = MaHD,
                        IDProduct = item.IDProduct,
                        Amount = item.Amount,
                        Price = item.Price
                    };
                    db.DetailOrders.Add(chiTietHoaDon);
                    db.SaveChanges();
                }
            }
            try
            {
                if (y != null)
                    SendMail(MaHD, y.FullName, y.Email);
                else
                    SendMail(MaHD, z.FullName, z.Email);
            }
            catch (Exception) {
                Session["GioHang"] = null;
                return RedirectToAction("Confirmation", new { id = MaHD });
            }
            Session["GioHang"] = null;
            return RedirectToAction("Confirmation", new {id = MaHD });
        }


        public JsonResult GetOrderList(int? status, int page, int pageSize)
        {
            if (Session["Account"] is Member member)
            {
                var OrderList = (from s in db.Orders
                                 join b in db.Members on s.IDMember equals b.IDMember
                                 join t in db.StatusOrders on s.Status equals t.ID
                                 where s.Status == status && s.IDMember == member.IDMember
                                 select new
                                 {
                                     s.IDOrder,
                                     t.Name,
                                     s.OrderedDate,
                                     b.FullName
                                 }).ToList();
                if (status == 10)
                {
                    OrderList = (from s in db.Orders
                                 join b in db.Members on s.IDMember equals b.IDMember
                                 join t in db.StatusOrders on s.Status equals t.ID
                                 where s.IDMember == member.IDMember
                                 select new
                                 {
                                     s.IDOrder,
                                     t.Name,
                                     s.OrderedDate,
                                     s.FullName
                                 }).ToList();
                }

                var x = OrderList.OrderByDescending(n => n.Name).Skip((page - 1) * pageSize).Take(pageSize);
                int totalRow = OrderList.Count();
                return Json(new
                {
                    data = x,
                    total = totalRow,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {               
                status = false
            }, JsonRequestBehavior.AllowGet);

        }

        // Xóa đơn hàng
        [HttpPost]
        public JsonResult BrowseStatusOrder(int? id)
        {
            Order model = db.Orders.SingleOrDefault(n => n.IDOrder == id);
            model.Status = 0 ;
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(new
            {
                status = true

            }, JsonRequestBehavior.AllowGet);
        }
    }
}