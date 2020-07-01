using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Data;
using Web.Models.ViewModels;

namespace Web.Models.DB
{
    public class DetailImportDao
    {
        ShopBanDoTheThao db = null;
        public DetailImportDao()
        {
            db = new ShopBanDoTheThao();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public IEnumerable<DetailImportModel> LoadData(int idimport)
        {
            IQueryable<DetailImportModel> model = from a in db.DetailImports
                                                  join b in db.Products
                                                  on a.IDProduct equals b.IDProduct
                                                  where a.IDImport == idimport
                                                  select new DetailImportModel()
                                                  {
                                                      IDDetailImport = a.IDDetailImport,
                                                      IDProduct = a.IDProduct,
                                                      NameProduct = b.NameProduct,
                                                      Price = a.Price,
                                                      Amount = a.Amount
                                                  };
            return model;

        }
    }
}