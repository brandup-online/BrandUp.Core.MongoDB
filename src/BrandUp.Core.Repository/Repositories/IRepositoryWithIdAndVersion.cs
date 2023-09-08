using BrandUp.MongoDB.Repository.Items;
using System.Linq.Expressions;

namespace BrandUp.Core.Repository.Repositories
{

    public interface IRepository<TDocument, TId, TVersion> : IRepository<TDocument, TId>
        where TDocument : IDocument<TId, TVersion>
    {
        #region Querying

        Task<TDocument> FindByIdAsync(TId id, CancellationToken cancellationToken);
        Task<IList<TDocument>> QueryAsync(Expression<Func<TDocument, bool>> predicate, int skip, int take, CancellationToken cancellationToken);
        Task<TDocument> FirstOrDefaultAsync(Expression<Func<TDocument, bool>> predicate, CancellationToken cancellationToken);

        #endregion

        #region Insert

        Task CreateAsync(TDocument document, CancellationToken cancellationToken);
        Task<TId> CreateAndGetIdAsync(TDocument document, CancellationToken cancellationToken);

        #endregion

        #region Update

        Task<bool> UpdateAsync(TDocument document, TVersion version, CancellationToken cancellationToken);

        #endregion

        #region Delete

        Task<bool> DeleteAsync(TDocument document, TVersion version, CancellationToken cancellationToken);

        Task<long> DeleteManyAsync(Expression<Func<TDocument, bool>> predicate, CancellationToken cancellationToken);

        #endregion
    }
}