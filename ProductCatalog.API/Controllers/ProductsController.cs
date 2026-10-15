using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Products.Commands;
using ProductCatalog.Application.Products.Queries;
using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Domain.Entities;
using System.Threading.Tasks;

namespace ProductCatalog.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var query = new GetProductsQuery();
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var query = new GetProductByIdQuery { Id = id };
            var product = await _mediator.Send(query);

            return product != null ? Ok(product) : NotFound("Produto não encontrado.");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommand command)
        {
            if (command == null)
                return BadRequest("Dados inválidos");

            var createdProduct = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand command)
        {
            if (command == null)
                return BadRequest("Dados inválidos");

            if (id != command.Id)
                return BadRequest("O ID da rota e do comando não coincidem.");

            try
            {
                var updatedProduct = await _mediator.Send(command);
                return Ok(updatedProduct);
            }
            catch (InvalidOperationException)
            {
                return NotFound("Produto não encontrado.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var command = new RemoveProductCommand { Id = id };

            try
            {
                var deletedProduct = await _mediator.Send(command);
                return Ok(deletedProduct);
            }
            catch (InvalidOperationException)
            {
                return NotFound("Produto não encontrado.");
            }
        }

    }
}
