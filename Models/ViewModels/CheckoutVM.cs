using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ViewModels
{
    public class CheckoutVM
    {
        public List<CartVM> carts { get; set; }
        public int IDMember { get; set; }

        public string UserName { get; set; }

        public string PassWord { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

    }

    public class CartVM
    {
        public int IDProduct { get; set; }
        public int? Amount { get; set; }
        public decimal? Price { get; set; }
        public string NameProduct { get; set; }
        public string Image0 { get; set; }
        public decimal? TotalMoney { get; set; }
    }

    public class Cart
    {    
        public int IDProduct { get; set; }

        public int? Amount { get; set; }

        public decimal? Price { get; set; }
    }
}