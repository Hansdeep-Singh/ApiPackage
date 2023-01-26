using ApiWeb.Models;
using EfficacySend.Models;
using EfficacySend.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiWeb.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
 
    public class BroadcastController : Controller
    {
        private readonly IConfiguration configuration;
        public BroadcastController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(EmailModel em)
        {
            var SendGridConfig = configuration.GetSection("SendGridConfig").Get<SendGridConfig>();
            string Html = "<table>" +
                $"<tr><td><b>Full name</b></td><td>{em.FullName}</td></tr>" +
                $"<tr><td><b>Mobile number</b></td><td>{em.MobileNumber}</td></tr>" +
                $"<tr><td><b>Email address</b></td><td>{em.EmailAddress}</td></tr>" +
                $"<tr><td><b>Message</b></td><td>{em.Message}</td></tr>" +
                "</table>";

            var sendEmail = new Email
            {
                FromEmail = SendGridConfig.EmailFrom,
                FromName = "Corevi Website",
                ToEmail = SendGridConfig.EmailTo,
                Subject = "Contact message",
                PlainEmail = "Hi",
                HtmlEmail = Html
            };
            ISender sender = new Sender(SendGridConfig.Key);

            var apiResponse = new ApiResponse
            {
                Success = await sender.SendEmailAll(sendEmail),
                Payload = null,
                Notify = new Notify
                {
                    Success = true,
                    Message = "**Congratulations**, the email has been sent"
                }
            };
            return Ok(apiResponse);
        }
    }
}
