using System;
using System.Collections.Generic;

namespace KynaShop.Models
{
    public partial class NhaSanXuat
    {
        public NhaSanXuat()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public int MaNhaSanXuat { get; set; }
        public string TenNhaSanXuat { get; set; } = null!;
        public string? Hinh { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
