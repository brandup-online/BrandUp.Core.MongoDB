namespace BrandUp.Core.Repository.Transactions
{
    public interface ITransactionFactory
    {
        Task<ITransaction> BeginAsync(CancellationToken cancellationToken = default);
    }
}