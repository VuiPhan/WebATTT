namespace Web.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Review")]
    public partial class Review
    {
        [Key]
        public int IDReview { get; set; }

        public int IDMember { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        public int IDProduct { get; set; }

        public int Star { get; set; }

        public string Image { get; set; }

        [StringLength(250)]
        public string Message { get; set; }

        public DateTime? Date { get; set; }

        public virtual Member Member { get; set; }

        public virtual Product Product { get; set; }
    }
}
