using System;
using System.Collections.Generic;

namespace KynaShop.Models
{
    public partial class ChiTietHoaDon
    {
        public int Stt { get; set; }
        public int? MaHoaDon { get; set; }
        public int? MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public Double TriGia { get; set; }

        public virtual HoaDon? MaHoaDonNavigation { get; set; }
        public virtual SanPham? MaSanPhamNavigation { get; set; }
    }
}
