using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Data;

namespace Web.Models.DB
{
    public class ProductTypeDao
    {
        ShopBanDoTheThao db = null;
        public ProductTypeDao()
        {
            db = new ShopBanDoTheThao();
        }

        public long Insert(ProductType producttype)
        {
            db.ProductTypes.Add(producttype);
            db.SaveChanges();
            return producttype.IDProductType;
        }

        public void Edit(ProductType producttype)
        {
            var temp = db.ProductTypes.Find(producttype.IDProductType);
            temp.NameProductType = producttype.NameProductType;
            temp.Icon = producttype.Icon;
            db.SaveChanges();
        }

        public bool Delete(int id)
        {
            var producttype = db.ProductTypes.Find(id);
            db.ProductTypes.Remove(producttype);
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

        public IEnumerable<ProductType> ListAllPage(string name, int page, int pageSize)
        {
            IQueryable<ProductType> model = db.ProductTypes;
            if (!string.IsNullOrEmpty(name))
                model = model.Where(x => x.NameProductType.Contains(name));
            return model.OrderBy(x => x.IDProductType).Skip((page - 1) * pageSize).Take(pageSize);

        }

        public ProductType FindID(int id)
        {
            return db.ProductTypes.Find(id);
        }

        public int Count(string name)
        {
            IQueryable<ProductType> model = db.ProductTypes;
            if (!string.IsNullOrEmpty(name))
                model = model.Where(x => x.NameProductType.Contains(name));
            return model.Count();
        }
    }
}