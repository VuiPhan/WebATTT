using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Data;

namespace BanDoTheThao.Areas.SalesMan.Controllers
{
    public class HomeController : Controller
    {
        ShopBanDoTheThao db = new ShopBanDoTheThao();
        // GET: SalesMan/Home
        //git
        //Git lan2
        // git lan3
        //gitlan4
        public ActionResult Index()
        {
            return View();
        }
    }
}
