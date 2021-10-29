using AwsCs.Services.Api.Dto.Request;
using System.Threading.Tasks;

namespace AwsCs.Services.Api.Services.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Sent a email message to receivers
        /// </summary>
        /// <param name="sendMail">The params of email type</param>
        Task SendMailAsync(SendMailPost sendMail);
    }
}
