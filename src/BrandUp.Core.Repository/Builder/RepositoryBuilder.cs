using BrandUp.Core.Repository.Repositories;
using BrandUp.Core.Repository.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace BrandUp.Core.Repository.Builder
{
    public class RepositoryBuilder : IRepositoryBuilder
    {
        public RepositoryBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #region IRepositoryBuilder

        public IServiceCollection Services { get; }

        public IRepositoryBuilder AddContext<TContext>() where TContext : class
        {
            throw new NotImplementedException();
        }

        public IRepositoryBuilder AddRepository<TRepository>() where TRepository : IRepository
        {
            throw new NotImplementedException();
        }

        public IRepositoryBuilder AddTransactionFactory<TFactory>() where TFactory : ITransactionFactory
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public interface IRepositoryBuilder
    {
        public IServiceCollection Services { get; }

        public IRepositoryBuilder AddContext<TContext>() where TContext : class;

        public IRepositoryBuilder AddTransactionFactory<TFactory>() where TFactory : ITransactionFactory;

        public IRepositoryBuilder AddRepository<TRepository>() where TRepository : IRepository;
    }
}