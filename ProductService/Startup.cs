using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductService.Services;

namespace ProductService
{
    public class Startup
    {
        public IServiceProvider Setup()
        {
            var services = new ServiceCollection();
            services.AddScoped<IProductReaderService, ProductReaderService>();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            IServiceProvider provider = services.BuildServiceProvider();

            return provider;
        }
    }
}
