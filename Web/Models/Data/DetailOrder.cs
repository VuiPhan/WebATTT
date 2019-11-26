namespace Web.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DetailOrder")]
    public partial class DetailOrder
    {
        [Key]
        public int IDOrderDetail { get; set; }

        public int? IDOrder { get; set; }

        public int? IDProduct { get; set; }

        [StringLength(255)]
        public string NameProduct { get; set; }

        public int? Amount { get; set; }

        public decimal? Price { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
