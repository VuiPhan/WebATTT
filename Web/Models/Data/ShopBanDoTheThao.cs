namespace Web.Models.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ShopBanDoTheThao : DbContext
    {
        public ShopBanDoTheThao()
            : base("name=ShopBanDoTheThao")
        {
        }

        public virtual DbSet<Authority> Authorities { get; set; }
        public virtual DbSet<Authority_MemType> Authority_MemType { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DetailImport> DetailImports { get; set; }
        public virtual DbSet<DetailOrder> DetailOrders { get; set; }
        public virtual DbSet<ImportBill> ImportBills { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MemType> MemTypes { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<StatusOrder> StatusOrders { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<WareHouse> WareHouses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authority>()
                .HasMany(e => e.Authority_MemType)
                .WithRequired(e => e.Authority)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.PhoneCus)
                .IsFixedLength();

            modelBuilder.Entity<DetailImport>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DetailOrder>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ImportBill>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Member>()
                .Property(e => e.Salary)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MemType>()
                .HasMany(e => e.Authority_MemType)
                .WithRequired(e => e.MemType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.TotalMoney)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.DetailOrders)
                .WithOptional(e => e.Order)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .Property(e => e.YearManufacture)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StatusOrder>()
                .HasMany(e => e.Orders)
                .WithOptional(e => e.StatusOrder)
                .HasForeignKey(e => e.Status);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.PhoneNumber)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
