using MediatR;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Products.Commands.Notifications
{
    public class ProductCreatedNotification : INotification
    {
        public Product Product { get; }

        public ProductCreatedNotification(Product product)
        {
            Product = product;
        }
    }
}
