using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace KynaShop.Models
{
    [Keyless]
    public class Top10SanPham
    {
        public int MaSanPham { get; set; }
        public String TenSanPham { get; set; }
        public Double GiaGoc { get; set; }
        public Double GiaBan { get; set; }
        public int SoLuongTrongKho { get; set; }
        public String? TenNhaSanXuat { get; set; }
        public String? MoTa { get; set; }
        public int? TrangThai { get; set; }
        public int? MaKhuyenMai { get; set; }
        public int? Tong { get; set; }
        [NotMapped]
        public List<Hinh> hinhs { get; set; }

    }
}
