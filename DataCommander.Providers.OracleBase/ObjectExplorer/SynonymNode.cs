﻿using System.Collections.Generic;
using System.Windows.Forms;
using Foundation.Data;

namespace DataCommander.Providers.OracleBase.ObjectExplorer
{
    public sealed class SynonymNode : ITreeNode
    {
        private readonly SchemaNode _schema;
        private readonly string _name;

        public SynonymNode(SchemaNode schema, string name)
        {
            _schema = schema;
            _name = name;
        }

        #region ITreeNode Members

        string ITreeNode.Name => _name;

        bool ITreeNode.IsLeaf => false;

        IEnumerable<ITreeNode> ITreeNode.GetChildren(bool refresh)
        {
            var commandText = $@"select	s.TABLE_OWNER,
	s.TABLE_NAME
from SYS.ALL_SYNONYMS s
where s.OWNER			= '{_schema.Name}'
    and s.SYNONYM_NAME	= '{_name}'";
            var executor = _schema.SchemasNode.Connection.CreateCommandExecutor();
            var dataTable = executor.ExecuteDataTable(new ExecuteReaderRequest(commandText));
            var dataRow = dataTable.Rows[0];
            var schemaName = (string) dataRow["TABLE_OWNER"];
            var schemaNode = new SchemaNode(_schema.SchemasNode, schemaName);
            var tableNode = new TableNode(schemaNode, (string) dataRow["TABLE_NAME"], true);
            return new ITreeNode[] {tableNode};
        }

        bool ITreeNode.Sortable => false;
        string ITreeNode.Query => null;
        ContextMenuStrip ITreeNode.ContextMenu => null;

        #endregion
    }
}