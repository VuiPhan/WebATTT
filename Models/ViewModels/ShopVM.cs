using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Data;

namespace Web.Models.ViewModels
{
    public class ShopVM
    {
        public Product product { get; set; }
        public List<BinhLuanVM> comments { get; set; }
        public List<DanhGiaVM> reviews { get; set; }
        public int IDProduct { get; set; }
        public int Amount { get; set; }
    }
}