﻿namespace DataCommander.Foundation.Data.TextData
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public sealed class TextDataRow
    {
        private readonly Convert _convert;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="convert"></param>
        public TextDataRow(TextDataColumnCollection columns, Convert convert)
        {
#if CONTRACTS_FULL
            Contract.Requires(columns != null);
            Contract.Requires(convert != null);
#endif

            this.Columns = columns;
            this._convert = convert;
            this.ItemArray = new object[columns.Count];

            for (var i = 0; i < this.ItemArray.Length; i++)
            {
                this.ItemArray[i] = DBNull.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public delegate object Convert(object value, TextDataColumn column);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public object this[string columnName]
        {
            get
            {
                var index = this.Columns.IndexOf(columnName, true);
                return this.ItemArray[index];
            }

            set
            {
                var index = this.Columns.IndexOf(columnName, true);
                var column = this.Columns[index];
                var convertedValue = this._convert(value, column);
                this.ItemArray[index] = convertedValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object[] ItemArray { get; }

        /// <summary>
        /// 
        /// </summary>
        public TextDataColumnCollection Columns { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public object this[TextDataColumn column]
        {
            get
            {
                var index = this.Columns.IndexOf(column, true);
                return this.ItemArray[index];
            }

            set
            {
                var index = this.Columns.IndexOf(column, true);
                var convertedValue = this._convert(value, column);
                this.ItemArray[index] = convertedValue;
            }
        }
    }
}