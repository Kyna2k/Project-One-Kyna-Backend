using Microsoft.EntityFrameworkCore;

namespace KynaShop.Models
{
    [Keyless]
    public class KhachHangApi
    {
        public string TenKhachHang { get; set; }
        public string? Avatar { get; set; }
        public string Email { get; set; }
    }
}
