using System;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductService.Configuration;

namespace ProductService
{
    public class Startup
    {
        public IServiceProvider Setup()
        {
            var services = new ServiceCollection();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            IServiceProvider provider = services.BuildServiceProvider();
            var configuration = provider.GetService<ILambdaConfiguration>();

            return provider;
        }
    }
}
