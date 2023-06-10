using BrandUp.MongoDB.Repository.Items;

namespace BrandUp.MongoDB.Repository
{
    public interface IRepository<TId, TVersion>
        where TId : class
        where TVersion : class
    {
        Task CreateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, IDocument<TId, TVersion>;
        Task<TEntity> FindAsync<TEntity>(TId id, CancellationToken cancellationToken) where TEntity : class, IDocument<TId, TVersion>;
        Task<bool> UpdateAsync<TEntity>(TEntity entity, TVersion version, CancellationToken cancellationToken) where TEntity : class, IDocument<TId, TVersion>;
        Task<bool> DeleteAsync<TEntity>(TEntity entity, TVersion version, CancellationToken cancellationToken) where TEntity : class, IDocument<TId, TVersion>;
    }
}
