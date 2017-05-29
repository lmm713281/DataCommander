﻿namespace DataCommander.Providers.Odp.ObjectExplorer
{
    using System.Collections.Generic;
    using System.Data;
    using Oracle.ManagedDataAccess.Client;

    /// <summary>
    /// Summary description for ObjectBrowser.
    /// </summary>
    internal sealed class ObjectExplorer : IObjectExplorer
    {
        private OracleConnection _connection;
        private SchemaCollectionNode _schemasNode;

        public OracleConnection OracleConnection => _connection;

        public IEnumerable<ITreeNode> GetChildren(bool refresh)
        {
            return new ITreeNode[] { _schemasNode };
        }

        public bool Sortable => false;

        public SchemaCollectionNode SchemasNode => _schemasNode;

        #region IObjectExplorer Members

        void IObjectExplorer.SetConnection( string connectionString, IDbConnection connection )
        {
            _connection = (OracleConnection) connection;
            _schemasNode = new SchemaCollectionNode( _connection );
        }

        #endregion
    }
}