using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web.Models.Data;
using Web.Models.ViewModels;

namespace Web.Controllers
{
    public class LoginSystemController : Controller
    {
        // GET: LoginSystem
        ShopBanDoTheThao db = new ShopBanDoTheThao();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Login(string username,string password)
        {
            // 2 là NVBH
            // 3 là NVLK
            // 1 là Admin
            password = MaHoa.MaHoaSangMD5(password);
            Member member = db.Members.Where(n => n.UserName == username && n.PassWord == password).SingleOrDefault();
            if (member == null)
            {
                return Json(new
                {
                    isRedirect = false
                });
            }
            else
            {
                 var lstQuyen = db.Authority_MemType.Where(n => n.IDMemType == member.IDMemType );                 //Duyệt list quyền                 string Quyen = "";                 foreach (var item in lstQuyen)                 {                     // Sẽ đưa các quyền của tài khoản đó vào một chuỗi để lấy ra các quyền                     Quyen += item.IDAuthority + ",";                 }                 // Cắt dấu ","                 Quyen = Quyen.Substring(0, Quyen.Length - 1);                 PhanQuyen(member.UserName, Quyen);
                if (member.IDMemType == 2)
                {                     Session["NVBH"] = member;
                    return Json(new
                    {
                        redirectUrl = Url.Action("ViewOrders", "SalesMan/Order"),
                        isRedirect = true
                    });
                }
                else
                {
                    if (member.IDMemType == 1)
                    {
                        Session["Admin"] = member;
                        return Json(new
                        {
                            redirectUrl = Url.Action("Index", "Admin/Dashboard"),
                            isRedirect = true
                        });
                    }
                    else
                    {
                        Session["NVLK"] = member;
                        return Json(new
                        {
                            redirectUrl = Url.Action("Index", "WarehouseStaff/Product"),
                            isRedirect = true
                        });
                    }
               
                }

            }
        }
        public void PhanQuyen(string TaiKhoan, string Quyen)         {             FormsAuthentication.Initialize();             var ticket = new FormsAuthenticationTicket(1,                                           TaiKhoan, //user                                           DateTime.Now, //Thời gian bắt đầu                                           DateTime.Now.AddHours(3), //Thời gian kết thúc. Sau 3 giờ sẽ gỡ cookie                                           false, //Ghi nhớ? ghi nhớ tài khoản hay không?                                           Quyen, // "DangKy,QuanLyDonHang,QuanLySanPham"                                           FormsAuthentication.FormsCookiePath);             // Tạo ra cookie             // Sau đó chúng ta tiến hành cấu hình file Web.config             var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));             if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;             Response.Cookies.Add(cookie);         }
        public ActionResult LoiPhanQuyen()         {
            // Khi một tài khoản cố tình get vào link thì sẽ trả về thông báo
            return View();         }
         public ActionResult Logout()           {             Session["Admin"] = null;             Session["NVBH"] = null;             Session["NVLK"] = null;             // Sẽ gỡ cookie ra khỏi phiên...             FormsAuthentication.SignOut();              return RedirectToAction("Login","LoginSystem");           }
    }
}