﻿using Foundation.Data;

namespace DataCommander.Providers.SqlServer.ObjectExplorer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Windows.Forms;

    internal sealed class UserDefinedTableTypeCollectionNode : ITreeNode
    {
        private readonly DatabaseNode _database;

        public UserDefinedTableTypeCollectionNode(DatabaseNode database)
        {
            _database = database;
        }

        string ITreeNode.Name => "User-Defined Table Types";

        bool ITreeNode.IsLeaf => false;

        IEnumerable<ITreeNode> ITreeNode.GetChildren(bool refresh)
        {
            var commandText = string.Format(@"select
    s.name,
    t.name,
    type_table_object_id
from [{0}].sys.schemas s (nolock)
join [{0}].sys.table_types t (nolock)
    on s.schema_id = t.schema_id
order by 1,2", _database.Name);

            var tableTypeNodes = new List<UserDefinedTableTypeNode>();
            var connectionString = _database.Databases.Server.ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var transactionScope = new DbTransactionScope(connection, null);
                using (var reader = transactionScope.ExecuteReader(new CommandDefinition {CommandText = commandText}, CommandBehavior.Default))
                {
                    reader.Read(dataRecord =>
                    {
                        var schema = dataRecord.GetString(0);
                        var name = dataRecord.GetString(1);
                        var id = dataRecord.GetInt32(2);
                        var tableTypeNode = new UserDefinedTableTypeNode(_database, id, schema, name);
                        tableTypeNodes.Add(tableTypeNode);
                    });
                }
            }

            return tableTypeNodes;
        }

        bool ITreeNode.Sortable => false;

        string ITreeNode.Query => null;

        ContextMenuStrip ITreeNode.ContextMenu => null;
    }
}