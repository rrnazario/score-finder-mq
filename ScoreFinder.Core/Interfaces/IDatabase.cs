using ScoreFinder.Core.Model;
using System.Collections.Generic;

namespace ScoreFinder.Core.Interfaces
{
    public interface IDatabase
    {
        /// <summary>
        /// Save live metrics for machines. These metrics will be used by orchestrator to create new crawlers.
        /// </summary>
        /// <param name="metrics"></param>
        void SaveMetric(MetricStatus metrics);
        /// <summary>
        /// Get best queue to create a crawler based on some metrics (such as Cpu Usage, Memory Usage, crawler numbers)
        /// </summary>
        /// <returns></returns>
        string GetBestQueue();

        /// <summary>
        /// Get sheets that were scheduled to be searched
        /// </summary>
        /// <returns></returns>
        IEnumerable<ScheduledSearch> GetSheetsToSearch();
    }
}
