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
    internal class CbsaToMsaMappingsParser : InputFileParser<CbsaMsaItem>
    {
        private const string _cbsaToMsaFileHeaders = "CBSA,MDIV,STCOU,NAME,LSAD";
        private const string _headerPopEstimate2014 = "POPESTIMATE2014";
        private const string _headerPopEstimate2015 = "POPESTIMATE2015";

        private enum MappingColumnIndex
        {
            Cbsa = 0,
            Mdiv,
            Stcou,
            Name,
            Lsad
        }

        private int _columnIndex_PopEstimate2014;
        private int _columnIndex_PopEstimate2015;

        public override CbsaMsaItem[] ParseData(string data)
        {
            //Split lines of CSV into array
            var rows = data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            //Check if header row matches expected CSV layout
            if (!rows[0].ToUpper().StartsWith(_cbsaToMsaFileHeaders))
            {
                // todo return an error code here and have the controller throw the http exception
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = $"File does not contain the expected header fields: {_cbsaToMsaFileHeaders}"
                });
            }

            var headerRow = rows[0].Split(',');
            _columnIndex_PopEstimate2014 = Array.FindIndex(headerRow, q => q.Equals(_headerPopEstimate2014, StringComparison.InvariantCultureIgnoreCase));
            _columnIndex_PopEstimate2015 = Array.FindIndex(headerRow, q => q.Equals(_headerPopEstimate2015, StringComparison.InvariantCultureIgnoreCase));

            //Remove header row
            rows = rows.Skip(1).ToArray();

            return rows.Select(GetMappingItem).ToArray();
        }
        private CbsaMsaItem GetMappingItem(string rowData)
        {
            //var columnValues = rowData.Split(',');
            var columnValues = SplitCSV(rowData);
            try
            {
                int.TryParse(columnValues[(int)MappingColumnIndex.Cbsa], out int cbsaCode);
                int.TryParse(columnValues[(int)MappingColumnIndex.Mdiv], out int mDiv);
                int.TryParse(columnValues[_columnIndex_PopEstimate2014], out int popEst2014);
                int.TryParse(columnValues[_columnIndex_PopEstimate2015], out int popEst2015);

                return new CbsaMsaItem
                {
                    CbsaCode = cbsaCode > 0 ? cbsaCode.ToString("D5") : "",
                    MDiv = mDiv > 0 ? mDiv.ToString("D5") : "",
                    MsaName = columnValues[(int)MappingColumnIndex.Name],
                    Lsad = columnValues[(int)MappingColumnIndex.Lsad],
                    PopulationEstimates = new[] {
                        new PopulationEstimateItem { Year = 2014, PopulationEstimate = popEst2014 },
                        new PopulationEstimateItem { Year = 2015, PopulationEstimate = popEst2015 }
                    }
                };
            }
            catch (Exception)
            {
                // for now, if a row doesn't have all the required columns, return empty object
                return new CbsaMsaItem { PopulationEstimates = new PopulationEstimateItem[] { } };
            }
        }
    }
}