namespace DataCommander.Providers.OleDb
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Windows.Forms;

    internal sealed class ProcedureCollectionNode : ITreeNode
    {
        private readonly SchemaNode schema;

        public ProcedureCollectionNode(SchemaNode schema)
        {
            this.schema = schema;
        }

        public string Name
        {
            get
            {
                return "Procedures";
            }
        }

        public bool IsLeaf
        {
            get
            {
                return false;
            }
        }

        public IEnumerable<ITreeNode> GetChildren(bool refresh)
        {
            ITreeNode[] treeNodes;

            try
            {
                object[] restrictions = new object[] {this.schema.Catalog.Name, this.schema.Name};
                DataTable dataTable = this.schema.Catalog.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Procedures,
                    restrictions);
                int count = dataTable.Rows.Count;
                DataColumn procedureName = dataTable.Columns["PROCEDURE_NAME"];
                treeNodes = new ITreeNode[count];

                for (int i = 0; i < count; i++)
                {
                    string name = (string) dataTable.Rows[i][procedureName];
                    treeNodes[i] = new ProcedureNode(name);
                }
            }
            catch
            {
                treeNodes = new ITreeNode[] {new ProcedureNode(null)};
            }

            return treeNodes;
        }

        public bool Sortable
        {
            get
            {
                return false;
            }
        }

        public string Query
        {
            get
            {
                return null;
            }
        }

        public ContextMenuStrip ContextMenu
        {
            get
            {
                return null;
            }
        }

        public void BeforeExpand()
        {
        }
    }
}