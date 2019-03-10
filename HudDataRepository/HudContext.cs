using Hud.Data.Service.Models;
using MongoDB.Driver;

namespace Hud.Data.Service
{
    public class HudContext: IHudContext
    {
        public IMongoDatabase Database { get; set; }

        public HudContext()
        {
            var client = new MongoClient(Properties.Settings.Default.HudConnectionString);
            Database = client.GetDatabase(Properties.Settings.Default.HudDatabaseName);
        }

        public IMongoCollection<ZipCbsaItem> ZipCollection => Database.GetCollection<ZipCbsaItem>(Properties.Settings.Default.ZipCollectionName);

        public IMongoCollection<CbsaMsaItem> MsaCollection => Database.GetCollection<CbsaMsaItem>(Properties.Settings.Default.MsaCollectionName);
    }
}