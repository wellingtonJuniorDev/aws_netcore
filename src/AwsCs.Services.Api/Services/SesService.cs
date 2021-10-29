using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using AwsCs.Services.Api.Dto.Request;
using AwsCs.Services.Api.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace AwsCs.Services.Api.Services
{
    public class SesService : IEmailService
    {
        public async Task SendMailAsync(SendMailPost sendMail)
        {
            /*
             * This is only for dev purposes
             * For production environments follow the recommended practices: 
             * https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html
             */
            var key = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var secret = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

            using var client = new AmazonSimpleEmailServiceClient(key, secret, RegionEndpoint.USEast1);
            var sendRequest = new SendEmailRequest
            {
                Source = sendMail.Sender,
                Destination = new Destination
                {
                    ToAddresses = sendMail.ReceiversAddress,
                    CcAddresses = sendMail.ReceiversInCopy,
                    BccAddresses = sendMail.ReceiversInHiddenCopy
                },
                Message = new Message
                {
                    Subject = new Content(sendMail.Subject),
                    Body = DefineBodyContent(sendMail)
                }
            };

            await client.SendEmailAsync(sendRequest);
        }

        private Body DefineBodyContent(SendMailPost sendMail)
        {
            var bodyContent = new Body();
            if (sendMail.IsHtml)
            {
                bodyContent.Html = new Content
                {
                    Charset = "UTF-8",
                    Data = sendMail.BodyContent
                };
            }
            else
            {
                bodyContent.Text = new Content
                {
                    Charset = "UTF-8",
                    Data = sendMail.BodyContent
                };
            }

            return bodyContent;
        }
    }
}
