using LightSqlProfiler.Core.Enums;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace LightSqlProfiler.Core.Trace.Entities
{
    /// <summary>
    /// Data handler for trace event column
    /// Parses raw binary data to proper type
    /// 
    /// Each EventColumnType has one of these attached to it
    /// </summary>
    public class TraceColumn
    {
        /// <summary>
        /// Converts value in DbReader to column specific type
        /// </summary>
        /// <param name="reader"></param>
        private delegate object ConverterFunction(DbDataReader reader);

        /// <summary>
        /// Points to specific converter function for this column
        /// </summary>
        private ConverterFunction _dataConverterFunction;

        /// <summary>
        /// Official column type
        /// </summary>
        public EventColumnType ColumnType { get; set; }

        /// <summary>
        /// Column value data type
        /// </summary>
        public EventColumnDataType DataType { get; set; }
        
        /// <summary>
        /// Define new trace column
        /// </summary>
        /// <param name="columnType">Column type</param>
        /// <param name="dataType">Column data type</param>
        public TraceColumn(EventColumnType columnType, EventColumnDataType dataType)
        {
            ColumnType = columnType;
            DataType = dataType;

            // assign appropriate converter
            switch (dataType)
            {
                case EventColumnDataType.String:
                    _dataConverterFunction = StringConverter;
                    break;

                case EventColumnDataType.Int:
                    _dataConverterFunction = IntConverter;
                    break;

                case EventColumnDataType.Long:
                    _dataConverterFunction = LongConverter;
                    break;

                case EventColumnDataType.Byte:
                    _dataConverterFunction = ByteConverter;
                    break;

                case EventColumnDataType.Guid:
                    _dataConverterFunction = GuidConverter;
                    break;

                case EventColumnDataType.DateTime:
                    _dataConverterFunction = DateTimeConverter;
                    break;

                default:
                    throw new NotImplementedException("Invalid column data type");
            }
        }

        /// <summary>
        /// Reads value (using converter based on the column data type) from the reader
        /// </summary>
        /// <param name="dbReader"></param>
        public object Process(SqlDataReader dbReader)
        {
            return _dataConverterFunction(dbReader);
        }

        private object StringConverter(DbDataReader reader)
        {
            return Encoding.Unicode.GetString((byte[])reader[2]);
        }

        private object IntConverter(DbDataReader reader)
        {
            byte[] buffer = new byte[4];
            reader.GetBytes(2, 0, buffer, 0, 4);
            return (int)((buffer[0]) | (buffer[1] << 8) | (buffer[2] << 16) | (buffer[3] << 24));
        }

        private object LongConverter(DbDataReader reader)
        {
            byte[] buffer = new byte[8];
            reader.GetBytes(2, 0, buffer, 0, 8);

            int i1 = (buffer[0]) | (buffer[1] << 8) | (buffer[2] << 16) | (buffer[3] << 24);
            int i2 = (buffer[4]) | (buffer[5] << 8) | (buffer[6] << 16) | (buffer[7] << 24);
            return (uint)i1 | ((long)i2 << 32);
        }

        private object ByteConverter(DbDataReader reader)
        {
            // 3rd parameter contains byte[] array
            if (reader.FieldCount >= 2)
            {
                byte[] b = (byte[])reader[2];

                // build a 0x... string (each byte will take 2 characters + "0x" in front)
                StringBuilder sb = new StringBuilder("0x", 2 + b.Length * 2);
                for (int i = 0; i < b.Length; i++)
                    sb.Append(b[i].ToString("X2"));

                return sb.ToString();
            }
            return null;
        }

        private object GuidConverter(DbDataReader reader)
        {
            byte[] buffer = new byte[16];
            reader.GetBytes(2, 0, buffer, 0, 16);
            return new Guid(buffer);
        }

        private object DateTimeConverter(DbDataReader reader)
        {
            byte[] buffer = new byte[16];
            reader.GetBytes(2, 0, buffer, 0, 16);
            
            int year = buffer[0] | buffer[1] << 8;
            int month = buffer[2] | buffer[3] << 8;
            int day = buffer[6] | buffer[7] << 8;
            int hour = buffer[8] | buffer[9] << 8;
            int min = buffer[10] | buffer[11] << 8;
            int sec = buffer[12] | buffer[13] << 8;
            int msec = buffer[14] | buffer[15] << 8;
            return new DateTime(year, month, day, hour, min, sec, msec);
        }
    }
}
