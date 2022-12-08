using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace KynaShop.Models
{
    [Keyless]
    public class SanPhamApi
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; } 
        public Double GiaGoc { get; set; }
        public Double GiaBan { get; set; }
        public int? SoLuongTrongKho { get; set; }
        public int? MaNhaSanXuat { get; set; }
        public string? Mota { get; set; }
        public int? TrangThai { get; set; }
        public int? MaKhuyenMai { get; set; }

        public string? ThongSo { get; set; }
        public List<CommentApi> commentApis { get; set; }
        public List<Hinh> hinhs { get; set; }
        
    }
}
