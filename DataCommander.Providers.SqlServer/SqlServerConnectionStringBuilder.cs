﻿using Foundation.Linq;

namespace DataCommander.Providers.SqlServer
{
    using System.Data.SqlClient;

    internal sealed class SqlServerConnectionStringBuilder : IDbConnectionStringBuilder
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder = new SqlConnectionStringBuilder();

        #region IDbConnectionStringBuilder Members

        string IDbConnectionStringBuilder.ConnectionString
        {
            get => _sqlConnectionStringBuilder.ConnectionString;

            set => _sqlConnectionStringBuilder.ConnectionString = value;
        }

        bool IDbConnectionStringBuilder.IsKeywordSupported(string keyword)
        {
            var suppoertedKeywords = new[]
            {
                "Integrated Security"
            };

            return suppoertedKeywords.Contains(keyword);
        }

        bool IDbConnectionStringBuilder.TryGetValue(string keyword, out object value)
        {
            return _sqlConnectionStringBuilder.TryGetValue(keyword, out value);
        }

        void IDbConnectionStringBuilder.SetValue(string keyword, object value)
        {
            _sqlConnectionStringBuilder[keyword] = value;
        }

        #endregion
    }
}