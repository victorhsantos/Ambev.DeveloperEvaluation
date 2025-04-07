using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using System;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleItemTests
    {
        /// <summary>
        /// Tests that a sale item is created with valid data.
        /// </summary>
        [Fact(DisplayName = "Should create sale item with valid data")]
        public void Given_ValidData_When_CreatingSaleItem_Then_ShouldCreateSaleItem()
        {
            // Arrange
            var saleItem = SaleItemTestData.GenerateValidSaleItem();

            // Act & Assert
            saleItem.Should().NotBeNull();
            saleItem.SaleNumber.Should().NotBeEmpty();
            saleItem.ProductId.Should().NotBeEmpty();
            saleItem.Quantity.Should().BeGreaterThan(0);
            saleItem.UnitPrice.Should().BeGreaterThan(0);
            saleItem.TotalAmount.Should().Be((saleItem.UnitPrice * saleItem.Quantity) - saleItem.Discount);
        }

        /// <summary>
        /// Tests that a sale item applies the correct discount based on quantity.
        /// </summary>
        [Fact(DisplayName = "Should apply correct discount based on quantity")]
        public void Given_Quantity_When_CreatingSaleItem_Then_ShouldApplyCorrectDiscount()
        {
            // Arrange
            var saleItem = SaleItemTestData.GenerateSaleItemWithQuantity(15);

            // Act & Assert
            saleItem.Discount.Should().Be(saleItem.Quantity * saleItem.UnitPrice * 0.20m);
        }

        /// <summary>
        /// Tests that a sale item applies no discount for quantity less than 4.
        /// </summary>
        [Fact(DisplayName = "Should apply no discount for quantity less than 4")]
        public void Given_QuantityLessThan4_When_CreatingSaleItem_Then_ShouldApplyNoDiscount()
        {
            // Arrange
            var saleItem = SaleItemTestData.GenerateSaleItemWithQuantity(3);

            // Act & Assert
            saleItem.Discount.Should().Be(0);
        }

        /// <summary>
        /// Tests that a sale item applies 10% discount for quantity between 4 and 9.
        /// </summary>
        [Fact(DisplayName = "Should apply 10% discount for quantity between 4 and 9")]
        public void Given_QuantityBetween4And9_When_CreatingSaleItem_Then_ShouldApply10PercentDiscount()
        {
            // Arrange
            var saleItem = SaleItemTestData.GenerateSaleItemWithQuantity(5);

            // Act & Assert
            saleItem.Discount.Should().Be(saleItem.Quantity * saleItem.UnitPrice * 0.10m);
        }

        /// <summary>
        /// Tests that a sale item applies 20% discount for quantity between 10 and 20.
        /// </summary>
        [Fact(DisplayName = "Should apply 20% discount for quantity between 10 and 20")]
        public void Given_QuantityBetween10And20_When_CreatingSaleItem_Then_ShouldApply20PercentDiscount()
        {
            // Arrange
            var saleItem = SaleItemTestData.GenerateSaleItemWithQuantity(15);

            // Act & Assert
            saleItem.Discount.Should().Be(saleItem.Quantity * saleItem.UnitPrice * 0.20m);
        }

        /// <summary>
        /// Tests that a sale item throws a validation exception for quantity greater than 20.
        /// </summary>
        [Fact(DisplayName = "Should throw validation exception for quantity greater than 20")]
        public void Given_QuantityGreaterThan20_When_CreatingSaleItem_Then_ShouldThrowValidationException()
        {
            // Arrange
            Action act = () => SaleItemTestData.GenerateSaleItemWithQuantity(21);

            // Act & Assert
            act.Should().Throw<FluentValidation.ValidationException>()
                .WithMessage("Validation failed: \n -- Quantity: It's not possible to sell above 20 identical items. Severity: Error");
        }
    }
}
