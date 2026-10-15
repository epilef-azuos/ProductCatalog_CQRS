using ProductCatalog.Domain.Validation;

namespace ProductCatalog.Domain.Entities
{
    public sealed class Product : Entity
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }

        // Construtor vazio para o EF Core
        private Product() { }

        // Construtor para criação de novo registro
        public Product(string name, decimal price, int stockQuantity)
        {
            ValidateDomain(name, price, stockQuantity);
        }

        // Construtor para materialização (recebendo o ID do banco)
        public Product(int id, string name, decimal price, int stockQuantity)
        {
            DomainValidation.When(id < 0, "Invalid Id value.");
            Id = id;
            ValidateDomain(name, price, stockQuantity);
        }

        // Método para atualizar a entidade
        public void Update(string name, decimal price, int stockQuantity)
        {
            ValidateDomain(name, price, stockQuantity);
        }

        // Validação centralizada e atribuição das propriedades
        private void ValidateDomain(string name, decimal price, int stockQuantity)
        {
            DomainValidation.When(string.IsNullOrEmpty(name), "Invalid name. Name is required.");
            DomainValidation.When(name.Length < 3, "Invalid name, too short, minimum 3 characters.");
            DomainValidation.When(price < 0, "Invalid price. Price must be greater than or equal to zero.");
            DomainValidation.When(stockQuantity < 0, "Invalid stock. Quantity cannot be negative.");

            Name = name;
            Price = price;
            StockQuantity = stockQuantity;
        }
    }
}
