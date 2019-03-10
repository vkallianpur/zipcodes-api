using Hud.Application.Service.Helpers;
using Hud.Domain.Service;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Hud.Application.Service.Controllers
{
    /// <summary>
    /// The API to add or update CBSA to MSA mappings.
    /// </summary>
    public class CbsaToMsaMappingsController : ApiController
    {
        private readonly IMappingService _mappingService = new MappingService(); //todo remove after DI is in place
        private readonly CbsaToMsaMappingsParser _inputFileParser = new CbsaToMsaMappingsParser(); //todo remove after DI is in place

        //private readonly IMappingService _mappingService;
        //public CbsaToMsaMappingsController(IMappingService mappingService = null)
        //{
        //    _mappingService = mappingService;
        //}

        /// <summary>
        /// Adds new (or replaces existing) CBSA to MSA mappings.
        /// The mappings are specified in a comma separated file that has a header with the following fields in sequence:
        /// CBSA,MDIV,STCOU,NAME,LSAD,...POPESTIMATE2014,POPESTIMATE2015
        /// </summary>
        public async Task<HttpResponseMessage> Post()
        {
            var mappings = await _inputFileParser.GetMappings(Request).ConfigureAwait(false);
            await _mappingService.UpdateStatisticalAreaMapping(mappings).ConfigureAwait(false);

            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }
}