﻿using System.Collections.Generic;
using System.Linq;
using Foundation.Assertions;

namespace Foundation.Text
{
    public static class EnumerableExtensions
    {
        public static string ToString<TSource>(this IEnumerable<TSource> source, IReadOnlyCollection<StringTableColumnInfo<TSource>> columns)
        {
            Assert.IsNotNull(source);
            Assert.IsNotNull(columns);

            var table = new StringTable(columns.Count);

            #region First row: column names

            var row = table.NewRow();
            var columnIndex = 0;
            foreach (var column in columns)
            {
                row[columnIndex] = column.ColumnName;
                table.Columns[columnIndex].Align = column.Align;
                ++columnIndex;
            }

            table.Rows.Add(row);

            #endregion

            #region Second row: underline first row

            var secondRow = table.NewRow();
            table.Rows.Add(secondRow);

            #endregion

            foreach (var item in source)
            {
                row = table.NewRow();
                columnIndex = 0;
                foreach (var column in columns)
                {
                    row[columnIndex] = column.ToStringFunction(item);
                    ++columnIndex;
                }

                table.Rows.Add(row);
            }

            #region Fill second row

            var columnWidths = new int[columns.Count];
            columnIndex = 0;
            foreach (var column in columns)
            {
                var max = table.Rows.Select(r => r[columnIndex] == null ? 0 : r[columnIndex].Length).Max();
                secondRow[columnIndex] = new string('-', max);
                columnWidths[columnIndex] = max;
                ++columnIndex;
            }

            #endregion

            return table.ToString(columnWidths, " ");
        }
    }
}