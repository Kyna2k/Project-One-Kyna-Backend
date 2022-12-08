using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace KynaShop.Models
{
    
    [Keyless]
    public class CommentApi
    {
        public string NoiDung { get; set; }
        public byte? Rate { get; set; }
        public DateTime? Ngay { get; set; }
        public int MaKhachHang { get; set; }
        [NotMapped]
        public KhachHangApi khach { get; set; }
    }
}
