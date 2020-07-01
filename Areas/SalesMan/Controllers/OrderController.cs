using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Models.Data;
using Web.Models.DB;
using Web.Models.ViewModels;

namespace Web.Areas.SalesMan.Controllers
{
    public class OrderController : BaseController
    {
        // GET: SalesMan/Order
        ShopBanDoTheThao db = new ShopBanDoTheThao();

        public ActionResult ViewOrders()
        {
            var OrderList = (from s in db.Orders

                             join t in db.StatusOrders on s.Status equals t.ID
                             where s.Status == 1
                             select new
                             {
                                 s.IDOrder,
                                 t.Name,
                                 s.OrderedDate,
                                 s.FullName
                             }).ToList();
            ViewBag.DonChuaDuyet = OrderList.Count;
            return View();
        }
        [HttpPost]
        public JsonResult DeleteOrder(int? id)
        {
            Order model = db.Orders.SingleOrDefault(n => n.IDOrder == id);
            model.Status = 0;
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(new
            {
                status = true

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditAccount(Member f, HttpPostedFileBase Avatar)
        {
            //string username = f["username"].ToString();
            string username = f.UserName.ToString();
            string password = MaHoa.MaHoaSangMD5(username + f.PassWord);
            f.PassWord = password;
            Member mb = db.Members.Where(n => n.UserName == username).SingleOrDefault();
            mb.PassWord = password;
            mb.PhoneNumber = f.PhoneNumber;
            mb.Address = f.Address;
            if (Avatar != null)
            { 
            if (Avatar.ContentLength > 0)
            {
                //Lấy tên hình ảnh
                var fileName = Path.GetFileName(Avatar.FileName);
                // Lấy hình ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Resources/Admin/images/faces"), fileName);
                // Nếu tồn tại xuất ra thông báo
                if (System.IO.File.Exists(path))
                {
                    ViewBag.upload = "Hình đã tồn tại";
                    return View();
                }
                else
                {
                    Avatar.SaveAs(path);
                        // sp.HinhAnh = fileName;
                        // Avatar = fileName;
                        mb.Avatar = fileName;
                }
            }
            }
            db.SaveChanges();
            Session["NVBH"] = mb;
            return RedirectToAction("ViewOrders");
        }


        public ActionResult TestPagging()
        {
            return View();
        }

        [HttpGet]
        //..,bool status1  && s.Payed ==status1
        //,
        public JsonResult GetOrderList(int? status, int page, int pageSize)
        {
            var OrderList = (from s in db.Orders
                             
                             join t in db.StatusOrders on s.Status equals t.ID
                             where s.Status == status
                             select new
                             {
                                 s.IDOrder,
                                 t.Name,
                                 s.OrderedDate,
                                 s.FullName
                             }).ToList();
            if (status == 10)
            {
                OrderList = (from s in db.Orders
                           
                             join t in db.StatusOrders on s.Status equals t.ID
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


        [HttpGet]
        public JsonResult SearchOrderList(int? status, int page, int pageSize,string strSearch)

        {
            int idOrderSearch = Convert.ToInt32(strSearch);
            var OrderList = (from s in db.Orders

                             join t in db.StatusOrders on s.Status equals t.ID
                             where s.Status == status && s.IDOrder== idOrderSearch
                             select new
                             {
                                 s.IDOrder,
                                 t.Name,
                                 s.OrderedDate,
                                 s.FullName
                             }).ToList();
            if (status == 10)
            {
                OrderList = (from s in db.Orders

                             join t in db.StatusOrders on s.Status equals t.ID
                             where s.IDOrder == idOrderSearch
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
            string ss = "Không tìm thấy đơn hàng có mã "+ idOrderSearch.ToString();
            if (x.Count() == 0)
            {
                return Json(new
                {
                    mess = ss,
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }


            return Json(new
            {
                data = x,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetDetailOrder(int? IDOrder)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var OrderList = (from s in db.Orders
                            
                             where s.IDOrder == IDOrder
                             select new
                             {
                                 s.IDOrder,
                                 s.FullName
                             });

            // var OrderList = db.Orders.Find(IDOrder);
            return Json(new
            {
                data = OrderList,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult BrowseOrder(int id)
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
            switch (hoaDon.Status)
            {
                case 0:
                    model.Status = "Đã hủy";
                    break;
                case 1:
                    model.Status = "Chưa xác nhận";
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

        [HttpPost]
        public JsonResult BrowseStatusOrder(int? id)
        {
            Order model = db.Orders.SingleOrDefault(n => n.IDOrder == id);
            if (model.Status == 0 || model.Status == 5)
            {
                return Json(new
                {
                    status = false,
                    mess = "Bạn không thể thực hiện được khi đơn hàng đã hủy hoặc đơn hàng đang ở trạng thái trả hàng/Hoàn tiền",

                }, JsonRequestBehavior.AllowGet);
            }
            switch (model.Status)
            {
                case 1: model.ConfirmDate = DateTime.Now;
                        break;
                case 2:
                    model.DeliveryDate = DateTime.Now;
                    break;
                case 3:
                    model.DeliveredDate = DateTime.Now;
                    break;
            }
            model.Status++;
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(new
            {
                status = true

            }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult SearchDayOrderList(int? status, int page, int pageSize, string dateSearch1, string dateSearch2)

        {
            DateTime Date1 = DateTime.Parse(dateSearch1);
            DateTime Date2 = DateTime.Parse(dateSearch2);
            var OrderList = (from s in db.Orders

                             join t in db.StatusOrders on s.Status equals t.ID
                             where s.Status == status && s.OrderedDate >= Date1 && s.OrderedDate <= Date2
                             select new
                             {
                                 s.IDOrder,
                                 t.Name,
                                 s.OrderedDate,
                                 s.FullName
                             }).ToList();
            if (status == 10)
            {
                OrderList = (from s in db.Orders
                             join t in db.StatusOrders on s.Status equals t.ID
                             where s.OrderedDate >= Date1 && s.OrderedDate <= Date2
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
            string ss = "Không tìm thấy đơn hàng có ngày từ " + Date1.ToString() +" đến ngày " +Date2.ToString();
            if (x.Count() == 0)
            {
                return Json(new
                {
                    mess = ss,
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                data = x,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }


        // Cập nhật địa chỉ giao hàng
        [HttpPost]
        public JsonResult UpdateAddressOrder(int? id,string address)
        {
            Order model = db.Orders.SingleOrDefault(n => n.IDOrder == id);
            model.Address = address;
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(new
            {
                status = true

            }, JsonRequestBehavior.AllowGet);



        }
    }
}