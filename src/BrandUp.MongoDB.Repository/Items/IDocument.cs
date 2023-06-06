using BrandUp.Items;

namespace BrandUp.MongoDB.Repository.Items
{
    public interface IDocument<out TId, TVersion> : IItem<TId>
        where TId : class
        where TVersion : class
    {
        public TVersion Version { get; set; }
    }
}
