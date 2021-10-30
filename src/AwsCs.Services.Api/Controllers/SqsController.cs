using AwsCs.Core.Contracts;
using AwsCs.Services.Api.Dto.Request;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AwsCs.Services.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SqsController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public SqsController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        /// <summary>
        /// Publish a message on SNS Topic
        /// </summary>
        /// <param name="message">Message to publish on topic</param>
        /// <response code="200">The message has been publish</response>
        /// <response code="500">Oops! Internal server error</response>
        [HttpPost("send-message")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task SendMessage([FromQuery] FirstMessaegDto message)
        {
            await _publishEndpoint.Publish<ISendMessage>(new
            {
                message.Text,
                message.Value
            });
        }

        /// <summary>
        /// Publish a message on SNS Topic
        /// </summary>
        /// <param name="message">Message to publish on topic</param>
        /// <response code="200">The message has been publish</response>
        /// <response code="500">Oops! Internal server error</response>
        [HttpPost("another-message")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task AnotherdMessage([FromQuery] SecondMessageDto message)
        {
            await _publishEndpoint.Publish<IAnotherMessage>(new
            {
                message.Username,
                message.Cpf,
                message.Birth
            });
        }
    }
}
