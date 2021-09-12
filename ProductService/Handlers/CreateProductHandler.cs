using System;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using ProductService.Dtos;

namespace ProductService.Handlers
{
    public class CreateProductHandler : BaseHandler
    {

        public CreateProductHandler()
        {
            var startup = new Startup();
            var provider = startup.Setup();
        }

        public async Task<APIGatewayProxyResponse> CreateProductAsync(
            APIGatewayProxyRequest request,
            ILambdaContext context)
        {
            Console.WriteLine($"CreateProductAsync: Incoming request {JsonConvert.SerializeObject(request)}.");

            Cake cake = null;

            try
            {
                cake = JsonConvert.DeserializeObject<Cake>(request.Body);

            }
            catch (Exception ex)
            {
                var errorMessage = $"Creation of product failed: cannot deserialize passed object {ex.Message}.";
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

                await using (var connection = new NpgsqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("Connection to the db established.");

                    await connection.OpenAsync();

                    const string sql = "WITH ins AS (INSERT INTO \"Products\"(\"Title\", \"Description\", \"Price\") " +
                                       "VALUES (@title, @description, @price) " +
                                       "RETURNING \"Id\") " +
                                       "INSERT INTO \"Stocks\"(\"ProductId\", \"Count\") " +
                                       "SELECT \"Id\", @count " +
                                       "FROM ins;";

                    await using (var command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("title", NpgsqlDbType.Text, cake.Title);
                        command.Parameters.AddWithValue("description", NpgsqlDbType.Text, cake.Description);
                        command.Parameters.AddWithValue("price", NpgsqlDbType.Integer, cake.Price);
                        command.Parameters.AddWithValue("count", NpgsqlDbType.Integer, cake.Count);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                Console.WriteLine($"Creation of product succeed: {request.Body} has been added.");

                return Ok("success");
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
