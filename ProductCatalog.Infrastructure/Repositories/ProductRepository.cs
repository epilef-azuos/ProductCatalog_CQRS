using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Infrastructure.Context;

namespace ProductCatalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        protected readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        // A implementação real dos métodos do EF Core (Add, Update, etc.) virá aqui...
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var productList = await _context.Products.ToListAsync();
            return productList ?? Enumerable.Empty<Product>();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null)
                throw new InvalidOperationException("Product not found");

            // FindAsync é a forma mais otimizada e direta de buscar pela chave primária no EF Core
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            await _context.AddAsync(product);
            return product;
        }

        public Task<Product> UpdateAsync(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            _context.Products.Update(product);
            return Task.FromResult(product);
        }

        public Task<Product> RemoveAsync(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            _context.Products.Remove(product);
            return Task.FromResult(product);
        }
    }
}
