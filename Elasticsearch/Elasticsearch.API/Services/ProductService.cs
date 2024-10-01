using Elasticsearch.API.Data;
using Microsoft.EntityFrameworkCore;
using Nest;
using Newtonsoft.Json;
using System.Text;

namespace Elasticsearch.API.Services
{

    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IElasticClient _elasticClient;

        public ProductService(AppDbContext context, IElasticClient elasticClient)
        {
            _context = context;
            _elasticClient = elasticClient;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Log to Elasticsearch
            await LogToElasticsearchAsync(product.Id, "Insert", "Product added");
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            // Log to Elasticsearch
            await LogToElasticsearchAsync(product.Id, "Update", "Product updated");
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                // Log to Elasticsearch
                await LogToElasticsearchAsync(id, "Delete", "Product deleted");
            }
        }

        private async Task LogToElasticsearchAsync(int productId, string operationType, string details)
        {
            var log = new ProductLog
            {
                ProductId = productId,
                OperationType = operationType,
                OperationDate = DateTime.Now,
                Details = details
            };

            _context.ProductLogs.Add(log);
            await _context.SaveChangesAsync();

            await _elasticClient.IndexDocumentAsync(log);
        }
    }
}
