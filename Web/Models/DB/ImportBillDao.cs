using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Data;

namespace Web.Models.DB
{
    public class ImportBillDao
    {
        ShopBanDoTheThao db = null;
        public ImportBillDao()
        {
            db = new ShopBanDoTheThao();
        }

        public IEnumerable<ImportBill> ListAllPage(string IdImport, int page, int pageSize)
        {
            IQueryable<ImportBill> model = db.ImportBills;
            if (!string.IsNullOrEmpty(IdImport))
                model = model.Where(x => x.IDImport.ToString().Contains(IdImport));
            return model.OrderBy(x => x.IDImport).Skip((page - 1) * pageSize).Take(pageSize);

        }

        public ImportBill FindID(int id)
        {
            return db.ImportBills.Find(id);
        }

        public int Count(string IdImport)
        {
            IQueryable<ImportBill> model = db.ImportBills;
            if (!string.IsNullOrEmpty(IdImport))
                model = model.Where(x => x.IDImport.ToString().Contains(IdImport));
            return model.Count();
        }
    }
}