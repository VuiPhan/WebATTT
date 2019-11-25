using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.DB;

namespace Web.Areas.Admin.Controllers
{
    public class ImportBillController : Controller
    {
        // GET: Admin/ImportBill
        public ActionResult Index()
        {
            return View();
        }

        //Load dữ liệu lên với Ajax
        [HttpGet]
        public JsonResult LoadData(string idImport, int page, int pageSize)
        {
            var dao = new ImportBillDao();
            var model = dao.ListAllPage(idImport, page, pageSize);
            int totalRow = dao.Count(idImport);
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        //Lấy thông tin chi tiết theo ajax
        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            var dao = new DetailImportDao();
            var importbill = dao.LoadData(id);
            return Json(new
            {
                data = importbill,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}