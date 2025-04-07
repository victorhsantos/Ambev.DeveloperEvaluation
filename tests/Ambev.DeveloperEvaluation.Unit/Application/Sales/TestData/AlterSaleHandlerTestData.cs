using Ambev.DeveloperEvaluation.Application.Sales.AlterSale;
using Ambev.DeveloperEvaluation.Application.Sales.Models;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData
{
    public static class AlterSaleHandlerTestData
    {
        private static readonly Faker<CustomerInputModel> CustomerFaker = new Faker<CustomerInputModel>()
            .RuleFor(c => c.FullName, f => f.Name.FullName())
            .RuleFor(c => c.Email, f => f.Internet.Email());

        private static readonly Faker<PaymentAddressInputModel> PaymentAddressFaker = new Faker<PaymentAddressInputModel>()
            .RuleFor(p => p.Street, f => f.Address.StreetName())
            .RuleFor(p => p.Number, f => f.Address.BuildingNumber())
            .RuleFor(p => p.City, f => f.Address.City())
            .RuleFor(p => p.State, f => f.Address.State())
            .RuleFor(p => p.PostalCode, f => f.Address.ZipCode())
            .RuleFor(p => p.Country, f => f.Address.Country());

        private static readonly Faker<SaleItemInputModel> SaleItemFaker = new Faker<SaleItemInputModel>()
            .RuleFor(i => i.ProductId, f => f.Random.Guid())
            .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
            .RuleFor(i => i.Price, f => f.Finance.Amount(1, 1000));

        private static readonly Faker<AlterSaleCommand> AlterSaleFaker = new Faker<AlterSaleCommand>()
            .RuleFor(s => s.SaleNumber, f => f.Random.Guid())
            .RuleFor(s => s.Customer, f => CustomerFaker.Generate())
            .RuleFor(s => s.Items, f => SaleItemFaker.Generate(3))
            .RuleFor(s => s.PaymentAddress, f => PaymentAddressFaker.Generate());

        public static AlterSaleCommand GenerateAlterSaleCommand()
        {
            return AlterSaleFaker.Generate();
        }
    }
}
