using Bogus;
using MyPerfectRazorExamples.Models;

namespace MyPerfectRazorExamples.Services
{
    public class ProductService
    {
        private readonly List<Product> _products = new();

        public ProductService()
        {
            var faker = new Faker<Product>()
                .RuleFor(e => e.Id, f => f.Random.Int(1))
                .RuleFor(e => e.Name, f => f.Commerce.Product())
                .RuleFor(e => e.Description, f => f.Commerce.ProductDescription())
                .RuleFor(e => e.Count, f => f.Random.UInt(0))
                .RuleFor(e => e.Price, f => f.Random.Decimal(1));
            _products.AddRange(faker.GenerateBetween(20, 20));
        }
        public Task<IEnumerable<Product>> GetProductsAsync()
        {
            return Task.FromResult(_products.AsEnumerable());
        }

        public Task<Product?> GetProductByIdAsync(int id)
        {
            return Task.FromResult(_products.FirstOrDefault(p=>p.Id == id));
        }

        public Product AddProduct(Product product)
        {
            var faker = new Faker<Product>()
                 .RuleFor(e => e.Id, f => f.Random.Int(1));
            product.Id = faker.Generate().Id;
            _products.Add(product);
            return product;
        }
    }
}
