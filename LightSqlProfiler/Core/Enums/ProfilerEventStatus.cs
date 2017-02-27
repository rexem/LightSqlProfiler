namespace LightSqlProfiler.Core.Enums
{
    /// <summary>
    /// Types of event data read-out types
    /// </summary>
    public enum ProfilerEventStatus
    {
        /// <summary>
        /// When we read-back event data from DB, it gets filled one column at the time;
        /// Update type events means that we're not done reading all event data, but simply "updating" the column
        /// </summary>
        Update,

        /// <summary>
        /// Once each column returns its data, the event is considered done and we can trigger a NewEvent
        /// </summary>
        NewEvent
    }
}
