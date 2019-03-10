namespace Hud.Application.Service.Contracts
{
    /// <summary>
    /// The population estimate for a specific year.
    /// </summary>
    public class PopulationEstimateItem
    {
        /// <summary>
        /// The year the population estimate is for.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// The estimated population.
        /// </summary>
        public int PopulationEstimate { get; set; }
    }
}