using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Data;

namespace Web.Models.DB
{
    public class StatisticalDao
    {

        ShopBanDoTheThao db = null;
        public StatisticalDao()
        {
            db = new ShopBanDoTheThao();
            db.Configuration.ProxyCreationEnabled = false;
        }


        //Lấy tổng khách hàng
        public int TotalCustomer()
        {
            var model = db.Members.Where(x => x.IDMemType == 4);
            return model.Count();
        }

        //lấy tổng doanh thu
        public decimal? TotalMoney()
        {
            var totalmoney = db.Orders.Where(x => x.Status == 4).Sum(x => x.TotalMoney);
            return totalmoney;
        }

        //Lấy số lượng sản phẩm đã bán
        public int? SalesedQuantity()
        {
            var salesedquantity = db.Products.Sum(x => x.SalesedQuantity);
            return salesedquantity;
        }


        //Lấy số lượng sản phẩm còn lại
        public int? RemainingQuantity()
        {
            var remainingquantity = db.Products.Sum(x => x.RemainingQuantity);
            return remainingquantity;
        }


        //Lấy phần trăm tổng doanh thu theo từng tháng của năm
        public List<decimal?> TotalMoneyWithMonth()
        {
            List<decimal?> lsttotalmoney = new List<decimal?>();
            decimal? total = 1;
            for (int i = 1; i <= 12; i++)
            {
                var totalmoney = db.Orders.Where(
                    x => x.OrderedDate.Value.Month == i &&
                    x.OrderedDate.Value.Year == DateTime.Today.Year && 
                    x.Status == 4).Sum(x => x.TotalMoney);
                if (totalmoney == null)
                {
                    totalmoney = 0;
                }
                total += totalmoney;
                lsttotalmoney.Add(totalmoney);
            }
            List<decimal?> lstpercenttotalmoney = new List<decimal?>();
            for (int i = 0; i < lsttotalmoney.Count; i++)
            {
                lstpercenttotalmoney.Add(lsttotalmoney[i] / total * 100);
            }
            return lstpercenttotalmoney;
        }
    }
}