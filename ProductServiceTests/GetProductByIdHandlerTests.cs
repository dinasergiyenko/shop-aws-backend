using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Moq;
using ProductService.Handlers;
using Xunit;

namespace ProductServiceTests
{
    public class GetProductByIdHandlerTests
    {
        [Fact]
        public async Task GetProductById_PassNoId_ReturnsBadRequest()
        {
            var request = new Mock<APIGatewayProxyRequest>();
            request.Object.PathParameters = new Dictionary<string, string>();

            var handler = new GetProductByIdHandler();
            var response = await handler.GetProductByIdAsync(request.Object, It.IsAny<ILambdaContext>());

            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetProductById_PassIdInWrongFormat_ReturnsBadRequest()
        {
            var request = new Mock<APIGatewayProxyRequest>();

            request.Object.PathParameters = new Dictionary<string, string>
            {
                { "id", "wrong-format-of-id"}
            };

            var handler = new GetProductByIdHandler();
            var response = await handler.GetProductByIdAsync(request.Object, It.IsAny<ILambdaContext>());

            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetProductById_PassNotExistingId_ReturnsNotFound()
        {
            var request = new Mock<APIGatewayProxyRequest>();

            request.Object.PathParameters = new Dictionary<string, string>
            {
                { "id", "d85b73b4-15a2-422f-a8f9-ec72bd28fbf4" }
            };

            var handler = new GetProductByIdHandler();
            var response = await handler.GetProductByIdAsync(request.Object, It.IsAny<ILambdaContext>());

            Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetProductById_PassExistingId_ReturnsOk()
        {
            var request = new Mock<APIGatewayProxyRequest>();

            request.Object.PathParameters = new Dictionary<string, string>
            {
                { "id", "7567ec4b-b10c-48c5-9345-fc73c48a80aa"}
            };

            var handler = new GetProductByIdHandler();
            var response = await handler.GetProductByIdAsync(request.Object, It.IsAny<ILambdaContext>());

            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        }
    }
}
