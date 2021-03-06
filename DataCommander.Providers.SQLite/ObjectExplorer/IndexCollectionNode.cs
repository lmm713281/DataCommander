﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Foundation.Data;
using Foundation.Diagnostics.Contracts;

namespace DataCommander.Providers.SQLite.ObjectExplorer
{
    internal sealed class IndexCollectionNode : ITreeNode
    {
        private readonly TableNode _tableNode;

        public IndexCollectionNode(TableNode tableNode)
        {
            FoundationContract.Requires<ArgumentException>(tableNode != null);

            _tableNode = tableNode;
        }

        #region ITreeNode Members

        string ITreeNode.Name => "Indexes";
        bool ITreeNode.IsLeaf => false;

        IEnumerable<ITreeNode> ITreeNode.GetChildren(bool refresh)
        {
            var commandText = $"PRAGMA index_list({_tableNode.Name});";
            var executor = DbCommandExecutorFactory.Create(_tableNode.Database.Connection);
            var indexNodes = executor.ExecuteReader(new ExecuteReaderRequest(commandText), dataRecord =>
            {
                var name = dataRecord.GetString(0);
                return new IndexNode(_tableNode, name);
            });
            return indexNodes;
        }

        bool ITreeNode.Sortable => false;
        string ITreeNode.Query => null;
        ContextMenuStrip ITreeNode.ContextMenu => null;

        #endregion
    }
}