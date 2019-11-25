using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Data;

namespace Web.Models.DB
{
    public class SupplierDao
    {
        ShopBanDoTheThao db = null;
        public SupplierDao()
        {
            db = new ShopBanDoTheThao();
        }

        public long Insert(Supplier supplier)
        {
            db.Suppliers.Add(supplier);
            db.SaveChanges();
            return supplier.IDSupplier;
        }

        public void Edit(Supplier supplier)
        {
            var temp = db.Suppliers.Find(supplier.IDSupplier);
            temp.NameSupplier = supplier.NameSupplier;
            temp.Address = supplier.Address;
            temp.Email = supplier.Email;
            temp.PhoneNumber = supplier.PhoneNumber;
            temp.Fax = supplier.Fax;
            db.SaveChanges();
        }

        public bool Delete(int id)
        {
            var supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Supplier> ListAllPage(string name, int page, int pageSize)
        {
            IQueryable<Supplier> model = db.Suppliers;
            if (!string.IsNullOrEmpty(name))
                model = model.Where(x => x.NameSupplier.Contains(name));
            return model.OrderBy(x => x.IDSupplier).Skip((page - 1) * pageSize).Take(pageSize);

        }

        public Supplier FindID(int id)
        {
            return db.Suppliers.Find(id);
        }

        public int Count(string name)
        {
            IQueryable<Supplier> model = db.Suppliers;
            if (!string.IsNullOrEmpty(name))
                model = model.Where(x => x.NameSupplier.Contains(name));
            return model.Count();
        }
    }
}