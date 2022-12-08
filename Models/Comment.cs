using System;
using System.Collections.Generic;

namespace KynaShop.Models
{
    public partial class Comment
    {
        public int Stt { get; set; }
        public int? MaSanPham { get; set; }
        public int? MaKhachHang { get; set; }
        public string NoiDung { get; set; } = null!;
        public byte? Rate { get; set; }
        public DateTime? Ngay { get; set; }
        public virtual KhachHang? MaKhachHangNavigation { get; set; }
        public virtual SanPham? MaSanPhamNavigation { get; set; }
    }
}
