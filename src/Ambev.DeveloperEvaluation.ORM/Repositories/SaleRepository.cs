using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.PostgreSQL.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }

        public async Task<Sale?> GetBySaleNumberAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.PaymentAddress)
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.SaleNumber == id, cancellationToken);
        }

        public async Task<Sale?> GetByIdWithItemsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.SaleNumber == id, cancellationToken);
        }

        public async Task<List<Sale>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.PaymentAddress)
                .Include(s => s.Items)
                .ToListAsync(cancellationToken);
        }



        public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            var currentSale = await _context.Sales
                .Where(s => s.SaleNumber == sale.SaleNumber)
                .Include(s => s.Items)
                .FirstOrDefaultAsync(cancellationToken);

            if (currentSale is null) return;

            currentSale.Customer = sale.Customer;
            currentSale.PaymentAddress = sale.PaymentAddress;
            currentSale.TotalAmount = sale.TotalAmount;

            _context.SaleItems.RemoveRange(currentSale.Items);
            _context.SaleItems.AddRange(sale.Items);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = await GetBySaleNumberAsync(id, cancellationToken);
            if (sale != null)
            {
                sale.SetAsCancelled();
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<List<Sale>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.PaymentAddress)
                .Include(s => s.Items)
                .Where(s => s.Customer.Id == customerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Sale>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.PaymentAddress)
                .Include(s => s.Items)
                .Where(s => s.Items.Any(i => i.ProductId == productId))
                .ToListAsync(cancellationToken);
        }
        public async Task<List<Sale>> GetPagedSalesAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.PaymentAddress)
                .Include(s => s.Items)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
