﻿using System.Collections.Generic;
using System.Windows.Forms;

namespace DataCommander.Providers.OracleBase.ObjectExplorer
{
    /// <summary>
    /// Summary description for SchemaNode.
    /// </summary>
    public sealed class SchemaNode : ITreeNode
    {
        private readonly SchemaCollectionNode schemasNode;
        private readonly string name;

        public SchemaNode(
            SchemaCollectionNode schemasNode,
            string name)
        {
            this.schemasNode = schemasNode;
            this.name = name;
        }

        public string Name => name;

        public bool IsLeaf => false;

        public IEnumerable<ITreeNode> GetChildren(bool refresh)
        {
            var treeNodes = new ITreeNode[]
            {
                new TableCollectionNode(this),
                new ViewCollectionNode(this),
                new SequenceCollectionNode(this),
                new ProcedureCollectionNode(this),
                new FunctionCollectionNode(this),
                new PackageCollectionNode(this),
                new SynonymCollectionNode(this)
            };

            return treeNodes;
        }

        public bool Sortable => false;
        public string Query => null;
        public ContextMenuStrip ContextMenu => null;
        public SchemaCollectionNode SchemasNode => schemasNode;
    }
}