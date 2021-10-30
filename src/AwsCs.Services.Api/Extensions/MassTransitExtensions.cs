using AwsCs.Core.Configurations;
using AwsCs.Core.Contracts;
using AwsCs.Services.Api.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AwsCs.Services.Api.Extensions
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddSqsConfiguration(
            this IServiceCollection services,
            SqsConfiguration sqsConfiguration)
        {
            services.AddMassTransit(configure =>
            {
                configure.UsingAmazonSqs((context, sqsConfig) =>
                {
                    sqsConfig.Host(sqsConfiguration.Region, host =>
                    {
                        /*
                        * This is only for dev purposes
                        * For production environments follow the recommended practices: 
                        * https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html
                        */
                        var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
                        var secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

                        host.AccessKey(accessKey);
                        host.SecretKey(secretKey);
                    });

                    sqsConfig.Message<ISendMessage>(x => 
                        x.SetEntityName(sqsConfiguration.FirstTopicName));

                    sqsConfig.Message<IAnotherMessage>(x => 
                        x.SetEntityName(sqsConfiguration.SecondTopicName));

                    // IMPORTANT: Only in Subscriber Aplication
                    // if the aplication only publish you don't need this section below
                    sqsConfig.ReceiveEndpoint(sqsConfiguration.FirstQueueName, endpoint =>
                    {
                        // the consumer will receive sns notification, ideal for use in distribuited apps
                        endpoint.Consumer<ConsumerMessage>(); ;
                    });

                    sqsConfig.ReceiveEndpoint(sqsConfiguration.SecondQueueName, endpoint =>
                    {
                        endpoint.Consumer<ConsumerAnotherMessage>();
                    });
                });
            });

            // Only in Subscriber Aplication too
            services.AddMassTransitHostedService();

            return services;
        }
    }
}
