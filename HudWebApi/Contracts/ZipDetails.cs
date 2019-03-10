namespace Hud.Application.Service.Contracts
{
    /// <summary>
    /// Zip code details.
    /// </summary>
    public class ZipDetails
    {
        /// <summary>
        /// The Zip code. 
        /// </summary>
        public string ZipCode { get; set; } 

        /// <summary>
        /// The Core Based Statistical Area code. 
        /// </summary>
        public string CbsaCode { get; set; }

        /// <summary>
        /// The Metropolitan Statistical Area name.
        /// </summary>
        public string MsaName { get; set; }

        /// <summary>
        /// The Legal/Statistical Area Description.
        /// </summary>
        public string Lsad { get; set; }

        /// <summary>
        /// Population estimates by year.
        /// </summary>
        public PopulationEstimateItem[] PopulationEstimates { get; set; }
    }
}