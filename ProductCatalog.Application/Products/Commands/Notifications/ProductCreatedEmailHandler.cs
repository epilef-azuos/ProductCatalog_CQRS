using MediatR;
using Microsoft.Extensions.Logging;

namespace ProductCatalog.Application.Products.Commands.Notifications
{
    public class ProductCreatedEmailHandler : INotificationHandler<ProductCreatedNotification>
    {
        private readonly ILogger<ProductCreatedEmailHandler> _logger;

        public ProductCreatedEmailHandler(ILogger<ProductCreatedEmailHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
        {
            // Aqui entraria a lógica real de envio de email, RabbitMQ, ou sincronização de banco de dados
            _logger.LogInformation($"[NOTIFICAÇÃO EMAIL] O produto '{notification.Product.Name}' com preço {notification.Product.Price:C} foi adicionado ao catálogo com sucesso.");

            // lógica para enviar e-mail

            return Task.CompletedTask;
        }
    }
}
