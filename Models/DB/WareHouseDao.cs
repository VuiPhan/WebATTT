using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Data;

namespace Web.Models.DB
{
    public class WareHouseDao
    {
        ShopBanDoTheThao db = null;
        public WareHouseDao()
        {
            db = new ShopBanDoTheThao();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public long Insert(WareHouse warehouse)
        {
            db.WareHouses.Add(warehouse);
            db.SaveChanges();
            return warehouse.IDWareHouse;
        }

        public void Edit(WareHouse warehouse)
        {
            var temp = db.WareHouses.Find(warehouse.IDWareHouse);
            temp.Address = warehouse.Address;
            temp.Amount = warehouse.Amount;
            db.SaveChanges();
        }

        public bool Delete(int id)
        {
            var warehouse = db.WareHouses.Find(id);
            db.WareHouses.Remove(warehouse);
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

        public IEnumerable<WareHouse> ListAllPage(string name, int page, int pageSize)
        {
            IQueryable<WareHouse> model = db.WareHouses;
            if (!string.IsNullOrEmpty(name))
                model = model.Where(x => x.Address.Contains(name));
            return model.OrderBy(x => x.IDWareHouse).Skip((page - 1) * pageSize).Take(pageSize);

        }

        public WareHouse FindID(int id)
        {
            return db.WareHouses.Find(id);
        }

        public int Count(string name)
        {
            IQueryable<WareHouse> model = db.WareHouses;
            if (!string.IsNullOrEmpty(name))
                model = model.Where(x => x.Address.Contains(name));
            return model.Count();
        }
    }
}