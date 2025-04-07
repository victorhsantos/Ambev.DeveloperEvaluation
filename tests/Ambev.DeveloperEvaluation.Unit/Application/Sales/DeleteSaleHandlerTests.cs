using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using FluentAssertions;
using MassTransit;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.DeleteSale
{
    public class DeleteSaleHandlerTests
    {
        private readonly IBus _busMock;
        private readonly ISaleRepository _saleRepositoryMock;
        private readonly DeleteSaleHandler _handler;

        public DeleteSaleHandlerTests()
        {
            _busMock = Substitute.For<IBus>();
            _saleRepositoryMock = Substitute.For<ISaleRepository>();
            _handler = new DeleteSaleHandler(_busMock, _saleRepositoryMock);
        }

        /// <summary>
        /// Tests that a sale is deleted and an event is published when the handle method is called.
        /// </summary>
        [Fact(DisplayName = "Should delete sale and publish event")]
        public async Task Handle_ShouldDeleteSaleAndPublishEvent()
        {
            // Arrange
            var deleteSale = DeleteSaleHandlerTestData.GenerateDeleteSaleCommand();
            var sale = new Sale { SaleNumber = deleteSale.SaleNumber };
            _saleRepositoryMock.GetBySaleNumberAsync(deleteSale.SaleNumber).Returns(sale);

            // Act
            var result = await _handler.Handle(deleteSale, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            await _saleRepositoryMock.Received(1).DeleteAsync(deleteSale.SaleNumber);
            await _busMock.Received(1).Publish(Arg.Any<SaleCancelled>(), Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that the handle method returns false when the sale is not found.
        /// </summary>
        [Fact(DisplayName = "Should return false when sale not found")]
        public async Task Handle_ShouldReturnFalse_WhenSaleNotFound()
        {
            // Arrange
            var deleteSale = DeleteSaleHandlerTestData.GenerateDeleteSaleCommand();
            _saleRepositoryMock.GetBySaleNumberAsync(deleteSale.SaleNumber).Returns((Sale)null);

            // Act
            var result = await _handler.Handle(deleteSale, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Tests that an exception is thrown when sale deletion fails.
        /// </summary>
        [Fact(DisplayName = "Should throw exception when sale deletion fails")]
        public async Task Handle_ShouldThrowException_WhenSaleDeletionFails()
        {
            // Arrange
            var deleteSale = DeleteSaleHandlerTestData.GenerateDeleteSaleCommand();
            var sale = new Sale { SaleNumber = deleteSale.SaleNumber };
            _saleRepositoryMock.GetBySaleNumberAsync(deleteSale.SaleNumber).Returns(sale);
            _saleRepositoryMock.DeleteAsync(deleteSale.SaleNumber).Throws(new Exception("Sale deletion failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(deleteSale, CancellationToken.None));
        }

        /// <summary>
        /// Tests that an exception is thrown when event publication fails.
        /// </summary>
        [Fact(DisplayName = "Should throw exception when event publication fails")]
        public async Task Handle_ShouldThrowException_WhenEventPublicationFails()
        {
            // Arrange
            var deleteSale = DeleteSaleHandlerTestData.GenerateDeleteSaleCommand();
            var sale = new Sale { SaleNumber = deleteSale.SaleNumber };
            _saleRepositoryMock.GetBySaleNumberAsync(deleteSale.SaleNumber).Returns(sale);
            _busMock.Publish(Arg.Any<SaleCancelled>(), Arg.Any<CancellationToken>()).Throws(new Exception("Event publication failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(deleteSale, CancellationToken.None));
        }
    }
}
