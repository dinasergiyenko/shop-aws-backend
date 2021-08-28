using System.Threading.Tasks;
using Amazon.Lambda.Core;
using ProductService.Dtos;

namespace ProductService.Services
{
    public interface IProductReaderService
    {
        Task<Cake[]> ReadCakesFromFileAsync(string path);
    }
}
