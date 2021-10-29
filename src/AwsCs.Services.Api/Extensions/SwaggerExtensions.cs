using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace AwsCs.Services.Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "AWS Services API",
                    Description = "A simple ASP.NET Core Web Api using Amazon Web Services",
                    Contact = new OpenApiContact
                    {
                        Name = "Wellington Júnior",
                        Email = "wellsilva.developer@gmail.com",
                        Url = new Uri("https://github.com/wellingtonJuniorDev/aws_netcore")
                    }
                });

                var assemblyname = Assembly.GetExecutingAssembly().FullName.Split(",")[0];
                var filePath = Path.Combine(AppContext.BaseDirectory, $"{assemblyname}.xml");

                if(File.Exists(filePath))
                {
                    c.IncludeXmlComments(filePath);
                }
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger";
                options.SwaggerEndpoint($"v1/swagger.json", "Aws NetCore");
                options.InjectStylesheet("/swagger-ui/SwaggerDark.css");
            });

            return app;
        }

    }
}
