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
    public class CategoryController : BaseController
    {
        ShopBanDoTheThao db = new ShopBanDoTheThao();
        // GET: WarehouseStaff/Category
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetPaggedData(int pageNumber = 1)
        {
            List<ProductTypeVM> listData = db.ProductTypes.Select(x => new ProductTypeVM
            {
                IDProductType = x.IDProductType,
                NameProductType = x.NameProductType,
                Icon = x.Icon
            }).ToList();
            var pagedData = Pagination.PagedResult(listData, pageNumber, 4);
            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetById(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
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
            file.SaveAs(Server.MapPath("~/App_Img/productType/" + file.FileName));
            return file.FileName;
        }
    }
}