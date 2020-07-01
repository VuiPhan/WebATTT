using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Data;
using Web.Models.DB;

namespace Web.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View();
        }

        //Load dữ liệu lên với Ajax
        [HttpGet]
        public JsonResult LoadData(string name, int page, int pageSize)
        {
            var dao = new ProductTypeDao();
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
        public JsonResult SaveData(ProductType category)
        {
            var dao = new ProductTypeDao();
            bool temp = false;
            //Save
            //Add category
            if (category.IDProductType == 0)
            {
                dao.Insert(category);
                temp = true;
            }
            else
            {
                dao.Edit(category);
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
            var dao = new ProductTypeDao();
            var category = dao.FindID(id);
            return Json(new
            {
                data = category,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        //Xóa danh mục theo ajax
        [HttpPost]
        public JsonResult DeleteCategory(int id)
        {
            var dao = new ProductTypeDao();
            bool temp = dao.Delete(id);
            return Json(new
            {
                status = temp
            });
        }
    }
}