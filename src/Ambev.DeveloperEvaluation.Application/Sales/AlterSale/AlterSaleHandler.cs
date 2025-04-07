using Ambev.DeveloperEvaluation.Domain.Repositories;
using MassTransit;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.AlterSale
{
    public class AlterSaleHandler(IBus _bus, ISaleRepository _saleRepository) : IRequestHandler<AlterSaleCommand, bool>
    {
        public async Task<bool> Handle(AlterSaleCommand request, CancellationToken cancellationToken)
        {
            var currentSale = await _saleRepository.GetBySaleNumberAsync(request.SaleNumber);
            if (currentSale is null) return false;

            await _saleRepository.UpdateAsync(request.ToEntity());
            await _bus.Publish(new SaleAltered { SaleNumber = currentSale.SaleNumber }, cancellationToken);

            return true;
        }
    }
}
