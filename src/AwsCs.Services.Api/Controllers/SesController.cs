using AwsCs.Services.Api.Dto.Request;
using AwsCs.Services.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AwsCs.Services.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SesController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public SesController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        /// <summary>
        /// Sent a email message to receivers
        /// </summary>
        /// <param name="sendMail">The params of email type</param>
        /// <response code="200">The email has been sent</response>
        /// <response code="500">Oops! Internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task PostAsync(SendMailPost sendMail)
        {
            await _emailService.SendMailAsync(sendMail);
        }
    }
}
