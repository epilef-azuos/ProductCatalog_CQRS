using FluentAssertions;
using MediatR;
using Moq;
using ProductCatalog.Application.Products.Commands;
using ProductCatalog.Application.Products.Commands.Notifications;
using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static ProductCatalog.Application.Products.Commands.CreateProductCommand;

namespace ProductCatalog.Tests.Application.Products.Commands
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            // Arrange Global: Instancia os dublês (Mocks)
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mediatorMock = new Mock<IMediator>();

            // Mock do Repositório dentro do UnitOfWork
            _unitOfWorkMock.Setup(u => u.ProductRepository.CreateAsync(It.IsAny<Product>()))
                           .ReturnsAsync((Product p) => p);

            // Injeta os Mocks no Handler real
            _handler = new CreateProductCommandHandler(_unitOfWorkMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateProduct_Commit_AndPublishNotification()
        {
            // Arrange
            var command = new CreateProductCommand { Name = "Mouse", Price = 150, StockQuantity = 20 };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Mouse");

            // Verifica se o método de criar no banco foi chamado exatamente 1 vez
            _unitOfWorkMock.Verify(u => u.ProductRepository.CreateAsync(It.IsAny<Product>()), Times.Once);

            // Verifica se o Commit foi chamado para salvar a transação
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);

            // Verifica se o evento de notificação de Email/SMS foi disparado no MediatR
            _mediatorMock.Verify(m => m.Publish(It.IsAny<ProductCreatedNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}