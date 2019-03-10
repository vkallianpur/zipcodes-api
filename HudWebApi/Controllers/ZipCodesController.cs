using Hud.Application.Service.Contracts;
using Hud.Domain.Service;
using DomainModel = Hud.Domain.Service.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;

namespace Hud.Application.Service.Controllers
{
    /// <summary>
    /// The API to get zip code details.
    /// </summary>
    public class ZipCodesController : ApiController
    {
        //todo DI
        //private readonly IZipCodeService _zipCodeService;

        //public ZipCodesController(IZipCodeService zipCodeService)
        //{
        //    _zipCodeService = zipCodeService;
        //}

        private readonly IZipCodeService _zipCodeService = new ZipCodeService(); //todo remove after DI is in place

        /// <summary>
        /// Gets the details for the specified zip code.
        /// </summary>
        /// <param name="zip">The zip code to get the details for.</param>
        /// <returns><see cref="ZipDetails"/></returns>
        public async Task<ZipDetails> Get(int zip)
        {
            DomainModel.ZipDetails zipData;
            try
            {
                zipData = await _zipCodeService.GetZipDetails(zip).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // todo log error details if necessary
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ReasonPhrase = "An unexpected error occurred while getting the zip code details."
                });
            }
            if (zipData == null)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = "The specified zip code or its associated data could not be found."
                });
            }
            return new ZipDetails
            {
                ZipCode = zipData.ZipCode,
                CbsaCode = zipData.CbsaCode,
                MsaName = zipData.MsaName,
                Lsad = zipData.Lsad,
                PopulationEstimates = zipData.PopulationEstimates == null 
                    ? new PopulationEstimateItem[] { }
                    : zipData.PopulationEstimates.Select(q => new PopulationEstimateItem
                       {
                           Year = q.Year,
                           PopulationEstimate = q.PopulationEstimate
                       }).ToArray()
            };
        }
    }
}
