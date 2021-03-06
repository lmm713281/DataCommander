﻿using Foundation.Data;

namespace DataCommander.Providers.Odp.ObjectExplorer
{
    using System.Collections.Generic;
    using System.Windows.Forms;

    /// <summary>
    /// Summary description for TablesNode.
    /// </summary>
    internal sealed class PackageCollectionNode : ITreeNode
    {
        public PackageCollectionNode(SchemaNode schema)
        {
            _schema = schema;
        }

        public string Name => "Packages";

        public bool IsLeaf => false;

        public IEnumerable<ITreeNode> GetChildren(bool refresh)
        {
            var folder = DataCommanderApplication.Instance.ApplicationData.CurrentType;
            var key = _schema.SchemasNode.Connection.DataSource + "." + _schema.Name;
            string[] packages;
            var contains = folder.Attributes.TryGetAttributeValue(key, out packages);

            if (!contains || refresh)
            {
                var commandText = "select object_name from all_objects where owner = '{0}' and object_type = 'PACKAGE' order by object_name";
                commandText = string.Format(commandText, _schema.Name);
                var executor = _schema.SchemasNode.Connection.CreateCommandExecutor();
                var dataTable = executor.ExecuteDataTable(new ExecuteReaderRequest(commandText));
                var count = dataTable.Rows.Count;
                packages = new string[count];

                for (var i = 0; i < count; i++)
                    packages[i] = (string) dataTable.Rows[i][0];

                folder.Attributes.SetAttributeValue(key, packages);
            }

            var treeNodes = new ITreeNode[packages.Length];

            for (var i = 0; i < packages.Length; i++)
                treeNodes[i] = new PackageNode(_schema, packages[i]);

            return treeNodes;
        }

        public bool Sortable => false;
        public string Query => null;
        public SchemaNode Schema => _schema;
        public ContextMenuStrip ContextMenu => null;

        public void BeforeExpand()
        {
        }

        private readonly SchemaNode _schema;
    }
}