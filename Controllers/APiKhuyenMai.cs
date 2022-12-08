using KynaShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KynaShop.Controllers
{
    [Route("api/")]
    [ApiController]
    public class APiKhuyenMai : Controller
    {

        private readonly KynaShopContext dpHelper;
        public APiKhuyenMai(KynaShopContext dpHelper)
        {
            this.dpHelper = dpHelper;
        }

        [HttpGet]
        [Route("getAllKhuyenMai")]
        public IActionResult getAllKhuyenMai() {
            List<KhuyenMai> khuyens = dpHelper.KhuyenMais.ToList();
            List<KhuyenMai> check = new List<KhuyenMai>();
            for(int i = 0; i < khuyens.Count;i++)
            {
                if (DateTime.Compare(DateTime.Now, khuyens[i].NgayKetThuc) <= 0)
                {
                    check.Add(khuyens[i]);
                }    
            }    
            return Ok(check);
        }
        [HttpGet]
        [Route("getSanPhamKhuyenMai")]
        public IActionResult getSanPhamKhuyenMai(int MaKhuyenMai)
        {
            String querySP = "Exec getSanPhamList ";
            var sanPham = dpHelper.SanPhamApis.FromSqlRaw(querySP).AsEnumerable().ToList();
            List<SanPhamApi> sanpham_khuyenmai = new List<SanPhamApi>();
            for (int i = 0; i < sanPham.Count; i++)
            {
                if (sanPham[i].MaKhuyenMai == MaKhuyenMai)
                {
                    sanpham_khuyenmai.Add(sanPham[i]);
                }
            }
            for (int i = 0; i < sanpham_khuyenmai.Count; i++)
            {
                sanpham_khuyenmai[i].hinhs = dpHelper.Hinhs.Where(p => p.MaSanPham == sanpham_khuyenmai[i].MaSanPham).ToList();
            }
            return Ok(sanpham_khuyenmai);

        }
        [HttpGet]
        [Route("getKhuyenMai")]
        public IActionResult getKhuyenMai(int MaKhuyenMai)
        {
            KhuyenMai khuyenMai = dpHelper.KhuyenMais.SingleOrDefault(p => p.MaKhuyenMai == MaKhuyenMai);
            if (DateTime.Compare(DateTime.Now, khuyenMai.NgayKetThuc) <= 0)
            {
                return Ok(khuyenMai);
            }
            return Ok(new KhuyenMai());
        }
    
    }
}
