﻿using System;
using System.Collections.Generic;

namespace Foundation.Diagnostics.Log
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class MemoryLogWriter : ILogWriter
    {
        private readonly ICollection<LogEntry> _logEntries;

        /// <summary>
        /// 
        /// </summary>
        public MemoryLogWriter()
        {
            this._logEntries = new List<LogEntry>();
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<LogEntry> LogEntries => this._logEntries;

        #region ILogWriter Members

        void ILogWriter.Open()
        {
        }

        void ILogWriter.Write( LogEntry logEntry )
        {
            lock (this._logEntries)
            {
                this._logEntries.Add( logEntry );
            }
        }

        void ILogWriter.Flush()
        {
        }

        void ILogWriter.Close()
        {
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
        }

        #endregion
    }
}