using System;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CloudDb
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        static void Main(string[] args)
        {
            Console.Title = "DynamoDb client";

            #region configuration
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            #endregion

            RegisterServices();

            UseServices();
            DisposeServices();
        }

        private static void UseServices()
        {
            var x = _serviceProvider.GetService<IDynamoDbService>();
            x.AddItem();
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            AddConsoleLogging();

            services.AddTransient<IDynamoDbService>(x => new DynamoDbService());

            //AWSCredentials credentials = new BasicAWSCredentials(configuration["AccessKey"], configuration["AccessSecret"]);

            //services.AddTransient(IAmazonDynamoDB, (a) =>
            //{
            //    return new AmazonDynamoDBClient(credentials, RegionEndpoint.GetBySystemName(""))
            //});

            _serviceProvider = services.BuildServiceProvider();
        }

        private static void AddConsoleLogging()
        {
            //configure console logging
            _serviceProvider
                .GetService<ILoggerFactory>().add
                .AddConsole(LogLevel.Debug);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
