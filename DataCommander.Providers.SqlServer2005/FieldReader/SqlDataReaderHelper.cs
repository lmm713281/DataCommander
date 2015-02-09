namespace DataCommander.Providers.SqlServer2005
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using DataCommander.Foundation.Data;

    internal sealed class SqlDataReaderHelper : IDataReaderHelper
    {
        private SqlDataReader sqlDataReader;
        private readonly IDataFieldReader[] dataFieldReaders;

        public SqlDataReaderHelper(IDataReader dataReader)
        {
            this.sqlDataReader = (SqlDataReader)dataReader;
            DataTable schemaTable = dataReader.GetSchemaTable();

            if (schemaTable != null)
            {
                DataRowCollection rows = schemaTable.Rows;
                int count = rows.Count;
                dataFieldReaders = new IDataFieldReader[count];

                for (int i = 0; i < count; i++)
                {
                    dataFieldReaders[i] = CreateDataFieldReader(dataReader, new DataColumnSchema(rows[i]));
                }
            }
        }

        private static IDataFieldReader CreateDataFieldReader(
            IDataRecord dataRecord,
            DataColumnSchema dataColumnSchema)
        {
            int columnOrdinal = dataColumnSchema.ColumnOrdinal;
            SqlDbType providerType = (SqlDbType)dataColumnSchema.ProviderType;
            IDataFieldReader dataFieldReader;

            switch (providerType)
            {
                case SqlDbType.BigInt:
                case SqlDbType.Bit:
                case SqlDbType.Int:
                case SqlDbType.SmallInt:
                case SqlDbType.TinyInt:
                case SqlDbType.UniqueIdentifier:
                case SqlDbType.Real: //
                    dataFieldReader = new DefaultDataFieldReader(dataRecord, columnOrdinal);
                    break;

                case SqlDbType.Float: //
                    dataFieldReader = new DoubleFieldReader(dataRecord, columnOrdinal);
                    break;

                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.NText:
                    int columnSize = dataColumnSchema.ColumnSize;

                    if (columnSize <= SqlServerProvider.ShortStringSize)
                    {
                        dataFieldReader = new ShortStringFieldReader(dataRecord, columnOrdinal, providerType);
                    }
                    else
                    {
                        dataFieldReader = new LongStringFieldReader(dataRecord, columnOrdinal);
                    }

                    break;

                case SqlDbType.Binary:
                case SqlDbType.VarBinary:
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                    dataFieldReader = new BinaryDataFieldReader(dataRecord, columnOrdinal);
                    break;

                case SqlDbType.Decimal:
                    dataFieldReader = new DefaultDataFieldReader(dataRecord, columnOrdinal);
                    break;

                case SqlDbType.SmallDateTime:
                    dataFieldReader = new SmallDateTimeDataFieldReader(dataRecord, columnOrdinal);
                    break;

                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                    dataFieldReader = new DateTimeDataFieldReader(dataRecord, columnOrdinal);
                    break;

                case SqlDbType.DateTimeOffset:
                    dataFieldReader = new DateTimeOffsetDataFieldReader(dataRecord, columnOrdinal);
                    break;


                case SqlDbType.Time:
                    dataFieldReader = new DefaultDataFieldReader(dataRecord, columnOrdinal);
                    break;

                case SqlDbType.Money:
                    dataFieldReader = new MoneyDataFieldReader(dataRecord, columnOrdinal);
                    break;

                case SqlDbType.Variant:
                    dataFieldReader = new VariantDataFieldReader(dataRecord, columnOrdinal);
                    break;

                //                case SqlDbType.Timestamp:
                //                    dataFieldReader = new TimeStampDataFieldReader(dataRecord,columnOrdinal);
                //                    break;

                case SqlDbType.Xml:
                    dataFieldReader = new LongStringFieldReader(dataRecord, columnOrdinal);
                    break;

                default:
                    throw new Exception();
            }

            return dataFieldReader;
        }

        int IDataReaderHelper.GetValues(object[] values)
        {
            for (int i = 0; i < this.dataFieldReaders.Length; i++)
            {
                values[i] = dataFieldReaders[i].Value;
            }

            return this.dataFieldReaders.Length;
        }
    }
}