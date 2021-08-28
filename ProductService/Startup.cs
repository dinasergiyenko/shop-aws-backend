using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Services;

namespace ProductService
{
    public class Startup
    {
        public IServiceProvider Setup()
        {
            var services = new ServiceCollection();
            services.AddScoped<IProductReaderService, ProductReaderService>();

            IServiceProvider provider = services.BuildServiceProvider();

            return provider;
        }
    }
}
