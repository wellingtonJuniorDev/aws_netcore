using AwsCs.Core.Contracts;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace AwsCs.Services.Api.IntegrationEvents
{
    public class ConsumerMessage : IConsumer<ISendMessage>
    {
        public async Task Consume(ConsumeContext<ISendMessage> context)
        {
            var body = context.Message;
            // implement business logic

            throw new NotImplementedException();
        }
    }
}
