using MongoDB.Driver;

namespace BrandUp.MongoDB.Repository.Transaction
{
    public class AppDocumentSession : ITransactionFactory, IDisposable
    {
        readonly IClientSessionHandle clientSession;
        private DocumentTransaction transaction;

        public IClientSessionHandle Current => clientSession;

        public AppDocumentSession(MongoDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            clientSession = context.Database.Client.StartSession(new ClientSessionOptions
            {
                CausalConsistency = true,
                Snapshot = false,
                DefaultTransactionOptions = new TransactionOptions(ReadConcern.Majority, ReadPreference.Primary, WriteConcern.WMajority)
            });
        }

        public Task<ITransaction> BeginAsync(CancellationToken cancellationToken = default)
        {
            if (!clientSession.IsInTransaction)
            {
                clientSession.StartTransaction();
                transaction = new DocumentTransaction(this, DateTime.UtcNow);

                return Task.FromResult<ITransaction>(transaction);
            }

            return Task.FromResult<ITransaction>(new DocumentTransaction(transaction));
        }

        public void Dispose()
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }

            clientSession.Dispose();
        }
    }

    public interface ITransactionFactory
    {
        Task<ITransaction> BeginAsync(CancellationToken cancellationToken = default);
    }
}
