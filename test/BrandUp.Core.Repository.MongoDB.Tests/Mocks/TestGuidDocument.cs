using BrandUp.MongoDB.Repository.Items;

namespace BrandUp.Core.Repository.MongoDB.Mocks
{
    public class TestGuidDocument : IDocument<Guid, Guid>
    {
        public Guid Id { get; set; }
        public Guid Version { get; set; }
    }
}
