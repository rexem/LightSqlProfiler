using LightSqlProfiler.Core.Enums;
using LightSqlProfiler.Core.Trace.Entities;
using System.Collections.Generic;

namespace LightSqlProfiler.Core.Trace.Definitions
{
    /// <summary>
    /// Class contains all definitions of trace events and columns
    /// Maps their properties, names, etc
    /// 
    /// Use as a singleton
    /// </summary>
    public class EventDefinition
    {
        #region Singleton
        private static EventDefinition _instance;

        /// <summary>
        /// Unique global instance of this entity
        /// </summary>
        public static EventDefinition Instance
        {
            get { return _instance ?? (_instance = new EventDefinition()); }
        }
        #endregion

        /// <summary>
        /// All registered events
        /// </summary>
        public List<TraceEvent> Events { get; set; } = new List<TraceEvent>();

        /// <summary>
        /// All registered columns
        /// </summary>
        public List<TraceColumn> Columns { get; set; } = new List<TraceColumn>();

        /// <summary>
        /// Constructor which creates all event/column definitions
        /// </summary>
        private EventDefinition()
        {
            InitColumns();
            InitEvents();
        }

        /// <summary>
        /// Creates column definitions
        /// </summary>
        private void InitColumns()
        {
            Columns.Add(new TraceColumn(EventColumnType.TextData, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.BinaryData, EventColumnDataType.Byte));
            Columns.Add(new TraceColumn(EventColumnType.DatabaseID, EventColumnDataType.Int));
            Columns.Add(new TraceColumn(EventColumnType.TransactionID, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.LineNumber, EventColumnDataType.Int));
            Columns.Add(new TraceColumn(EventColumnType.NTUserName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.NTDomainName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.HostName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.ClientProcessID, EventColumnDataType.Int));
            Columns.Add(new TraceColumn(EventColumnType.ApplicationName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.LoginName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.SPID, EventColumnDataType.Int));
            Columns.Add(new TraceColumn(EventColumnType.Duration, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.StartTime, EventColumnDataType.DateTime));
            Columns.Add(new TraceColumn(EventColumnType.EndTime, EventColumnDataType.DateTime));
            Columns.Add(new TraceColumn(EventColumnType.Reads, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.Writes, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.CPU, EventColumnDataType.Int));
            Columns.Add(new TraceColumn(EventColumnType.Permissions, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.Severity, EventColumnDataType.Int));
            Columns.Add(new TraceColumn(EventColumnType.EventSubClass, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.ObjectId, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.Success, EventColumnDataType.Int));
            Columns.Add(new TraceColumn(EventColumnType.IndexId, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.IntegerData, EventColumnDataType.Int));
            Columns.Add(new TraceColumn(EventColumnType.ServerName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.EventClass, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.ObjectType, EventColumnDataType.Int));
            Columns.Add(new TraceColumn(EventColumnType.NestLevel, EventColumnDataType.Int));
            Columns.Add(new TraceColumn(EventColumnType.State, EventColumnDataType.Int));
            Columns.Add(new TraceColumn(EventColumnType.Error, EventColumnDataType.Int));
            Columns.Add(new TraceColumn(EventColumnType.Mode, EventColumnDataType.Int));
            Columns.Add(new TraceColumn(EventColumnType.Handle, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.ObjectName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.DatabaseName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.FileName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.OwnerName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.RoleName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.TargetUserName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.DBUserName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.LoginSId, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.TargetLoginName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.TargetLoginSid, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.ColumnPermissions, EventColumnDataType.Int));
            Columns.Add(new TraceColumn(EventColumnType.LinkedServerName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.ProviderName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.MethodName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.RowCounts, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.RequestId, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.XactSequence, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.EventSequence, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.BigintData1, EventColumnDataType.Byte));
            Columns.Add(new TraceColumn(EventColumnType.BigintData2, EventColumnDataType.Byte));
            Columns.Add(new TraceColumn(EventColumnType.Guid, EventColumnDataType.Guid));
            Columns.Add(new TraceColumn(EventColumnType.IntefetData2, EventColumnDataType.Byte));
            Columns.Add(new TraceColumn(EventColumnType.ObjectID2, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.Type, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.OwnerId, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.ParentName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.IsSystem, EventColumnDataType.Int));
            Columns.Add(new TraceColumn(EventColumnType.Offset, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.SourceDatabaseId, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.SqlHandle, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.SessionLoginName, EventColumnDataType.String));
            Columns.Add(new TraceColumn(EventColumnType.PlanHandle, EventColumnDataType.Long));
            Columns.Add(new TraceColumn(EventColumnType.GroupID, EventColumnDataType.Long));
        }

        /// <summary>
        /// Creates event definitions
        /// </summary>
        private void InitEvents()
        {
            /*** Auto-generated using:
                 SELECT
                    CONCAT(
                        'Events.Add(new TraceEvent("',
                        t.[Name],
                        '", "',
                        c.[Name],
                        '", (EventClassType)',
                        t.trace_event_id,
                        '));'
                    )
                FROM sys.trace_events t
                JOIN sys.trace_categories c ON c.category_id = t.category_id
             */

            Events.Add(new TraceEvent("RPC:Completed", "Stored Procedures", (EventClassType)10));
            Events.Add(new TraceEvent("RPC:Starting", "Stored Procedures", (EventClassType)11));
            Events.Add(new TraceEvent("SQL:BatchCompleted", "TSQL", (EventClassType)12));
            Events.Add(new TraceEvent("SQL:BatchStarting", "TSQL", (EventClassType)13));
            Events.Add(new TraceEvent("Audit Login", "Security Audit", (EventClassType)14));
            Events.Add(new TraceEvent("Audit Logout", "Security Audit", (EventClassType)15));
            Events.Add(new TraceEvent("Attention", "Errors and Warnings", (EventClassType)16));
            Events.Add(new TraceEvent("ExistingConnection", "Sessions", (EventClassType)17));
            Events.Add(new TraceEvent("Audit Server Starts And Stops", "Security Audit", (EventClassType)18));
            Events.Add(new TraceEvent("DTCTransaction", "Transactions", (EventClassType)19));
            Events.Add(new TraceEvent("Audit Login Failed", "Security Audit", (EventClassType)20));
            Events.Add(new TraceEvent("EventLog", "Errors and Warnings", (EventClassType)21));
            Events.Add(new TraceEvent("ErrorLog", "Errors and Warnings", (EventClassType)22));
            Events.Add(new TraceEvent("Lock:Released", "Locks", (EventClassType)23));
            Events.Add(new TraceEvent("Lock:Acquired", "Locks", (EventClassType)24));
            Events.Add(new TraceEvent("Lock:Deadlock", "Locks", (EventClassType)25));
            Events.Add(new TraceEvent("Lock:Cancel", "Locks", (EventClassType)26));
            Events.Add(new TraceEvent("Lock:Timeout", "Locks", (EventClassType)27));
            Events.Add(new TraceEvent("Degree of Parallelism", "Performance", (EventClassType)28));
            Events.Add(new TraceEvent("Exception", "Errors and Warnings", (EventClassType)33));
            Events.Add(new TraceEvent("SP:CacheMiss", "Stored Procedures", (EventClassType)34));
            Events.Add(new TraceEvent("SP:CacheInsert", "Stored Procedures", (EventClassType)35));
            Events.Add(new TraceEvent("SP:CacheRemove", "Stored Procedures", (EventClassType)36));
            Events.Add(new TraceEvent("SP:Recompile", "Stored Procedures", (EventClassType)37));
            Events.Add(new TraceEvent("SP:CacheHit", "Stored Procedures", (EventClassType)38));
            Events.Add(new TraceEvent("SQL:StmtStarting", "TSQL", (EventClassType)40));
            Events.Add(new TraceEvent("SQL:StmtCompleted", "TSQL", (EventClassType)41));
            Events.Add(new TraceEvent("SP:Starting", "Stored Procedures", (EventClassType)42));
            Events.Add(new TraceEvent("SP:Completed", "Stored Procedures", (EventClassType)43));
            Events.Add(new TraceEvent("SP:StmtStarting", "Stored Procedures", (EventClassType)44));
            Events.Add(new TraceEvent("SP:StmtCompleted", "Stored Procedures", (EventClassType)45));
            Events.Add(new TraceEvent("Object:Created", "Objects", (EventClassType)46));
            Events.Add(new TraceEvent("Object:Deleted", "Objects", (EventClassType)47));
            Events.Add(new TraceEvent("SQLTransaction", "Transactions", (EventClassType)50));
            Events.Add(new TraceEvent("Scan:Started", "Scans", (EventClassType)51));
            Events.Add(new TraceEvent("Scan:Stopped", "Scans", (EventClassType)52));
            Events.Add(new TraceEvent("CursorOpen", "Cursors", (EventClassType)53));
            Events.Add(new TraceEvent("TransactionLog", "Transactions", (EventClassType)54));
            Events.Add(new TraceEvent("Hash Warning", "Errors and Warnings", (EventClassType)55));
            Events.Add(new TraceEvent("Auto Stats", "Performance", (EventClassType)58));
            Events.Add(new TraceEvent("Lock:Deadlock Chain", "Locks", (EventClassType)59));
            Events.Add(new TraceEvent("Lock:Escalation", "Locks", (EventClassType)60));
            Events.Add(new TraceEvent("OLEDB Errors", "OLEDB", (EventClassType)61));
            Events.Add(new TraceEvent("Execution Warnings", "Errors and Warnings", (EventClassType)67));
            Events.Add(new TraceEvent("Showplan Text (Unencoded)", "Performance", (EventClassType)68));
            Events.Add(new TraceEvent("Sort Warnings", "Errors and Warnings", (EventClassType)69));
            Events.Add(new TraceEvent("CursorPrepare", "Cursors", (EventClassType)70));
            Events.Add(new TraceEvent("Prepare SQL", "TSQL", (EventClassType)71));
            Events.Add(new TraceEvent("Exec Prepared SQL", "TSQL", (EventClassType)72));
            Events.Add(new TraceEvent("Unprepare SQL", "TSQL", (EventClassType)73));
            Events.Add(new TraceEvent("CursorExecute", "Cursors", (EventClassType)74));
            Events.Add(new TraceEvent("CursorRecompile", "Cursors", (EventClassType)75));
            Events.Add(new TraceEvent("CursorImplicitConversion", "Cursors", (EventClassType)76));
            Events.Add(new TraceEvent("CursorUnprepare", "Cursors", (EventClassType)77));
            Events.Add(new TraceEvent("CursorClose", "Cursors", (EventClassType)78));
            Events.Add(new TraceEvent("Missing Column Statistics", "Errors and Warnings", (EventClassType)79));
            Events.Add(new TraceEvent("Missing Join Predicate", "Errors and Warnings", (EventClassType)80));
            Events.Add(new TraceEvent("Server Memory Change", "Server", (EventClassType)81));
            Events.Add(new TraceEvent("UserConfigurable:0", "User configurable", (EventClassType)82));
            Events.Add(new TraceEvent("UserConfigurable:1", "User configurable", (EventClassType)83));
            Events.Add(new TraceEvent("UserConfigurable:2", "User configurable", (EventClassType)84));
            Events.Add(new TraceEvent("UserConfigurable:3", "User configurable", (EventClassType)85));
            Events.Add(new TraceEvent("UserConfigurable:4", "User configurable", (EventClassType)86));
            Events.Add(new TraceEvent("UserConfigurable:5", "User configurable", (EventClassType)87));
            Events.Add(new TraceEvent("UserConfigurable:6", "User configurable", (EventClassType)88));
            Events.Add(new TraceEvent("UserConfigurable:7", "User configurable", (EventClassType)89));
            Events.Add(new TraceEvent("UserConfigurable:8", "User configurable", (EventClassType)90));
            Events.Add(new TraceEvent("UserConfigurable:9", "User configurable", (EventClassType)91));
            Events.Add(new TraceEvent("Data File Auto Grow", "Database", (EventClassType)92));
            Events.Add(new TraceEvent("Log File Auto Grow", "Database", (EventClassType)93));
            Events.Add(new TraceEvent("Data File Auto Shrink", "Database", (EventClassType)94));
            Events.Add(new TraceEvent("Log File Auto Shrink", "Database", (EventClassType)95));
            Events.Add(new TraceEvent("Showplan Text", "Performance", (EventClassType)96));
            Events.Add(new TraceEvent("Showplan All", "Performance", (EventClassType)97));
            Events.Add(new TraceEvent("Showplan Statistics Profile", "Performance", (EventClassType)98));
            Events.Add(new TraceEvent("RPC Output Parameter", "Stored Procedures", (EventClassType)100));
            Events.Add(new TraceEvent("Audit Database Scope GDR Event", "Security Audit", (EventClassType)102));
            Events.Add(new TraceEvent("Audit Schema Object GDR Event", "Security Audit", (EventClassType)103));
            Events.Add(new TraceEvent("Audit Addlogin Event", "Security Audit", (EventClassType)104));
            Events.Add(new TraceEvent("Audit Login GDR Event", "Security Audit", (EventClassType)105));
            Events.Add(new TraceEvent("Audit Login Change Property Event", "Security Audit", (EventClassType)106));
            Events.Add(new TraceEvent("Audit Login Change Password Event", "Security Audit", (EventClassType)107));
            Events.Add(new TraceEvent("Audit Add Login to Server Role Event", "Security Audit", (EventClassType)108));
            Events.Add(new TraceEvent("Audit Add DB User Event", "Security Audit", (EventClassType)109));
            Events.Add(new TraceEvent("Audit Add Member to DB Role Event", "Security Audit", (EventClassType)110));
            Events.Add(new TraceEvent("Audit Add Role Event", "Security Audit", (EventClassType)111));
            Events.Add(new TraceEvent("Audit App Role Change Password Event", "Security Audit", (EventClassType)112));
            Events.Add(new TraceEvent("Audit Statement Permission Event", "Security Audit", (EventClassType)113));
            Events.Add(new TraceEvent("Audit Schema Object Access Event", "Security Audit", (EventClassType)114));
            Events.Add(new TraceEvent("Audit Backup/Restore Event", "Security Audit", (EventClassType)115));
            Events.Add(new TraceEvent("Audit DBCC Event", "Security Audit", (EventClassType)116));
            Events.Add(new TraceEvent("Audit Change Audit Event", "Security Audit", (EventClassType)117));
            Events.Add(new TraceEvent("Audit Object Derived Permission Event", "Security Audit", (EventClassType)118));
            Events.Add(new TraceEvent("OLEDB Call Event", "OLEDB", (EventClassType)119));
            Events.Add(new TraceEvent("OLEDB QueryInterface Event", "OLEDB", (EventClassType)120));
            Events.Add(new TraceEvent("OLEDB DataRead Event", "OLEDB", (EventClassType)121));
            Events.Add(new TraceEvent("Showplan XML", "Performance", (EventClassType)122));
            Events.Add(new TraceEvent("SQL:FullTextQuery", "Performance", (EventClassType)123));
            Events.Add(new TraceEvent("Broker:Conversation", "Broker", (EventClassType)124));
            Events.Add(new TraceEvent("Deprecation Announcement", "Deprecation", (EventClassType)125));
            Events.Add(new TraceEvent("Deprecation Final Support", "Deprecation", (EventClassType)126));
            Events.Add(new TraceEvent("Exchange Spill Event", "Errors and Warnings", (EventClassType)127));
            Events.Add(new TraceEvent("Audit Database Management Event", "Security Audit", (EventClassType)128));
            Events.Add(new TraceEvent("Audit Database Object Management Event", "Security Audit", (EventClassType)129));
            Events.Add(new TraceEvent("Audit Database Principal Management Event", "Security Audit", (EventClassType)130));
            Events.Add(new TraceEvent("Audit Schema Object Management Event", "Security Audit", (EventClassType)131));
            Events.Add(new TraceEvent("Audit Server Principal Impersonation Event", "Security Audit", (EventClassType)132));
            Events.Add(new TraceEvent("Audit Database Principal Impersonation Event", "Security Audit", (EventClassType)133));
            Events.Add(new TraceEvent("Audit Server Object Take Ownership Event", "Security Audit", (EventClassType)134));
            Events.Add(new TraceEvent("Audit Database Object Take Ownership Event", "Security Audit", (EventClassType)135));
            Events.Add(new TraceEvent("Broker:Conversation Group", "Broker", (EventClassType)136));
            Events.Add(new TraceEvent("Blocked process report", "Errors and Warnings", (EventClassType)137));
            Events.Add(new TraceEvent("Broker:Connection", "Broker", (EventClassType)138));
            Events.Add(new TraceEvent("Broker:Forwarded Message Sent", "Broker", (EventClassType)139));
            Events.Add(new TraceEvent("Broker:Forwarded Message Dropped", "Broker", (EventClassType)140));
            Events.Add(new TraceEvent("Broker:Message Classify", "Broker", (EventClassType)141));
            Events.Add(new TraceEvent("Broker:Transmission", "Broker", (EventClassType)142));
            Events.Add(new TraceEvent("Broker:Queue Disabled", "Broker", (EventClassType)143));
            Events.Add(new TraceEvent("Broker:Mirrored Route State Changed", "Broker", (EventClassType)144));
            Events.Add(new TraceEvent("Showplan XML Statistics Profile", "Performance", (EventClassType)146));
            Events.Add(new TraceEvent("Deadlock graph", "Locks", (EventClassType)148));
            Events.Add(new TraceEvent("Broker:Remote Message Acknowledgement", "Broker", (EventClassType)149));
            Events.Add(new TraceEvent("Trace File Close", "Server", (EventClassType)150));
            Events.Add(new TraceEvent("Database Mirroring Connection", "Database", (EventClassType)151));
            Events.Add(new TraceEvent("Audit Change Database Owner", "Security Audit", (EventClassType)152));
            Events.Add(new TraceEvent("Audit Schema Object Take Ownership Event", "Security Audit", (EventClassType)153));
            Events.Add(new TraceEvent("Audit Database Mirroring Login", "Security Audit", (EventClassType)154));
            Events.Add(new TraceEvent("FT:Crawl Started", "Full text", (EventClassType)155));
            Events.Add(new TraceEvent("FT:Crawl Stopped", "Full text", (EventClassType)156));
            Events.Add(new TraceEvent("FT:Crawl Aborted", "Full text", (EventClassType)157));
            Events.Add(new TraceEvent("Audit Broker Conversation", "Security Audit", (EventClassType)158));
            Events.Add(new TraceEvent("Audit Broker Login", "Security Audit", (EventClassType)159));
            Events.Add(new TraceEvent("Broker:Message Undeliverable", "Broker", (EventClassType)160));
            Events.Add(new TraceEvent("Broker:Corrupted Message", "Broker", (EventClassType)161));
            Events.Add(new TraceEvent("User Error Message", "Errors and Warnings", (EventClassType)162));
            Events.Add(new TraceEvent("Broker:Activation", "Broker", (EventClassType)163));
            Events.Add(new TraceEvent("Object:Altered", "Objects", (EventClassType)164));
            Events.Add(new TraceEvent("Performance statistics", "Performance", (EventClassType)165));
            Events.Add(new TraceEvent("SQL:StmtRecompile", "TSQL", (EventClassType)166));
            Events.Add(new TraceEvent("Database Mirroring State Change", "Database", (EventClassType)167));
            Events.Add(new TraceEvent("Showplan XML For Query Compile", "Performance", (EventClassType)168));
            Events.Add(new TraceEvent("Showplan All For Query Compile", "Performance", (EventClassType)169));
            Events.Add(new TraceEvent("Audit Server Scope GDR Event", "Security Audit", (EventClassType)170));
            Events.Add(new TraceEvent("Audit Server Object GDR Event", "Security Audit", (EventClassType)171));
            Events.Add(new TraceEvent("Audit Database Object GDR Event", "Security Audit", (EventClassType)172));
            Events.Add(new TraceEvent("Audit Server Operation Event", "Security Audit", (EventClassType)173));
            Events.Add(new TraceEvent("Audit Server Alter Trace Event", "Security Audit", (EventClassType)175));
            Events.Add(new TraceEvent("Audit Server Object Management Event", "Security Audit", (EventClassType)176));
            Events.Add(new TraceEvent("Audit Server Principal Management Event", "Security Audit", (EventClassType)177));
            Events.Add(new TraceEvent("Audit Database Operation Event", "Security Audit", (EventClassType)178));
            Events.Add(new TraceEvent("Audit Database Object Access Event", "Security Audit", (EventClassType)180));
            Events.Add(new TraceEvent("TM: Begin Tran starting", "Transactions", (EventClassType)181));
            Events.Add(new TraceEvent("TM: Begin Tran completed", "Transactions", (EventClassType)182));
            Events.Add(new TraceEvent("TM: Promote Tran starting", "Transactions", (EventClassType)183));
            Events.Add(new TraceEvent("TM: Promote Tran completed", "Transactions", (EventClassType)184));
            Events.Add(new TraceEvent("TM: Commit Tran starting", "Transactions", (EventClassType)185));
            Events.Add(new TraceEvent("TM: Commit Tran completed", "Transactions", (EventClassType)186));
            Events.Add(new TraceEvent("TM: Rollback Tran starting", "Transactions", (EventClassType)187));
            Events.Add(new TraceEvent("TM: Rollback Tran completed", "Transactions", (EventClassType)188));
            Events.Add(new TraceEvent("Lock:Timeout (timeout > 0)", "Locks", (EventClassType)189));
            Events.Add(new TraceEvent("Progress Report: Online Index Operation", "Progress Report", (EventClassType)190));
            Events.Add(new TraceEvent("TM: Save Tran starting", "Transactions", (EventClassType)191));
            Events.Add(new TraceEvent("TM: Save Tran completed", "Transactions", (EventClassType)192));
            Events.Add(new TraceEvent("Background Job Error", "Errors and Warnings", (EventClassType)193));
            Events.Add(new TraceEvent("OLEDB Provider Information", "OLEDB", (EventClassType)194));
            Events.Add(new TraceEvent("Mount Tape", "Server", (EventClassType)195));
            Events.Add(new TraceEvent("Assembly Load", "CLR", (EventClassType)196));
            Events.Add(new TraceEvent("XQuery Static Type", "TSQL", (EventClassType)198));
            Events.Add(new TraceEvent("QN: Subscription", "Query Notifications", (EventClassType)199));
            Events.Add(new TraceEvent("QN: Parameter table", "Query Notifications", (EventClassType)200));
            Events.Add(new TraceEvent("QN: Template", "Query Notifications", (EventClassType)201));
            Events.Add(new TraceEvent("QN: Dynamics", "Query Notifications", (EventClassType)202));
            Events.Add(new TraceEvent("Bitmap Warning", "Errors and Warnings", (EventClassType)212));
            Events.Add(new TraceEvent("Database Suspect Data Page", "Errors and Warnings", (EventClassType)213));
            Events.Add(new TraceEvent("CPU threshold exceeded", "Errors and Warnings", (EventClassType)214));
            Events.Add(new TraceEvent("PreConnect:Starting", "Sessions", (EventClassType)215));
            Events.Add(new TraceEvent("PreConnect:Completed", "Sessions", (EventClassType)216));
            Events.Add(new TraceEvent("Plan Guide Successful", "Performance", (EventClassType)217));
            Events.Add(new TraceEvent("Plan Guide Unsuccessful", "Performance", (EventClassType)218));
            Events.Add(new TraceEvent("Audit Fulltext", "Security Audit", (EventClassType)235));
        }
    }
}
