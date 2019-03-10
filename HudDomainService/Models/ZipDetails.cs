namespace Hud.Domain.Service.Models
{
    /// <summary>
    /// Zip code details.
    /// </summary>
    public class ZipDetails
    {
        /// <summary>
        /// The Zip code. 
        /// </summary>
        public string ZipCode { get; set; } //storing as string so we can store 5 digit code that may include leading 0s.

        /// <summary>
        /// The Core Based Statistical Area code. 
        /// </summary>
        public string CbsaCode { get; set; } //storing as string so we can store 5 digit code that may include leading 0s.

        /// <summary>
        /// The name of the MSA.
        /// </summary>
        public string MsaName { get; set; }

        /// <summary>
        /// Legal/Statistical Area Description.
        /// </summary>
        public string Lsad { get; set; }

        /// <summary>
        /// Yearly population estimates.
        /// </summary>
        public PopulationEstimateItem[] PopulationEstimates { get; set; }
    }
}