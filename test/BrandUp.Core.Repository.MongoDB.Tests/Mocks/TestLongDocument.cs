using BrandUp.MongoDB.Repository.Items;

namespace BrandUp.Core.Repository.MongoDB.Mocks
{
    public class TestLongDocument : IDocument<long, long>
    {
        public long Id { get; set; }

        public long Version { get; set; }
    }
}
