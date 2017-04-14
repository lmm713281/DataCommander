﻿namespace DataCommander.Providers.Wmi
{
    using System.Data.Common;
    using System.Data.SqlClient;

    internal sealed class ConnectionStringBuilder : IDbConnectionStringBuilder
    {
        private readonly DbConnectionStringBuilder dbConnectionStringBuilder = new SqlConnectionStringBuilder();

        string IDbConnectionStringBuilder.ConnectionString
        {
            get => this.dbConnectionStringBuilder.ConnectionString;

            set => this.dbConnectionStringBuilder.ConnectionString = value;
        }

        bool IDbConnectionStringBuilder.IsKeywordSupported(string keyword)
        {
            return false;
        }

        void IDbConnectionStringBuilder.SetValue(string keyword, object value)
        {
            this.dbConnectionStringBuilder[keyword] = value;
        }

        bool IDbConnectionStringBuilder.TryGetValue(string keyword, out object value)
        {
            return this.dbConnectionStringBuilder.TryGetValue(keyword, out value);
        }
    }
}