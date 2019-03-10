using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hud.Data.Service;
using DataModels = Hud.Data.Service.Models;
using Hud.Domain.Service.Models;

namespace Hud.Domain.Service
{
    public class MappingService : IMappingService
    {
        private readonly IHudDataRepository _hudDataRepository;

        public MappingService(IHudDataRepository hudDataRepository = null)
        {
            _hudDataRepository = hudDataRepository
                ?? new HudDataRepository(); //todo remove after DI is in place
        }
        public Task UpdateStatisticalAreaMapping(CbsaMsaItem[] cbsaData)
        {
            var cbsaMappings = new List<DataModels.CbsaMsaItem>();
            cbsaMappings.AddRange(
                cbsaData.Select(q => new DataModels.CbsaMsaItem {
                                CbsaCode = q.CbsaCode,
                                MDiv = q.MDiv,
                                MsaName = q.MsaName,
                                Lsad = q.Lsad,
                                PopulationEstimates = q.PopulationEstimates.Select(
                                    s => new DataModels.PopulationEstimateItem
                                    {
                                        Year = s.Year,
                                        PopulationEstimate = s.PopulationEstimate}
                                    ).ToArray()}));

            return _hudDataRepository.UpdateStatisticalAreaMapping(cbsaMappings);
        }

        public Task UpdateZipMapping(ZipCbsaItem[] zipData)
        {
            var zipMappings = new List<DataModels.ZipCbsaItem>();
            zipMappings.AddRange(
                zipData.Select(q => new DataModels.ZipCbsaItem
                {
                    ZipCode = q.ZipCode,
                    CbsaCode = q.CbsaCode,
                    ResidentialRatio = q.ResidentialRatio,
                    BusinessRatio = q.BusinessRatio,
                    OtherRatio = q.OtherRatio,
                    TotalRatio = q.TotalRatio
                }).ToArray());

            return _hudDataRepository.UpdateZipMapping(zipMappings);
        }
    }
}