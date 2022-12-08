using KynaShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace KynaShop.Controllers
{
    [Route("api/")]
    [ApiController]
    public class SendMailController : Controller
    {
        
        private readonly IMailService mailService;
        public SendMailController(IMailService mailService)
        {
            this.mailService = mailService;
        }

        [HttpPost]
        [Route("sendmail")]
        public async Task<IActionResult> SendMail(MailRequest request)
        {
            try
            {
                await mailService.SendMailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                Ok(ex.Message);

                throw;
            }

        }

        [HttpPost]
        [Route("sendmail_temp")]
        public async Task<IActionResult> SendEmaiWithTemplate(MailRequest request)
        {
            try
            {
                await mailService.SendMailWithTemplateAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                Ok(ex.Message);
                throw;
            }
        }
    }
}
