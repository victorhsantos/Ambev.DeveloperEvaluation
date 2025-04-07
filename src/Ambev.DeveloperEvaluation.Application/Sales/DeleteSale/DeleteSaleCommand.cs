using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public class DeleteSaleCommand : IRequest<bool>
    {
        public Guid SaleNumber { get; set; }
    }

    public class SaleCancelled
    {
        public Guid SaleNumber { get; set; }
    }
}
