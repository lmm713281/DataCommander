﻿namespace DataCommander.Providers
{
    public interface IDbConnectionStringBuilder
    {
        string ConnectionString { get; set; }

        bool IsKeywordSupported(string keyword);

        void SetValue(string keyword, object value);
        bool TryGetValue(string keyword, out object value);
    }
}