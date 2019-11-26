using Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Data;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        ShopBanDoTheThao db = new ShopBanDoTheThao();

        // GET: Cart
        public ActionResult Index()
        {
            var model = new CheckoutVM();
            List<Cart> list = Session["GioHang"] as List<Cart>;
            List<CartVM> c = new List<CartVM>();

            if (list != null)
            {
                foreach (var item in list)
                {
                    CartVM cartVM = new CartVM();
                    var product = db.Products.SingleOrDefault(x => x.IDProduct == item.IDProduct);
                    cartVM.IDProduct = item.IDProduct;
                    cartVM.Image0 = product.Image0;
                    cartVM.NameProduct = product.NameProduct;
                    cartVM.Amount = item.Amount;
                    cartVM.Price = item.Price;
                    cartVM.TotalMoney = item.Price * item.Amount;
                    c.Add(cartVM);
                }
            }
            model.carts = c;
            ViewBag.Total = Total();

            return View(model);
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


    }
}