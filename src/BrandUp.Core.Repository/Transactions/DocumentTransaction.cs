namespace BrandUp.MongoDB.Repository.Transaction
{
    public class DocumentTransaction : ITransaction
    {
        readonly AppDocumentSession appDocumentSession;
        readonly bool isChild = false;
        private bool disposedValue;

        internal DocumentTransaction(AppDocumentSession appDocumentSession, DateTime beginDate)
        {
            this.appDocumentSession = appDocumentSession ?? throw new ArgumentNullException(nameof(appDocumentSession));
            BeginDate = beginDate;
        }
        internal DocumentTransaction(DocumentTransaction ownerTransaction)
        {
            if (ownerTransaction == null)
                throw new ArgumentNullException(nameof(ownerTransaction));

            appDocumentSession = ownerTransaction.appDocumentSession;
            BeginDate = ownerTransaction.BeginDate;
            isChild = true;
        }

        public DateTime BeginDate { get; private set; }
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (!isChild)
            {
                await appDocumentSession.Current.CommitTransactionAsync(cancellationToken);
            }
        }
        private void Abort()
        {
            if (!isChild && appDocumentSession.Current.IsInTransaction)
                appDocumentSession.Current.AbortTransaction();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    Abort();

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
        }
    }

    public interface ITransaction : IDisposable
    {
        DateTime BeginDate { get; }
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
