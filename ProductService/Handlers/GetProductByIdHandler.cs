using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ProductService.Constants;
using ProductService.Services;

[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace ProductService.Handlers
{
    public class GetProductByIdHandler : BaseHandler
    {
        private readonly IProductReaderService productReaderService;

        public GetProductByIdHandler()
        {
            var startup = new Startup();
            var provider = startup.Setup();

            productReaderService = provider.GetRequiredService<IProductReaderService>();
        }

        public async Task<APIGatewayProxyResponse> GetProductByIdAsync(
            APIGatewayProxyRequest request,
            ILambdaContext context)
        {
            if (request.PathParameters == null || !request.PathParameters.ContainsKey("id"))
            {
                const string errorMessage = "Retrieving of cake failed: no id has been passed.";
                Console.WriteLine(errorMessage);

                return BadRequest(errorMessage);
            }

            var idString = request.PathParameters["id"];

            if (!Guid.TryParse(idString, out var id))
            {
                var errorMessage = $"Retrieving of cake failed: '{idString}' has a wrong format.";
                Console.WriteLine(errorMessage);

                return BadRequest(errorMessage);
            }

            try
            {
                var cakes = await productReaderService.ReadCakesFromFileAsync(AppConstants.CakesFilePath);
                var cake = cakes.FirstOrDefault(c => c.Id == id);

                if (cake == null)
                {
                    var errorMessage = $"Retrieving of cake failed: no cake with '{id}' has been found.";
                    Console.WriteLine(errorMessage);

                    return NotFound(errorMessage);
                }

                Console.WriteLine($"Retrieving of cake succeed: {JsonConvert.SerializeObject(cake)}");

                return Ok(JsonConvert.SerializeObject(cake));
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
