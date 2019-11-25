using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Models.Data;
using Web.Models.ViewModels;

namespace BanDoTheThao.Areas.SalesMan.Controllers
{
    public class OrderController : Controller
    {
        // GET: SalesMan/Order
        //Git hub
        ShopBanDoTheThao db = new ShopBanDoTheThao();

        public ActionResult ViewOrders()
        {
            return View();
        }
        public ActionResult TestPagging()
        {
            return View();
        }
        [HttpGet]
        public JsonResult LoadData()
        {
            //   var lstSP = db.Products.Select(n => n);
            var lst = new List<Employee>();
            lst.Add(new Employee()
            {
                ID = 1,
                Name = "A",
                Salary = 20000,
                Status = true
            });
            lst.Add(new Employee()
            {
                ID = 1,
                Name = "A",
                Salary = 20000,
                Status = true
            });
            lst.Add(new Employee()
            {
                ID = 1,
                Name = "A",
                Salary = 20000,
                Status = true
            });
            return Json(new
            {
                data = lst,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        //..,bool status1  && s.Payed ==status1
        //, //Git lan2
        public JsonResult GetOrderList(int? status, int page, int pageSize)
        {
            var OrderList = (from s in db.Orders
                             join b in db.Customers on s.IDCus equals b.IDCus
                             join t in db.StatusOrders on s.Status equals t.ID
                             where s.Status == status
                             select new
                             {
                                 s.IDOrder,
                                 t.Name,
                                 s.OrderedDate,
                                 b.NameCus
                             }).ToList();
            if (status == 10)
            {
                OrderList = (from s in db.Orders
                             join b in db.Customers on s.IDCus equals b.IDCus
                             join t in db.StatusOrders on s.Status equals t.ID
                             select new
                             {
                                 s.IDOrder,
                                 t.Name,
                                 s.OrderedDate,
                                 b.NameCus
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
        public JsonResult GetDetailOrder(int? IDOrder)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var OrderList = (from s in db.Orders
                             join b in db.Customers on s.IDCus equals b.IDCus
                             where s.IDOrder == IDOrder
                             select new
                             {
                                 s.IDOrder,
                                 b.NameCus
                             });

            // var OrderList = db.Orders.Find(IDOrder);
            return Json(new
            {
                data = OrderList,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult BrowseOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order model = db.Orders.SingleOrDefault(n => n.IDOrder == id);
            if (model == null)
            {
                return HttpNotFound();
            }

            var lstChiTietDH = db.DetailOrders.Where(n => n.IDOrder == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }

        [HttpPost]
        public JsonResult BrowseStatusOrder(int? id)
        {
            Order model = db.Orders.SingleOrDefault(n => n.IDOrder == id);
            if(model.Status==0||model.Status==5)
            {
                return Json(new
                {
                    status = false,
                    mess = "Bạn không thể thực hiện được khi đơn hàng đã hủy hoặc đơn hàng đang ở trạng thái trả hàng/Hoàn tiền",

                }, JsonRequestBehavior.AllowGet);
            }




            model.Status++;
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(new
            {
                status = true

            }, JsonRequestBehavior.AllowGet);



        }


    }
}