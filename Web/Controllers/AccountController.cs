using Web.Models.Data;
using Web.Models.DB;
using Web.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        ShopBanDoTheThao db = new ShopBanDoTheThao();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Login()
        {
            return PartialView();
        }

        public ActionResult LogIn(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = db.Members.SingleOrDefault(x => x.UserName == model.UserName);
                if (result.PassWord == MaHoa.MaHoaSangMD5(model.PassWord))
                {
                    Session["Account"] = db.Members.Where(x => x.IDMember == result.IDMember).SingleOrDefault();
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View("Index");
        }

        public ActionResult LogOut()
        {
            Session["Account"] = null;
            return View("Index");
        }

        public JsonResult Register(Member model)
        {
            var khachHang = new Member();
            var result = false;
            try
            {
                khachHang.FullName = model.FullName;
                khachHang.UserName = model.UserName;
                khachHang.PassWord = MaHoa.MaHoaSangMD5(model.PassWord);

                db.Members.Add(khachHang);
                db.SaveChanges();

                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (result)
                Session["Account"] = db.Members.Where(x => x.UserName == model.UserName).SingleOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult _Update()
        {
            Member model = new Member();
            Member x = (Member)Session["Account"];

            model.IDMember = x.IDMember;
            model.UserName = x.UserName;
            model.FullName = x.FullName;

            if (x.Email != null)
            {
                model.Email = x.Email;
                model.Address = x.Address;
                model.Avatar = x.Avatar;
                model.PhoneNumber = x.PhoneNumber;
            }

            return PartialView(model);
        }

        public ActionResult Update(Member model)
        {
            var khachHang = db.Members.SingleOrDefault(x => x.IDMember == model.IDMember);
            try
            {
                khachHang.UserName = model.UserName;
                khachHang.PassWord = MaHoa.MaHoaSangMD5(model.PassWord);
                khachHang.Email = model.Email; 
                khachHang.Address = model.Address;                
                khachHang.Avatar = model.Avatar;
                khachHang.FullName = model.FullName;
                khachHang.PhoneNumber = model.PhoneNumber;

                db.SaveChanges();
                ViewBag.Notification = "Thông tin tài khoản của bạn đã được cập nhật thành công!";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Session["Account"] = db.Members.Where(x => x.UserName == model.UserName).SingleOrDefault();

            return View("Index");
        }

        public JsonResult DeleteRecord(int Id)
        {
            bool result = false;
            var KhachHang = db.Members.SingleOrDefault(x => x.IDMember == Id);
            if (KhachHang != null)
            {
                db.Members.Remove(KhachHang);
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}