﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DataCommander.Providers.Query;
using Foundation.Data;

namespace DataCommander.Providers.OracleBase.ObjectExplorer
{
    public sealed class FunctionNode : ITreeNode
    {
        private readonly SchemaNode _schemaNode;
        private readonly PackageNode _packageNode;
        private readonly string _name;

        public FunctionNode(SchemaNode schemaNode, PackageNode packageNode, string name)
        {
            _schemaNode = schemaNode;
            _packageNode = packageNode;
            _name = name;
        }

        public string Name => _name;

        public bool IsLeaf => true;

        public IEnumerable<ITreeNode> GetChildren(bool refresh)
        {
            return null;
        }

        public bool Sortable => false;

        public string Query
        {
            get
            {
                var query = "EXEC " + _schemaNode.Name + '.';

                if (_packageNode != null)
                {
                    query += _packageNode.Name + '.';
                }

                query += _name;
                return query;
            }
        }

        private void ScriptObject_Click(object sender, EventArgs e)
        {
            var commandText =
                $@"select	text
from	all_source
where	owner = '{_schemaNode.Name}'
	and name = '{_name}'
	and type = 'FUNCTION'
order by line";
            var sb = new StringBuilder();
            var executor = _schemaNode.SchemasNode.Connection.CreateCommandExecutor();
            executor.ExecuteReader(new ExecuteReaderRequest(commandText), dataReader =>
            {
                dataReader.ReadResult(() =>
                {
                    var text = dataReader.GetString(0);
                    sb.Append(text);
                });
            });

            QueryForm.ShowText(sb.ToString());
        }

        public ContextMenuStrip ContextMenu
        {
            get
            {
                ContextMenuStrip contextMenu;

                if (_packageNode != null)
                    contextMenu = null;
                else
                {
                    var menuItem = new ToolStripMenuItem("Script Object", null, ScriptObject_Click);
                    contextMenu = new ContextMenuStrip();
                    contextMenu.Items.Add(menuItem);
                }

                return contextMenu;
            }
        }
    }
}