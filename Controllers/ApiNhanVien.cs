using KynaShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KynaShop.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiNhanVien : Controller
    {
        private readonly KynaShopContext dpHelper;
        public ApiNhanVien(KynaShopContext dpHelper)
        {
            this.dpHelper = dpHelper;
        }
        [HttpGet]
        [Route("login_nhanvien")]
        public IActionResult login_nhanvien(string username, string password)
        {
            var nv = dpHelper.NhanVienBanHangs.SingleOrDefault(p=>p.Username== username && p.Password == password);
            if(nv != null)
            {
                return Ok(nv);
            }
            return BadRequest();
        }
        
        [HttpGet]
        [Route("getHoaDonXuLy")]
        public IActionResult getHoaDon(int manhanvien, int trangthai)
        {
            //Lay HoaDon KhachHang có trạng thái la 0
            List<HoaDon> hoaDons = new List<HoaDon>();
            if (manhanvien != 0)
            {
                hoaDons  = dpHelper.HoaDons.Where(p => p.MaNhanVien == manhanvien && p.TrangThai == trangthai).OrderByDescending(p => p.MaHoaDon).Distinct().ToList();

            }
            else
            {
                hoaDons = dpHelper.HoaDons.Where(p => p.TrangThai == trangthai && p.MaNhanVien == null).OrderByDescending(p => p.MaHoaDon).Distinct().ToList();

            }

            //Lay chi tiet gio gio hang
            List<GioHang> gioHang = new List<GioHang>();
            if (hoaDons != null)
            {
                for (int i = 0; i < hoaDons.Count; i++)
                {
                    GioHang gioHang1 = new GioHang();
                    gioHang1.MaKhachHang = hoaDons[i].MaKhachHang;
                    gioHang1.MaHoaDon = hoaDons[i].MaHoaDon;
                    gioHang1.NgayXuatHoaDon = hoaDons[i].NgayXuatHoaDon;
                    gioHang1.MaNhanVien = hoaDons[i].MaNhanVien;
                    gioHang1.TrangThai = hoaDons[i].TrangThai;
                    gioHang.Add(gioHang1);
                }

            }
            for (int z = 0; z < gioHang.Count; z++)
            {
                String queryCTHD = "Exec getChiTietHoaDon @MaHoaDon = " + gioHang[z].MaHoaDon;
                gioHang[z].chiTietHoaDons = dpHelper.ChiTietHoaDon_GioHangs.FromSqlRaw(queryCTHD).AsEnumerable().ToList();
                //Lay Cac San pham trong hoa don no
                for (int i = 0; i < gioHang[z].chiTietHoaDons.Count; i++)
                {
                    gioHang[z].chiTietHoaDons[i].SanPham = dpHelper.SanPhams.SingleOrDefault(p => p.MaSanPham == gioHang[z].chiTietHoaDons[i].MaSanPham);

                    if (gioHang[z].chiTietHoaDons[i].SanPham != null)
                    {
                        gioHang[z].chiTietHoaDons[i].SanPham.Hinhs = dpHelper.Hinhs.Where(p => p.MaSanPham == gioHang[z].chiTietHoaDons[i].SanPham.MaSanPham).ToList();
                    }
                }
            }



            return Ok(gioHang);
        }
        [HttpPost]
        [Route("updateHoaDonXuLy")]
        public IActionResult updateHoaDonXuLy(XuLyHoaDonModel xuLy)
        {
            HoaDon hoaDon = dpHelper.HoaDons.SingleOrDefault(p=>p.MaHoaDon== xuLy.MaHoaDon);
            if(hoaDon != null)
            {
                hoaDon.TrangThai = xuLy.TrangThai;
                dpHelper.Update(hoaDon);
                var resul = dpHelper.SaveChanges();
                return Ok(resul);
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("getInfoNhanVien")]
        public IActionResult getInfoNhanVien(int manhanvien)
        {
            var nv = dpHelper.NhanVienBanHangs.SingleOrDefault(p => p.MaNhanVien == manhanvien);
            if(nv != null)
            {
                return Ok(nv);
            }

            return BadRequest();
        }
        [HttpPost]
        [Route("updateNhanVien")]
        public IActionResult updateNhanVien(NhanVienBanHang nhanVien)
        {
            dpHelper.Update(nhanVien);
            var result = dpHelper.SaveChanges();
            return Ok(result);
        }

    }
}
