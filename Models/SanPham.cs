 using System;
using System.Collections.Generic;

namespace KynaShop.Models
{
    public partial class SanPham
    {
        public SanPham()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
            Comments = new HashSet<Comment>();
            Hinhs = new HashSet<Hinh>();
        }

        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; } = null!;
        public Double GiaGoc { get; set; }
        public Double GiaBan { get; set; }
        public int? SoLuongTrongKho { get; set; }
        public int? MaNhaSanXuat { get; set; }
        public string? Mota { get; set; }
        public int? TrangThai { get; set; }
        public int? MaKhuyenMai { get; set; }
        public string? ThongSo { get; set; }

        public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }
        public virtual NhaSanXuat? MaNhaSanXuatNavigation { get; set; }
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Hinh> Hinhs { get; set; }
    }
}
