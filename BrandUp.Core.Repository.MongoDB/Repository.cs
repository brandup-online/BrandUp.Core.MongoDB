using BrandUp.Core.Repository.Repositories;
using BrandUp.Core.Repository.Transactions;
using BrandUp.MongoDB;
using BrandUp.MongoDB.Repository.Items;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace BrandUp.Core.Repository.MongoDB
{
    public abstract class Repository<TDocument, TId, TVersion> : IRepository<TDocument, TId, TVersion>
        where TDocument : class, IDocument<TId, TVersion>, new()
    {

        readonly protected AppDocumentSession session;
        readonly protected IMongoCollection<TDocument> items;

        public Repository(MongoDbContext context, AppDocumentSession session)
        {
            this.session = session ?? throw new ArgumentNullException(nameof(session));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            items = context.GetCollection<TDocument>();
        }

        #region IRepository members

        #region Querying

        public async Task<TDocument> FindByIdAsync(TId id, CancellationToken cancellationToken)
        {
            var cursor = await items.FindAsync(session.Current, it => it.Id.Equals(id), null, cancellationToken);

            return await cursor.FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<IList<TDocument>> QueryAsync(Expression<Func<TDocument, bool>> predicate, int skip, int take, CancellationToken cancellationToken)
        {
            var cursor = await items.FindAsync(session.Current, predicate, null, cancellationToken);

            return await cursor.ToListAsync(cancellationToken);
        }
        public async Task<TDocument> FirstOrDefaultAsync(Expression<Func<TDocument, bool>> predicate, CancellationToken cancellationToken)
        {
            var cursor = await items.FindAsync(session.Current, predicate, null, cancellationToken);

            return await cursor.FirstOrDefaultAsync(cancellationToken);
        }

        #endregion

        #region Insert

        public Task CreateAsync(TDocument document, CancellationToken cancellationToken)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            return items.InsertOneAsync(session.Current, document, new InsertOneOptions { BypassDocumentValidation = false }, cancellationToken);
        }
        public async Task<TId> CreateAndGetIdAsync(TDocument document, CancellationToken cancellationToken)
        {
            await CreateAsync(document, cancellationToken);
            return document.Id;
        }

        #endregion

        #region Update

        public async Task<bool> UpdateAsync(TDocument document, TVersion version, CancellationToken cancellationToken)
        {
            var result = await items.ReplaceOneAsync(session.Current, it => it.Id.Equals(document.Id) && it.Version.Equals(version), document, cancellationToken: cancellationToken);
            return result.ModifiedCount == 1;
        }

        #endregion

        #region Delete

        public async Task<bool> DeleteAsync(TDocument document, TVersion version, CancellationToken cancellationToken)
        {
            var result = await items.DeleteOneAsync(session.Current, it => it.Id.Equals(document.Id) && it.Version.Equals(version), cancellationToken: cancellationToken);
            return result.DeletedCount == 1;
        }

        public async Task<long> DeleteManyAsync(Expression<Func<TDocument, bool>> predicate, CancellationToken cancellationToken)
        {
            var result = await items.DeleteManyAsync(session.Current, predicate, cancellationToken: cancellationToken);
            return result.DeletedCount;
        }

        #endregion

        #endregion
    }
}