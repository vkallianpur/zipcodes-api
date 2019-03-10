using Hud.Data.Service.Models;
using MongoDB.Driver;

namespace Hud.Data.Service
{
    /// <summary>
    /// Provides the database context for Hud data operations.
    /// </summary>
    public interface IHudContext
    {
        /// <summary>
        /// The Mongo database object. 
        /// </summary>
        IMongoDatabase Database { get; set; }

        /// <summary>
        /// The Mongo collection for the Zip to CBSA mappings. 
        /// </summary>
        IMongoCollection<ZipCbsaItem> ZipCollection { get; }

        /// <summary>
        /// The Mongo collection for the CBSA to MSA mappings. 
        /// </summary>
        IMongoCollection<CbsaMsaItem> MsaCollection { get; }
    }
}