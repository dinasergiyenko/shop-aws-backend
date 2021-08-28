using System.IO;
using System.Threading.Tasks;
using ProductService.Constants;
using ProductService.Services;
using Xunit;

namespace ProductServiceTests
{
    public class ProductReaderServiceTests
    {
        [Fact]
        public async Task ReadCakesFromFileAsync_PassCorrectPath_ReturnsCakes()
        {
            const string path = AppConstants.CakesFilePath;
            var productReaderService = new ProductReaderService();

            var cakes = await productReaderService.ReadCakesFromFileAsync(path);

            Assert.NotEmpty(cakes);
        }

        [Fact]
        public async Task ReadCakesFromFileAsync_PassIncorrectPath_ThrowsFileNotFoundException()
        {
            const string path = "wrong-path.json";
            var productReaderService = new ProductReaderService();

            await Assert.ThrowsAsync<FileNotFoundException>(
                async () => await productReaderService.ReadCakesFromFileAsync(path));
        }
    }
}
