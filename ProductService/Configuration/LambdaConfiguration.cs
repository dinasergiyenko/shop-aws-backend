using System.IO;
using Microsoft.Extensions.Configuration;

namespace ProductService.Configuration
{
    public class LambdaConfiguration : ILambdaConfiguration
    {
        public IConfiguration Configuration => new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

    }
}
