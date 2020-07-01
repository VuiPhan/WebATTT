using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Web.Models.Data;
using Web.Models.ViewModels;

namespace Web.Controllers
{
    public class LoginSystemController : Controller
    {

        static Random rd = new Random();
        static int CODEOTP = rd.Next(100000, 999999);
        // GET: LoginSystem
        ShopBanDoTheThao db = new ShopBanDoTheThao();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Login(string username, string password)
        {
            // 2 là NVBH
            // 3 là NVLK
            // 1 là Admin
            password = MaHoa.MaHoaSangMD5(username + password);
            // password = MaHoa.MaHoaSangMD5(password);
            //password = MaHoa.MaHoaSangMD5(password);
            Member member = db.Members.Where(n => n.UserName == username && n.PassWord == password).SingleOrDefault();
            if (member == null)
            {
                return Json(new
                {
                    isRedirect = -1
                });
            }
            else
            {
                Session["Account"] = member;
                var lstQuyen = db.Authority_MemType.Where(n => n.IDMemType == member.IDMemType);
                //Duyệt list quyền
                string Quyen = "";
                foreach (var item in lstQuyen)
                {
                    // Sẽ đưa các quyền của tài khoản đó vào một chuỗi để lấy ra các quyền
                    Quyen += item.IDAuthority + ",";
                }
                // Cắt dấu ","
                Quyen = Quyen.Substring(0, Quyen.Length - 1);
                PhanQuyen(member.UserName, Quyen);
                if (member.IDMemType == 2)
                {
                    Session["NVBH"] = member;
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
                        Session["AdminNoOTP"] = member;
                        // Gọi đến hàm kiểm tra - Tới đây thì vẫn chưa vô được tài khoản
                        // Kiểm tra nếu member đó chọn xác thực OTP 
                        // Lấy member lên để Update;
                        member = db.Members.Where(n => n.UserName == username && n.PassWord == password).SingleOrDefault();
                        // Nếu không bị khóa thì cấp mới
                        if (member.IsLock == true && (member.TimeEndLock < DateTime.Now || member.TimeEndLock == null))
                        {
                            member.NumberOfTries = 2;
                            member.TimeEndLock = null;
                            member.TimeStartLock = null;
                            member.IsLock = false;
                            db.SaveChanges();
                        }
                        if (member.IsLock == false)
                        {
                            member.NumberOfTries = 2;
                            member.TimeEndLock = null;
                            member.TimeStartLock = null;
                            db.SaveChanges();
                        }

                        if (member.IsCheckOTP == true)
                        {
                            if (member.IsLock == true)
                            {
                                DateTime TimeLock = (DateTime)member.TimeEndLock;
                                string s = "Tài khoản bị khóa đến " + TimeLock.ToString("dd/MM/yyyy HH:mm:ss");
                                return Json(new
                                {
                                    //  redirectUrl = Url.Action("ConfLogin", "LoginSystem"),
                                    isRedirect = 100,
                                    message = s

                                });
                            }
                            return Json(new
                            {
                                isRedirect = 1,
                                redirectUrl = Url.Action("ConfLogin", "LoginSystem"),
                            });
                        }
                        Session["Admin"] = Session["AdminNoOTP"];
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
        public ActionResult ConfLogin()
        {
            if (Session["AdminNoOTP"] == null)
            {
                return RedirectToAction("Error", "Home");
            }
            // Thực hiện gửi OTP
            try
            {
                //string t = "Quý khách sẽ mất Tiền và thông tin nếu cung cấp mã OPT cho bất ky ai. Mã OTP xác thực của quý khách là:" + CODEOTP.ToString();
                Member member = (Member)Session["AdminNoOTP"];
                //new SendSMSHelper().SendSMS(member.PhoneNumber, t);
                member = db.Members.Where(p => p.IDMember == member.IDMember).SingleOrDefault();
                member.OTP = CODEOTP.ToString();
                member.TimeSendOTP = DateTime.Now;
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }


            return View();
        }
        public JsonResult ReSendOTP(string code)
        {
            Member member = (Member)Session["AdminNoOTP"];
            member = db.Members.Where(p => p.IDMember == member.IDMember).SingleOrDefault();
            DateTime TimeSendOTP = (DateTime)member.TimeSendOTP;
            DateTime TimeSendOTPCong1P = TimeSendOTP.AddSeconds(30);
            int timeSecond = DateTime.Now.Subtract(TimeSendOTP).Seconds;
            int timeSecond2 = 30 - timeSecond;
            bool CheckReSendOTP = timeSecond <= 30 ? true : false;
            if (CheckReSendOTP && member.IsLock == false)
            {
                return Json(new
                {
                    NumberOfTries = -1000, // Mã gửi lại OTP
                    isRedirect = false,
                    mess = "Vui lòng gửi lại OTP sau : " + timeSecond2 + " giây",
                    timeleft = timeSecond2
                });
            }
            if ((member.IsLock == true && (member.TimeEndLock < DateTime.Now || member.TimeEndLock == null))||member.IsLock == false)
            {
                // Thì mới được cấp lại 
                member.NumberOfTries = 2;
                member.TimeEndLock = null;
                member.TimeSendOTP = DateTime.Now;
                member.TimeStartLock = null;
                CODEOTP = rd.Next(100000, 999999);
                member.OTP = CODEOTP.ToString();
                member.IsLock = false;
                db.SaveChanges();
                return Json(new
                {
                    NumberOfTries = -1000, // Mã gửi lại OTP
                    isRedirect = false,
                    mess = "Mã OTP đã được gửi lại. Vui lòng kiểm tra điện thoại !!!",
                    timeleft = timeSecond2
                });
            }
            else
            {
                // Tài khoản đã bị khóa
                DateTime TimeLock = (DateTime)member.TimeEndLock;
                string s = "Bạn đã nhập sai quá nhiều lần. Tài khoản của bạn bị khóa đến " + TimeLock.ToString("dd/MM/yyyy HH:mm:ss");
                return Json(new
                {
                    //  redirectUrl = Url.Action("ConfLogin", "LoginSystem"),
                    NumberOfTries = -1000, // Mã gửi lại OTP
                    isRedirect = false,
                    mess = s,
                    timeleft = timeSecond2

                });
            }

           
        }

        public JsonResult RequestCode(string code)
        {
            // Xác định xem người đó có được nhập mã Code hay không?
            // Đây là vẫn còn nhập được
            Member member = (Member)Session["AdminNoOTP"];
            member = db.Members.Where(p => p.IDMember == member.IDMember).SingleOrDefault();
            DateTime TimecheckExpried = (DateTime)member.TimeSendOTP;
            bool CheckExpried = DateTime.Now.Subtract(TimecheckExpried).Seconds > 30 ? true : false;
            if (CheckExpried)
            {
                return Json(new
                {
                    NumberOfTries = 1000, // Bị khóa rồi
                    isRedirect = false,
                    mess = "Mã OTP đã hết hạn"
                });
            }
            // Đúng thì vào luôn
            if (code == member.OTP)
            {
                Session["Admin"] = Session["AdminNoOTP"];
                return Json(new
                {
                    redirectUrl = Url.Action("Index", "Admin/Dashboard"),
                    isRedirect = true
                });
            }
            else
            {
                // Nhập sai
                // Giảm số lần thử
                member = db.Members.Where(p => p.IDMember == member.IDMember).SingleOrDefault();
                if (member.NumberOfTries == 0)
                {
                    DateTime t = (DateTime)member.TimeEndLock;
                    return Json(new
                    {
                        NumberOfTries = 1000, // Bị khóa rồi
                        isRedirect = false,
                        mess = "Tài khoản bị khóa đến " + t.ToString("dd/MM/yyyy HH:mm:ss")
                    });
                }
                if (member.NumberOfTries == 1)
                {
                    // Tiến hành khóa tài khoản
                    member.IsLock = true;
                    member.TimeStartLock = DateTime.Now;
                    member.NumberOfTries = 0;
                    member.TimeEndLock = DateTime.Now.AddSeconds(30);
                    DateTime t = (DateTime)member.TimeEndLock;
                    db.SaveChanges();
                    return Json(new
                    {
                        NumberOfTries = 1000, // Bị khóa rồi
                        isRedirect = false,
                        mess = "Tài khoản bị khóa đến " + t.ToString("dd/MM/yyyy HH:mm:ss")
                    });
                }
                else
                {
                    member.NumberOfTries -= 1;
                    db.SaveChanges();
                    return Json(new
                    {
                        NumberOfTries = member.NumberOfTries,
                        isRedirect = false
                    });

                }

            }
        }
        public static bool KiemTraOTP()
        {

            return true;
        }
        public void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                                          TaiKhoan, //user
                                          DateTime.Now, //Thời gian bắt đầu
                                          DateTime.Now.AddHours(3), //Thời gian kết thúc. Sau 3 giờ sẽ gỡ cookie
                                          false, //Ghi nhớ? ghi nhớ tài khoản hay không?
                                          Quyen, // "DangKy,QuanLyDonHang,QuanLySanPham"
                                          FormsAuthentication.FormsCookiePath);
            // Tạo ra cookie
            // Sau đó chúng ta tiến hành cấu hình file Web.config
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
        public ActionResult LoiPhanQuyen()
        {
            // Khi một tài khoản cố tình get vào link thì sẽ trả về thông báo
            return View();
        }
        public ActionResult Logout()
        {
            Session["Admin"] = null;
            Session["NVBH"] = null;
            Session["NVLK"] = null;
            // Sẽ gỡ cookie ra khỏi phiên...
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "LoginSystem");
        }
        //public JsonResult Sendotp(string strEmployee)
        //{

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    User user = serializer.Deserialize<User>(strEmployee);
        //    bool status = false;
        //    string message = string.Empty;
        //    int m = new Random().Next(1000, 9999);
        //    int dao = new UserDao().Login(user.UserName, Encryptor.MD5Hash(user.Password));
        //    if (dao == 0)
        //    {
        //        status = false;
        //        message = "Tên đăng nhập hoặc mật khâu không đúng";
        //    }
        //    else if (dao == 1)
        //    {
        //        status = true;
        //        var userSession = new UserDao().FindUserByNamePass(user.UserName, Encryptor.MD5Hash(user.Password));
        //        Session.Add(CommonConstants.ADMIN_SESSION, userSession);


        //    }
        //    else if (dao == -1)
        //    {
        //        message = "tài khoản bị khóa";
        //    }
        //    else if (dao == 2)
        //    {
        //        message = "tài khoản không có quyền truy cập";
        //    }
        //    return Json(new
        //    {
        //        data = CODEOTP.ToString(),
        //        status = status,
        //        message = message
        //    });
        //}
    }
}