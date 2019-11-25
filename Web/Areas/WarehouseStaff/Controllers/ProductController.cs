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
    public class ProductController : Controller
    {
        ShopBanDoTheThao db = new ShopBanDoTheThao();
        // GET: WarehouseStaff/Product
        public ActionResult Index()
        {
            ViewBag.Producer = new SelectList(db.Producers.ToList(), "IDProducer", "NameProducer");
            ViewBag.ProductType = new SelectList(db.ProductTypes.ToList(), "IDProductType", "NameProductType");
            ViewBag.WareHouse = new SelectList(db.WareHouses.ToList(), "IDWareHouse", "Address");
            ViewBag.Supplier = new SelectList(db.Suppliers.ToList(), "IDSupplier", "NameSupplier");
            return View();
        }

        public JsonResult GetList()
        {
            List<ProductVM> List = db.Products.Select(x => new ProductVM
            {
                IDProduct = x.IDProduct,
                NameProduct = x.NameProduct,
                Price =x.Price,
                YearManufacture = x.YearManufacture,
                Introduce =x.Introduce,
                Description =x.Description,
                Image0=x.Image0,
                Image1=x.Image1,
                Image2=x.Image2,
                Image3=x.Image3,
                SalesedQuantity=x.SalesedQuantity,
                New=x.New, 
                DateUpdate=x.DateUpdate,
                NameProducer=x.Producer.NameProducer,
                NameProductType =x.ProductType.NameProductType,
                Address=x.WareHouse.Address,
                Deleted=x.Deleted,
                NameSupplier=x.Supplier.NameSupplier,
                RemainingQuantity = x.RemainingQuantity
            }).ToList();

            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetById(int id)
        {
            Product model = db.Products.Where(x => x.IDProduct == id).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveDataInDatabase(ProductVM model)
        {
            var result = false;

            try
            {
                if (model.IDProduct > 0)
                {
                    Product product = db.Products.SingleOrDefault(x => x.IDProduct == model.IDProduct);
                    product.NameProduct = model.NameProduct;
                    product.Price = model.Price;
                    product.YearManufacture = model.YearManufacture;
                    product.Introduce = model.Introduce;
                    product.Description = model.Description;
                    product.Image0 = model.Image0;
                    product.Image1 = model.Image1;
                    product.Image2 = model.Image2;
                    product.Image3 = model.Image3;
                    product.SalesedQuantity = model.SalesedQuantity;
                    product.New = true;
                    product.DateUpdate = Convert.ToDateTime( DateTime.Now.ToString());
                    product.IDProducer = model.IDProducer;
                    product.IDProductType = model.IDProductType;
                    product.IDWareHouse = model.IDWareHouse;
                    product.Deleted = false;
                    product.IDSupplier = model.IDSupplier;
                    product.RemainingQuantity = model.RemainingQuantity;

                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    Product product = new Product();
                    product.NameProduct = model.NameProduct;
                    product.Price = model.Price;
                    product.YearManufacture = model.YearManufacture;
                    product.Introduce = model.Introduce;
                    product.Description = model.Description;
                    product.Image0 = model.Image0;
                    product.Image1 = model.Image1;
                    product.Image2 = model.Image2;
                    product.Image3 = model.Image3;
                    product.SalesedQuantity = 0;
                    product.New = true;
                    product.DateUpdate = Convert.ToDateTime(DateTime.Now.ToString());
                    product.IDProducer = model.IDProducer;
                    product.IDProductType = model.IDProductType;
                    product.IDWareHouse = model.IDWareHouse;
                    product.Deleted = false;
                    product.IDSupplier = model.IDSupplier;
                    product.RemainingQuantity = model.RemainingQuantity;

                    db.Products.Add(product);
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
            Product product = db.Products.SingleOrDefault(x => x.IDProduct == Id);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadImg()
        {
            List<string> url = new List<string>();
            string path = Server.MapPath("~/App_Img/product/");
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                file.SaveAs(path + file.FileName);
                url.Add(file.FileName);
            }
            return Json(url, JsonRequestBehavior.AllowGet);
        }
    }
}