using LightSqlProfiler.Core.Enums;
using LightSqlProfiler.Core.Trace.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LightSqlProfiler.Core
{
    /// <summary>
    /// Collection of commonly used methods
    /// </summary>
    public static class Common
    {
        private static byte[] iv = { 2, 2, 3, 4, 4, 6, 8, 8 };
        private static byte[] key = { 1, 1, 3, 3, 5, 5, 7, 7 };

        /// <summary>
        /// Encrypts and encodes the password
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string EncodePassword(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Convert.ToBase64String(outputBuffer);
        }

        /// <summary>
        /// Decrypts and decodes the password to the raw format
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string DecodePassword(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            byte[] inputbuffer = Convert.FromBase64String(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Encoding.Unicode.GetString(outputBuffer);
        }

        /// <summary>
        /// Locates parent object in WPF tree by its type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current"></param>
        /// <returns></returns>
        public static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                    return (T)current;
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        /// <summary>
        /// Produces a dictionary of all mapped events and columns for a trace
        /// 
        /// Takes in independently selected events and columns, and combines them into a map;
        /// Takes into account that not all selected columns are available for all event types
        /// </summary>
        /// <param name="events"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static Dictionary<EventClassType, List<EventColumnType>> MapRegisteredColumnsToEvents(IEnumerable<EventClassType> events, IEnumerable<EventColumnType> columns)
        {
            var result = new Dictionary<EventClassType, List<EventColumnType>>();
            foreach (var ev in events)
            {
                // grab all available columns for this event type
                var all = EventColumnMap.Instance.AvailableColumns[ev];

                // filter user selected columns to the ones that are available by MSSQL definition
                var filtered = columns.Where(x => all.Contains(x));
                result.Add(ev, filtered.ToList());
            }
            return result;
        }

        /// <summary>
        /// Gets full absolute name to application file by its name
        /// Effectively prepends special-folder location to the given filename
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetAppFilePath(string filename)
        {
            // root folder where all dynamic files sit
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LightSqlProfiler");

            // todo: allow loading from file in the same folder (portable app)
            return Path.Combine(folder, filename);
        }
    }
}
