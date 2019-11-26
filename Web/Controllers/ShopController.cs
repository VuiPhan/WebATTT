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
        ShopBanDoTheThao db = new ShopBanDoTheThao();

        // GET: Shop
        public ActionResult Index(int page = 1, int IDProductType = 0)
        {
            ViewBag.IDProductType = new SelectList(db.ProductTypes.ToList(), "IDProductType", "NameProductType");

            var list = new ProductViewModel();
            list.products = db.Products.ToList();

            if (IDProductType != 0)
            {
                list.products =  db.Products.Where(x => x.IDProductType == IDProductType).ToList();
            }

            list.BlogPerPage = 8;            
            list.CurrentPage = page;

            return View(list);
        }


        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            var list = new ShopVM();
            list.comments = db.Comments.Where(x => x.IDProduct == id).ToList();
            list.product = db.Products.SingleOrDefault(x => x.IDProduct == id);
            list.IDProduct = id;
            if (list.product == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        public ActionResult Similar(int id, int MaDanhMuc)
        {
            var list = db.Products.Where(x => x.IDProduct != id && x.IDProductType == MaDanhMuc);
            return PartialView(list.ToList());
        }

        public ActionResult _Comments(int id)
        {
            CommentVM model = new CommentVM();
            if(Session["Account"] != null)
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
            foreach(var item in list)
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

        public ActionResult AddToCart(ShopVM model)
        {
            List<Cart> list = GetCart();
            var Product = db.Products.SingleOrDefault(x => x.IDProduct == model.IDProduct);

            Cart c = list.SingleOrDefault(x => x.IDProduct == model.IDProduct);
            if (c != null)
            {
                c.Amount += model.Amount;

                list.Add(c);
                Session["GioHang"] = list;

                return RedirectToAction("Details", "Shop", new { id = model.IDProduct });

            }

            var cart = new Cart();
            cart.IDProduct = model.IDProduct;
            cart.Amount = model.Amount;
            cart.Price = Product.Price;

            list.Add(cart);
            Session["GioHang"] = list;

            return RedirectToAction("Details", "Shop", new { id = model.IDProduct });
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
    }
}