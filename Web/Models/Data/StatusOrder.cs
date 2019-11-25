namespace Web.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StatusOrder")]
    public partial class StatusOrder
    {
        public byte ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}
