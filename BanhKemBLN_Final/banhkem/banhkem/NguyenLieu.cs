namespace banhkem
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NguyenLieu")]
    public partial class NguyenLieu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NguyenLieu()
        {
            SanPhams = new HashSet<SanPham>();
        }

        [Key]
        public int MaNguyenLieu { get; set; }

        [StringLength(50)]
        public string TenNguyenLieu { get; set; }

        public int SoLuong { get; set; }

        [Required]
        [StringLength(50)]
        public string MaNCC { get; set; }

        public int MaLoaiNguyenLieu { get; set; }

        [Column(TypeName = "money")]
        public decimal Gia { get; set; }

        [Required]
        [StringLength(50)]
        public string TrangThai { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayNhap { get; set; }

        public virtual LoaiNguyenLieu LoaiNguyenLieu { get; set; }

        public virtual NhaCungCap NhaCungCap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
