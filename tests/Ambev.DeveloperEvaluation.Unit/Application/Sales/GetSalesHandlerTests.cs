using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Xunit;
using NSubstitute;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSales
{
    public class GetSalesHandlerTests
    {
        private readonly ISaleRepository _saleRepositoryMock;
        private readonly GetSalesHandler _handler;

        public GetSalesHandlerTests()
        {
            _saleRepositoryMock = Substitute.For<ISaleRepository>();
            _handler = new GetSalesHandler(_saleRepositoryMock);
        }

        /// <summary>
        /// Tests that sales are returned when they exist.
        /// </summary>
        [Fact(DisplayName = "Should return sales when they exist")]
        public async Task Handle_ShouldReturnSales_WhenSalesExist()
        {
            // Arrange
            var getSalesQuery = GetSalesHandlerTestData.GenerateGetSalesQuery();
            var sales = new List<Sale> { new Sale { SaleNumber = Guid.NewGuid() } };
            _saleRepositoryMock.GetPagedSalesAsync(getSalesQuery.PageSize, getSalesQuery.PageNumber, Arg.Any<CancellationToken>()).Returns(sales);

            // Act
            var result = await _handler.Handle(getSalesQuery, CancellationToken.None);

            // Assert
            result.Sales.Should().BeEquivalentTo(sales);
            result.CurrentPage.Should().Be(getSalesQuery.PageNumber);
            result.TotalPages.Should().Be(1);
            result.TotalCount.Should().Be(sales.Count);
        }

        /// <summary>
        /// Tests that an empty response is returned when no sales exist.
        /// </summary>
        [Fact(DisplayName = "Should return empty response when no sales exist")]
        public async Task Handle_ShouldReturnEmptyResponse_WhenNoSalesExist()
        {
            // Arrange
            var getSalesQuery = GetSalesHandlerTestData.GenerateGetSalesQuery();
            _saleRepositoryMock.GetPagedSalesAsync(getSalesQuery.PageSize, getSalesQuery.PageNumber, Arg.Any<CancellationToken>()).Returns((List<Sale>)null);

            // Act
            var result = await _handler.Handle(getSalesQuery, CancellationToken.None);

            // Assert
            result.Sales.Should().BeEmpty();
            result.CurrentPage.Should().Be(0);
            result.TotalPages.Should().Be(0);
            result.TotalCount.Should().Be(0);
        }

        /// <summary>
        /// Tests that an empty response is returned when the sales list is empty.
        /// </summary>
        [Fact(DisplayName = "Should return empty response when sales list is empty")]
        public async Task Handle_ShouldReturnEmptyResponse_WhenSalesListIsEmpty()
        {
            // Arrange
            var getSalesQuery = GetSalesHandlerTestData.GenerateGetSalesQuery();
            var sales = new List<Sale>();
            _saleRepositoryMock.GetPagedSalesAsync(getSalesQuery.PageSize, getSalesQuery.PageNumber, Arg.Any<CancellationToken>()).Returns(sales);

            // Act
            var result = await _handler.Handle(getSalesQuery, CancellationToken.None);

            // Assert
            result.Sales.Should().BeEmpty();
            result.CurrentPage.Should().Be(0);
            result.TotalPages.Should().Be(0);
            result.TotalCount.Should().Be(0);
        }
    }
}
