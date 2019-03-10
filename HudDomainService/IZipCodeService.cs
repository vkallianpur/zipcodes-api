using Hud.Domain.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Hud.Domain.Service
{
    /// <summary>
    /// contains domain logic and methods to query and return Hud data
    /// </summary>
    public interface IZipCodeService
    {
        /// <summary>
        /// gets data associated with the specified Zip code.
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        Task<ZipDetails> GetZipDetails(int zipCode);
    }
}