using BrandUp.Items;

namespace BrandUp.Core.Repository.Repositories
{
    public interface IRepository<TDocument, TId> : IRepository
        where TDocument : IItem<TId>
    {
    }
}
