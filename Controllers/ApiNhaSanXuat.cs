using KynaShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KynaShop.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiNhaSanXuat : Controller
    {
        private readonly KynaShopContext dpHelper;
        public ApiNhaSanXuat(KynaShopContext dpHelper)
        {
            this.dpHelper = dpHelper;
        }
        [HttpGet]
        [Route("getAllNhaSanXuat")]
        public IActionResult getAllNhaSanXuat() {
            return Ok(dpHelper.NhaSanXuats.ToList());
        }
        [HttpGet]
        [Route("getSanPhamTheoNhaSanXuat")]
        public IActionResult getSanPhamTheoNhaSanXuat(int MaNhaSanXuat)
        {
            String querySP = "Exec getSanPhamList ";
            var sanPham = dpHelper.SanPhamApis.FromSqlRaw(querySP).AsEnumerable().ToList();
            List<SanPhamApi> sanpham_khuyenmai = new List<SanPhamApi>();
            for (int i = 0; i < sanPham.Count; i++)
            {
                if (sanPham[i].MaNhaSanXuat == MaNhaSanXuat)
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
        [Route("getNhaSanXuat")]
        public IActionResult getNhaSanXuat(int MaNhaSanXuat)
        {
            return Ok(dpHelper.NhaSanXuats.SingleOrDefault(p=>p.MaNhaSanXuat==MaNhaSanXuat));
        }
    }
}
