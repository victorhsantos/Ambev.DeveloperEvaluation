using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public class GetSalesHandler : IRequestHandler<GetSalesQuery, GetSalesResponse>
    {
        private readonly ISaleRepository _saleRepository;

        public GetSalesHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<GetSalesResponse> Handle(GetSalesQuery request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetPagedSalesAsync(request.PageSize, request.PageNumber, cancellationToken);
            if (sales is null || !sales.Any()) return new GetSalesResponse(new List<Sale>());
            return new GetSalesResponse(sales, request.PageNumber, (int)Math.Ceiling((double)sales.Count / request.PageSize), sales.Count);
        }
    }
}
