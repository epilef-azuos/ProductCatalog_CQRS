using FluentAssertions;
using ProductCatalog.Domain.Entities;
using System;
using Xunit;

namespace ProductCatalog.Tests.Domain.Entities
{
    public class ProductTests
    {
        [Fact] // Indica ao xUnit que isto é um teste
        public void CreateProduct_WithValidParameters_ShouldCreateStateCorrectly()
        {
            // Arrange
            string expectedName = "Teclado Mecânico";
            decimal expectedPrice = 350.00m;
            int expectedStock = 10;

            // Act
            var product = new Product(expectedName, expectedPrice, expectedStock);

            // Assert (Usando FluentAssertions)
            product.Should().NotBeNull();
            product.Name.Should().Be(expectedName);
            product.Price.Should().Be(expectedPrice);
            product.StockQuantity.Should().Be(expectedStock);
        }

        [Fact]
        public void CreateProduct_WithNegativePrice_ShouldThrowException()
        {
            // Arrange
            string name = "Monitor";
            decimal invalidPrice = -10.00m;
            int stock = 5;

            // Act
            Action action = () => new Product(name, invalidPrice, stock);

            // Assert
            //action.Should().Throw<ArgumentException>()
            action.Should().Throw<ProductCatalog.Domain.Validation.DomainValidation>()
            .WithMessage("*price*");
            //.WithMessage("*preço*"); // Espera que a mensagem de erro contenha a palavra "preço"
        }
    }
}