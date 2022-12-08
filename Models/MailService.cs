using MailKit;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
namespace KynaShop.Models
{
    public class MailService : IMailService
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly MailSettings _mailSettings;
        public MailService(Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment)
        {
            this._hostingEnvironment = _hostingEnvironment;
        }

        public async Task SendMailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse("yuhgiabao1809@gmail.com");
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using SmtpClient smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("yuhgiabao1809@gmail.com", "mtylmywyaibdcbda");
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        //send mail with template
        public async Task SendMailWithTemplateAsync(MailRequest mailRequest)
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Templates", "huhu.html");
            StreamReader str = new StreamReader(path);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[HCMSeries_FullName]", mailRequest.FullName).Replace("[NOIDUNG]", mailRequest.Body);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse("yuhgiabao1809@gmail.com");
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = $"Chúc mừng bạn {mailRequest.FullName} đã đăng ký tài khoản HCMSeries thành công";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("yuhgiabao1809@gmail.com", "mtylmywyaibdcbda");
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
