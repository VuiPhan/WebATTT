using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Data;
using Web.Models.ViewModels;

namespace Web.Areas.WarehouseStaff.Controllers
{
    public class CategoryController : Controller
    {
        ShopBanDoTheThao db = new ShopBanDoTheThao();
        // GET: WarehouseStaff/Category
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetList()
        {
            List<ProductTypeVM> List = db.ProductTypes.Select(x => new ProductTypeVM
            {
                IDProductType = x.IDProductType,
                NameProductType = x.NameProductType,
                Icon = x.Icon
            }).ToList();

            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetById(int id)
        {
            ProductType model = db.ProductTypes.Where(x => x.IDProductType == id).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveDataInDatabase(ProductTypeVM model)
        {
            var result = false;

            try
            {
                if (model.IDProductType > 0)
                {
                    ProductType productType = db.ProductTypes.SingleOrDefault(x => x.IDProductType == model.IDProductType);
                    productType.NameProductType = model.NameProductType;
                    productType.Icon = model.Icon;

                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    ProductType productType = new ProductType();
                    productType.NameProductType = model.NameProductType;
                    productType.Icon = model.Icon;

                    db.ProductTypes.Add(productType);
                    db.SaveChanges();
                    result = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteRecord(int Id)
        {
            bool result = false;
            ProductType productType = db.ProductTypes.SingleOrDefault(x => x.IDProductType == Id);
            if (productType != null)
            {
                db.ProductTypes.Remove(productType);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string UploadImg(HttpPostedFileBase file)
        {
            file.SaveAs(Server.MapPath("~/App_Img/ProductType/" + file.FileName));
            return file.FileName;
        }
    }
}