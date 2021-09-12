using System;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using ProductService.Dtos;

[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace ProductService.Handlers
{
    public class GetProductByIdHandler : BaseHandler
    {
        public GetProductByIdHandler()
        {
            var startup = new Startup();
            var provider = startup.Setup();
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
                var builder = new NpgsqlConnectionStringBuilder
                {
                    Host = Environment.GetEnvironmentVariable("PG_HOST"),
                    Port = int.Parse(Environment.GetEnvironmentVariable("PG_PORT")),
                    Username = Environment.GetEnvironmentVariable("PG_USERNAME"),
                    Password = Environment.GetEnvironmentVariable("PG_PASSWORD"),
                    Database = Environment.GetEnvironmentVariable("PG_DATABASE"),
                };

                Cake cake = null;

                await using (var connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("Connection to the db established.");

                    await connection.OpenAsync();

                    const string sql = "SELECT p.\"Id\", \"Title\", \"Description\", \"Price\", \"Count\" " +
                                       "FROM \"Products\" p " +
                                       "JOIN \"Stocks\" s on p.\"Id\" = s.\"ProductId\" " +
                                       "WHERE p.\"Id\" = @productId";

                    await using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("productId", NpgsqlDbType.Uuid, id);

                        await using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var i = 0;

                                cake = new Cake
                                {
                                    Id = reader.GetGuid(i++),
                                    Title = reader.GetString(i++),
                                    Description = reader.GetString(i++),
                                    Price = reader.GetDouble(i++),
                                    Count = reader.GetInt32(i)
                                };
                            }
                        }
                    }
                }

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
