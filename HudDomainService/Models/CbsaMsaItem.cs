namespace Hud.Domain.Service.Models
{
    /// <summary>
    /// Represents each record in the CBSA, MSA mapping lookup table.
    /// </summary>
    public class CbsaMsaItem: MappingItem
    {
        /// <summary>
        /// The Core Based Statistical Area code.
        /// </summary>
        public string CbsaCode { get; set; } //storing as string so we can store 5 digit code that may include leading 0s.

        /// <summary>
        /// Metropolitan Division code.
        /// </summary>
        public string MDiv { get; set; } //storing as string so we can store 5 digit code that may include leading 0s.

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