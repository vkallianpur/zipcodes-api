using Hud.Domain.Service.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Hud.Application.Service.Helpers
{
    /// <summary>
    /// Parses CBSA to MSA mappings input. 
    /// </summary>
    internal class ZipToCbsaMappingsParser : InputFileParser<ZipCbsaItem>
    {
        private const string _zipToCbsaFileHeaders = "ZIP,CBSA,RES_RATIO,BUS_RATIO,OTH_RATIO,TOT_RATIO";

        private enum MappingColumnIndex
        {
            Zip = 0,
            Cbsa,
            ResidentialRatio,
            BusinessRatio,
            OtherRatio,
            TotalRatio
        }

        public override ZipCbsaItem[] ParseData(string data)
        {
            //Split lines of CSV into array
            var rows = data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            //Check if header row matches expected CSV layout
            //if (!rows[0].Equals(_zipToCbsaFileHeaders, StringComparison.InvariantCultureIgnoreCase))
            if (!rows[0].ToUpper().Contains(_zipToCbsaFileHeaders)) // for now using this since I'm getting some invalid characters prepended to ZIP
            {
                // todo return an error code here and have the controller throw the http exception
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = $"File does not contain the expected header fields: {_zipToCbsaFileHeaders}"
                });
            }

            //Remove header row
            rows = rows.Skip(1).ToArray();

            return rows.Select(GetZipMappingItem).ToArray();
        }

        private ZipCbsaItem GetZipMappingItem(string rowData)
        {
            var columnValues = rowData.Split(',');

            try
            {
                int.TryParse(columnValues[(int)MappingColumnIndex.Zip], out int zipCode);
                int.TryParse(columnValues[(int)MappingColumnIndex.Cbsa], out int cbsaCode);
                decimal.TryParse(columnValues[(int)MappingColumnIndex.ResidentialRatio], out decimal residentialRatio);
                decimal.TryParse(columnValues[(int)MappingColumnIndex.BusinessRatio], out decimal businesRatio);
                decimal.TryParse(columnValues[(int)MappingColumnIndex.OtherRatio], out decimal otherRatio);
                decimal.TryParse(columnValues[(int)MappingColumnIndex.TotalRatio], out decimal totalRatio);

                return new ZipCbsaItem
                {
                    ZipCode = zipCode > 0 ? zipCode.ToString("D5") : "",
                    CbsaCode = cbsaCode > 0 ? cbsaCode.ToString("D5") : "",
                    ResidentialRatio = residentialRatio,
                    BusinessRatio = businesRatio,
                    OtherRatio = otherRatio,
                    TotalRatio = totalRatio
                };
            }
            catch (Exception)
            {
                // for now, if a row doesn't have all the required columns, return empty object
                return new ZipCbsaItem();
            }
        }
    }
}