using MediatR;
using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Products.Queries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }
        public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
        {
            private readonly IProductDapperRepository _productDapperRepository;

            public GetProductByIdQueryHandler(IProductDapperRepository productDapperRepository)
            {
                _productDapperRepository = productDapperRepository;
            }

            public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
            {
                var product = await _productDapperRepository.GetProductById(request.Id);
                return product;
            }
        }
    }
}
