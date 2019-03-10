using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hud.Data.Service;
using Hud.Domain.Service.Models;
using DataModels = Hud.Data.Service.Models;

namespace Hud.Domain.Service
{
    public class ZipCodeService : IZipCodeService
    {
        private const string _invalidCbsaCode = "99999";
        private const string _msaDescription = "Metropolitan Statistical Area";
        private readonly IHudDataRepository _hudDataRepository;

        public ZipCodeService(IHudDataRepository hudDataRepository = null)
        {
            _hudDataRepository = hudDataRepository 
                ?? new HudDataRepository(); //todo remove after DI is in place
            //SetupData().Wait();
        }
        public async Task<ZipDetails> GetZipDetails(int zipCode)
        {
            var zipMappings = await _hudDataRepository.GetZipCbsaItems(zipCode).ConfigureAwait(false);
            foreach (var zipMapping in zipMappings.Where(q => IsValidCbsaCode(q.CbsaCode)).ToList())
            {
                int cbsaCode;
                if (int.TryParse(zipMapping.CbsaCode, out cbsaCode) && cbsaCode > 0)
                {
                    var zipDetails = await GetZipMsaDetails(zipCode, cbsaCode).ConfigureAwait(false);
                    if(zipDetails != null)
                    {
                        return zipDetails;
                    }
                }
            }

            return null;
        }

        private async Task<ZipDetails> GetZipMsaDetails(int zipCode, int cbsaCode)
        {
            var alternateCbsaCode = await GetAlternateCbsaCode(cbsaCode).ConfigureAwait(false);
            var cbsaMapping = await GetCbsaMsaDetails(alternateCbsaCode ?? cbsaCode).ConfigureAwait(false);

            if (cbsaMapping != null)
            {
                var populationEstimates = new List<PopulationEstimateItem>();
                if (cbsaMapping.PopulationEstimates != null && cbsaMapping.PopulationEstimates.Any())
                {
                    populationEstimates.AddRange(
                        cbsaMapping.PopulationEstimates.Select(q => new PopulationEstimateItem
                        {
                            Year = q.Year,
                            PopulationEstimate = q.PopulationEstimate
                        }));
                }

                return new ZipDetails
                {
                    ZipCode = zipCode.ToString("D5"),
                    CbsaCode = cbsaMapping.CbsaCode,
                    MsaName = cbsaMapping.MsaName.Replace("\"",""),
                    Lsad = cbsaMapping.Lsad,
                    PopulationEstimates = populationEstimates.ToArray()
                };
            }

            return null;
        }

        private async Task<int?> GetAlternateCbsaCode(int cbsaCode)
        {
            var cbsaMappings = await _hudDataRepository.GetCbsaMsaItems(
                new CbsaMsaItemsSearchRequest { MDiv = cbsaCode }).ConfigureAwait(false);

            var mapping = cbsaMappings.FirstOrDefault();
            return (mapping != null && int.TryParse(mapping.CbsaCode, out var alternateCbsaCode) 
                && alternateCbsaCode > 0) 
                ? alternateCbsaCode 
                : (int?)null;
        }

        private async Task<DataModels.CbsaMsaItem> GetCbsaMsaDetails(int cbsaCode)
        {
            var cbsaMappings = await _hudDataRepository.GetCbsaMsaItems(
                new CbsaMsaItemsSearchRequest { CbsaCode = cbsaCode, Lsad = _msaDescription }).ConfigureAwait(false);
            return cbsaMappings.FirstOrDefault();
        }

        private static bool IsValidCbsaCode(string cbsaCode)
        {
            return !String.IsNullOrEmpty(cbsaCode) && cbsaCode != _invalidCbsaCode;
        }
    }
}