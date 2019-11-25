using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Data;
using Web.Models.DB;

namespace Web.Areas.Admin.Controllers
{
    public class InfoAdminController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var dao = new InfoAdminDao();
            var model = dao.GetInfoAdmin();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Member admin)
        {
            var dao = new InfoAdminDao();
            var result = dao.Update(admin);
            if (result)
            {
                return RedirectToAction("Index", "InfoAdmin");
            }
            else
            {
                ModelState.AddModelError("", "Cập nhật thông tin cá nhân thành công!!");
            }
            return View("Index");
        }


        public PartialViewResult Header(Member admin)
        {
            var dao = new InfoAdminDao();
            var model = dao.GetInfoAdmin();
            ViewBag.InfoAdmin = model;
            return PartialView();
        }

        public PartialViewResult Sidebar(Member admin)
        {
            var dao = new InfoAdminDao();
            var model = dao.GetInfoAdmin();
            ViewBag.InfoAdmin = model;
            return PartialView();
        }
    }
}