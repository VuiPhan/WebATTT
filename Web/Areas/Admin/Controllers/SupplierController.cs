using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Data;
using Web.Models.DB;

namespace Web.Areas.Admin.Controllers
{
    public class SupplierController : Controller
    {
        // GET: Admin/Supplier
        public ActionResult Index()
        {
            return View();
        }

        //Load dữ liệu lên với Ajax
        [HttpGet]
        public JsonResult LoadData(string name, int page, int pageSize)
        {
            var dao = new SupplierDao();
            var model = dao.ListAllPage(name, page, pageSize);
            int totalRow = dao.Count(name);
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        //Save dữ liệu với Ajax

        [HttpPost]
        public JsonResult SaveData(Supplier supplier)
        {
            var dao = new SupplierDao();
            bool temp = false;
            //Save
            //Add category
            if (supplier.IDSupplier == 0)
            {
                dao.Insert(supplier);
                temp = true;
            }
            else
            {
                dao.Edit(supplier);
                temp = true;
            }
            return Json(new
            {
                status = temp
            });
        }


        //Lấy thông tin chi tiết theo ajax
        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            var dao = new SupplierDao();
            var supplier = dao.FindID(id);
            return Json(new
            {
                data = supplier,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        //Xóa danh mục theo ajax
        [HttpPost]
        public JsonResult DeleteSupplier(int id)
        {
            var dao = new SupplierDao();
            bool temp = dao.Delete(id);
            return Json(new
            {
                status = temp
            });
        }
    }
}