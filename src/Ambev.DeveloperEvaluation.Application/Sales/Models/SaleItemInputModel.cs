namespace Ambev.DeveloperEvaluation.Application.Sales.Models
{
    public class SaleItemInputModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
         
    }
}
