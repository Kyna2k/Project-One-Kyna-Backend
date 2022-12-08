using KynaShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;

namespace KynaShop.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiSanPham : Controller
    {
        private readonly KynaShopContext dpHelper;

        public ApiSanPham(KynaShopContext dpHelper)
        {
            this.dpHelper = dpHelper;
        }

        [HttpGet]
        [Route("getchitietSanPham")]
        public IActionResult getchitietSanPham(int maSanPham)
        {
            String querySP = "Exec getSanPham @MaSanPham = " + maSanPham;
            var sanPham = dpHelper.SanPhamApis.FromSqlRaw(querySP).AsEnumerable().SingleOrDefault();
            String queryCM = "Exec getComment2 @MaSanPham = " + maSanPham;
            var comment = dpHelper.CommentApis.FromSqlRaw(queryCM).AsEnumerable().ToList();
            for(int i = 0;i < comment.Count ;i++)
            {
                String queryKh = "Exec getKhachHang @MaKhachHang =" + comment[i].MaKhachHang;
                var khachHangne = dpHelper.KhachHangApis.FromSqlRaw(queryKh).AsEnumerable().SingleOrDefault();
                if(khachHangne !=null)
                {
                    comment[i].khach = khachHangne;
                }
            }
            sanPham.hinhs = dpHelper.Hinhs.Where(p => p.MaSanPham == maSanPham).ToList();
            sanPham.commentApis = comment;
            return Ok(sanPham);
        }
        [HttpGet]
        [Route("getAllSanPham")]
        public IActionResult getAllSanPham()
        {
            String querySP = "Exec getSanPhamList ";
            var sanPham = dpHelper.SanPhamApis.FromSqlRaw(querySP).AsEnumerable().ToList();
            for(int i = 0; i < sanPham.Count; i++)
            {
                sanPham[i].hinhs = dpHelper.Hinhs.Where(p => p.MaSanPham == sanPham[i].MaSanPham).ToList();
            }
            return Ok(sanPham);
        }
        [HttpGet]
        [Route("getSanPham")]
        public IActionResult getSanPham(int maSanPham)
        {
            return Ok(dpHelper.SanPhams.SingleOrDefault(p => p.MaSanPham == maSanPham));
        }
        [HttpPost]
        [Route("updateSoLuongTrongKho")]
        public IActionResult updateSoLuongTrongKho(SanPham sanPham)
        {
            var sanpham_update = dpHelper.SanPhams.SingleOrDefault(p => p.MaSanPham == sanPham.MaSanPham);
            sanpham_update.SoLuongTrongKho = sanPham.SoLuongTrongKho;
            var resutl = dpHelper.SaveChanges();
            return Ok(resutl);
        }
        [HttpGet]
        [Route("getTopSanPham")]
        public IActionResult getTopSanPham()
        {
            String querySP = "Exec getTop10";
            var SanPhams = dpHelper.Top10SanPhams.FromSqlRaw(querySP).AsEnumerable().ToList();
            if(SanPhams != null)
            {
                for (int i = 0; i < SanPhams.Count; i++)
                {
                    SanPhams[i].hinhs = dpHelper.Hinhs.Where(p => p.MaSanPham == SanPhams[i].MaSanPham).ToList();
                }
            } 
            return Ok(SanPhams);

        }
    }
}
