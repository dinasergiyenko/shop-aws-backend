using System;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ProductService.Constants;
using ProductService.Services;

namespace ProductService.Handlers
{
    public class GetProductListHandler : BaseHandler
    {
        private readonly IProductReaderService productReaderService;

        public GetProductListHandler()
        {
            var startup = new Startup();
            var provider = startup.Setup();

            productReaderService = provider.GetRequiredService<IProductReaderService>();
        }

        public async Task<APIGatewayProxyResponse> GetProductListAsync(
            APIGatewayProxyRequest request,
            ILambdaContext context)
        {
            try
            {
                var cakes = await productReaderService.ReadCakesFromFileAsync(AppConstants.CakesFilePath);
                Console.WriteLine($"Retrieving of products succeed: {cakes.Length} cakes have been read.");

                return Ok(JsonConvert.SerializeObject(cakes));
            }
            catch (Exception ex)
            {
                var errorMessage = $"Retrieving of products failed: {ex.Message}.";
                Console.WriteLine(errorMessage);

                return InternalServerError(errorMessage);
            }
        }
    }
}
