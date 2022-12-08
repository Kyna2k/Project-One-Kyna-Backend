namespace KynaShop.Models
{
    public class MailRequest
    {
        public string FullName { get; set; }
        public string ToEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
