﻿using System;
using System.Collections.Generic;
using System.Xml;
using Foundation.Xml;

namespace Foundation.XmlSpreadsheet
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class XmlSpreadsheetWriter
    {
        private XmlSpreadsheetTable _table;
        private int _tableIndex = -1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlWriter"></param>
        public XmlSpreadsheetWriter(XmlWriter xmlWriter)
        {
#if CONTRACTS_FULL
            Contract.Requires(xmlWriter != null);
#endif

            this.XmlWriter = xmlWriter;

            this.XmlWriter.WriteStartDocument();
            this.XmlWriter.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");
            this.XmlWriter.WriteStartElement("Workbook");
            this.XmlWriter.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:spreadsheet");
            this.XmlWriter.WriteAttributeString("xmlns:ss", "urn:schemas-microsoft-com:office:spreadsheet");
        }

        /// <summary>
        /// 
        /// </summary>
        public XmlWriter XmlWriter { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tables"></param>
        public void WriteStyles(IEnumerable<XmlSpreadsheetTable> tables)
        {
            using (this.XmlWriter.WriteElement("Styles"))
            {
                using (this.XmlWriter.WriteElement("Style"))
                {
                    this.XmlWriter.WriteAttributeString("ss:ID", "ColumnHeader");

                    using (this.XmlWriter.WriteElement("Alignment"))
                    {
                        this.XmlWriter.WriteAttributeString("ss:Vertical", "Top");
                        this.XmlWriter.WriteAttributeString("ss:WrapText", "1");
                    }

                    using (this.XmlWriter.WriteElement("Borders"))
                    {
                        using (this.XmlWriter.WriteElement("Border"))
                        {
                            this.XmlWriter.WriteAttributeString("ss:Position", "Bottom");
                            this.XmlWriter.WriteAttributeString("ss:LineStyle", "Continuous");
                            this.XmlWriter.WriteAttributeString("ss:Weight", "1");
                        }
                    }

                    using (this.XmlWriter.WriteElement("Font"))
                    {
                        this.XmlWriter.WriteAttributeString("ss:Bold", "1");
                    }
                }

                var tableIndex = 0;
                foreach (var tableSchema in tables)
                {
                    var columnIndex = 0;
                    foreach (var column in tableSchema.Columns)
                    {
                        using (this.XmlWriter.WriteElement("Style"))
                        {
                            var id = $"{tableIndex},{columnIndex}";
                            this.XmlWriter.WriteAttributeString("ss:ID", id);

                            if (column.NumberFormat != null)
                            {
                                using (this.XmlWriter.WriteElement("NumberFormat"))
                                {
                                    this.XmlWriter.WriteAttributeString("ss:Format", column.NumberFormat);
                                }
                            }
                        }
                        columnIndex++;
                    }

                    tableIndex++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        public void WriteStartTable(XmlSpreadsheetTable table)
        {
#if CONTRACTS_FULL
            Contract.Requires(table != null);
#endif
            this._tableIndex++;
            this._table = table;
            int columnIndex;

            this.XmlWriter.WriteStartElement("Worksheet");
            this.XmlWriter.WriteAttributeString("ss:Name", this._table.TableName);

            this.XmlWriter.WriteStartElement("Table");

            columnIndex = 1;
            foreach (var column in this._table.Columns)
            {
                using (this.XmlWriter.WriteElement("Column"))
                {
                    this.XmlWriter.WriteAttributeString("ss:Index", columnIndex.ToString());

                    if (column.Width != null)
                    {
                        this.XmlWriter.WriteAttributeString("ss:Width", column.Width);
                    }
                }
                columnIndex++;
            }

            using (this.XmlWriter.WriteElement("Row"))
            {
                foreach (var column in this._table.Columns)
                {
                    var cell =
                        new XmlSpreadsheetCell(XmlSpreadsheetDataType.String, column.ColumnName)
                        {
                            StyleId = "ColumnHeader"
                        };
                    cell.Write(this.XmlWriter);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void WriteEndTable()
        {
            // Table
            this.XmlWriter.WriteEndElement();

            using (this.XmlWriter.WriteElement("WorksheetOptions"))
            {
                this.XmlWriter.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:excel");

                this.XmlWriter.WriteElementString("Selected", null);
                this.XmlWriter.WriteElementString("FreezePanes", null);
                this.XmlWriter.WriteElementString("FrozenNoSplit", null);
                this.XmlWriter.WriteElementString("SplitHorizontal", "1");
                this.XmlWriter.WriteElementString("TopRowBottomPane", "1");
                this.XmlWriter.WriteElementString("ActivePane", "2");
            }

            // Worksheet
            this.XmlWriter.WriteEndElement();
        }

        /// <summary>
        /// 
        /// </summary>
        public void WriteStartRow()
        {
            this.XmlWriter.WriteStartElement("Row");
        }

        /// <summary>
        /// 
        /// </summary>
        public void WriteEndRow()
        {
            this.XmlWriter.WriteEndElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        public void WriteRow(object[] values)
        {
#if CONTRACTS_FULL
            Contract.Requires(values != null);
#endif

            this.WriteStartRow();

            for (var columnIndex = 0; columnIndex < values.Length; columnIndex++)
            {
                var value = values[columnIndex];
                XmlSpreadsheetDataType type;
                string xmlValue;

                if (value == DBNull.Value)
                {
                    xmlValue = null;
                    type = XmlSpreadsheetDataType.String;
                }
                else
                {
                    var column = this._table.Columns[columnIndex];
                    type = column.DataType;
                    xmlValue = column.Convert(value);
                }

                var cell = new XmlSpreadsheetCell(type, xmlValue);
                cell.StyleId = $"{this._tableIndex},{columnIndex}";
                cell.Write(this.XmlWriter);
            }

            this.WriteEndRow();
        }
    }
}