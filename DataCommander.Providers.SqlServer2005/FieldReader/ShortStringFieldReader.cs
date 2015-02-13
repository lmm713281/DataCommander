namespace DataCommander.Providers.SqlServer2005
{
    using System;
    using System.Data;

    sealed class ShortStringFieldReader : IDataFieldReader
    {
        private IDataRecord dataRecord;
        private int columnOrdinal;
        private SqlDbType sqlDbType;

        public ShortStringFieldReader(
            IDataRecord dataRecord,
            int columnOrdinal,
            SqlDbType sqlDbType)
        {
            this.dataRecord = dataRecord;
            this.columnOrdinal = columnOrdinal;
            this.sqlDbType = sqlDbType;
        }

        #region IDataFieldReader Members
        object IDataFieldReader.Value
        {
            get
            {
                object value;

                if (this.dataRecord.IsDBNull(this.columnOrdinal))
                {
                    value = DBNull.Value;
                }
                else
                {
                    string s = this.dataRecord.GetString(this.columnOrdinal);

                    if (this.sqlDbType == SqlDbType.Char ||
                        this.sqlDbType == SqlDbType.NChar)
                    {
                        s = s.TrimEnd();
                    }

                    value = s;
                }

                return value;
            }
        }
        #endregion
    }
}