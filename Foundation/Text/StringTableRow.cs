﻿using System;
using Foundation.Assertions;
using Foundation.Diagnostics.Contracts;

namespace Foundation.Text
{
    /// <summary>
    /// Represents a row in the <see cref="StringTable"/>.
    /// </summary>
    public sealed class StringTableRow
    {
        private readonly string[] _cells;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        internal StringTableRow(StringTable table)
        {
            Assert.IsNotNull(table);

            Table = table;
            _cells = new string[table.Columns.Count];
        }

        /// <summary>
        /// 
        /// </summary>
        public StringTable Table { get; }

        /// <summary>
        /// 
        /// </summary>
        public string this[int columnIndex]
        {
            get
            {
                FoundationContract.Requires<ArgumentException>(0 <= columnIndex && columnIndex < Table.Columns.Count);
                return _cells[columnIndex];
            }

            set
            {
                FoundationContract.Requires<ArgumentException>(0 <= columnIndex && columnIndex < Table.Columns.Count);
                _cells[columnIndex] = value;
            }
        }
    }
}