﻿namespace DataCommander.Providers.SqlServer.ObjectExplorer
{
    using System.Collections.Generic;
    using System.Windows.Forms;

    internal sealed class SchemaNode : ITreeNode
    {
        private readonly DatabaseNode _database;

        public SchemaNode(DatabaseNode database, string name)
        {
            _database = database;
            Name = name;
        }

        public string Name { get; }

        public bool IsLeaf => true;

        IEnumerable<ITreeNode> ITreeNode.GetChildren(bool refresh)
        {
            return null;
        }

        public bool Sortable => false;

        public string Query => null;

        public ContextMenuStrip ContextMenu => null;
    }
}