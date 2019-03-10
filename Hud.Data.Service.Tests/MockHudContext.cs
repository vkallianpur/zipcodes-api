using Hud.Data.Service.Models;
using MongoDB.Driver;

namespace Hud.Data.Service.Tests
{
    internal class MockHudContext : IHudContext
    {
        // todo these can come from config file
        private const string _connectionString = "mongodb://localhost";
        private const string _hudDatabaseName = "testHud";
        private const string _zipCollectionName = "testZip";
        private const string _msaCollectionName = "testMsa";

        public MockHudContext()
        {
            var client = new MongoClient(_connectionString);
            Database = client.GetDatabase(_hudDatabaseName);
        }

        public IMongoDatabase Database { get; set; }

        public IMongoCollection<ZipCbsaItem> ZipCollection => Database.GetCollection<ZipCbsaItem>(_zipCollectionName);

        public IMongoCollection<CbsaMsaItem> MsaCollection => Database.GetCollection<CbsaMsaItem>(_msaCollectionName);

        public void Cleanup()
        {
            Database.DropCollection(_zipCollectionName);
            Database.DropCollection(_msaCollectionName);
            Database.Client.DropDatabase(_hudDatabaseName);
        }
    }
}
