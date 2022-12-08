using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using KynaShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace KynaShop.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiKhachHang : Controller
    {
        private readonly KynaShopContext dpHelper;
        private Cloudinary cloudinary;
        public ApiKhachHang(KynaShopContext dpHelper)
        {
            this.dpHelper = dpHelper;
            Account account = new Account(
            "dptqmou33",
            "363347973346831",
            "Wlj-qgZs2RSZT029z3Enc8svx-M");
            cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;
        }

        [HttpGet]
        [Route("getKhachHang")]
        public IActionResult getKhachHang(int maKhachHang)
        {
            return Ok(dpHelper.KhachHangs.SingleOrDefault(p => p.MaKhachHang == maKhachHang));
        }
        [HttpPost]
        [Route("updateKhachHang")]
        public IActionResult updateKhachHang(KhachHang khachHang)
        {
            dpHelper.Update(khachHang);
            var result =  dpHelper.SaveChanges();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetAllKhachHang")]
        public IActionResult GetAllKhachHang()
        {
            var list = dpHelper.KhachHangs.ToList();
            return Ok(list);
        }
        [HttpPost]
        [Route("login")]
        public IActionResult login(LoginModel loginModel)
        {
            //type 1 sdt, type google, type 3 facebook
            //Khong co result thi chuyen sang dang ky
            //xac thuc qua nam o client 

            switch(loginModel.type)
            {
                case 1:
                    var result = dpHelper.KhachHangs.SingleOrDefault(p => p.SoDienThoai == loginModel.value);
                    if(result != null)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        KhachHang kh = new KhachHang();
                        kh.SoDienThoai = loginModel.value;
                        dpHelper.KhachHangs.Add(kh);
                        dpHelper.SaveChanges();
                        return Ok(dpHelper.KhachHangs.SingleOrDefault(p => p.SoDienThoai == loginModel.value));
                    }
                    break;
                case 2:
                    var result1 = dpHelper.KhachHangs.SingleOrDefault(p => p.Google == loginModel.value);
                    if (result1 != null)
                    {
                        return Ok(result1);
                    }
                    else
                    {
                        KhachHang kh = new KhachHang();
                        kh.Google = loginModel.value;
                        dpHelper.KhachHangs.Add(kh);
                        dpHelper.SaveChanges();
                        return Ok(dpHelper.KhachHangs.SingleOrDefault(p => p.Google == loginModel.value));
                    }
                    break;
                case 3:
                    var result2 = dpHelper.KhachHangs.SingleOrDefault(p => p.Facebook == loginModel.value);
                    if (result2 != null)
                    {
                        return Ok(result2);
                    }
                    else
                    {
                        KhachHang kh = new KhachHang();
                        kh.Facebook = loginModel.value;
                        dpHelper.KhachHangs.Add(kh);
                        dpHelper.SaveChanges();
                        return Ok(dpHelper.KhachHangs.SingleOrDefault(p => p.Facebook == loginModel.value));
                    }
                    break;
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("UpdateAvartar")]
        public IActionResult UpdateAvartar([FromForm] UploadAvatarModel upload)
        {
            int x = Int32.Parse(upload.id);
            var link = "";
            //string name = file.FileName.Substring(0, file.FileName.IndexOf("."));
            IFormFile file = upload.file;
            try
            {

                var image = file.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(String.Format("{0}_{1}", "huydeptrai", upload.id), image),
                    UseFilename = true,
                };
                var uploadResult = cloudinary.Upload(uploadParams);

                link += uploadResult.SecureUrl.ToString();

                if (link == "")
                {
                    return Ok(new Message(0, "Thêm ảnh thất bại", null));
                }
            }
            catch
            {
                return Ok(new Message(0, "Thêm ảnh thất bại", null));
            }
            var khachhang = dpHelper.KhachHangs.SingleOrDefault(p => p.MaKhachHang == x);
            khachhang.Avatar = link;
            var result = dpHelper.SaveChanges();
            return Ok(result);
        }
    }
}
