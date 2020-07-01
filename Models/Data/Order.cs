namespace Web.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            DetailOrders = new HashSet<DetailOrder>();
        }

        [Key]
        public int IDOrder { get; set; }

        public byte? Status { get; set; }

        public DateTime? OrderedDate { get; set; }

        public DateTime? ConfirmDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public DateTime? DeliveredDate { get; set; }

        public decimal? TotalMoney { get; set; }

        public int? TotalAmount { get; set; }

        public int? IDMember { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        public string Notes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailOrder> DetailOrders { get; set; }

        public virtual Member Member { get; set; }

        public virtual StatusOrder StatusOrder { get; set; }
    }
}
