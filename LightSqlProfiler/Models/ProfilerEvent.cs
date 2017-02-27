using LightSqlProfiler.Core.Enums;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace LightSqlProfiler.Models
{
    /// <summary>
    /// User visible, directly mapped to GUI class
    /// Represents a full event row, implemented as dynamic object
    /// </summary>
    public class ProfilerEvent : DynamicObject
    {
        /// <summary>
        /// Dynamic dictionary containing all columns for the profiler event
        /// </summary>
        private Dictionary<EventColumnType, object> _values = new Dictionary<EventColumnType, object>();

        /// <summary>
        /// Event/row type
        /// </summary>
        private EventClassType _evType;

        public EventClassType EventType => _evType;

        /// <summary>
        /// Converts GUI dynamic binding and returns displayable object shown directly to user
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            EventColumnType col;
            if (Enum.TryParse<EventColumnType>(binder.Name, out col))
            {
                result = Humanize(col);
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Value for the "EventClass" column
        /// </summary>
        public string EventClass => _evType.ToString();

        private string Humanize(EventColumnType columnType)
        {
            if (_values.ContainsKey(columnType))
                return _values[columnType].ToString();
            return "-";
        }

        /// <summary>
        /// Identifies if captured event is "real" or "internal" (produced by our own app - and should be ignored)
        /// Custom events are not-internal
        /// </summary>
        public bool IsInternal
        {
            get
            {
                // during trace registration, we might capture our own calls
                // this workaround marks trace registration even coming from us as internal events
                if (
                    HasColumn(EventColumnType.ApplicationName) &&
                    HasColumn(EventColumnType.TextData) &&
                    string.Equals(GetValue(EventColumnType.ApplicationName).ToString(), "Light SQL Profiler", StringComparison.InvariantCultureIgnoreCase) &&
                    GetValue(EventColumnType.TextData).ToString().Contains("sp_trace_setstatus"))
                {
                    return true;
                }

                return (int)_evType > 65000 || _evType == EventClassType.Unknown;
            }
        }

        public ProfilerEvent(EventClassType evType)
        {
            _evType = evType;
        }

        public void SetColumnValue(EventColumnType columnType, object value)
        {
            _values[columnType] = value;
        }

        public bool HasColumn(EventColumnType columnType)
        {
            return _values.ContainsKey(columnType);
        }

        public object GetValue(EventColumnType columnType)
        {
            if (!_values.ContainsKey(columnType))
                return null;
            return _values[columnType];
        }

        /// <summary>
        /// Checks if this event passes a filter for a given column
        /// </summary>
        public bool PassesFilter(EventColumnType columnType, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return true;

            if (_values.ContainsKey(columnType))
                return _values[columnType].ToString().Contains(query);

            return false;
        }
    }
}
