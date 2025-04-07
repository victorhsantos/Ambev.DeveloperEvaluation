using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;
using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class SaleTestData
    {
        private static readonly Faker<Customer> CustomerFaker = new Faker<Customer>()
            .CustomInstantiator(f => new Customer(f.Person.FullName, f.Person.Email));

        private static readonly Faker<PaymentAddress> PaymentAddressFaker = new Faker<PaymentAddress>()
            .CustomInstantiator(f => new PaymentAddress(
                f.Address.StreetName(),
                f.Address.BuildingNumber(),
                f.Address.City(),
                f.Address.State(),
                f.Address.ZipCode(),
                f.Address.Country()));

        private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
            .CustomInstantiator(f => new SaleItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                f.Random.Int(1, 10),
                f.Random.Decimal(1, 100)))
            .RuleFor(i => i.TotalAmount, (f, i) => i.Quantity * i.UnitPrice);

        private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
            .CustomInstantiator(f => new Sale(
                Guid.NewGuid(),
                CustomerFaker.Generate(),
                PaymentAddressFaker.Generate(),
                SaleItemFaker.Generate(3)));

        public static Sale GenerateValidSale()
        {
            return SaleFaker.Generate();
        }

        public static Sale GenerateInvalidSale()
        {
            return new Sale(
                Guid.Empty,
                null,
                null,
                new List<SaleItem>());
        }
    }
}
