using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Data;
using Web.Models.ViewModels;
using OfficeOpenXml;

namespace Web.Areas.WarehouseStaff.Controllers
{
    public class ImportProductController : BaseController
    {
        // GET: WarehouseStaff/ImportProduct
        ShopBanDoTheThao db = new ShopBanDoTheThao();

        public ActionResult Index()
        {
            ViewBag.MaNCC = db.Suppliers;
            ViewBag.ListSanPham = db.Products;
            return View();
        }
        [HttpPost]
        public ActionResult NhapHang(ImportBill model, IEnumerable<DetailImport> lstModel)
        {
            model.Amount = 0;
            model.Price = 0;
            ViewBag.MaNCC = db.Suppliers;
            ViewBag.ListSanPham = db.Products;
            // Kiểm tra dữ liệu đầu vào bằng javascript hay bên metadata đều được
            // Phải ktra để khớp với kiểu dữ liệu của database

            //Gán đã xóa = false
            
            db.ImportBills.Add(model);
            db.SaveChanges();
            // SaveChanges lần đầu để  sinh ra mã phiếu nhập gán cho lstChiTietPhieuNhap
            Product sp;
            foreach (var item in lstModel)
            {
                // Cập nhật số lượng tồn
                // vì sản phẩm trong lstModel chắc chắn có nên k tạo new SanPham
                sp = db.Products.Single(n => n.IDProduct == item.IDProduct);
                //sp.RemainingQuantity += item.Amount;
                // Gán mã phiếu nhập cho từng chi tiết phiếu nhập
                item.IDImport = model.IDImport;
            }
            // lệnh gán theo list
            db.DetailImports.AddRange(lstModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}