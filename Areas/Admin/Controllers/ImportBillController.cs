using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Web.Models.Data;
using Web.Models.DB;

namespace Web.Areas.Admin.Controllers
{
    public class ImportBillController : BaseController
    {
        // GET: Admin/ImportBill
        public ActionResult Index()
        {
            return View();
        }

        //Load dữ liệu lên với Ajax
        [HttpGet]
        public JsonResult LoadData(string idImport, int page, int pageSize)
        {
            var dao = new ImportBillDao();
            var model = dao.ListAllPage(idImport, page, pageSize);
            int totalRow = dao.Count(idImport);
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        //Lấy thông tin chi tiết theo ajax
        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            var dao = new DetailImportDao();
            var importbill = dao.LoadData(id);
            return Json(new
            {
                data = importbill,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Export(string filename)
        {
            if (ModelState.IsValid)
            {
                var dao = new ImportBillDao();
                Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.ActiveSheet;
                worksheet.Cells[1, 1] = "IDProducer";
                worksheet.Cells[1, 2] = "NameProducer";
                worksheet.Cells[1, 3] = "Information";
                worksheet.Cells[1, 4] = "Logo";
                int row = 2;
                foreach (ImportBill p in dao.FindAll())
                {
                    worksheet.Cells[row, 1] = p.IDImport;
                    worksheet.Cells[row, 2] = p.Amount;
                    worksheet.Cells[row, 3] = p.Price;
                    worksheet.Cells[row, 4] = p.IDSupplier;
                    row++;
                }
                workbook.SaveAs(Server.MapPath("~/Content/excels/") + filename);
                workbook.Close();
                Marshal.ReleaseComObject(workbook);
                application.Quit();
                Marshal.FinalReleaseComObject(application);
            }
            return Json(true);
        }
    }
}