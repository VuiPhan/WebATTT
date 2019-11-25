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
    public class SupplierController : Controller
    {
        ShopBanDoTheThao db = new ShopBanDoTheThao();
        // GET: WarehouseStaff/Supplier
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetList()
        {
            List<SupplierVM> List = db.Suppliers.Select(x => new SupplierVM
            {
                IDSupplier = x.IDSupplier,
                NameSupplier = x.NameSupplier,
                Address = x.Address,
                Email =x.Email,
                PhoneNumber =x.PhoneNumber,
                Fax = x.Fax
            }).ToList();

            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetById(int id)
        {
            Supplier model = db.Suppliers.Where(x => x.IDSupplier == id).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveDataInDatabase(SupplierVM model)
        {
            var result = false;

            try
            {
                if (model.IDSupplier > 0)
                {
                    Supplier supplier = db.Suppliers.SingleOrDefault(x => x.IDSupplier == model.IDSupplier);
                    supplier.NameSupplier = model.NameSupplier;
                    supplier.Address = model.Address;
                    supplier.Email = model.Email;
                    supplier.PhoneNumber = model.PhoneNumber;
                    supplier.Fax = model.Fax;

                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    Supplier supplier = new Supplier();
                    supplier.NameSupplier = model.NameSupplier;
                    supplier.Address = model.Address;
                    supplier.Email = model.Email;
                    supplier.PhoneNumber = model.PhoneNumber;
                    supplier.Fax = model.Fax;

                    db.Suppliers.Add(supplier);
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
            Supplier supplier = db.Suppliers.SingleOrDefault(x => x.IDSupplier == Id);
            if (supplier != null)
            {
                db.Suppliers.Remove(supplier);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}