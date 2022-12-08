using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace KynaShop.Models
{
    [Keyless]
    public class ChiTietHoaDon_GioHang
    {
        public int Stt { get; set; }
        public int? MaHoaDon { get; set; }
        public int? MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public Double TriGia { get; set; }
        [NotMapped]
        public SanPham SanPham { get; set; }
    }
}
