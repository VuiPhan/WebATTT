using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ViewModels
{   

    public class AccountVM
    {
        public int IDMember { get; set; }

        public string UserName { get; set; }

        public string PassWord { get; set; }
        public string NewPassWord { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Avatar { get; set; }

        public int? IDMemType { get; set; }

        public string IDCard { get; set; }

        public decimal? Salary { get; set; }

        public int TwoFactor { get; set; }
    }
}