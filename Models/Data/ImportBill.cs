namespace Web.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ImportBill")]
    public partial class ImportBill
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ImportBill()
        {
            DetailImports = new HashSet<DetailImport>();
        }

        [Key]
        public int IDImport { get; set; }

        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        public int? Amount { get; set; }

        public int? IDSupplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailImport> DetailImports { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
