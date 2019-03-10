using Hud.Domain.Service.Models;
using System.Threading.Tasks;

namespace Hud.Domain.Service
{
    /// <summary>
    /// contains methods to perform admin-level functional like updating lookup tables
    /// </summary>
    public interface IMappingService
    {
        Task UpdateZipMapping(ZipCbsaItem[] zipData);

        Task UpdateStatisticalAreaMapping(CbsaMsaItem[] cbsaData);
    }
}