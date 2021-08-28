using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Moq;
using ProductService.Handlers;
using Xunit;

namespace ProductServiceTests
{
    public class GetProductsListHandlerTests
    {
        [Fact]
        public async Task GetAllProducts_ReadProducts_ReturnsProducts()
        {
            var handler = new GetProductListHandler();

            var response = await handler.GetProductListAsync(It.IsAny<APIGatewayProxyRequest>(), It.IsAny<ILambdaContext>());

            Assert.NotEmpty(response.Body);
        }
    }
}
