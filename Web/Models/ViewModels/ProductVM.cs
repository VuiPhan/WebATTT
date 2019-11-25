using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ViewModels
{
    public class ProductVM
    {
        public int IDProduct { get; set; }

        public string NameProduct { get; set; }

        public decimal? Price { get; set; }

        public string YearManufacture { get; set; }

        public string Introduce { get; set; }

        public string Description { get; set; }

        public string Image0 { get; set; }

        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }

        public int? SalesedQuantity { get; set; }

        public bool? New { get; set; }

        public DateTime? DateUpdate { get; set; }

        public int? IDProducer { get; set; }

        public int? IDProductType { get; set; }

        public int? IDWareHouse { get; set; }

        public bool? Deleted { get; set; }

        public int? IDSupplier { get; set; }

        public int? RemainingQuantity { get; set; }

        public string NameProducer { get; set; }
        public string NameProductType { get; set; }
        public string Address { get; set; }
        public string NameSupplier { get; set; }
    }
}