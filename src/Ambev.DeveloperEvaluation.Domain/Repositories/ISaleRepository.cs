using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);
        Task<Sale?> GetBySaleNumberAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Sale?> GetByIdWithItemsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Sale>> GetAllAsync(CancellationToken cancellationToken = default);
        Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Sale>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task<List<Sale>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<List<Sale>> GetPagedSalesAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default);
    }
}
