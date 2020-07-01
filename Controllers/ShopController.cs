using Web.Models.Data;
using Web.Models.DB;
using Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ShopController : Controller
    {
        readonly ShopBanDoTheThao db = new ShopBanDoTheThao();
        // GET: Shop
        public ActionResult Index(int page = 1, int IDProductType = 0, int IDProducer = 0, int show = 6)
        {            
            var list = new ProductViewModel();
            list.products = db.Products.Where(x=>x.Deleted == false && x.RemainingQuantity > 0).ToList();            
            list.productTypes = db.ProductTypes.ToList();
            list.producers = db.Producers.ToList();
            list.IDProductType = IDProductType;
            list.IDProducer = IDProducer;
            list.show = show;
            if (IDProductType != 0)
            {
                list.products = list.products.Where(x => x.IDProductType == IDProductType).ToList();
            }
            if(IDProducer != 0)
            {
                list.products = list.products.Where(x => x.IDProducer == IDProducer).ToList();
            }
            list.BlogPerPage = show;
            list.CurrentPage = page;
            return View(list);
        }      
        
        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            var pro = db.Products.Where(x => x.IDProduct == id).SingleOrDefault();
            var listS = GetListSeen();
            var proS = listS.Where(x => x.IDProduct == id).SingleOrDefault();
            if (proS == null)
            {
                listS.Add(pro);
            }
            Session["Seen"] = listS;

            var list = new ShopVM();
            List<Comment> y = db.Comments.Where(x => x.IDProduct == id).ToList();
            List<BinhLuanVM> listCmt = new List<BinhLuanVM>();
            foreach (var item in y)
            {
                BinhLuanVM binhLuanVM = new BinhLuanVM();
                binhLuanVM.IDMember = item.IDMember;
                binhLuanVM.IDProduct = item.IDProduct;
                binhLuanVM.Message = item.Message;
                var ac = db.Members.Where(x => x.IDMember == item.IDMember).SingleOrDefault();
                binhLuanVM.FullName = ac.FullName;
                binhLuanVM.IDComment = item.IDComment;
                binhLuanVM.Avartar = ac.Avatar;
                binhLuanVM.Date = String.Format("{0:f}", item.Date);
                listCmt.Add(binhLuanVM);
            }

            list.product = db.Products.SingleOrDefault(x => x.IDProduct == id);
            listCmt.Reverse();
            list.comments = listCmt;
            list.IDProduct = id;

            List<DanhGiaVM> danhGias = new List<DanhGiaVM>();
            List<Review> z = db.Reviews.Where(x => x.IDProduct == id).ToList();
            foreach(var item in z)
            {
                DanhGiaVM danhGia = new DanhGiaVM();
                danhGia.IDReview = item.IDReview;
                danhGia.IDProduct = item.IDProduct;
                danhGia.IDMember = item.IDMember;
                danhGia.Image = item.Image;
                danhGia.Message = item.Message;
                danhGia.Star = item.Star;
                danhGia.Date = String.Format("{0:f}", item.Date);
                danhGia.FullName = item.FullName;
                var ac = db.Members.Where(x => x.IDMember == item.IDMember).SingleOrDefault();
                danhGia.Avartar = ac.Avatar;
                danhGias.Add(danhGia);
            }
            danhGias.Reverse();
            list.reviews = danhGias;
            ViewBag.star1 = Convert.ToInt32(db.Reviews.Count(n => n.Star == 1 && n.IDProduct==id));
            ViewBag.star2 = Convert.ToInt32(db.Reviews.Count(n => n.Star == 2 && n.IDProduct == id));
            ViewBag.star3 = Convert.ToInt32(db.Reviews.Count(n => n.Star == 3 && n.IDProduct == id));
            ViewBag.star4 = Convert.ToInt32(db.Reviews.Count(n => n.Star == 4 && n.IDProduct == id));
            ViewBag.star5 = Convert.ToInt32(db.Reviews.Count(n => n.Star == 5 && n.IDProduct == id));
            decimal Allstar = 0;
            decimal Overall1 = 0;

            try
            {
                 Allstar = Convert.ToDecimal((db.Reviews.Where(n => n.IDProduct == id).Sum(n => n.Star)));
                Overall1 = Convert.ToDecimal((Allstar / db.Reviews.Count(n => n.IDProduct == id)));
                ViewBag.Overall = String.Format("{0:0.0}", Overall1);
                ViewBag.Count = db.Reviews.Count(n => n.IDProduct == id);
            }
            catch
            {
                ViewBag.Overall = 0;
                ViewBag.Count = 0;
            }

            if (list.product == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        public ActionResult Similar(int id, int MaDanhMuc)
        {
            var list = db.Products.Where(x => x.IDProduct != id && x.IDProductType == MaDanhMuc &&x.Deleted == false);
            return PartialView(list.ToList());
        }
        public ActionResult Seen()
        {
            List<Product> lst = GetListSeen();
            lst.Reverse();
            return PartialView(lst);
        }
        public ActionResult ListWishParialView()
        {
            var list = GetListWish();
            return PartialView(list.ToList());
        }
        public JsonResult AddListWish(int IdProduct)
        {
            List<Product> list = GetListWish();
            var Product = db.Products.SingleOrDefault(x => x.IDProduct == IdProduct);
            Product product = list.SingleOrDefault(x => x.IDProduct == IdProduct);
            
            if (product != null)
            {
                return Json(new
                {
                    mess = "Bạn đã có sản phẩm này trong mục sản phẩm yêu thích rồi",
                    status = false
                });
            }
            list.Add(Product);
            Session["ListWish"] = list;
            return Json(new
            {
                status = true
            });
        }

        public List<Product> GetListWish()
        {
            List<Product> list = Session["ListWish"] as List<Product>;
            if (list == null)
            {
                list = new List<Product>();
                Session["ListWish"] = list;
            }
            return list;
        }
        public List<Product> GetListSeen()
        {
            List<Product> list = Session["Seen"] as List<Product>;
            if (list == null)
            {
                list = new List<Product>();
                Session["Seen"] = list;
            }
            return list;
        }

        public ActionResult _Comments(int id)
        {
            CommentVM model = new CommentVM();
            if (Session["Account"] != null)
            {
                Member x = (Member)Session["Account"];
                model.IDMember = x.IDMember;
                model.FullName = x.FullName;
            }
            model.IDProduct = id;
            return PartialView(model);
        }

        public ActionResult Comments(CommentVM model)
        {
            model.Date = DateTime.Now;
            var binhLuan = new Comment();
            try
            {
                binhLuan.IDProduct = model.IDProduct;
                binhLuan.IDMember = model.IDMember;
                binhLuan.FullName = model.FullName;
                binhLuan.Message = model.Message;
                binhLuan.Date = model.Date;

                db.Comments.Add(binhLuan);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Details", "Shop", new { id = model.IDProduct });
        }
        public ActionResult DeleteComment(int id, int MaSP)
        {
            var binhLuan = db.Comments.SingleOrDefault(x => x.IDProduct == MaSP && x.IDComment == id);
            if (binhLuan != null)
            {
                db.Comments.Remove(binhLuan);
                db.SaveChanges();
            }

            return RedirectToAction("Details", "Shop", new { id = MaSP });
        }
        public ActionResult DeleteReview(int id, int MaSP)
        {
            var danhGia = db.Reviews.SingleOrDefault(x => x.IDProduct == MaSP && x.IDReview == id);
            if (danhGia != null)
            {
                db.Reviews.Remove(danhGia);
                db.SaveChanges();
            }

            return RedirectToAction("Details", "Shop", new { id = MaSP });
        }

        public List<Cart> GetCart()
        {
            List<Cart> list = Session["GioHang"] as List<Cart>;
            if (list == null)
            {
                list = new List<Cart>();
                Session["GioHang"] = list;
            }
            return list;
        }

        public decimal? Total()
        {
            List<Cart> list = Session["GioHang"] as List<Cart>;
            if (list == null)
            {
                return 0;
            }
            decimal? TongTien = 0;
            foreach (var item in list)
            {
                TongTien += item.Price * item.Amount;
            }
            return TongTien;
        }

        public int? Count()
        {
            List<Cart> list = Session["GioHang"] as List<Cart>;
            if (list == null)
            {
                return 0;
            }
            int? TongSo = 0;
            foreach (var item in list)
            {
                TongSo += item.Amount;
            }
            return TongSo;
        }
        [HttpPost]
        public JsonResult AddToCart(int? IdProduct, int? Amount)
        {

            List<Cart> list = GetCart();
            var Product = db.Products.SingleOrDefault(x => x.IDProduct == IdProduct);
            if(Product.RemainingQuantity < Amount)
            {
                return Json(new
                {
                    status = false,
                    Message = "Số lượng bạn nhập đã vượt quá số lượng tồn. Sản phẩm trong kho còn lại " + Product.RemainingQuantity
                });
            }
            Cart c = list.SingleOrDefault(x => x.IDProduct == IdProduct);
            if (c != null)
            {
                c.Amount += Amount;
               // c.Price *= Amount;
                //list.Add(c);
                Session["GioHang"] = list;
                ViewBag.Count = Count();
                ViewBag.Total = Total();
                int Quanlity = Convert.ToInt32(Count());
                decimal y = Convert.ToDecimal(Total());
                string TotalMoney = y.ToString("N0") + "VNĐ";
                //string zz1 = String.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:N0}",y);
                return Json(new
                {
                    Quanlity1 = Quanlity,
                    TotalMoney1 = TotalMoney,
                    status = true
                });

            }

            var cart = new Cart();
            cart.IDProduct = Convert.ToInt32(IdProduct);
            cart.Amount = Amount;
            cart.Price = Product.Price;
            ViewBag.Count = Count();
            ViewBag.Total = Total();
            list.Add(cart);
            Session["GioHang"] = list;
            ViewBag.Count = Count();
            ViewBag.Total = Total();
            int Quanlity2 = Convert.ToInt32(Count());
            decimal y2 = Convert.ToDecimal(Total());
            string TotalMoney2 = y2.ToString("N0") + "VNĐ";

            return Json(new
            {
                Quanlity1 = Quanlity2,
                TotalMoney1 = TotalMoney2,
                status = true
            });
        } 
        public JsonResult AddReview(int? IdProduct, string message,int? star, string Img)
        {
            Member x = (Member)Session["Account"];
            // Kiểm tra xem cái Member đã mua sản phẩm đó chưa???
            var OrderList = (from s in db.Orders
                             join t in db.DetailOrders on s.IDOrder equals t.IDOrder
                             where s.IDMember == x.IDMember && t.IDProduct==IdProduct && s.DeliveredDate != null
                             select new
                             {
                                 s.IDOrder,
                             }).ToList();
            if (OrderList.Count==0)
            {
                return Json(new
                {
                    message = "Bạn chưa từng mua sản phẩm này hoặc chưa nhận hàng sản phẩm này. Vui lòng mua sản phẩm hoặc khi nhận được sản phẩm để đánh giá sản phẩm",
                    status = false
                });
            }
            var Review = new Review();
            Review.Date = DateTime.Now;
           
            try
            {
                Review.IDProduct = Convert.ToInt32(IdProduct);
                Review.IDMember = x.IDMember;
                Review.FullName = x.FullName;
                Review.Message = message;
                Review.Image = Img;
                Review.Star = Convert.ToInt32(star);
                db.Reviews.Add(Review);
                db.SaveChanges();
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    message = ex.Message,
                    status = false
                });
            }
        }
        public JsonResult EditCart(int id, int quantity)
        {
            var Product = db.Products.SingleOrDefault(x => x.IDProduct == id);
            if (Product.RemainingQuantity < quantity)
            {
                return Json(new
                {
                    status = false,
                    Message = "Số lượng bạn nhập đã vượt quá số lượng tồn. Sản phẩm trong kho còn lại " + Product.RemainingQuantity,
                    maxSL = Product.RemainingQuantity
                });
            }
            try
            {
                List<Cart> list = GetCart();
                foreach (var item in list)
                {
                    if (item.IDProduct == id)
                    {
                        item.Amount = quantity;
                    }
                }
                Session["GioHang"] = list;
                string totalItem = ((decimal)list.Where(x => x.IDProduct == id).Sum(x => x.Price * x.Amount)).ToString("N0") + "VNĐ";
                string totalMoney = ((decimal)list.Sum(x => x.Price * x.Amount)).ToString("N0") + "VNĐ";
                int Quanlity2 = Convert.ToInt32(Count());
                return Json(new
                {
                    Count = Quanlity2,
                    totalItem = totalItem,
                    totalMoney = totalMoney,
                    status = true,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    Message = ex.Message,
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeleteItemInCart(int id)
        {
            try
            {
                List<Cart> list = GetCart(); ;
                var product = list.Find(x => x.IDProduct == id);
                list.Remove(product);
                Session["GioHang"] = list;
                int cartquantity = list.Count;
                string cartTotal = ((decimal)list.Sum(x => x.Price * x.Amount)).ToString("N0") + " VNĐ";
                int Quanlity2 = Convert.ToInt32(Count());
                return Json(new
                {
                    Count = Quanlity2,
                    //cartquantity = cartquantity,
                    cartTotal = cartTotal,
                    status = true,
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    Message = ex.Message,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CartPartial()
        {
            if (Count() == 0)
            {
                ViewBag.Count = 0;
                ViewBag.Total = 0;
                return PartialView();
            }
            ViewBag.Count = Count();
            ViewBag.Total = Total();
            return PartialView();
        }

        public ActionResult Review()
        {
            CommentVM model = new CommentVM();
            if (Session["Account"] != null)
            {
                Member x = (Member)Session["Account"];
                model.IDMember = x.IDMember;
                model.FullName = x.FullName;
            }
            model.IDProduct = 6;
            return PartialView(model);
        }
        [HttpPost]
        public string UploadImg(HttpPostedFileBase file)
        {
            file.SaveAs(Server.MapPath("~/App_Img/review/" + file.FileName));
            return file.FileName;
        }
    }
}