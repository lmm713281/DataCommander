﻿using Foundation.Assertions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DataCommander.Providers.Connection
{
    public abstract class ConnectionBase
    {
        public IDbConnection Connection { get; protected set; }

        public abstract Task OpenAsync(CancellationToken cancellationToken);

        public void Close()
        {
            if (Connection != null)
                Connection.Close();
        }

        public abstract IDbCommand CreateCommand();

        public abstract string ConnectionName { get; set; }

        public string ConnectionString => Connection.ConnectionString;

        public abstract string Caption { get; }

        public abstract string DataSource { get; }

        protected abstract void SetDatabase(string database);

        public string Database
        {
            get
            {
                string database;

                if (Connection != null)
                {
                    database = Connection.Database;
                }
                else
                {
                    database = null;
                }

                return database;
            }

            set => SetDatabase(value);
        }

        public abstract string ServerVersion { get; }

        public ConnectionState State
        {
            get
            {
                Assert.IsTrue(Connection != null);
                return Connection.State;
            }
        }

        public abstract int TransactionCount { get; }

        protected void InvokeInfoMessage(IEnumerable<InfoMessage> messages)
        {
            if (InfoMessage != null)
                InfoMessage(messages);
        }

        protected void InvokeDatabaseChanged(string database)
        {
            if (DatabaseChanged != null)
            {
                var args = new DatabaseChangedEventArgs
                {
                    Database = database
                };
                DatabaseChanged(this, args);
            }
        }

        public event InfoMessageEventHandler InfoMessage;
        public event EventHandler<DatabaseChangedEventArgs> DatabaseChanged;
    }
}