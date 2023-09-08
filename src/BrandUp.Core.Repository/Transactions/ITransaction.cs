namespace BrandUp.Core.Repository.Transactions
{
    public interface ITransaction : IDisposable
    {
        DateTime BeginDate { get; }
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}