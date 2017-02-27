﻿namespace LightSqlProfiler.Core.Enums
{
    /// <summary>
    /// Supported columns (which are bound to even class)
    /// 
    /// See:
    /// SELECT * FROM sys.trace_columns
    /// </summary>
    public enum EventColumnType
    {
        TextData = 1,
        BinaryData = 2,
        DatabaseID = 3,
        TransactionID = 4,
        LineNumber = 5,
        NTUserName = 6,
        NTDomainName = 7,
        HostName = 8,
        ClientProcessID = 9,
        ApplicationName = 10,
        LoginName = 11,
        SPID = 12,
        Duration = 13,
        StartTime = 14,
        EndTime = 15,
        Reads = 16,
        Writes = 17,
        CPU = 18,
        Permissions = 19,
        Severity = 20,
        EventSubClass = 21,
        ObjectId = 22,
        Success = 23,
        IndexId = 24,
        IntegerData = 25,
        ServerName = 26,
        EventClass = 27,
        ObjectType = 28,
        NestLevel = 29,
        State = 30,
        Error = 31,
        Mode = 32,
        Handle = 33,
        ObjectName = 34,
        DatabaseName = 35,
        FileName = 36,
        OwnerName = 37,
        RoleName = 38,
        TargetUserName = 39,
        DBUserName = 40,
        LoginSId = 41,
        TargetLoginName = 42,
        TargetLoginSid = 43,
        ColumnPermissions = 44,
        LinkedServerName = 45,
        ProviderName = 46,
        MethodName = 47,
        RowCounts = 48,
        RequestId = 49,
        XactSequence = 50,
        EventSequence = 51,
        BigintData1 = 52,
        BigintData2 = 53,
        Guid = 54,
        IntefetData2 = 55,
        ObjectID2 = 56,
        Type = 57,
        OwnerId = 58,
        ParentName = 59,
        IsSystem = 60,
        Offset = 61,
        SourceDatabaseId = 62,
        SqlHandle = 63,
        SessionLoginName = 64,
        PlanHandle = 65,
        GroupID = 66
    }
}
