using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using ProductService.Dtos;

namespace ProductService.Services
{
    public class ProductReaderService : IProductReaderService
    {
        public async Task<Cake[]> ReadCakesFromFileAsync(string path)
        {
            using var reader = new StreamReader(path);
            var json = await reader.ReadToEndAsync();
            var cakes = JsonConvert.DeserializeObject<Cake[]>(json);

            return cakes;
        }
    }
}
