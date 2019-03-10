namespace Hud.Domain.Service.Models
{
    /// <summary>
    /// Represents each record in the Zip to CBSA codes mapping lookup table.
    /// </summary>
    public class ZipCbsaItem: MappingItem
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
        /// The ratio of residential addresses in the Zip CBSA part to the total number of residential addresses in the entire ZIP.
        /// </summary>
        public decimal ResidentialRatio { get; set; }

        /// <summary>
        /// /// The ratio of business addresses in the Zip CBSA part to the total number of business addresses in the entire ZIP.
        /// </summary>
        public decimal BusinessRatio { get; set; }

        /// <summary>
        /// The ratio of other addresses in the Zip CBSA part to the total number of other addresses in the entire ZIP.
        /// </summary>
        public decimal OtherRatio { get; set; }

        /// <summary>
        /// The ratio of total addresses in the Zip CBSA part to the total number of total addresses in the entire ZIP.
        /// </summary>
        public decimal TotalRatio { get; set; }
    }
}