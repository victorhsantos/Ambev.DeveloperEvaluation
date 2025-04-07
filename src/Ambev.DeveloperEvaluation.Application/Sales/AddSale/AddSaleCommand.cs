using Ambev.DeveloperEvaluation.Application.Sales.Models;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.AddSale
{
    public class AddSaleCommand : IRequest<Guid>
    {
        public Guid SaleNumber { get; set; } = Guid.NewGuid();
        public CustomerInputModel Customer { get; set; } = default!;
        public List<SaleItemInputModel> Items { get; set; } = default!;
        public PaymentAddressInputModel PaymentAddress { get; set; } = default!;

        public Sale ToEntity() => new(
            SaleNumber,
            new Customer(Customer.FullName, Customer.Email),
            new PaymentAddress(PaymentAddress.Street, PaymentAddress.Number, PaymentAddress.City, PaymentAddress.State, PaymentAddress.PostalCode, PaymentAddress.Country),
            Items.Select(i => new SaleItem(SaleNumber, i.ProductId, i.Quantity, i.Price)).ToList());
    }

    public class SaleCreated 
    {
        public Guid SaleNumber { get; set; }
    }
}
