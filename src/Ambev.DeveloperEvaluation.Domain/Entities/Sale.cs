using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public Sale() { }

        public Sale(Guid saleNumber, Customer customer, PaymentAddress paymentAddress, List<SaleItem> items)
        {
            SaleNumber = saleNumber;
            SaleDate = DateTime.UtcNow;
            Customer = customer;
            TotalAmount = items.Sum(i => i.TotalAmount);
            PaymentAddress = paymentAddress;
            Items = items;
            SetAsNotCancelled();

            new SaleValidator().ValidateAndThrow(this);
        }

        public Guid SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public Customer Customer { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentAddress PaymentAddress { get; set; }
        public List<SaleItem> Items { get; set; }
        public SaleStatus Status { get; private set; }

        public void SetAsCancelled() => Status = SaleStatus.Cancelled;
        public void SetAsNotCancelled() => Status = SaleStatus.NotCancelled;

        public Sale Update(Sale sale)
        {
            Customer = sale.Customer;
            PaymentAddress = sale.PaymentAddress;
            Items = sale.Items;
            TotalAmount = sale.Items.Sum(i => i.TotalAmount);

            return this;
        }
    }
}
