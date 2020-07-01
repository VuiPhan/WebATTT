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
    public class ProducerController : BaseController
    {
        ShopBanDoTheThao db = new ShopBanDoTheThao();
        // GET: WarehouseStaff/Producer
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetPaggedData(int pageNumber = 1)
        {
            List<ProducerVM> listData = db.Producers.Select(x => new ProducerVM
            {
                IDProducer = x.IDProducer,
                NameProducer = x.NameProducer,
                Information = x.Information,
                Logo = x.Logo
            }).ToList();
            var pagedData = Pagination.PagedResult(listData, pageNumber, 4);
            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetById(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Producer model = db.Producers.Where(x => x.IDProducer == id).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveDataInDatabase(ProducerVM model)
        {
            var result = false;

            try
            {
                if (model.IDProducer > 0)
                {
                    Producer producer = db.Producers.SingleOrDefault(x => x.IDProducer == model.IDProducer);
                    producer.NameProducer = model.NameProducer;
                    producer.Information = model.Information;
                    producer.Logo = model.Logo;

                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    Producer producer = new Producer();
                    producer.NameProducer = model.NameProducer;
                    producer.Information = model.Information;
                    producer.Logo = model.Logo;

                    db.Producers.Add(producer);
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
            Producer producer = db.Producers.SingleOrDefault(x => x.IDProducer == Id);
            if (producer != null)
            {
                db.Producers.Remove(producer);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string UploadImg(HttpPostedFileBase file)
        {
            file.SaveAs(Server.MapPath("~/App_Img/producer/" + file.FileName));
            return file.FileName;
        }
    }
}