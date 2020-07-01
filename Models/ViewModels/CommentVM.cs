using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ViewModels
{
    public class CommentVM
    {
        public int IDComment { get; set; }

        public int IDMember { get; set; }

        public string FullName { get; set; }

        public int IDProduct { get; set; }

        public string Message { get; set; }

        public DateTime? Date { get; set; }

    }
    public class BinhLuanVM
    {
        public int IDComment { get; set; }

        public int IDMember { get; set; }

        public string FullName { get; set; }

        public int IDProduct { get; set; }

        public string Message { get; set; }

        public string Date { get; set; }

        public string Avartar { get; set; }

    }

    public class DanhGiaVM
    {
        public int IDReview { get; set; }

        public int IDMember { get; set; }

        public string FullName { get; set; }

        public int IDProduct { get; set; }

        public int Star { get; set; }

        public string Image { get; set; }

        public string Message { get; set; }

        public string Date { get; set; }

        public string Avartar { get; set; }
    }
}