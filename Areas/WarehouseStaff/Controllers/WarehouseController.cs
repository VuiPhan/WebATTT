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
    public class WarehouseController : BaseController
    {
        ShopBanDoTheThao db = new ShopBanDoTheThao();
        // GET: WarehouseStaff/Warehouse
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetPaggedData(int pageNumber = 1)
        {
            List<WarehouseVM> listData = db.WareHouses.Select(x => new WarehouseVM
            {
                IDWareHouse = x.IDWareHouse,
                Address = x.Address,
                Amount = x.Amount
            }).ToList();
            var pagedData = Pagination.PagedResult(listData, pageNumber, 5);
            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetById(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            WareHouse model = db.WareHouses.Where(x => x.IDWareHouse == id).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveDataInDatabase(WarehouseVM model)
        {
            var result = false;

            try
            {
                if (model.IDWareHouse > 0)
                {
                    WareHouse warehouse = db.WareHouses.SingleOrDefault(x => x.IDWareHouse == model.IDWareHouse);
                    warehouse.Address = model.Address;
                    warehouse.Amount = model.Amount;

                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    WareHouse warehouse = new WareHouse();
                    warehouse.Address = model.Address;
                    warehouse.Amount = model.Amount;

                    db.WareHouses.Add(warehouse);
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
            WareHouse warehouse = db.WareHouses.SingleOrDefault(x => x.IDWareHouse == Id);
            if (warehouse != null)
            {
                db.WareHouses.Remove(warehouse);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}