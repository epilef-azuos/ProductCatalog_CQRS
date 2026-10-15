using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Infrastructure.EntityConfiguration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Chave Primária
        builder.HasKey(p => p.Id);

        // Regras de propriedades
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Price).HasPrecision(10, 2).IsRequired();
        builder.Property(p => p.StockQuantity).IsRequired();

        // Data Seeding: Populando o banco de dados inicialmente
        builder.HasData(
            new Product(1, "Teclado Mecânico Keychron", 550.00m, 15),
            new Product(2, "Mouse Logitech MX Master 3", 480.00m, 20),
            new Product(3, "Monitor Dell 27 4K", 2100.00m, 5)
        );
    }
}