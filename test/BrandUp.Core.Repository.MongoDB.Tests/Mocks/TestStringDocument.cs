using BrandUp.MongoDB.Repository.Items;

namespace BrandUp.Core.Repository.MongoDB.Mocks
{
    public class TestStringDocument : IDocument<string, string>
    {
        public string Id { get; set; }

        public string Version { get; set; }
    }
}