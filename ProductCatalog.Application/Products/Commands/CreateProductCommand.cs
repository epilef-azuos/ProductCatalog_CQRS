using FluentValidation;
using MediatR;
using ProductCatalog.Application.Products.Commands.Notifications;
using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Products.Commands
{
    public class CreateProductCommand : ProductCommandBase
    {
        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
        {
            private readonly IUnitOfWork _unitOfWork;
            //private readonly IValidator<CreateProductCommand> _validator;
            private readonly IMediator _mediator;

            public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMediator mediator) //, IValidator<CreateProductCommand> validator)
            {
                _unitOfWork = unitOfWork;
                _mediator = mediator;
                //_validator = validator;
            }

            public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                // Não precisa mais, a validação vai ser feita no Pipeline
                //_validator.ValidateAndThrow(request);

                var newProduct = new Product(request.Name, request.Price, request.StockQuantity);
                await _unitOfWork.ProductRepository.CreateAsync(newProduct);
                await _unitOfWork.CommitAsync();

                await _mediator.Publish(new ProductCreatedNotification(newProduct), cancellationToken);

                return newProduct;
            }
        }
    }
}
