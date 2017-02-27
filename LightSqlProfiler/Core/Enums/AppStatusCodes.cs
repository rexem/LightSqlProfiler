namespace LightSqlProfiler.Core.Enums
{
    /// <summary>
    /// Application status codes
    /// </summary>
    public enum AppStatusCodes
    {
        /// <summary>
        /// No connection, ready to connect
        /// </summary>
        Ready = 0,

        /// <summary>
        /// Trace is running
        /// </summary>
        Running = 1,

        /// <summary>
        /// Connection/registration in process
        /// </summary>
        Connecting = 2,

        /// <summary>
        /// Disconnecting/unregistering in process
        /// </summary>
        Disconnecting = 3
    }
}
