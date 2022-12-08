using KynaShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace KynaShop.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiHoaDon : Controller
    {
        private readonly KynaShopContext dpHelper;

        public ApiHoaDon(KynaShopContext dpHelper)
        {
            this.dpHelper = dpHelper;
        }

        [HttpGet]
        [Route("getGioHang")]
        public IActionResult getGioHang(int maKhachHang,int trangthai)
        {
            //Lay HoaDon KhachHang có trạng thái la 0
            HoaDon hoaDons = dpHelper.HoaDons.SingleOrDefault(p=>p.MaKhachHang == maKhachHang && p.TrangThai == trangthai);

            //Lay chi tiet gio gio hang
            GioHang gioHang = new GioHang();
            if (hoaDons != null)
            {
                gioHang.MaKhachHang = hoaDons.MaKhachHang;
                gioHang.MaHoaDon = hoaDons.MaHoaDon;
                gioHang.NgayXuatHoaDon = hoaDons.NgayXuatHoaDon;
                gioHang.MaNhanVien = hoaDons.MaNhanVien;
                gioHang.TrangThai = hoaDons.TrangThai;
            }
            String queryCTHD = "Exec getChiTietHoaDon @MaHoaDon = " + gioHang.MaHoaDon; 
            gioHang.chiTietHoaDons =dpHelper.ChiTietHoaDon_GioHangs.FromSqlRaw(queryCTHD).AsEnumerable().ToList();
            //Lay Cac San pham trong hoa don no
            for (int i = 0;i < gioHang.chiTietHoaDons.Count;i++)
            {
                gioHang.chiTietHoaDons[i].SanPham = dpHelper.SanPhams.SingleOrDefault(p => p.MaSanPham == gioHang.chiTietHoaDons[i].MaSanPham);
               
                if(gioHang.chiTietHoaDons[i].SanPham != null)
                { 
                    if(gioHang.chiTietHoaDons[i].SanPham.MaKhuyenMai > 0)
                    {
                        KhuyenMai khuyenMai = dpHelper.KhuyenMais.SingleOrDefault(p => p.MaKhuyenMai == gioHang.chiTietHoaDons[i].SanPham.MaKhuyenMai);
                        if(khuyenMai != null && DateTime.Compare(DateTime.Now, khuyenMai.NgayKetThuc) <= 0)
                        {
                            
                            var x = gioHang.chiTietHoaDons[i].SanPham.GiaGoc * (Double)((100 - (Double)khuyenMai.PhanTramKhuyenMai) / 100);
                            gioHang.chiTietHoaDons[i].TriGia = x;
                        }
                    }    
                    gioHang.chiTietHoaDons[i].SanPham.Hinhs = dpHelper.Hinhs.Where(p => p.MaSanPham == gioHang.chiTietHoaDons[i].SanPham.MaSanPham).ToList();
                }    
            }    
            
            return Ok(gioHang);

        }
        [HttpPost]
        [Route("addSanPhamVaoGioHang")]
        public  IActionResult addSanPhamVaoGioHang(KhachHangAddSanPhamVaoGioHang value)
        {
            var layvogiau = -100;
            HoaDon hoaDons = dpHelper.HoaDons.SingleOrDefault(p => p.MaKhachHang == value.MaKhachHang && p.TrangThai == 0);
            if (hoaDons == null)
            {
                MuaSanPham muaSanPham = new MuaSanPham(value.MaKhachHang, value.MaSanPham, 0, 1, value.TriGia);
                layvogiau = MuaSanPham(muaSanPham);
                hoaDons = dpHelper.HoaDons.SingleOrDefault(p => p.MaKhachHang == value.MaKhachHang && p.TrangThai == 0);
            }
            else
            {
                var check_sanpham = dpHelper.ChiTietHoaDons.SingleOrDefault(p => p.MaSanPham == value.MaSanPham && p.MaHoaDon == hoaDons.MaHoaDon);
                if (check_sanpham == null)
                {
                    SanPham sanPham = dpHelper.SanPhams.SingleOrDefault(p => p.MaSanPham == value.MaSanPham);
                    if(sanPham != null && sanPham.SoLuongTrongKho >= value.SoLuong)
                    {
                        ChiTietHoaDon chiTietHoaDon = new ChiTietHoaDon();
                        chiTietHoaDon.TriGia = value.TriGia;
                        chiTietHoaDon.MaSanPham = value.MaSanPham;
                        chiTietHoaDon.MaHoaDon = hoaDons.MaHoaDon;
                        chiTietHoaDon.SoLuong = value.SoLuong;
                        dpHelper.ChiTietHoaDons.Add(chiTietHoaDon);
                        var nganhL = dpHelper.SaveChanges();
                        return Ok(nganhL);
                    }
                    return Ok(-200);
                }
                else
                {
                    check_sanpham.SoLuong += value.SoLuong;
                    var result = updateSoLuongTrongGioHang(check_sanpham);
                    return Ok(result);
                }   
                
            }

            var z = dpHelper.ChiTietHoaDons.FirstOrDefault(p => p.MaSanPham == value.MaSanPham && p.MaHoaDon == hoaDons.MaHoaDon);
            if(z != null && layvogiau != -200)
            {
                return Ok(1);

            }
            else
            {
                if(layvogiau == -200)
                {
                    return Ok(layvogiau);
                }
                else
                {
                    return Ok(0);

                }
            }
        }      
        
        public int MuaSanPham(MuaSanPham muaSanPham)
        {
            var sanPham = dpHelper.SanPhams.SingleOrDefault(p => p.MaSanPham == muaSanPham.MaSanPham);
            if (muaSanPham.SoLuong <= sanPham.SoLuongTrongKho)
            {
                string query = "exec themSanPham_GioHang @MaKhachHang,@MaSanPham,@TrangThai,@SoLuong,@TriGia";
                List<SqlParameter> parms = new List<SqlParameter> {
                new SqlParameter{ParameterName = "@MaKhachHang",Value = muaSanPham.MaKhachHang},
                new SqlParameter{ParameterName = "@MaSanPham",Value = muaSanPham.MaSanPham},
                new SqlParameter{ParameterName = "@TrangThai",Value = muaSanPham.TrangThai},
                new SqlParameter{ParameterName = "@SoLuong",Value = muaSanPham.SoLuong},
                new SqlParameter{ParameterName = "@TriGia",Value = muaSanPham.TriGia}
                };
                return dpHelper.Database.ExecuteSqlRaw(query, parms);
            }
            return -200;
        }
        [HttpPost]
        [Route("MuaSanPham")]
        public IActionResult MuaSanPhamAPI(MuaSanPham muaSanPham)
        {
            var sanPham = dpHelper.SanPhams.SingleOrDefault(p => p.MaSanPham == muaSanPham.MaSanPham);
            if (muaSanPham.SoLuong <= sanPham.SoLuongTrongKho)
            {
                string query = "exec themSanPham_GioHang2 @MaKhachHang,@MaSanPham,@TrangThai,@SoLuong,@TriGia,@NgayXuatHoaDon";
                List<SqlParameter> parms = new List<SqlParameter> {
                new SqlParameter{ParameterName = "@MaKhachHang",Value = muaSanPham.MaKhachHang},
                new SqlParameter{ParameterName = "@MaSanPham",Value = muaSanPham.MaSanPham},
                new SqlParameter{ParameterName = "@TrangThai",Value = muaSanPham.TrangThai},
                new SqlParameter{ParameterName = "@SoLuong",Value = muaSanPham.SoLuong},
                new SqlParameter{ParameterName = "@TriGia",Value = muaSanPham.TriGia},
                new SqlParameter{ParameterName = "@NgayXuatHoaDon",Value = muaSanPham.NgayXuatHoaDon},
                };
                var value = dpHelper.Database.ExecuteSqlRaw(query, parms);
                if(value > 0)
                {
                    sanPham.SoLuongTrongKho = sanPham.SoLuongTrongKho - muaSanPham.SoLuong;
                    dpHelper.Update(sanPham);
                    var ketqua = dpHelper.SaveChanges();
                }    
                return Ok(value);
            }
            return Ok(0);
        }
            
        [HttpPost]
        [Route("XoaSanPhamKhoiGioHang")]
        public IActionResult XoaSanPhamKhoiGioHang(XoaHoaDonGioHang hoaDonGioHang)
        {
            var chitiethoadonxoa = dpHelper.ChiTietHoaDons.SingleOrDefault(p=> p.MaSanPham == hoaDonGioHang.MaSanPham && p.MaHoaDon == hoaDonGioHang.MaHoaDon);
            if(chitiethoadonxoa != null)
            {
                dpHelper.ChiTietHoaDons.Remove(chitiethoadonxoa);
                var result = dpHelper.SaveChanges();
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("updateSoLuongTrongGioHang")]
        public IActionResult updateSoLuongTrongGioHangApi(ChiTietHoaDon chiTietHoaDon)
        {
            
            var result = updateSoLuongTrongGioHang(chiTietHoaDon);
            //-1000 la lỗi 
            if(result == -1000)
            {
                return BadRequest();
            }
            else
            {
                return Ok(result);
            }
        }
        public int updateSoLuongTrongGioHang(ChiTietHoaDon chiTietHoaDon)
        {
            var chitiethoadon_updae = dpHelper.ChiTietHoaDons.SingleOrDefault(p => p.Stt == chiTietHoaDon.Stt);
            SanPham sanPham = dpHelper.SanPhams.SingleOrDefault(p => p.MaSanPham == chiTietHoaDon.MaSanPham);
            if (chitiethoadon_updae != null)
            {
                if(sanPham != null && sanPham.SoLuongTrongKho >= chiTietHoaDon.SoLuong)
                {
                    chitiethoadon_updae.SoLuong = chiTietHoaDon.SoLuong;
                    dpHelper.ChiTietHoaDons.Update(chitiethoadon_updae);
                    var result = dpHelper.SaveChanges();
                    return result;
                }
                else
                {
                    return -200;
                }
                
            }
          
            return -1000;
        }
        [HttpPost]
        [Route("updateTrangThaiMuaHang")]
        public IActionResult updateTrangThaiMuaHang(HoaDon hoaDon)
        {
            if(hoaDon.MaNhanVien == 0)
            {
                hoaDon.MaNhanVien= null;
            }    
            dpHelper.HoaDons.Update(hoaDon);
            var result = dpHelper.SaveChanges();
            if(result > 0)
            {
                var list = dpHelper.ChiTietHoaDons.Where(p => p.MaHoaDon == hoaDon.MaHoaDon).ToList();
                if(list.Count > 0)
                {
                    for(int i = 0; i < list.Count; i++)
                    {
                        var sanPham = dpHelper.SanPhams.SingleOrDefault(p => p.MaSanPham == list[i].MaSanPham);
                        sanPham.SoLuongTrongKho = sanPham.SoLuongTrongKho - list[i].SoLuong;
                        dpHelper.Update(sanPham);
                        dpHelper.SaveChanges();
                    }
                }
            }    
            return Ok(result);
        }
        [HttpPost]
        [Route("UpdateTriGia")]
        public IActionResult UpdateTriGia(ChiTietHoaDon chiTietHoaDon)
        {
            var chitiethoadon_updae = dpHelper.ChiTietHoaDons.SingleOrDefault(p => p.Stt == chiTietHoaDon.Stt);
            if (chitiethoadon_updae != null)
            {
                chitiethoadon_updae.TriGia = chiTietHoaDon.TriGia;
                dpHelper.ChiTietHoaDons.Update(chitiethoadon_updae);
                var result = dpHelper.SaveChanges();
                return Ok(result);
            }
            return Ok(-1000);
        }
        [HttpGet]
        [Route("getAllHoaDon")]
        public IActionResult getHoaDon(int maKhachHang, int trangthai)
        {
            //Lay HoaDon KhachHang có trạng thái la 0
            List<HoaDon> hoaDons = dpHelper.HoaDons.Where(p => p.MaKhachHang == maKhachHang && p.TrangThai != trangthai).OrderByDescending(p => p.MaHoaDon).ToList();

            //Lay chi tiet gio gio hang
            List<GioHang> gioHang = new List<GioHang>();
            if (hoaDons != null)
            {
                for(int i = 0; i < hoaDons.Count; i++)
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
            for(int z = 0; z < gioHang.Count; z++)
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
    }
    
}
