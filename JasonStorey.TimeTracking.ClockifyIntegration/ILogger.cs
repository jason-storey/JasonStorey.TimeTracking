using System;

namespace JasonStorey.TimeTracking.ClockifyIntegration
{
    public interface ILogger
    {
        void LogError(Exception ex);
    }
}