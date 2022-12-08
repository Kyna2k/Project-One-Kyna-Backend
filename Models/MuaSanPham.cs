using Newtonsoft.Json;

namespace KynaShop.Models
{
    public class MuaSanPham
    {
        public MuaSanPham() { }
        public MuaSanPham(int MaKhachHang, int MaSanPham, int TrangThai,int SoLuong, Double TriGia) {
            this.MaKhachHang = MaKhachHang;
            this.MaSanPham = MaSanPham;
            this.TrangThai = TrangThai;
            this.SoLuong = SoLuong;
            this.TriGia = TriGia;
        }
        public int MaKhachHang { get; set; }
        public int MaSanPham { get; set; }
        public int TrangThai { get; set; }
        public int SoLuong { get; set; }
        public Double TriGia { get; set; }
        public DateTime? NgayXuatHoaDon { get; set; }


    }
}
