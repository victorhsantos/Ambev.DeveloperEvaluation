using Ambev.DeveloperEvaluation.Domain.Repositories;
using MassTransit;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, bool>
    {
        private readonly IBus _bus;
        private readonly ISaleRepository _saleRepository;

        public DeleteSaleHandler(IBus bus, ISaleRepository saleRepository)
        {
            _bus = bus;
            _saleRepository = saleRepository;
        }

        public async Task<bool> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
        {
            var currentSale = await _saleRepository.GetBySaleNumberAsync(request.SaleNumber);
            if (currentSale is null) return false;

            await _saleRepository.DeleteAsync(request.SaleNumber);
            await _bus.Publish(new SaleCancelled { SaleNumber = currentSale.SaleNumber }, cancellationToken);

            return true;
        }
    }
}
