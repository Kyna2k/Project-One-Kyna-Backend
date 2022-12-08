namespace KynaShop.Models
{
    public class GioHang
    {
        public int MaHoaDon { get; set; }
        public int? MaNhanVien { get; set; }
        public int? MaKhachHang { get; set; }
        public DateTime? NgayXuatHoaDon { get; set; }
        public byte? TrangThai { get; set; }

        public List<ChiTietHoaDon_GioHang> chiTietHoaDons { get; set; }
    }
}
