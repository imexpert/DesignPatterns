using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Repositories
{
    public class ProductRepositoryMongoDb : IProductRepository
    {
        private readonly IMongoCollection<Product> _mongoCollection;

        public ProductRepositoryMongoDb(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("ProductDb");
            _mongoCollection = database.GetCollection<Product>("Products");
        }

        public async Task<Product> GetById(string id)
        {
            return await _mongoCollection.Find(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Product> Save(Product product)
        {
            await _mongoCollection.InsertOneAsync(product);
            return product;
        }

        public async Task Update(Product product)
        {
            await _mongoCollection.FindOneAndReplaceAsync(s => s.Id == product.Id, product);
        }

        public async Task Delete(Product product)
        {
            await _mongoCollection.DeleteOneAsync(s => s.Id == product.Id);
        }

        public async Task<List<Product>> GetAllByUserId(string userId)
        {
            return await _mongoCollection.Find(s => s.UserId == userId).ToListAsync();
        }
    }
}
