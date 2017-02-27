using LightSqlProfiler.Core.Enums;

namespace LightSqlProfiler.Core.Trace.Entities
{
    /// <summary>
    /// Contains information about specific profiler event
    /// </summary>
    public class TraceEvent
    {
        /// <summary>
        /// Event class/type
        /// </summary>
        public EventClassType EventClass { get; set; }

        /// <summary>
        /// Official event name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Official group name of the event
        /// </summary>
        public string Group { get; set; }

        public TraceEvent(string name, string group, EventClassType eventClass)
        {
            Name = name;
            Group = group;
            EventClass = eventClass;
        }
    }
}
