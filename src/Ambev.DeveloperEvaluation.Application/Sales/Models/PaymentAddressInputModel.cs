namespace Ambev.DeveloperEvaluation.Application.Sales.Models
{
    public class PaymentAddressInputModel
    {
        public string Street { get; set; } = default!;
        public string Number { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
        public string Country { get; set; } = default!;
    }
}
