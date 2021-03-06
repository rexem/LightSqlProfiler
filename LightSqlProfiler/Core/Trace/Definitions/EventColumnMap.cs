﻿using LightSqlProfiler.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace LightSqlProfiler.Core.Trace.Definitions
{
    /// <summary>
    /// Definitions of which columns are available for each EventClassType
    /// Use as a singleton
    /// </summary>
    internal class EventColumnMap
    {
        #region Singleton

        private static EventColumnMap _instance;

        /// <summary>
        /// Unique global instance of this entity
        /// </summary>
        public static EventColumnMap Instance
        {
            get { return _instance ?? (_instance = new EventColumnMap()); }
        }

        #endregion Singleton

        /// <summary>
        /// Dictionary listing all available columns for trace registration for each trace event type (key)
        /// </summary>
        public Dictionary<EventClassType, List<EventColumnType>> AvailableColumns { get; set; }

        /// <summary>
        /// Populates AvailableColumns
        /// </summary>
        private EventColumnMap()
        {
            // columns are auto-generated from SQL query (which uses INT values)
            // map them to proper EventClassType and EventColumnType blocks
            Dictionary<int, int[]> raw = new Dictionary<int, int[]>();
            FillRawData(raw);

            // map raw data to enum-based dictionary
            AvailableColumns = raw.ToDictionary(
                (x) => (EventClassType)x.Key,
                (x) => x.Value.Select(z => (EventColumnType)z).ToList()
            );
        }

        /// <summary>
        /// Fill auto-generated values from MSSQL
        /// </summary>
        /// <param name="raw"></param>
        private void FillRawData(Dictionary<int, int[]> raw)
        {
            /*** Auto generated using the following query:

            SELECT
                CONCAT('Map.Add(', b.trace_event_id, ', new int[] { '),
                Cols =
                STUFF
                (
                    (
                        SELECT
                            CONCAT(', ', x.trace_column_id)
                        FROM sys.trace_event_bindings AS x
                        WHERE x.trace_event_id = b.trace_event_id
                        FOR XML PATH(N'')
                    ),
                1, 2, ''),
                '});'
            FROM sys.trace_event_bindings b
            GROUP BY b.trace_event_id

            ***/

            raw.Add(10, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 25, 26, 27, 31, 34, 35, 41, 48, 49, 50, 51, 60, 64, 66 });
            raw.Add(11, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 25, 26, 27, 34, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(12, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 26, 27, 31, 35, 41, 48, 49, 50, 51, 60, 64, 66 });
            raw.Add(13, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(14, new int[] { 1, 2, 3, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 25, 26, 27, 35, 41, 49, 51, 57, 60, 64, 66 });
            raw.Add(15, new int[] { 3, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 21, 23, 26, 27, 35, 41, 49, 51, 57, 60, 64, 66 });
            raw.Add(16, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 26, 27, 35, 41, 49, 51, 60, 64 });
            raw.Add(17, new int[] { 1, 2, 3, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 25, 26, 27, 35, 41, 49, 51, 55, 57, 60, 64, 66 });
            raw.Add(18, new int[] { 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 41, 49, 51, 60, 64 });
            raw.Add(19, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 25, 26, 27, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(20, new int[] { 1, 3, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 30, 31, 35, 49, 51, 57, 60, 64 });
            raw.Add(21, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 20, 26, 27, 30, 31, 35, 41, 49, 51, 60, 64 });
            raw.Add(22, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 20, 26, 27, 30, 31, 35, 41, 49, 51, 60, 64 });
            raw.Add(23, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 22, 26, 27, 32, 41, 49, 51, 52, 55, 56, 57, 58, 60, 64, 66 });
            raw.Add(24, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 22, 26, 27, 32, 41, 49, 51, 52, 55, 56, 57, 58, 60, 64, 66 });
            raw.Add(25, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 22, 25, 26, 27, 32, 35, 41, 49, 51, 52, 55, 56, 57, 58, 60, 64, 66 });
            raw.Add(26, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 22, 26, 27, 32, 35, 41, 49, 51, 52, 55, 56, 57, 58, 60, 64, 66 });
            raw.Add(27, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 22, 26, 27, 32, 35, 41, 49, 51, 52, 55, 56, 57, 58, 60, 64, 66 });
            raw.Add(28, new int[] { 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 25, 26, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(33, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 20, 26, 27, 30, 31, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(34, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 22, 26, 27, 28, 34, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(35, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 22, 26, 27, 28, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(36, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 22, 26, 27, 28, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(37, new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 21, 22, 26, 27, 28, 29, 34, 35, 41, 49, 50, 51, 55, 60, 61, 63, 64, 66 });
            raw.Add(38, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 22, 26, 27, 28, 34, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(40, new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 29, 30, 35, 41, 49, 50, 51, 55, 60, 61, 64, 66 });
            raw.Add(41, new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 25, 26, 27, 29, 35, 41, 48, 49, 50, 51, 55, 60, 61, 64, 66 });
            raw.Add(42, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 22, 26, 27, 28, 29, 34, 35, 41, 49, 50, 51, 60, 62, 64, 66 });
            raw.Add(43, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 22, 26, 27, 28, 29, 34, 35, 41, 48, 49, 50, 51, 60, 62, 64, 66 });
            raw.Add(44, new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 22, 26, 27, 28, 29, 30, 34, 35, 41, 49, 50, 51, 55, 60, 61, 62, 64, 66 });
            raw.Add(45, new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 22, 25, 26, 27, 28, 29, 34, 35, 41, 48, 49, 50, 51, 55, 60, 61, 62, 64, 66 });
            raw.Add(46, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 22, 24, 25, 26, 27, 28, 34, 35, 41, 49, 50, 51, 52, 56, 60, 64, 66 });
            raw.Add(47, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 22, 24, 25, 26, 27, 28, 34, 35, 41, 49, 50, 51, 52, 56, 60, 64, 66 });
            raw.Add(50, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 21, 25, 26, 27, 34, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(51, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 22, 24, 26, 27, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(52, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 16, 22, 24, 26, 27, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(53, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 25, 26, 27, 33, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(54, new int[] { 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 22, 24, 25, 26, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(55, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 22, 25, 26, 27, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(58, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 21, 22, 23, 24, 25, 26, 27, 31, 35, 41, 49, 51, 55, 57, 60, 64, 66 });
            raw.Add(59, new int[] { 1, 2, 3, 4, 12, 14, 21, 22, 25, 26, 27, 32, 35, 41, 49, 51, 52, 55, 56, 57, 58, 60, 64 });
            raw.Add(60, new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 21, 22, 25, 26, 27, 32, 35, 41, 49, 51, 55, 56, 57, 58, 60, 61, 64, 66 });
            raw.Add(61, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 27, 31, 35, 41, 45, 46, 47, 49, 51, 60, 64, 66 });
            raw.Add(67, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 21, 26, 27, 31, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(68, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 22, 25, 26, 27, 28, 29, 34, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(69, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 26, 27, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(70, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 33, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(71, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 33, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(72, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 33, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(73, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 33, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(74, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 25, 26, 27, 33, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(75, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(76, new int[] { 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 25, 26, 27, 33, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(77, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 33, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(78, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 33, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(79, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(80, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(81, new int[] { 4, 12, 14, 21, 25, 26, 27, 49, 50, 51, 60, 64 });
            raw.Add(82, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(83, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(84, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(85, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(86, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(87, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(88, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(89, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(90, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(91, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(92, new int[] { 3, 7, 8, 9, 10, 11, 12, 13, 14, 15, 25, 26, 27, 35, 36, 41, 51, 60, 64 });
            raw.Add(93, new int[] { 3, 7, 8, 9, 10, 11, 12, 13, 14, 15, 25, 26, 27, 35, 36, 41, 51, 60, 64 });
            raw.Add(94, new int[] { 3, 7, 8, 9, 10, 11, 12, 13, 14, 15, 25, 26, 27, 35, 36, 41, 51, 60, 64 });
            raw.Add(95, new int[] { 3, 7, 8, 9, 10, 11, 12, 13, 14, 15, 25, 26, 27, 35, 36, 41, 51, 60, 64 });
            raw.Add(96, new int[] { 2, 3, 4, 5, 7, 8, 9, 10, 11, 12, 14, 22, 25, 26, 27, 28, 29, 34, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(97, new int[] { 2, 3, 4, 5, 7, 8, 9, 10, 11, 12, 14, 22, 25, 26, 27, 28, 29, 34, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(98, new int[] { 2, 3, 4, 5, 7, 8, 9, 10, 11, 12, 14, 22, 25, 26, 27, 28, 29, 34, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(100, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 34, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(102, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 19, 21, 23, 26, 27, 28, 29, 34, 35, 37, 39, 40, 41, 42, 43, 49, 50, 51, 60, 64 });
            raw.Add(103, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 19, 21, 23, 26, 27, 28, 29, 34, 35, 37, 39, 40, 41, 42, 43, 44, 49, 50, 51, 59, 60, 64 });
            raw.Add(104, new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 28, 35, 40, 41, 42, 43, 49, 50, 51, 60, 64 });
            raw.Add(105, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 28, 35, 40, 41, 42, 43, 49, 50, 51, 60, 64 });
            raw.Add(106, new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 42, 43, 49, 50, 51, 60, 64 });
            raw.Add(107, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 42, 43, 49, 50, 51, 60, 64 });
            raw.Add(108, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 34, 35, 37, 38, 40, 41, 42, 43, 49, 50, 51, 60, 64 });
            raw.Add(109, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 35, 37, 38, 39, 40, 41, 42, 43, 44, 49, 50, 51, 60, 64 });
            raw.Add(110, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 34, 35, 37, 38, 39, 40, 41, 42, 43, 49, 50, 51, 60, 64 });
            raw.Add(111, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 35, 38, 40, 41, 49, 50, 51, 60, 64 });
            raw.Add(112, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 23, 26, 27, 28, 29, 34, 35, 37, 38, 40, 41, 49, 50, 51, 60, 64 });
            raw.Add(113, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 19, 21, 23, 26, 27, 28, 29, 35, 40, 41, 49, 50, 51, 60, 64 });
            raw.Add(114, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 19, 21, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 44, 49, 50, 51, 59, 60, 64 });
            raw.Add(115, new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 49, 50, 51, 60, 64 });
            raw.Add(116, new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 23, 26, 27, 29, 35, 37, 40, 41, 44, 49, 50, 51, 60, 64 });
            raw.Add(117, new int[] { 1, 3, 5, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 29, 35, 37, 40, 41, 44, 49, 50, 51, 60, 64 });
            raw.Add(118, new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 49, 50, 51, 60, 64 });
            raw.Add(119, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 21, 27, 31, 35, 41, 45, 46, 47, 49, 51, 60, 64, 66 });
            raw.Add(120, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 21, 27, 31, 35, 41, 45, 46, 47, 49, 51, 60, 64, 66 });
            raw.Add(121, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 21, 27, 31, 35, 41, 45, 46, 47, 49, 51, 60, 64, 66 });
            raw.Add(122, new int[] { 1, 2, 3, 4, 5, 7, 8, 9, 10, 11, 12, 14, 22, 25, 26, 27, 28, 29, 34, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(123, new int[] { 1, 3, 4, 7, 8, 9, 10, 11, 12, 13, 14, 15, 22, 25, 26, 27, 31, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(124, new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 12, 14, 21, 26, 27, 34, 36, 37, 38, 39, 40, 41, 42, 46, 47, 51, 54, 60, 64 });
            raw.Add(125, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 22, 26, 27, 34, 35, 41, 49, 50, 51, 55, 60, 61, 63, 64 });
            raw.Add(126, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 22, 26, 27, 34, 35, 41, 49, 50, 51, 55, 60, 61, 63, 64 });
            raw.Add(127, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 22, 26, 27, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(128, new int[] { 1, 3, 4, 6, 7, 8, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 49, 50, 51, 60, 64 });
            raw.Add(129, new int[] { 1, 3, 4, 5, 6, 7, 8, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 49, 50, 51, 60, 64 });
            raw.Add(130, new int[] { 1, 3, 4, 6, 7, 8, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 34, 35, 37, 39, 40, 41, 42, 43, 49, 50, 51, 60, 64 });
            raw.Add(131, new int[] { 1, 3, 4, 6, 7, 8, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 49, 50, 51, 59, 60, 64 });
            raw.Add(132, new int[] { 1, 2, 3, 4, 6, 7, 8, 10, 11, 12, 14, 19, 21, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 49, 50, 51, 60, 64 });
            raw.Add(133, new int[] { 1, 2, 3, 4, 6, 7, 8, 10, 11, 12, 14, 19, 21, 23, 26, 27, 28, 29, 34, 35, 37, 38, 40, 41, 49, 50, 51, 60, 64 });
            raw.Add(134, new int[] { 1, 3, 4, 6, 7, 8, 10, 11, 12, 14, 23, 26, 27, 28, 29, 34, 35, 37, 39, 40, 41, 42, 43, 49, 50, 51, 60, 64 });
            raw.Add(135, new int[] { 1, 3, 4, 6, 7, 8, 10, 11, 12, 14, 23, 26, 27, 28, 29, 34, 35, 37, 39, 40, 41, 49, 50, 51, 60, 64 });
            raw.Add(136, new int[] { 3, 4, 6, 7, 8, 9, 10, 12, 14, 21, 26, 27, 41, 51, 54, 60, 64 });
            raw.Add(137, new int[] { 1, 3, 4, 12, 13, 14, 15, 22, 24, 26, 27, 32, 41, 51, 60, 64 });
            raw.Add(138, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 12, 14, 21, 26, 27, 31, 34, 41, 51, 54, 60, 64 });
            raw.Add(139, new int[] { 3, 4, 6, 7, 8, 9, 10, 12, 14, 22, 23, 24, 25, 26, 27, 36, 37, 38, 39, 40, 41, 42, 46, 47, 51, 52, 54, 64 });
            raw.Add(140, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 12, 14, 20, 22, 23, 24, 25, 26, 27, 30, 31, 36, 37, 38, 39, 40, 41, 42, 46, 47, 51, 52, 54, 64 });
            raw.Add(141, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 12, 14, 21, 26, 27, 31, 36, 37, 38, 41, 45, 47, 51, 54, 59, 60, 64 });
            raw.Add(142, new int[] { 3, 4, 6, 7, 8, 9, 10, 12, 14, 20, 21, 26, 27, 30, 31, 38, 41, 51, 54, 60, 64 });
            raw.Add(143, new int[] { 3, 4, 6, 7, 8, 9, 10, 12, 14, 22, 26, 27, 41, 51, 60, 64 });
            raw.Add(144, new int[] { 1, 3, 6, 7, 8, 9, 10, 11, 12, 14, 21, 26, 27, 34, 35, 37, 39, 42, 47, 51, 60, 64 });
            raw.Add(146, new int[] { 1, 2, 3, 4, 5, 7, 8, 9, 10, 11, 12, 14, 22, 25, 26, 27, 28, 29, 34, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(148, new int[] { 1, 4, 11, 12, 14, 26, 27, 41, 51, 60, 64 });
            raw.Add(149, new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 12, 14, 21, 25, 26, 27, 32, 38, 41, 51, 52, 53, 54, 55, 60, 64 });
            raw.Add(150, new int[] { 11, 12, 14, 22, 26, 27, 36, 51, 60, 64 });
            raw.Add(151, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 12, 14, 21, 26, 27, 31, 34, 41, 51, 54, 60, 64 });
            raw.Add(152, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 23, 26, 27, 28, 29, 34, 35, 37, 39, 40, 41, 42, 43, 49, 50, 51, 60, 64 });
            raw.Add(153, new int[] { 1, 3, 4, 6, 7, 8, 10, 11, 12, 14, 23, 26, 27, 28, 29, 34, 35, 37, 39, 40, 41, 42, 43, 49, 50, 51, 59, 60, 64 });
            raw.Add(154, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 26, 27, 30, 34, 36, 37, 38, 39, 41, 46, 51, 60, 64 });
            raw.Add(155, new int[] { 1, 3, 4, 12, 14, 22, 27, 51, 60, 64 });
            raw.Add(156, new int[] { 3, 4, 12, 14, 22, 27, 51, 60, 64 });
            raw.Add(157, new int[] { 3, 4, 12, 14, 22, 27, 30, 31, 51, 60, 64 });
            raw.Add(158, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 12, 14, 20, 21, 22, 25, 26, 27, 30, 31, 38, 40, 41, 42, 51, 52, 54, 60, 64 });
            raw.Add(159, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 26, 27, 30, 34, 36, 37, 38, 39, 41, 46, 51, 54, 60, 64 });
            raw.Add(160, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 20, 21, 25, 26, 27, 30, 31, 36, 37, 38, 39, 40, 41, 42, 46, 51, 52, 53, 54, 55, 60, 64 });
            raw.Add(161, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 12, 14, 20, 26, 27, 30, 31, 41, 51, 60, 64 });
            raw.Add(162, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 20, 26, 27, 30, 31, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(163, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 12, 13, 14, 15, 16, 17, 18, 21, 22, 25, 26, 27, 41, 51, 60, 64 });
            raw.Add(164, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 22, 24, 25, 26, 27, 28, 34, 35, 41, 49, 50, 51, 52, 56, 60, 64, 66 });
            raw.Add(165, new int[] { 1, 2, 3, 12, 13, 14, 18, 21, 22, 25, 27, 28, 51, 52, 53, 55, 61, 63, 64, 65, 66 });
            raw.Add(166, new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 21, 22, 26, 27, 28, 29, 34, 35, 41, 49, 50, 51, 55, 60, 61, 63, 64, 66 });
            raw.Add(167, new int[] { 1, 3, 4, 12, 14, 25, 26, 27, 30, 35, 41, 49, 51, 60, 64 });
            raw.Add(168, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 22, 25, 26, 27, 28, 29, 34, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(169, new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 22, 25, 26, 27, 28, 29, 34, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(170, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 19, 21, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 42, 43, 49, 50, 51, 60, 64 });
            raw.Add(171, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 19, 21, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 42, 43, 49, 50, 51, 60, 64 });
            raw.Add(172, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 19, 21, 23, 26, 27, 28, 29, 34, 35, 37, 39, 40, 41, 42, 43, 49, 50, 51, 60, 64 });
            raw.Add(173, new int[] { 1, 3, 4, 6, 7, 8, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 49, 50, 51, 60, 64 });
            raw.Add(175, new int[] { 1, 3, 4, 6, 7, 8, 10, 11, 12, 14, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 49, 50, 51, 60, 64 });
            raw.Add(176, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 45, 46, 49, 50, 51, 60, 64 });
            raw.Add(177, new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 34, 35, 37, 39, 40, 41, 42, 43, 45, 49, 50, 51, 60, 64 });
            raw.Add(178, new int[] { 1, 3, 4, 6, 7, 8, 10, 11, 12, 14, 21, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 49, 50, 51, 60, 64 });
            raw.Add(180, new int[] { 1, 2, 3, 4, 6, 7, 8, 10, 11, 12, 14, 19, 23, 26, 27, 28, 29, 34, 35, 37, 40, 41, 49, 50, 51, 60, 64 });
            raw.Add(181, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(182, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 23, 26, 27, 31, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(183, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(184, new int[] { 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 23, 26, 27, 31, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(185, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 26, 27, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(186, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 31, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(187, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 26, 27, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(188, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 23, 26, 27, 31, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(189, new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 22, 26, 27, 32, 35, 41, 49, 51, 52, 55, 56, 57, 58, 60, 64, 66 });
            raw.Add(190, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 21, 22, 24, 26, 27, 34, 35, 41, 51, 52, 53, 60, 64, 66 });
            raw.Add(191, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 35, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(192, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 23, 26, 27, 31, 35, 41, 49, 50, 51, 54, 60, 64, 66 });
            raw.Add(193, new int[] { 3, 12, 14, 20, 21, 22, 24, 25, 27, 30, 31, 35, 51, 55, 57, 64 });
            raw.Add(194, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 27, 35, 41, 45, 46, 49, 51, 60, 64, 66 });
            raw.Add(195, new int[] { 1, 3, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 21, 26, 27, 35, 51, 60, 64, 66 });
            raw.Add(196, new int[] { 1, 3, 6, 7, 8, 9, 10, 11, 12, 14, 22, 23, 26, 27, 34, 35, 41, 49, 51, 64, 66 });
            raw.Add(198, new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 26, 27, 35, 41, 47, 49, 50, 51, 60, 64, 66 });
            raw.Add(199, new int[] { 1, 3, 6, 7, 8, 9, 10, 11, 12, 14, 21, 26, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(200, new int[] { 1, 3, 6, 7, 8, 9, 10, 11, 12, 14, 21, 26, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(201, new int[] { 1, 3, 6, 7, 8, 9, 10, 11, 12, 14, 21, 26, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(202, new int[] { 1, 3, 6, 7, 8, 9, 10, 11, 12, 14, 21, 26, 27, 35, 41, 49, 51, 60, 64, 66 });
            raw.Add(212, new int[] { 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 21, 22, 25, 26, 27, 35, 41, 49, 50, 51, 60, 64 });
            raw.Add(213, new int[] { 3, 12, 14, 21, 22, 27, 31, 51, 56, 64 });
            raw.Add(214, new int[] { 12, 14, 18, 26, 27, 41, 51, 58, 60, 64, 66 });
            raw.Add(215, new int[] { 3, 6, 7, 8, 9, 10, 11, 12, 14, 21, 22, 26, 27, 28, 34, 35, 41, 49, 50, 51, 60, 64 });
            raw.Add(216, new int[] { 3, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 21, 22, 26, 27, 28, 30, 31, 34, 35, 39, 41, 49, 50, 51, 60, 64, 66 });
            raw.Add(217, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 22, 26, 27, 35, 41, 49, 50, 51, 60, 64 });
            raw.Add(218, new int[] { 1, 3, 4, 6, 7, 8, 9, 10, 11, 12, 14, 22, 26, 27, 35, 41, 49, 50, 51, 60, 64 });
            raw.Add(235, new int[] { 1, 12, 14, 21, 23, 27, 31, 42, 43, 51, 60, 64 });
        }
    }
}
