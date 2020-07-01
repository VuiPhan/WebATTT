using Web.Models.Data;
using Web.Models.DB;
using Web.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        ShopBanDoTheThao db = new ShopBanDoTheThao();
        string cs = ConfigurationManager.ConnectionStrings["ShopBanDoTheThao"].ConnectionString;

        // GET: Home        
        public ActionResult Index()
        {
            var list = new HomeVM();
            list.products = db.Products.Where(x => x.Deleted == false && x.RemainingQuantity > 0).ToList();
            var lst = new List<Deals>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand(
                    "select * from dbo.F_GetListDeals() order by Tong desc", con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Deals
                    {
                        IDProduct = Convert.ToInt32(rdr["IDProduct"]),
                        NameProduct = rdr["NameProduct"].ToString(),
                        Image = rdr["Image0"].ToString(),
                        Price = Convert.ToDecimal(rdr["Price"]),
                    });
                }
            }
            list.deals = lst;
            return View(list);
        }
        public ActionResult NavPartial()
        {
            var list = new CategoryViewModel();
            list.category = db.ProductTypes.ToList();
            return PartialView(list);
        }

        public ActionResult Error()
        {
            return View();
        }
        public ActionResult ErrorIsLock()
        {
            return View();
        }
    }
}