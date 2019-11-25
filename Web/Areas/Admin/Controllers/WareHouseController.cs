using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Data;
using Web.Models.DB;

namespace Web.Areas.Admin.Controllers
{
    public class WareHouseController : Controller
    {
        // GET: Admin/WareHouse
        public ActionResult Index()
        {
            return View();
        }

        //Load dữ liệu lên với Ajax
        [HttpGet]
        public JsonResult LoadData(string name, int page, int pageSize)
        {
            var dao = new WareHouseDao();
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
        public JsonResult SaveData(WareHouse warehouse)
        {
            var dao = new WareHouseDao();
            bool temp = false;
            //Save
            //Add warehouse
            if (warehouse.IDWareHouse == 0)
            {
                dao.Insert(warehouse);
                temp = true;
            }
            else
            {
                dao.Edit(warehouse);
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
            var dao = new WareHouseDao();
            var warehouse = dao.FindID(id);
            return Json(new
            {
                data = warehouse,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        //Xóa danh mục theo ajax
        [HttpPost]
        public JsonResult DeleteWarehouse(int id)
        {
            var dao = new WareHouseDao();
            bool temp = dao.Delete(id);
            return Json(new
            {
                status = temp
            });
        }
    }
}
