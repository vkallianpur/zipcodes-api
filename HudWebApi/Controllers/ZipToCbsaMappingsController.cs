using Hud.Application.Service.Helpers;
using Hud.Domain.Service;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Hud.Application.Service.Controllers
{
    /// <summary>
    /// The API to add or update Zip to CBSA mappings.
    /// </summary>
    public class ZipToCbsaMappingsController : ApiController
    {
        private readonly IMappingService _mappingService = new MappingService(); //todo remove after DI is in place
        private readonly ZipToCbsaMappingsParser _inputFileParser = new ZipToCbsaMappingsParser(); //todo remove after DI is in place

        //private readonly IMappingService _mappingService;
        //public ZipToCbsaMappingsController(IMappingService mappingService = null)
        //{
        //    _mappingService = mappingService;
        //}

        /// <summary>
        /// Adds new (or replaces existing) Zip to CBSA mappings.
        /// The mappings are specified in a comma separated file that has a header with the following fields in sequence:
        /// ZIP,CBSA,RES_RATIO,BUS_RATIO,OTH_RATIO,TOT_RATIO
        /// </summary>
        public async Task<HttpResponseMessage> Post()
        {
            var mappings = await _inputFileParser.GetMappings(Request).ConfigureAwait(false);
            await _mappingService.UpdateZipMapping(mappings).ConfigureAwait(false);

            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }
}
