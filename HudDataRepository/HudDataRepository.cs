using Hud.Data.Service.Models;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Hud.Data.Service
{
    public class HudDataRepository : IHudDataRepository
    {
        IHudContext _context;

        public HudDataRepository(IHudContext context = null)
        {
            _context = context 
                ?? new HudContext(); //todo remove after DI is in place
        }

        public async Task<IEnumerable<ZipCbsaItem>> GetZipCbsaItems(int zipCode)
        {
            return zipCode > 0
                ? await _context.ZipCollection.Find(q => q.ZipCode.Equals(zipCode.ToString("D5"))).ToListAsync().ConfigureAwait(false)
                : await _context.ZipCollection.Find(q => q.ZipCode != null).ToListAsync().ConfigureAwait(false); // mainly for testing
        }

        public async Task<IEnumerable<CbsaMsaItem>> GetCbsaMsaItems(CbsaMsaItemsSearchRequest request)
        {
            var filterBuilder = new FilterDefinitionBuilder<CbsaMsaItem>();
            var filters = new List<FilterDefinition<CbsaMsaItem>>();
            if (request.CbsaCode.HasValue)
            {
                filters.Add(filterBuilder.Where(q => q.CbsaCode.Equals(request.CbsaCode.Value.ToString("D5"))));
            }
            if (request.MDiv.HasValue)
            {
                filters.Add(filterBuilder.Where(q => q.MDiv.Equals(request.MDiv.Value.ToString("D5"))));
            }
            if (!String.IsNullOrEmpty(request.Lsad))
            {
                filters.Add(filterBuilder.Where(q => q.Lsad.ToLower() == request.Lsad.ToLower()));
            }

            return filters.Count > 0 
                ? await _context.MsaCollection.Find(filterBuilder.And(filters)).ToListAsync().ConfigureAwait(false)
                : new List<CbsaMsaItem>(); // for now returning empty list instead of all items if no filter criteria provided
        }

        public async Task UpdateZipMapping(IEnumerable<ZipCbsaItem> zipCbsaItems)
        {
            // choosing to drop and recreate the mappings since this is an admin-level function that is less frequently used and if needed can be done off-hours 
            await _context.Database.DropCollectionAsync(Properties.Settings.Default.ZipCollectionName).ConfigureAwait(false);
            await _context.ZipCollection.InsertManyAsync(zipCbsaItems).ConfigureAwait(false);
        }

        public async Task UpdateStatisticalAreaMapping(IEnumerable<CbsaMsaItem> cbsaMsaItems)
        {
            // choosing to drop and recreate the mappings since this is an admin-level function that is less frequently used and if needed can be done off-hours 
            await _context.Database.DropCollectionAsync(Properties.Settings.Default.MsaCollectionName).ConfigureAwait(false);
            await _context.MsaCollection.InsertManyAsync(cbsaMsaItems).ConfigureAwait(false);
        }
    }
}