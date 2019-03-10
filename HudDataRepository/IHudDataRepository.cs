using Hud.Data.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hud.Data.Service
{
    /// <summary>
    /// contains methods to access HUD data.
    /// </summary>
    public interface IHudDataRepository
    {
        /// <summary>
        /// Returns mappings associated with the specified Zip code.
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        Task<IEnumerable<ZipCbsaItem>> GetZipCbsaItems(int zipCode);

        /// <summary>
        /// Returns mappings associated with the specified CBSA code.
        /// </summary>
        /// <param name="cbsaCode">The CBSA code to filter by.</param>
        /// <param name="lsad">The LSAD text to filter by.</param>
        /// <returns></returns>
        Task<IEnumerable<CbsaMsaItem>> GetCbsaMsaItems(CbsaMsaItemsSearchRequest request);

        /// <summary>
        /// Updates the the Zip code to CBSA code mapping. 
        /// </summary>
        /// <param name="zipCbsaItems"></param>
        /// <returns></returns>
        Task UpdateZipMapping(IEnumerable<ZipCbsaItem> zipCbsaItems);

        /// <summary>
        /// Updates the CBSA, MSA details mapping.
        /// </summary>
        /// <param name="cbsaMsaItems"></param>
        /// <returns></returns>
        Task UpdateStatisticalAreaMapping(IEnumerable<CbsaMsaItem> cbsaMsaItems);
    }
}