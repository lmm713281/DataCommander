﻿namespace DataCommander.Providers.Tfs
{
    using System;
    using System.Configuration;
    using System.Data.Common;
    using System.Data.SqlClient;

    internal sealed class TfsConnectionStringBuilder : IDbConnectionStringBuilder
    {
        private readonly DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder();

        string IDbConnectionStringBuilder.ConnectionString
        {
            get
            {
                return this.connectionStringBuilder.ConnectionString;
            }
            set
            {
                this.connectionStringBuilder.ConnectionString = value;
            }
        }

        bool IDbConnectionStringBuilder.IsKeywordSupported(string keyword)
        {
            return false;
        }

        void IDbConnectionStringBuilder.SetValue(string keyword, object value)
        {
            this.connectionStringBuilder[keyword] = value;
        }

        bool IDbConnectionStringBuilder.TryGetValue(string keyword, out object value)
        {
            return this.connectionStringBuilder.TryGetValue(keyword, out value);
        }
    }
}