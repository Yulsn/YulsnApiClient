using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using YulsnApiClient.Client;

namespace YulsnApiClient.Test.Abstractions
{
    public class Setup
    {
        internal TestRepository Repository;
        public ServiceProvider ServiceProvider { get; private set; }

        public Setup()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", true)
                .Build();

            string env = configuration["yulsn-test-environment"] ?? throw new Exception("Environment is not set in settings!");

            Repository = new TestRepository(Enum.Parse<TestEnvironment>(env));

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<YulsnClient>();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddHttpClient(YulsnClient.HttpClientName);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}