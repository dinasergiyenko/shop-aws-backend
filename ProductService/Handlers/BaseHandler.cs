using System.Net;
using Amazon.Lambda.APIGatewayEvents;

namespace ProductService.Handlers
{
    public abstract class BaseHandler
    {
        protected APIGatewayProxyResponse Ok(string data)
        {
            return CreateResponse((int) HttpStatusCode.OK, data);
        }

        protected APIGatewayProxyResponse BadRequest(string errorMessage)
        {
            return CreateResponse((int) HttpStatusCode.BadRequest, errorMessage);
        }

        protected APIGatewayProxyResponse NotFound(string errorMessage)
        {
            return CreateResponse((int) HttpStatusCode.NotFound, errorMessage);
        }

        protected APIGatewayProxyResponse InternalServerError(string errorMessage)
        {
            return CreateResponse((int) HttpStatusCode.InternalServerError, errorMessage);
        }

        private APIGatewayProxyResponse CreateResponse(int statusCode, string body)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = statusCode,
                Body = body
            };
        }
    }
}
