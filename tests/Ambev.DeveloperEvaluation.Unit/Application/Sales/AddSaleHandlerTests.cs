using Ambev.DeveloperEvaluation.Application.Sales.AddSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MassTransit;
using Xunit;
using NSubstitute;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using NSubstitute.ExceptionExtensions;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.AddSale
{
    public class AddSaleHandlerTests
    {
        private readonly IBus _busMock;
        private readonly ISaleRepository _saleRepositoryMock;
        private readonly AddSaleHandler _handler;

        public AddSaleHandlerTests()
        {
            _busMock = Substitute.For<IBus>();
            _saleRepositoryMock = Substitute.For<ISaleRepository>();
            _handler = new AddSaleHandler(_busMock, _saleRepositoryMock);
        }

        /// <summary>
        /// Tests that a sale is created and an event is published when the handle method is called.
        /// </summary>
        [Fact(DisplayName = "Should create sale and publish event")]
        public async Task Handle_ShouldCreateSaleAndPublishEvent()
        {
            // Arrange
            var addSale = AddSaleHandlerTestData.GenerateAddSaleCommand();

            // Act
            var result = await _handler.Handle(addSale, CancellationToken.None);

            // Assert
            await _saleRepositoryMock.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
            await _busMock.Received(1).Publish(Arg.Any<SaleCreated>(), Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that an exception is thrown when sale creation fails.
        /// </summary>
        [Fact(DisplayName = "Should throw exception when sale creation fails")]
        public async Task Handle_ShouldThrowException_WhenSaleCreationFails()
        {
            // Arrange
            var addSale = AddSaleHandlerTestData.GenerateAddSaleCommand();
            _saleRepositoryMock.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Throws(new Exception("Sale creation failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(addSale, CancellationToken.None));
        }

        /// <summary>
        /// Tests that an exception is thrown when event publication fails.
        /// </summary>
        [Fact(DisplayName = "Should throw exception when event publication fails")]
        public async Task Handle_ShouldThrowException_WhenEventPublicationFails()
        {
            // Arrange
            var addSale = AddSaleHandlerTestData.GenerateAddSaleCommand();
            _busMock.Publish(Arg.Any<SaleCreated>(), Arg.Any<CancellationToken>()).Throws(new Exception("Event publication failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(addSale, CancellationToken.None));
        }

        /// <summary>
        /// Tests that a sale is created correctly with valid data.
        /// </summary>
        [Fact(DisplayName = "Should create sale correctly with valid data")]
        public async Task Handle_ShouldCreateSaleCorrectly_WithValidData()
        {
            // Arrange
            var addSale = AddSaleHandlerTestData.GenerateAddSaleCommand();

            // Act
            var result = await _handler.Handle(addSale, CancellationToken.None);

            // Assert
            await _saleRepositoryMock.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
            await _busMock.Received(1).Publish(Arg.Any<SaleCreated>(), Arg.Any<CancellationToken>());
        }
    }
}
