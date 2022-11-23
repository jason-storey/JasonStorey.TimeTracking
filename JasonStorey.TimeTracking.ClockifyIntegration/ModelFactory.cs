namespace JasonStorey.TimeTracking.ClockifyIntegration
{
    public static class ModelFactory
    {
        public static Entry ToEntry(TimeEntry entry) =>
            new Entry
            {
                Name = entry.Name,
                IsRunning = entry.IsRunning,
                Started = entry.Started,
                Ended = entry.Ended
            };
    }
}