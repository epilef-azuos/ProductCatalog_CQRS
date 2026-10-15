using MediatR;
using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Products.Commands;

public class RemoveProductCommand : IRequest<Product>
{
    public int Id { get; set; }

    public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommand, Product>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(request.Id);

            if (product == null)
                throw new InvalidOperationException("Produto não encontrado.");

            await _unitOfWork.ProductRepository.RemoveAsync(product);
            await _unitOfWork.CommitAsync();

            return product;
        }
    }
}