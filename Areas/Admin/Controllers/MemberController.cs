using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Data;
using Web.Models.DB;

namespace Web.Areas.Admin.Controllers
{
    public class MemberController : BaseController
    {
        // GET: Admin/Member
        public ActionResult Index()
        {
            return View();
        }

        //Load dữ liệu lên với Ajax
        [HttpGet]
        public JsonResult LoadData(string name, int page, int pageSize)
        {
            var dao = new MemberDao();
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
        public JsonResult SaveData(Member member)
        {
            var dao = new MemberDao();
            bool temp = false;
            //Save
            //Add category
            if (member.IDMember == 0)
            {
                dao.Insert(member);
                temp = true;
            }
            else
            {
                dao.Edit(member);
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
            var dao = new MemberDao();
            var member = dao.FindID(id);
            return Json(new
            {
                data = member,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        //Xóa danh mục theo ajax
        [HttpPost]
        public JsonResult DeleteMember(int id)
        {
            var dao = new MemberDao();
            bool temp = dao.Delete(id);
            return Json(new
            {
                status = temp
            });
        }

        //Upload hình ảnh
        [HttpPost]
        public string UploadImg(HttpPostedFileBase file)
        {
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }
    }
}