using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ViewModels
{
    public class OrderVM
    {
        public int IDOrder { get; set; }

        public byte? Status { get; set; }

        public decimal? TotalMoney { get; set; }

        public int? TotalAmount { get; set; }

        public List<CartVM> carts { get; set; }

        public DateTime? OrderedDate { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}