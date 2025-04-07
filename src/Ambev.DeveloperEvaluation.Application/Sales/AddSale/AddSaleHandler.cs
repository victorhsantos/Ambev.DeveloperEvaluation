using Ambev.DeveloperEvaluation.Domain.Repositories;
using MassTransit;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.AddSale
{
    public record class AddSaleHandler(IBus _bus, ISaleRepository _saleRepository) : IRequestHandler<AddSaleCommand, Guid>
    {
        public async Task<Guid> Handle(AddSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = request.ToEntity();

            await _saleRepository.CreateAsync(sale, cancellationToken);
            await _bus.Publish(new SaleCreated { SaleNumber = sale.SaleNumber }, cancellationToken);

            return sale.SaleNumber;
        }
    }
}
