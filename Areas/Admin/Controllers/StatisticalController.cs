using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.DB;

namespace Web.Areas.Admin.Controllers
{
    public class StatisticalController : BaseController
    {
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            StatisticalDao dao = new StatisticalDao();

            //Gán tổng khách hàng
            ViewBag.TotalCustomer = dao.TotalCustomer();

            //Gán tổng doanh thu
            ViewBag.TotalMoney = dao.TotalMoney();

            //Gán số lượng sản phẩm đã bán
            ViewBag.SalesedQuantity = dao.SalesedQuantity();

            //Gán số lượng sản phẩm còn lại
            ViewBag.RemainingQuantity = dao.RemainingQuantity();

            //Gán phần trăm tổng doanh thu theo từng tháng của năm
            ViewBag.TotalMoneyWithMonth = JsonConvert.SerializeObject(dao.TotalMoneyWithMonth());

            return View();
        }
    }
}