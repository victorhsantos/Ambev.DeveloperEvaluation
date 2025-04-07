using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        SaleItem() { }

        public SaleItem(Guid saleNumber, Guid productId, int quantity, decimal unitPrice)
        {
            Id = Guid.NewGuid();
            SaleNumber = saleNumber;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalAmount = (UnitPrice * Quantity) - Discount;

            ApplyDiscount();

            new SaleItemValidator().ValidateAndThrow(this);
        }

        public virtual Guid SaleNumber { get; private set; }
        public virtual Sale Sale { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalAmount { get; private set; }

        private void ApplyDiscount()
        {
            if (Quantity >= 10 && Quantity <= 20)
                Discount = Quantity * UnitPrice * 0.20m;
            else if (Quantity >= 4)
                Discount = Quantity * UnitPrice * 0.10m;
            else
                Discount = 0;
        }
    }
}
