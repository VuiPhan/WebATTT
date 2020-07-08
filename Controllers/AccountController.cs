using Web.Models.Data;
using Web.Models.ViewModels;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Web.Razor.Text;
using Common;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ShopBanDoTheThao db = new ShopBanDoTheThao();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Login()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult LogIn(string username, string password)
        {
            password = MaHoa.MaHoaSangMD5(username + password);
            Member member = db.Members.Where(n => n.UserName == username && n.PassWord == password).SingleOrDefault();
            if (member != null && member.IDMemType != 1)
            {
                if (member.TwoFactor == 1)
                {
                    Session["Account"] = db.Members.Where(x => x.UserName == member.UserName).SingleOrDefault();
                    return Json(new
                    {
                        redirectUrl = Url.Action("Index", "Account"),
                        isRedirect = 1
                    });
                }
                else
                {
                    Session["Login"] = member;
                    SendMail(member.FullName, member.Email);
                    return Json(new
                    {
                        redirectUrl = Url.Action("ConfLogin", "Account"),
                        isRedirect = 2
                    });
                }                
            }
            return Json(new
            {
                isRedirect = 0
            });
        }

        public ActionResult LogOut()
        {
            Session["Account"] = null;
            Session["Forgot"] = null;
            Session["Register"] = null;
            return RedirectToAction("Index");
        }        
        public JsonResult Register(Member model)
        {
            
            var khachHang = db.Members.Where(x => x.UserName == model.UserName).SingleOrDefault();
            var khachHangEmail = db.Members.Where(x => x.Email == model.Email).SingleOrDefault();

            if(khachHang != null)
            {
                return Json(new
                {
                    isRedirect = 5 // Trùng tài khoản
                });

            }
            if (khachHangEmail != null)
            {
                return Json(new
                {
                    isRedirect = 10 // Trùng Email
                });

            }
            if (khachHang != null)
                return Json(new
                {
                    isRedirect = 0
                });

            else
            {
                khachHang = new Member
                {
                    FullName = model.FullName,
                    IDMemType = 4,
                    UserName = model.UserName,
                    Email = model.Email,
                    TwoFactor = 1,
                    IsLoginByPhone = false,
                    PassWord = MaHoa.MaHoaSangMD5(model.UserName + model.PassWord)
                };
                Session["Register"] = khachHang;
                try
                {
                    SendMail(khachHang.FullName, khachHang.Email);
                }
                catch
                {
                    return Json(new
                    {
                        //redirectUrl = Url.Action("Error", "Home"),
                        isRedirect = 100
                    });
                }
            }

            return Json(new
            {
                redirectUrl = Url.Action("Conf", "Account"),
                isRedirect = 1
            });
        }
        public ActionResult Conf()
        {
            return View();
        }
        public ActionResult ConfLogin()
        {
            return View();
        }
        public ActionResult Forgot()
        {
            return View();
        }
        public ActionResult TwoFac()
        {
            Member member = (Member)Session["Account"];
            if (member == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(member);
        }
        public JsonResult TwoFactor()
        {
            try
            {
                Member member = (Member)Session["Account"];
                var khachHang = db.Members.SingleOrDefault(x => x.IDMember == member.IDMember);
                Session["Account"] = khachHang;
                if (khachHang.TwoFactor == 1)
                {
                    khachHang.TwoFactor = 2;
                    db.SaveChanges();
                }
                else
                {
                    khachHang.TwoFactor = 1;
                    db.SaveChanges();
                }
            }
            catch
            {
                return Json(new
                {
                    isRedirect = false
                });
            }            
            return Json(new
            {
                redirectUrl = Url.Action("TwoFac", "Account"),
                isRedirect = true
            });
        }
        public JsonResult LoginByPhoneIsCheck()
        {
            try
            {
                Member member = (Member)Session["Account"];
                var khachHang = db.Members.SingleOrDefault(x => x.IDMember == member.IDMember);
                Session["Account"] = khachHang;
                if (khachHang.IsLoginByPhone == true)
                {
                    khachHang.IsLoginByPhone = false;
                    db.SaveChanges();
                }
                else
                {
                    khachHang.IsLoginByPhone = true;
                    db.SaveChanges();
                }
            }
            catch
            {
                return Json(new
                {
                    isRedirect = false
                });
            }
            return Json(new
            {
                redirectUrl = Url.Action("TwoFac", "Account"),
                isRedirect = true
            });
        }
        public JsonResult RequestCode(string code)
        {
            string CODE = (string)Session["CODE"];
            if (code == CODE)
            {
                Member member = (Member)Session["Register"];
                Member member2 = (Member)Session["Forgot"];
                if (member != null)
                {
                    var khachHang = new Member
                    {
                        FullName = member.FullName,
                        IDMemType = 4,
                        UserName = member.UserName,
                        Email = member.Email,
                        PassWord = member.PassWord
                    };
                    db.Members.Add(khachHang);
                    db.SaveChanges();
                    Session["Account"] = db.Members.Where(x => x.UserName == member.UserName).SingleOrDefault();
                    Session["Register"] = null;

                    return Json(new
                    {
                        redirectUrl = Url.Action("Index", "Account"),
                        isRedirect = true
                    });
                }
                else if(member2 != null)
                {
                    Session["Account"] = Session["Forgot"];
                    return Json(new
                    {
                        redirectUrl = Url.Action("NewPassword", "Account", new { id =  member2.IDMember}),
                        isRedirect = true
                    });
                }
                else
                {
                    Member member1 = (Member)Session["Account"];
                    if (member1 == null)
                    {
                        Session["Account"] = Session["Login"];
                        return Json(new
                        {
                            redirectUrl = Url.Action("Index", "Account"),
                            isRedirect = true
                        });
                    }
                    return Json(new
                    {
                        isRedirect = false
                    });
                }
            }
            return Json(new
            {
                isRedirect = false
            });
        }
        public void SendMail(string Ten, string Email)
        {
            string CODE = RandomString(6);
            Session["code"] = CODE;
            var senderEmail = new MailAddress("phanvui286@gmail.com", "Karma Shop");
            var receiverEmail = new MailAddress(Email, Ten);
            var password = "thienH@iop_234";
            var sub = CODE + " is your Karma Shop login code";
            var body = "Hi " + Ten + ", you can enter this code to log into Karmashop: " + CODE;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
            }
            smtp.Dispose();
        }
        public ActionResult Update()
        {
            Member model = (Member)Session["Account"];
            if (model == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(model);
        }
        public JsonResult UpdateAccount(int id, string pass, string name, string address, string phone, string avatar)
        {
            var khachHang = db.Members.SingleOrDefault(x => x.IDMember == id);

            // check phonenumber

            var checkPhoneNumber = db.Members.SingleOrDefault(x => x.PhoneNumber == phone);
            if(checkPhoneNumber !=null)
            {
                return Json(new
                {
                    isRedirect = false,
                    mess = "Số điện thoại đã tồn tại trong hệ thống, vui lòng chọn số khác!"
                });
            }
            if (khachHang.PassWord != MaHoa.MaHoaSangMD5(khachHang.UserName + pass))
            {
                return Json(new
                {
                    isRedirect = false
                });
            }
            try
            {
                khachHang.Address = address;
                khachHang.Avatar = avatar;
                khachHang.FullName = name;
                khachHang.PhoneNumber = phone;
                db.SaveChanges();
            }
            catch
            {
                return Json(new
                {
                    isRedirect = false
                });
            }
            Session["Account"] = db.Members.Where(x => x.IDMember == id).SingleOrDefault();
            return Json(new
            {
                isRedirect = true
            });
        }
        public JsonResult TestEmail(string email, string username)
        {
            var khachHang = db.Members.Where(x => x.UserName == username).SingleOrDefault();
            var em = db.Members.Where(x => x.Email == email).SingleOrDefault();
            if (khachHang != null)
            {
                if (khachHang.Email == email || em == null)
                    return Json(new
                    {
                        isRedirect = true
                    });
            }

            else if (em == null)
                return Json(new
                {
                    isRedirect = true
                });

            return Json(new
            {
                isRedirect = false
            });
        }
        public JsonResult CheckPassword(string pass)
        {
            bool isUpper = false, isLower = false, isNumber = false, isSpecialChar = false;
            foreach (char x in pass)
            {
                if (48 <= x && x <= 57)
                    isNumber = true;
                if (65 <= x && x <= 90)
                    isLower = true;
                if (97 <= x && x <= 122)
                    isUpper = true;
                if ((33 <= x && x <= 47) || (58 <= x && x <= 64) || (91 <= x && x <= 96) || (123 <= x && x <= 126))
                    isSpecialChar = true;
            }
            if (!isLower)
                return Json(new
                {
                    isRedirect = 1
                });
            if (!isUpper)
                return Json(new
                {
                    isRedirect = 2
                });
            if (!isNumber)
                return Json(new
                {
                    isRedirect = 3
                });
            if (!isSpecialChar)
                return Json(new
                {
                    isRedirect = 4
                });
            if (pass.Length < 8)
                return Json(new
                {
                    isRedirect = 5
                });
            return Json(new
            {
                isRedirect = 0
            });
        }
        public JsonResult TestPassword(string username, string pass)
        {
            pass = MaHoa.MaHoaSangMD5(username + pass);
            var khachHang = db.Members.Where(x => x.UserName == username).SingleOrDefault();
            if (khachHang.PassWord == pass)
                return Json(new
                {
                    isRedirect = true
                });
            return Json(new
            {
                isRedirect = false
            });
        }
        public ActionResult Delete(int id)
        {
            var KhachHang = db.Members.SingleOrDefault(x => x.IDMember == id);
            var Account = new AccountVM
            {
                IDMember = KhachHang.IDMember,
                UserName = KhachHang.UserName,
                PassWord = KhachHang.PassWord,
                FullName = KhachHang.FullName,
                Address = KhachHang.Address,
                Email = KhachHang.Email,
                PhoneNumber = KhachHang.PhoneNumber,
                Avatar = KhachHang.Avatar
            };
            Member member = (Member)Session["Account"];
            if (member == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (member.IDMember != id)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(Account);
        }
        public ActionResult ChangePassword(int id)
        {
            var KhachHang = db.Members.SingleOrDefault(x => x.IDMember == id);
            var Account = new AccountVM
            {
                IDMember = KhachHang.IDMember,
                UserName = KhachHang.UserName,
                PassWord = KhachHang.PassWord,
                FullName = KhachHang.FullName,
                Address = KhachHang.Address,
                Email = KhachHang.Email,
                PhoneNumber = KhachHang.PhoneNumber,
                Avatar = KhachHang.Avatar
            };
            Member member = (Member)Session["Account"];
            if (member == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (member.IDMember != id)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(Account);
        }
        public ActionResult NewPassword(int id)
        {
            var KhachHang = db.Members.SingleOrDefault(x => x.IDMember == id);
            var Account = new AccountVM
            {
                IDMember = KhachHang.IDMember,
                UserName = KhachHang.UserName,
                PassWord = KhachHang.PassWord,
                FullName = KhachHang.FullName,
                Address = KhachHang.Address,
                Email = KhachHang.Email,
                PhoneNumber = KhachHang.PhoneNumber,
                Avatar = KhachHang.Avatar
            };
            Member member = (Member)Session["Account"];
            if (member == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (member.IDMember != id)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(Account);
        }
        public JsonResult ChangePass(int id, string newpass, string renewpass)
        {
            var KhachHang = db.Members.SingleOrDefault(x => x.IDMember == id);
            if (newpass != renewpass)
            {
                return Json(new
                {
                    isRedirect = false
                });
            }
            try
            {
                KhachHang.PassWord = MaHoa.MaHoaSangMD5(KhachHang.UserName + newpass);
                db.SaveChanges();
            }
            catch
            {
                return Json(new
                {
                    isRedirect = false
                });
            }
            return Json(new
            {
                isRedirect = true
            });
        }
        public JsonResult DeleteRecord(int Id, string Pass)
        {
            var KhachHang = db.Members.SingleOrDefault(x => x.IDMember == Id);
            if (KhachHang != null && KhachHang.PassWord == MaHoa.MaHoaSangMD5(KhachHang.UserName + Pass) && KhachHang.IDMemType != 1)
            {
                KhachHang.PassWord = null;
                db.SaveChanges();
                Session["Account"] = null;
                return Json(new
                {
                    redirectUrl = Url.Action("Index", "Account"),
                    isRedirect = true
                });
            }

            return Json(new
            {
                isRedirect = false
            });
        }
        public string RandomString(int size)
        {
            Random rand = new Random();
            StringBuilder sb = new StringBuilder();
            char c;
            for (int i = 0; i < size; i++)
            {
                c = Convert.ToChar(Convert.ToInt32(rand.Next(48, 57)));
                sb.Append(c);//thêm kí tự vào string
            }
            return sb.ToString();
        }
        public JsonResult ForgotPassword(string Email)
        {
            try
            {
                var khachHang = db.Members.Where(x => x.Email.Equals(Email) && x.PassWord != null && x.IDMemType != 1).SingleOrDefault();
                if(khachHang == null)
                {
                    return Json(new
                    {
                        isRedirect = false
                    });
                }
                Session["Forgot"] = db.Members.Where(x => x.UserName == khachHang.UserName).SingleOrDefault();
                SendMail(khachHang.FullName, Email);
            }
            catch
            {
                return Json(new
                {
                    isRedirect = false
                });
            }
            
            return Json(new
            {
                redirectUrl = Url.Action("Forgot", "Account"),
                isRedirect = true
            });
        }

        public JsonResult SendOTPLoginByPhone(string phone)
        {
            try
            {
                var khachHang = db.Members.Where(x => x.PhoneNumber.Equals(phone) && x.PassWord != null).SingleOrDefault();



                if (khachHang != null && (khachHang.PhoneNumber == "" || khachHang.PhoneNumber == null))
                {
                    string message = "Tài khoản chưa cập nhật số điện thoại, vui lòng cập nhật số điện thoại, để sử dụng tính năng này";
                    return Json(new
                    {
                        isRedirect = -5,
                        message = message
                    });
                }



                if (khachHang != null && khachHang.IsLoginByPhone == false)
                {
                    string message = "Tài khoản hiện thời không hỗ trợ đăng nhập qua" +
                        " điện thoại, vui lòng đăng nhập bằng cách khác. Và thiết lập đăng nhập qua điện thoại";
                    return Json(new
                    {
                        isRedirect = -2,
                        message = message
                    });
                }
                if (khachHang == null)
                {
                    return Json(new
                    {
                        isRedirect = -1 // Gửi thất bại Không tìm thấy số điện thoại
                    });
                }
                Member member = db.Members.Where(x => x.PhoneNumber == phone).SingleOrDefault();
                if (member!=null && member.TimeSendOTPLoginByPhone != null)
                {

                    DateTime TimeSendOTPLoginByPhone = (DateTime)member.TimeSendOTPLoginByPhone;
                    int timeSecond = DateTime.Now.Subtract(TimeSendOTPLoginByPhone).Seconds;
                    int timeSecond2 = 30 - timeSecond;
                    if (timeSecond < 30)
                    {
                        return Json(new
                        {
                            isRedirect = -500, // Mã gửi lại OTP
                            mess = "Vui lòng gửi lại OTP sau : " + timeSecond2 + " giây",
                            timeleft = timeSecond2
                        });
                    }
                }
                Session["MemberLoginByPhone"] = member;
                member.TimeSendOTPLoginByPhone = DateTime.Now;
                member.NumberOfTries = 2;
                Random rd = new Random();
                member.OTPLoginByPhone = rd.Next(100000, 999999).ToString();
                SendSMSHelper.SendSMS(member.PhoneNumber, "Vui lòng không cung cấp mã OTP cho bất kỳ ai.Mã OTP của bạn là " + member.OTPLoginByPhone.ToString());
                db.SaveChanges();
                return Json(new
                {
                    isRedirect = 1 // Gửi OTP thành công
                });
            }
            catch
            {
                return Json(new
                {
                    isRedirect = false
                });
            }
        }
        public JsonResult LoginByPhone(string OTP)
        {
            try
            {
                Member MemberLoginByPhone = (Member)Session["MemberLoginByPhone"];
                var MemberCheck = db.Members.Where(p=>p.IDMember == MemberLoginByPhone.IDMember).SingleOrDefault();
                DateTime TimecheckExpried = (DateTime)MemberCheck.TimeSendOTPLoginByPhone;
                DateTime getDateTime = DateTime.Now;
                int CheckExpried = getDateTime.Subtract(TimecheckExpried).Seconds;
                if (CheckExpried > 30)
                {
                    return Json(new
                    {
                        isRedirect = -1000, // Mã gửi lại OTP
                        mess = "Vui lòng thực hiện gửi lại OTP. Mã OTP của bạn đã hết hạn"
                    });
                }
                var Member = db.Members.Where(p => p.IDMember == MemberLoginByPhone.IDMember && p.OTPLoginByPhone == OTP).SingleOrDefault();
                Session["Account"] = Member;
                if (Member.IDMemType == 1)
                {
                    return Json(new
                    {
                        isRedirect = -1
                    });
                }
            }
            catch
            {
                return Json(new
                {
                    isRedirect = -1
                });
            }

            return Json(new
            {
                redirectUrl = Url.Action("Index", "Account"),
                isRedirect = 1
            });
        }




        [HttpPost]
        public string UploadImg(HttpPostedFileBase file)
        {
            file.SaveAs(Server.MapPath("~/App_Img/profile/" + file.FileName));
            return file.FileName;
        }
    }
}