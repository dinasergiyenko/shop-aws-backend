using Microsoft.Extensions.Configuration;

namespace ProductService.Configuration
{
    public interface ILambdaConfiguration
    {
        public IConfiguration Configuration { get; }
    }
}
