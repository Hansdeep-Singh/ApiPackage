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
    [Authorize]
    public class BroadcastController : Controller
    {
        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(EmailModel em)
        {
            var sendEmail = new Email
            {
                FromEmail = "hans.profession@gmail.com",
                FromName = "Hans",
                ToEmail = em.ToEmail,
                Subject = "Subject",
                PlainEmail = "Hi",
                HtmlEmail = $"<p>{em.Message}</p>"
            };
            ISender sender = new Sender("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");
            
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
