namespace Web.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Comments = new HashSet<Comment>();
            DetailOrders = new HashSet<DetailOrder>();
        }

        [Key]
        public int IDProduct { get; set; }

        [StringLength(255)]
        public string NameProduct { get; set; }

        public decimal? Price { get; set; }

        [StringLength(4)]
        public string YearManufacture { get; set; }

        public string Introduce { get; set; }

        public string Description { get; set; }

        public string Image0 { get; set; }

        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }

        public int? SalesedQuantity { get; set; }

        public bool? New { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateUpdate { get; set; }

        public int? IDProducer { get; set; }

        public int? IDProductType { get; set; }

        public int? IDWareHouse { get; set; }

        public bool? Deleted { get; set; }

        public int? IDSupplier { get; set; }

        public int? RemainingQuantity { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailOrder> DetailOrders { get; set; }

        public virtual Producer Producer { get; set; }

        public virtual ProductType ProductType { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual WareHouse WareHouse { get; set; }
    }
}
