﻿using Foundation.Data;

namespace DataCommander.Providers.SqlServer.ObjectExplorer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Windows.Forms;

    sealed class StoredProcedureCollectionNode : ITreeNode
    {
        public StoredProcedureCollectionNode(
            DatabaseNode database,
            bool isMsShipped)
        {
            _database = database;
            _isMsShipped = isMsShipped;
        }

        public string Name => _isMsShipped
            ? "System Stored Procedures"
            : "Stored Procedures";

        public bool IsLeaf => false;

        IEnumerable<ITreeNode> ITreeNode.GetChildren(bool refresh)
        {
            var commandText = string.Format(@"
select  s.name as Owner,
        o.name as Name        
from    [{0}].sys.all_objects o (readpast)
join    [{0}].sys.schemas s (readpast)
on      o.schema_id = s.schema_id
left join [{0}].sys.extended_properties p
on      o.object_id = p.major_id and p.minor_id = 0 and p.class = 1 and p.name = 'microsoft_database_tools_support'
where
    o.type = 'P'
    and o.is_ms_shipped = {1}
    and p.major_id is null
order by s.name,o.name", _database.Name, _isMsShipped
                ? 1
                : 0);

            DataTable dataTable;
            var connectionString = _database.Databases.Server.ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var transactionScope = new DbTransactionScope(connection, null);
                dataTable = transactionScope.ExecuteDataTable(new CommandDefinition {CommandText = commandText}, CancellationToken.None);
            }
            var dataRows = dataTable.Rows;
            var count = dataRows.Count;
            var treeNodes = new List<ITreeNode>();
            if (!_isMsShipped)
            {
                treeNodes.Add(new StoredProcedureCollectionNode(_database, true));
            }

            for (var i = 0; i < count; i++)
            {
                var row = dataRows[i];
                var owner = (string)row["Owner"];
                var name = (string)row["Name"];

                treeNodes.Add(new StoredProcedureNode(_database, owner, name));
            }

            return treeNodes;
        }

        public bool Sortable => false;

        public string Query => null;

        public ContextMenuStrip ContextMenu => null;

        readonly DatabaseNode _database;
        readonly bool _isMsShipped;
    }
}