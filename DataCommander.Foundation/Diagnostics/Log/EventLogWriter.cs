namespace DataCommander.Foundation.Diagnostics
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    public class EventLogWriter : ILogWriter
    {
        private static readonly ILog log = InternalLogFactory.Instance.GetTypeLog(typeof (EventLogWriter));
        private readonly EventLog eventLog;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logName"></param>
        /// <param name="machineName"></param>
        /// <param name="source"></param>
        public EventLogWriter(
            string logName,
            string machineName,
            string source)
        {
            try
            {
                if (!EventLog.SourceExists(source, machineName))
                {
                    var sourceData = new EventSourceCreationData(source, logName) {MachineName = machineName};
                    EventLog.CreateEventSource(sourceData);
                }
            }
            catch (Exception e)
            {
                log.Write(LogLevel.Error, e.ToString());
            }

            try
            {
                this.eventLog = new EventLog(logName, machineName, source);
            }
            catch (Exception e)
            {
                log.Write(LogLevel.Error, e.ToString());
            }
        }

        void IDisposable.Dispose()
        {
            this.eventLog.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Open()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Flush()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void Close()
        {
            try
            {
                this.eventLog.Close();
            }
            catch
            {
            }
        }

        void ILogWriter.Write(LogEntry entry)
        {
            EventLogEntryType eventLogEntryType;

            switch (entry.LogLevel)
            {
                case LogLevel.Debug:
                case LogLevel.Trace:
                case LogLevel.Information:
                    eventLogEntryType = EventLogEntryType.Information;
                    break;

                case LogLevel.Warning:
                    eventLogEntryType = EventLogEntryType.Warning;
                    break;

                default:
                    eventLogEntryType = EventLogEntryType.Error;
                    break;
            }

            try
            {
                string message = TextLogFormatter.Format(entry);
                this.eventLog.WriteEntry(message, eventLogEntryType);
            }
            catch (Exception e)
            {
                log.Write(LogLevel.Error, e.ToString());
            }
        }
    }
}