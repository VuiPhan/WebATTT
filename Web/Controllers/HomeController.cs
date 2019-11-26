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
    public class HomeController : Controller
    {
        ShopBanDoTheThao db = new ShopBanDoTheThao();

        // GET: Home        
        public ActionResult Index()
        {
            var list = new HomeVM();
            list.products = db.Products.ToList();

            return View(list);
        }
        public ActionResult NavPartial()
        {
            var list = new CategoryViewModel();
            list.category = db.ProductTypes.ToList();
            return PartialView(list);
        }
        
    }
}