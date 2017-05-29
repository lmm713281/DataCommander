﻿namespace DataCommander.Providers.ResultWriter
{
    using System;
    using System.Data;
    using Connection;
    using Foundation.Data;
    using Foundation.Diagnostics;
    using Foundation.Diagnostics.MethodProfiler;
    using Query;

    /// <summary>
    /// Summary description for DataSetResultWriter.
    /// </summary>
    internal sealed class DataSetResultWriter : IResultWriter
    {
        #region Private Fields

        private readonly IResultWriter _logResultWriter;
        private QueryForm _queryForm;
        private readonly bool _showShemaTable;
        private IProvider _provider;
        private DataTable _dataTable;
        private int _rowIndex;

        #endregion

        public DataSetResultWriter(
            Action<InfoMessage> addInfoMessage,
            QueryForm queryForm,
            bool showShemaTable)
        {
            _logResultWriter = new LogResultWriter(addInfoMessage);
            _queryForm = queryForm;
            _showShemaTable = showShemaTable;
        }

        #region Public Properties

        public DataSet DataSet { get; private set; }

        #endregion

        #region IResultWriter Members

        void IResultWriter.Begin(IProvider provider)
        {
            _logResultWriter.Begin(provider);
            _provider = provider;
        }

        void IResultWriter.BeforeExecuteReader(AsyncDataAdapterCommand command)
        {
            _logResultWriter.BeforeExecuteReader(command);
        }

        void IResultWriter.AfterExecuteReader(int fieldCount)
        {
            _logResultWriter.AfterExecuteReader(fieldCount);
            DataSet = new DataSet();
        }

        void IResultWriter.AfterCloseReader(int affectedRows)
        {
            _logResultWriter.AfterCloseReader(affectedRows);
        }

        void IResultWriter.WriteTableBegin(DataTable schemaTable)
        {
            _logResultWriter.WriteTableBegin(schemaTable);
            CreateTable(schemaTable);
        }

        void IResultWriter.FirstRowReadBegin()
        {
            _logResultWriter.FirstRowReadBegin();
        }

        void IResultWriter.FirstRowReadEnd(string[] dataTypeNames)
        {
            _logResultWriter.FirstRowReadEnd(dataTypeNames);
        }

        void IResultWriter.WriteRows(object[][] rows, int rowCount)
        {
            MethodProfiler.BeginMethod();
            _logResultWriter.WriteRows(rows, rowCount);

            try
            {
                var targetRows = _dataTable.Rows;

                for (var i = 0; i < rowCount; i++)
                {
                    targetRows.Add(rows[i]);
                }

                _rowIndex += rowCount;
            }
            finally
            {
                MethodProfiler.EndMethod();
            }
        }

        void IResultWriter.WriteTableEnd()
        {
            _logResultWriter.WriteTableEnd();

            GarbageMonitor.Add("DataSetResultWriter", "System.Data.DataTable", _dataTable.Rows.Count, _dataTable);
        }

        void IResultWriter.WriteParameters(IDataParameterCollection parameters)
        {
            // TODO TextResultWriter.WriteParameters(parameters, textWriter, queryForm);
        }

        void IResultWriter.End()
        {
            _logResultWriter.End();

            //int last = this.tableCount - 1;
            //if (last >= 0)
            //{
            //    DataTable table = dataSet.Tables[ last ];
            //    string name = string.Format( "DataTable({0})", table.Rows.Count );
            //    GarbageMonitor.Add( name, dataTable );
            //}
        }

        #endregion

        #region Private Methods

        private void CreateTable(DataTable schemaTable)
        {
            var tableIndex = DataSet.Tables.Count;
            var tableName = schemaTable.TableName;
            if (tableName == "SchemaTable")
            {
                tableName = $"Table {tableIndex}";
            }
            if (_showShemaTable)
            {
                schemaTable.TableName = $"Schema {tableIndex}";
                DataSet.Tables.Add(schemaTable);
            }
            _dataTable = DataSet.Tables.Add();
            if (!string.IsNullOrEmpty(tableName))
            {
                _dataTable.TableName = tableName;
            }
            foreach (DataRow schemaRow in schemaTable.Rows)
            {
                var dataColumnSchema = new DbColumn(schemaRow);
                var columnName = dataColumnSchema.ColumnName;
                var columnSize = dataColumnSchema.ColumnSize;
                var dataType = _provider.GetColumnType(dataColumnSchema);

                DataColumn dataColumn;
                var n = 2;
                var columnName2 = columnName;

                while (true)
                {
                    if (_dataTable.Columns.Contains(columnName2))
                    {
                        columnName2 = columnName + n;
                        n++;
                    }
                    else
                    {
                        columnName = columnName2;

                        if (dataType != null)
                        {
                            dataColumn = _dataTable.Columns.Add(columnName, dataType);
                        }
                        else
                        {
                            dataColumn = _dataTable.Columns.Add(columnName);
                        }

                        dataColumn.ExtendedProperties.Add("ColumnName", dataColumnSchema.ColumnName);

                        //dataColumn.AllowDBNull = sr.AllowDBNull == true;                                
                        //dataColumn.Unique = sr.IsUnique == true; // TFS provider does not support this column
                        dataColumn.ExtendedProperties.Add(0, schemaRow["DataType"]);
                        break;
                    }
                }
            }
        }

        #endregion
    }
}