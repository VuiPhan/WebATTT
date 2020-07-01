using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Data;
using Web.Models.DB;

namespace Web.Areas.Admin.Controllers
{
    public class EmployeeController : BaseController
    {
        ShopBanDoTheThao db = new ShopBanDoTheThao();
        // GET: Admin/Employee
        public ActionResult Index()
        {
            List<MemType> MemtypeList = db.MemTypes.Where(x => x.IDMemType == 2 || x.IDMemType == 3).ToList();
            ViewBag.ListOfMemType = new SelectList(MemtypeList, "IDMemType", "NameMemType");
            return View();
        }
        //Load dữ liệu lên với Ajax
        [HttpGet]
        public JsonResult LoadData(string name, int page, int pageSize, int type)
        {
            var dao = new EmployeeDao();
            var model = dao.ListAllPage(name, page, pageSize, type);
            int totalRow = dao.Count(name, type);
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true,
            }, JsonRequestBehavior.AllowGet);
        }

        //Save dữ liệu với Ajax

        [HttpPost]
        public JsonResult SaveData(Member employee, int IdMemType)
        {
            var dao = new EmployeeDao();
            bool temp = false;
            //Save
            //Add category
            if (employee.IDMember == 0)
            {
                dao.Insert(employee, IdMemType);
                temp = true;
            }
            else
            {
                dao.Edit(employee);
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
            var dao = new EmployeeDao();
            var employee = dao.FindID(id);
            return Json(new
            {
                data = employee,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        //Xóa danh mục theo ajax
        [HttpPost]
        public JsonResult DeleteEmployee(int id)
        {
            var dao = new EmployeeDao();
            bool temp = dao.Delete(id);
            return Json(new
            {
                status = temp
            });
        }

    }
}