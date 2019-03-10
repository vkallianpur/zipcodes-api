namespace Hud.Data.Service
{
    /// <summary>
    /// holds filter criteria used to query CBSA MSA mappings.
    /// </summary>
    public class CbsaMsaItemsSearchRequest
    {
        /// <summary>
        /// The Core Based Statistical Area code.
        /// </summary>
        public int? CbsaCode { get; set; } 

        /// <summary>
        /// Metropolitan Division code.
        /// </summary>
        public int? MDiv { get; set; } 

        /// <summary>
        /// Legal/Statistical Area Description.
        /// </summary>
        public string Lsad { get; set; }
    }
}