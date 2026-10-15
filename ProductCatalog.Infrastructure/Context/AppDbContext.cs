using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Infrastructure.EntityConfiguration;

namespace ProductCatalog.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        // Aplica as configurações definidas nas classes EntityConfiguration
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Aplicando a configuração exatamente como na arquitetura da imagem
            builder.ApplyConfiguration(new ProductConfiguration());
        }

    }
}
