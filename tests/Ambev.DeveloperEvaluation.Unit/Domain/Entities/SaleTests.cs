using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
    {
        /// <summary>
        /// Tests that a sale is created with valid data.
        /// </summary>
        [Fact(DisplayName = "Should create sale with valid data")]
        public void Given_ValidData_When_CreatingSale_Then_ShouldCreateSale()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();

            // Act & Assert
            sale.Should().NotBeNull();
            sale.SaleNumber.Should().NotBeEmpty();
            sale.Customer.Should().NotBeNull();
            sale.PaymentAddress.Should().NotBeNull();
            sale.Items.Should().NotBeEmpty();
            sale.TotalAmount.Should().Be(sale.Items.Sum(i => i.TotalAmount));
            sale.Status.Should().Be(SaleStatus.NotCancelled);
        }

        /// <summary>
        /// Tests that a sale is updated correctly.
        /// </summary>
        [Fact(DisplayName = "Should update sale correctly")]
        public void Given_ValidData_When_UpdatingSale_Then_ShouldUpdateSale()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            var updatedSale = SaleTestData.GenerateValidSale();

            // Act
            sale.Update(updatedSale);

            // Assert
            sale.Customer.Should().Be(updatedSale.Customer);
            sale.PaymentAddress.Should().Be(updatedSale.PaymentAddress);
            sale.Items.Should().BeEquivalentTo(updatedSale.Items);
            sale.TotalAmount.Should().Be(updatedSale.Items.Sum(i => i.TotalAmount));
        }

        /// <summary>
        /// Tests that a sale is set as cancelled.
        /// </summary>
        [Fact(DisplayName = "Should set sale as cancelled")]
        public void Given_Sale_When_SetAsCancelled_Then_StatusShouldBeCancelled()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();

            // Act
            sale.SetAsCancelled();

            // Assert
            sale.Status.Should().Be(SaleStatus.Cancelled);
        }

        /// <summary>
        /// Tests that a sale is set as not cancelled.
        /// </summary>
        [Fact(DisplayName = "Should set sale as not cancelled")]
        public void Given_Sale_When_SetAsNotCancelled_Then_StatusShouldBeNotCancelled()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();

            // Act
            sale.SetAsNotCancelled();

            // Assert
            sale.Status.Should().Be(SaleStatus.NotCancelled);
        }       
    }
}
