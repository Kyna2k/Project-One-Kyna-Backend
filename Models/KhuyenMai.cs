using System;
using System.Collections.Generic;

namespace KynaShop.Models
{
    public partial class KhuyenMai
    {
        public KhuyenMai()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public int MaKhuyenMai { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public String? Hinh { get; set; }
        public byte? PhanTramKhuyenMai { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
