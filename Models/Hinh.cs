using System;
using System.Collections.Generic;

namespace KynaShop.Models
{
    public partial class Hinh
    {
        public int Mahinh { get; set; }
        public int? MaSanPham { get; set; }
        public string? Url { get; set; }

        public virtual SanPham? MaSanPhamNavigation { get; set; }
    }
}
