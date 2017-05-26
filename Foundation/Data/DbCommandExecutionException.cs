﻿using System;
using System.Data;
using System.Text;
using Foundation.Linq;

namespace Foundation.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DbCommandExecutionException : Exception
    {
        private readonly string _database;
        private readonly string _commandText;
        private readonly int _commandTimeout;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="command"></param>
        public DbCommandExecutionException(string message, Exception innerException, IDbCommand command)
            : base(message, innerException)
        {
            this._commandText = command.ToLogString();
            this._commandTimeout = command.CommandTimeout;
            this._database = command.Connection.Database;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("DbCommandExecutionException: {0}\r\ninnerException:\r\n{1}\r\ndatabase: {2}\r\ncommandTimeout: {3}\r\ncommandText: {4}",
                this.Message,
                this.InnerException.ToLogString(),
                this._database,
                this._commandTimeout,
                this._commandText);
            var s = sb.ToString();
            return s;
        }
    }
}