using Ambev.DeveloperEvaluation.Application.Sales.AlterSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using FluentAssertions;
using MassTransit;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.AlterSale
{
    public class AlterSaleHandlerTests
    {
        private readonly IBus _busMock;
        private readonly ISaleRepository _saleRepositoryMock;
        private readonly AlterSaleHandler _handler;

        public AlterSaleHandlerTests()
        {
            _busMock = Substitute.For<IBus>();
            _saleRepositoryMock = Substitute.For<ISaleRepository>();
            _handler = new AlterSaleHandler(_busMock, _saleRepositoryMock);
        }

        /// <summary>
        /// Tests that a sale is altered and an event is published when the handle method is called.
        /// </summary>
        [Fact(DisplayName = "Should alter sale and publish event")]
        public async Task Handle_ShouldAlterSaleAndPublishEvent()
        {
            // Arrange
            var alterSale = AlterSaleHandlerTestData.GenerateAlterSaleCommand();
            var sale = new Sale { SaleNumber = alterSale.SaleNumber };
            _saleRepositoryMock.GetBySaleNumberAsync(alterSale.SaleNumber).Returns(sale);

            // Act
            var result = await _handler.Handle(alterSale, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            await _saleRepositoryMock.Received(1).UpdateAsync(Arg.Any<Sale>());
            await _busMock.Received(1).Publish(Arg.Any<SaleAltered>(), Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that the handle method returns false when the sale is not found.
        /// </summary>
        [Fact(DisplayName = "Should return false when sale not found")]
        public async Task Handle_ShouldReturnFalse_WhenSaleNotFound()
        {
            // Arrange
            var alterSale = AlterSaleHandlerTestData.GenerateAlterSaleCommand();
            _saleRepositoryMock.GetBySaleNumberAsync(alterSale.SaleNumber).Returns((Sale)null);

            // Act
            var result = await _handler.Handle(alterSale, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Tests that an exception is thrown when sale update fails.
        /// </summary>
        [Fact(DisplayName = "Should throw exception when sale update fails")]
        public async Task Handle_ShouldThrowException_WhenSaleUpdateFails()
        {
            // Arrange
            var alterSale = AlterSaleHandlerTestData.GenerateAlterSaleCommand();
            var sale = new Sale { SaleNumber = alterSale.SaleNumber };
            _saleRepositoryMock.GetBySaleNumberAsync(alterSale.SaleNumber).Returns(sale);
            _saleRepositoryMock.UpdateAsync(Arg.Any<Sale>()).Throws(new Exception("Sale update failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(alterSale, CancellationToken.None));
        }

        /// <summary>
        /// Tests that an exception is thrown when event publication fails.
        /// </summary>
        [Fact(DisplayName = "Should throw exception when event publication fails")]
        public async Task Handle_ShouldThrowException_WhenEventPublicationFails()
        {
            // Arrange
            var alterSale = AlterSaleHandlerTestData.GenerateAlterSaleCommand();
            var sale = new Sale { SaleNumber = alterSale.SaleNumber };
            _saleRepositoryMock.GetBySaleNumberAsync(alterSale.SaleNumber).Returns(sale);
            _busMock.Publish(Arg.Any<SaleAltered>(), Arg.Any<CancellationToken>()).Throws(new Exception("Event publication failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(alterSale, CancellationToken.None));
        }
    }
}
