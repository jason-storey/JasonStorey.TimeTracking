using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static JasonStorey.TimeTracking.ClockifyIntegration.ModelFactory;

namespace JasonStorey.TimeTracking.ClockifyIntegration
{
    public class Clockify : ITimeTracker
    {
        readonly ClockifyService _service;

        public Clockify(ClockifyService service) => _service = service;

        public async Task<Entry> GetCurrentEntryAsync()
        {
            var result = await _service.GetCurrentEntry();
            return ToEntry(result);
        }

        public async Task<IEnumerable<Entry>> GetRecentEntriesAsync()
        {
            var result = await _service.GetTimeEntries();
            return result.Select(ToEntry);
        }

        public async Task<Entry> StartTask(string taskName)
        {
            var result = await _service.StartTask(taskName);
            return ToEntry(result);
        }
    }
}