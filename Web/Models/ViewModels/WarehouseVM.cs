using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ViewModels
{
    public class WarehouseVM
    {
        public int IDWareHouse { get; set; }

        public string Address { get; set; }

        public int? Amount { get; set; }
    }
}