using BrandUp.MongoDB.Repository.Items;
using BrandUp.MongoDB.Repository.Transaction;
using MongoDB.Driver;

namespace BrandUp.MongoDB.Repository
{
    public class Repository<TId, TVersion> : IRepository<TId, TVersion>
    where TId : class
    where TVersion : class
    {
        readonly MongoDbContext context;
        readonly AppDocumentSession session;

        public Repository(MongoDbContext context, AppDocumentSession session)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.session = session ?? throw new ArgumentNullException(nameof(session));
        }

        #region IRepository

        public Task CreateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, IDocument<TId, TVersion>
        {
            return Collection<TEntity>().InsertOneAsync(session.Current, entity, new InsertOneOptions { BypassDocumentValidation = false }, cancellationToken);
        }

        public async Task<TEntity> FindAsync<TEntity>(TId id, CancellationToken cancellationToken) where TEntity : class, IDocument<TId, TVersion>
        {
            var cursor = await Collection<TEntity>().FindAsync(session.Current, it => it.Id == id, new FindOptions<TEntity>(), cancellationToken);
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteAsync<TEntity>(TEntity entity, TVersion version, CancellationToken cancellationToken) where TEntity : class, IDocument<TId, TVersion>
        {
            var result = await Collection<TEntity>().DeleteOneAsync(session.Current, it => it.Id == entity.Id, new DeleteOptions(), cancellationToken);
            return result.DeletedCount == 1;
        }

        public async Task<bool> UpdateAsync<TEntity>(TEntity entity, TVersion version, CancellationToken cancellationToken) where TEntity : class, IDocument<TId, TVersion>
        {
            var result = await Collection<TEntity>().ReplaceOneAsync(session.Current, it => it.Id == entity.Id && it.Version == entity.Version, entity, new ReplaceOptions(), cancellationToken);
            return result.ModifiedCount == 1;
        }

        #endregion

        #region Helpers
        IMongoCollection<T> Collection<T>() where T : class => context.GetCollection<T>();

        #endregion
    }
}
