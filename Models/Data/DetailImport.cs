namespace Web.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DetailImport")]
    public partial class DetailImport
    {
        [Key]
        public int IDDetailImport { get; set; }

        public int? IDImport { get; set; }

        public int? IDProduct { get; set; }

        public decimal? Price { get; set; }

        public int? Amount { get; set; }

        public virtual ImportBill ImportBill { get; set; }
    }
}
