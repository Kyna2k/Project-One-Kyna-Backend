using KynaShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;

namespace KynaShop.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiNhanVien : Controller
    {
        private readonly KynaShopContext dpHelper;
        private readonly IMailService mailService;
        public ApiNhanVien(KynaShopContext dpHelper, IMailService mailService)
        {
            this.dpHelper = dpHelper;
            this.mailService = mailService;
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
                hoaDons  = dpHelper.HoaDons.Where(p => p.MaNhanVien == manhanvien && p.TrangThai == trangthai).OrderByDescending(p => p.MaHoaDon).ToList();

            }
            else
            {
                hoaDons = dpHelper.HoaDons.Where(p => p.TrangThai == trangthai && p.MaNhanVien == null).OrderByDescending(p => p.MaHoaDon).ToList();

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
        public async Task<IActionResult> updateHoaDonXuLy(XuLyHoaDonModel xuLy)
        {
               
            HoaDon hoaDon = dpHelper.HoaDons.SingleOrDefault(p=>p.MaHoaDon== xuLy.MaHoaDon);

            if(hoaDon != null)
            {
                hoaDon.TrangThai = xuLy.TrangThai;
                hoaDon.MaNhanVien = xuLy.MaNhanVien;

                dpHelper.Update(hoaDon);
                var resul = dpHelper.SaveChanges();
                if(resul > 0)
                {
                    KhachHang kh = dpHelper.KhachHangs.SingleOrDefault(p => p.MaKhachHang == hoaDon.MaKhachHang);
                    if(kh != null)
                    {
                        MailRequest mailRequest = new MailRequest();
                        mailRequest.ToEmail = kh.Email;
                        mailRequest.FullName = kh.TenKhachHang;
                        switch(xuLy.TrangThai)
                        {
                           
                            case 2:
                                mailRequest.Subject = "Kynashop Thông báo đã xác đơn hàng";
                                mailRequest.Body ="Mã hóa đơn của quý khách là "+ hoaDon.MaHoaDon + ". Vui lòng giữ máy bên mình, sẽ sớm có nhân viên liên hệ với quý khách";
                                break;
                            case 3:
                                mailRequest.Subject = "Cảm ơn quý khách đã mua hàng tại KynaShop";
                                mailRequest.Body = "Mã hóa đơn của quý khách là " + hoaDon.MaHoaDon + ". Rất mong được tiếp tục đồng hành cùng quý khách";
                                break;

                        }
                        await mailService.SendMailWithTemplateAsync(mailRequest);

                    }
                }
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
        [HttpPost]
        [Route("HuyDonHang")]
        public IActionResult HuyDonHang(int MaDonHang,int maNhanVien)
        {
            var hoadon =  dpHelper.HoaDons.SingleOrDefault(p => p.MaHoaDon == MaDonHang);
            hoadon.TrangThai = 255;
            if(maNhanVien == 0)
            {
                hoadon.MaNhanVien = null;
            }
            else
            {
                hoadon.MaNhanVien = maNhanVien;

            }

            dpHelper.Update(hoadon);
            var kq1 = dpHelper.SaveChanges();

            if(kq1 > 0)
            {
                var list = dpHelper.ChiTietHoaDons.Where(p => p.MaHoaDon == MaDonHang).ToList();
                for (int i = 0; i < list.Count;i++)
                {
                    var sanPham = dpHelper.SanPhams.SingleOrDefault(p => p.MaSanPham == list[i].MaSanPham);
                    sanPham.SoLuongTrongKho = sanPham.SoLuongTrongKho + list[i].SoLuong;
                    dpHelper.Update(sanPham);
                    var kq2 = dpHelper.SaveChanges();
                    return Ok(kq2);
                }    
            }
            return BadRequest();
        }
    }
}
