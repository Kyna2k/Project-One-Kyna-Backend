using System;
using System.Collections.Generic;

namespace KynaShop.Models
{
    public partial class KhachHang
    {
        public KhachHang()
        {
            Comments = new HashSet<Comment>();
            HoaDons = new HashSet<HoaDon>();
        }

        public int MaKhachHang { get; set; }
        public string? SoDienThoai { get; set; } = null!;
        public string? Facebook { get; set; }
        public string? Google { get; set; }
        public string? Avatar { get; set; }
        public string? TenKhachHang { get; set; } = null!;
        public string? DiaChi { get; set; }
        public string? Email { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}
