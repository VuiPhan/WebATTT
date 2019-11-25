using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ViewModels
{
    public class DetailImportModel
    {
        public int IDDetailImport { get; set; }

        public int? IDImport { get; set; }

        public int? IDProduct { get; set; }

        public string NameProduct { get; set; }

        public decimal? Price { get; set; }

        public int? Amount { get; set; }

    }    
}