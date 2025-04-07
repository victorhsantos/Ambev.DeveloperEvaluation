using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using System;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class SaleItemTestData
    {
        private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
            .CustomInstantiator(f => new SaleItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                f.Random.Int(1, 20),
                f.Random.Decimal(1, 100)));

        public static SaleItem GenerateValidSaleItem()
        {
            return SaleItemFaker.Generate();
        }

        public static SaleItem GenerateSaleItemWithQuantity(int quantity)
        {
            return new SaleItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                quantity,
                new Faker().Random.Decimal(1, 100));
        }

        public static SaleItem GenerateInvalidSaleItem()
        {
            return new SaleItem(
                Guid.Empty,
                Guid.Empty,
                0,
                0);
        }
    }
}
