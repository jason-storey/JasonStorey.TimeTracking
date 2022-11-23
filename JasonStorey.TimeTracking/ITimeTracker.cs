using System.Collections.Generic;
using System.Threading.Tasks;

namespace JasonStorey.TimeTracking
{
    public interface ITimeTracker
    {
        Task<IEnumerable<Entry>> GetRecentEntriesAsync();
        Task<Entry> GetCurrentEntryAsync();
        Task<Entry> StartTask(string taskName);
    }
}