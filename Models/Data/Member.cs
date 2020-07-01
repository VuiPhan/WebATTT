namespace Web.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Member")]
    public partial class Member
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            Comments = new HashSet<Comment>();
            Orders = new HashSet<Order>();
            Reviews = new HashSet<Review>();
        }

        [Key]
        public int IDMember { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        [StringLength(100)]
        public string PassWord { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string Avatar { get; set; }

        public int? IDMemType { get; set; }

        [StringLength(10)]
        public string IDCard { get; set; }

        [Column(TypeName = "money")]
        public decimal? Salary { get; set; }

        public int TwoFactor { get; set; }


        public DateTime? TimeSendOTP { get; set; }
        public string OTP { get; set; }
        public bool? IsCheckOTP { get; set; }
        public int? NumberOfTries { get; set; }
        public int? TimeLock { get; set; }
            
        public DateTime? TimeStartLock { get; set; }
        public DateTime? TimeEndLock { get; set; }

        public bool? IsLock { get; set; }


        public bool? IsLoginByPhone { get; set; }
        public string OTPLoginByPhone { get; set; }
        public DateTime? TimeSendOTPLoginByPhone { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual MemType MemType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
