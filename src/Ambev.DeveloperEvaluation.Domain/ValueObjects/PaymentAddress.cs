namespace Ambev.DeveloperEvaluation.Domain.ValueObjects
{
    public record class PaymentAddress(string Street, string Number, string City, string State, string PostalCode, string Country)
    {
        public Guid Id { get; set; }
    }
}
