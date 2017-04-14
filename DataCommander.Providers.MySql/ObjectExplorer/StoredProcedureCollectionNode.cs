﻿namespace DataCommander.Providers.MySql.ObjectExplorer
{
    using System.Collections.Generic;
    using System.Data;
    using Foundation.Data;
    using Foundation.Data.SqlClient;
    using global::MySql.Data.MySqlClient;

    internal sealed class StoredProcedureCollectionNode : ITreeNode
    {
        private readonly DatabaseNode databaseNode;

        public StoredProcedureCollectionNode(DatabaseNode databaseNode)
        {
            this.databaseNode = databaseNode;
        }

        string ITreeNode.Name => "Stored Procedures";

        bool ITreeNode.IsLeaf => false;

        IEnumerable<ITreeNode> ITreeNode.GetChildren(bool refresh)
        {
            var commandText =
                $@"select r.ROUTINE_NAME
from information_schema.ROUTINES r
where
    r.ROUTINE_SCHEMA = {this.databaseNode.Name.ToTSqlVarChar()
                    }
    and r.ROUTINE_TYPE = 'PROCEDURE'
order by r.ROUTINE_NAME";

            return MySqlClientFactory.Instance.ExecuteReader(
                this.databaseNode.ObjectExplorer.ConnectionString,
                new CommandDefinition {CommandText = commandText},
                CommandBehavior.Default,
                dataRecord =>
                {
                    var name = dataRecord.GetString(0);
                    return new StoredProcedureNode(this.databaseNode, name);
                });
        }

        bool ITreeNode.Sortable => false;

        string ITreeNode.Query => null;

        System.Windows.Forms.ContextMenuStrip ITreeNode.ContextMenu => null;
    }
}