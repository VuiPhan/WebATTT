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
}