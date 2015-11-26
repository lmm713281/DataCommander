﻿namespace DataCommander.Providers.OracleBase
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public sealed class SequenceNode : ITreeNode
	{
		private SchemaNode schemaNode;
		private readonly string name;

		public SequenceNode( SchemaNode schemaNode, string name )
		{
			this.schemaNode = schemaNode;
			this.name = name;
		}

		#region ITreeNode Members

		string ITreeNode.Name
		{
			get
			{
				return this.name;
			}
		}

		bool ITreeNode.IsLeaf
		{
			get
			{
				return true;
			}
		}

		IEnumerable<ITreeNode> ITreeNode.GetChildren( bool refresh )
		{
			throw new NotImplementedException();
		}

		bool ITreeNode.Sortable
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		string ITreeNode.Query
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		ContextMenuStrip ITreeNode.ContextMenu
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		#endregion
	}
}