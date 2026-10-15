using MediatR;
using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Products.Queries
{
    public class GetProductsQuery : IRequest<IEnumerable<Product>>
    {
        public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
        {
            private readonly IProductDapperRepository _productDapperRepository;

            public GetProductsQueryHandler(IProductDapperRepository productDapperRepository)
            {
                _productDapperRepository = productDapperRepository;
            }

            public async Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
            {
                var products = await _productDapperRepository.GetProducts();
                return products;
            }
        }
    }
}
