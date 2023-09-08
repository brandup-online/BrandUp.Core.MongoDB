using BrandUp.Items;

namespace BrandUp.MongoDB.Repository.Items
{
    public interface IDocument<out TId, TVersion> : IItem<TId>
    {
        public TVersion Version { get; set; }
    }
}
