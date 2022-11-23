using System;

namespace JasonStorey.TimeTracking
{
    public class Entry
    {
        public DateTimeOffset Started { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; }
        public DateTimeOffset? Ended { get; set; }
    }
}