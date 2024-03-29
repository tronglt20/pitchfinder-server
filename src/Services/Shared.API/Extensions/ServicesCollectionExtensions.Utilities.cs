﻿using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PitchFinder.S3;
using PitchFinder.S3.Dtos;
using PitchFinder.S3.Interfaces;
using Shared.API.Services;
using Shared.Domain.Interfaces;
using Shared.Infrastructure;


namespace Shared.API.Extensions
{
    public static partial class ServicesCollectionExtensions
    {
        public static IServiceCollection AddS3(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration
                        .GetSection("S3Settings")
                        .Get<S3Settings>();

            services.AddSingleton(provider => settings);

            var customOptions = new AWSOptions
            {
                Credentials = new BasicAWSCredentials(settings.AccessKey, settings.SecretKey),
                Region = RegionEndpoint.GetBySystemName(settings.Region)
            };
                           
            return services
                           .AddDefaultAWSOptions(customOptions)
                           .AddAWSService<IAmazonS3>()
                           .AddScoped<IS3Service, S3Service>()
                           .AddScoped(typeof(IAttachmentRepository<,>), typeof(AttachmentRepository<,>))
                           .AddScoped(typeof(AttachmentService<,>));
        }
    }
}
