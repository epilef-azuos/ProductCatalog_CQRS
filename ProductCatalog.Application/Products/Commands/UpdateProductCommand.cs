using MediatR;
using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Products.Commands;

public class UpdateProductCommand : ProductCommandBase
{
    public int Id { get; set; }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(request.Id);

            if (product == null)
                throw new InvalidOperationException("Produto não encontrado.");

            product.Update(request.Name, request.Price, request.StockQuantity);

            await _unitOfWork.ProductRepository.UpdateAsync(product);
            await _unitOfWork.CommitAsync();

            return product;
        }
    }
}