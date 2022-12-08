using System;
using System.Collections.Generic;

namespace KynaShop.Models
{
    public partial class NhanVienBanHang
    {
        public NhanVienBanHang()
        {
            HoaDons = new HashSet<HoaDon>();
        }

        public int MaNhanVien { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Avatar { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }

        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}
