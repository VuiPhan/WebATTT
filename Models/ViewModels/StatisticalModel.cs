using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ViewModels
{
    public class StatisticalModel
    {
        //Mẫ sản phẩm
        public int? IdProduct { get; set; }

        //Mã đơn hàng
        public int? IdOrder { get; set; }

        //Mã thành viên
        public int? IdMember { get; set; }

        //Tiền thu được của 1 đơn hàng
        public decimal TotalPrice { get; set; }

        //số lượng sản phẩm đã bán
        public int QuantitySold { get; set; }

        //Ngày tạo đơn hàng 

        public DateTime DateOrderd { get; set; }
    }
}