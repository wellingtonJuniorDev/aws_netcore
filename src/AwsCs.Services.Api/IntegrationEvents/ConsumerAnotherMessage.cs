using AwsCs.Core.Contracts;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace AwsCs.Services.Api.IntegrationEvents
{
    public class ConsumerAnotherMessage : IConsumer<IAnotherMessage>
    {
        public async Task Consume(ConsumeContext<IAnotherMessage> context)
        {
            var body = context.Message;
            // implement business logic

            throw new NotImplementedException();
        }
    }
}
