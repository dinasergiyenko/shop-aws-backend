using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Npgsql;
using ProductService.Dtos;

namespace ProductService.Handlers
{
    public class GetProductListHandler : BaseHandler
    {

        public GetProductListHandler()
        {
            var startup = new Startup();
            var provider = startup.Setup();
        }

        public async Task<APIGatewayProxyResponse> GetProductListAsync(
            APIGatewayProxyRequest request,
            ILambdaContext context)
        {
            Console.WriteLine($"GetProductList: Incoming request {JsonConvert.SerializeObject(request)}.");

            try
            {
                var cakes = new List<Cake>();

                var builder = new NpgsqlConnectionStringBuilder
                {
                    Host = Environment.GetEnvironmentVariable("PG_HOST"),
                    Port = int.Parse(Environment.GetEnvironmentVariable("PG_PORT")),
                    Username = Environment.GetEnvironmentVariable("PG_USERNAME"),
                    Password = Environment.GetEnvironmentVariable("PG_PASSWORD"),
                    Database = Environment.GetEnvironmentVariable("PG_DATABASE"),
                };

                await using (var connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("Connection to the db established.");

                    await connection.OpenAsync();

                    const string sql = "SELECT p.\"Id\", \"Title\", \"Description\", \"Price\", \"Count\" " +
                                        "FROM \"Products\" p " +
                                        "JOIN \"Stocks\" s on p.\"Id\" = s.\"ProductId\"";

                    await using (var command = new NpgsqlCommand(sql, connection))
                    {
                        await using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var i = 0;

                                var cake = new Cake
                                {
                                    Id = reader.GetGuid(i++),
                                    Title = reader.GetString(i++),
                                    Description = reader.GetString(i++),
                                    Price = reader.GetDouble(i++),
                                    Count = reader.GetInt32(i)
                                };

                                cakes.Add(cake);
                            }
                        }
                    }
                }

                Console.WriteLine($"Retrieving of products succeed: {cakes.Count} cakes have been read.");

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
