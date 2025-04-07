using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public record class GetSalesResponse(IList<Sale> Sales, int CurrentPage = 0, int TotalPages = 0, int TotalCount = 0) { }
}
