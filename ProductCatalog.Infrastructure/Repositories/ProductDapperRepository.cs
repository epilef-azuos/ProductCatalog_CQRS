using Dapper;
using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Infrastructure.Repositories
{
    public class ProductDapperRepository : IProductDapperRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProductDapperRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            string query = "SELECT * FROM \"Products\"";
            return await _dbConnection.QueryAsync<Product>(query);
        }

        public async Task<Product> GetProductById(int id)
        {
            string query = "SELECT * FROM \"Products\" WHERE \"Id\" = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Product>(query, new { Id = id });
        }

    }
}
