using System;
using System.IO;
using System.Reflection;
using ADODB;
using Foundation.Text;

namespace DataCommander.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public static class AdoDB
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static RecordsetClass XmlToRecordset(string xml)
        {
            var stream = new StreamClass();

            stream.Open(
                Missing.Value,
                ConnectModeEnum.adModeUnknown,
                StreamOpenOptionsEnum.adOpenStreamUnspecified,
                null, null);

            stream.WriteText(xml, 0);
            stream.Position = 0;
            var rs = new RecordsetClass();

            rs.Open(
                stream, Missing.Value,
                CursorTypeEnum.adOpenUnspecified,
                LockTypeEnum.adLockUnspecified, 0);

            return rs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="writer"></param>
        [CLSCompliant(false)]
        public static void WriteSchema(_Recordset rs, TextWriter writer)
        {
            var index = 1;
            var d = (double) rs.Fields.Count;
            d = Math.Log10(d);
            var colWidth = (int) d;
            colWidth++;

            foreach (ADODB.Field field in rs.Fields)
            {
                var fieldType = field.Type;

                var line =
                    StringHelper.FormatColumn(index.ToString(), colWidth, false) + ". " +
                    StringHelper.FormatColumn(field.Name, 30, false);

                var fileTypeStr = Enum.Format(fieldType.GetType(), field.Type, "g");

                switch (fieldType)
                {
                    case DataTypeEnum.adNumeric:
                        fileTypeStr += "(" + field.Precision + "," + field.NumericScale + ")";
                        break;

                    case DataTypeEnum.adVarChar:
                        fileTypeStr += "(" + field.DefinedSize + ")";
                        break;

                    default:
                        break;
                }

                if ((field.Attributes & (int) FieldAttributeEnum.adFldMayBeNull) != 0)
                {
                    fileTypeStr += " NULL";
                }
                else
                {
                    fileTypeStr += " NOT NULL";
                }

                line += StringHelper.FormatColumn(fileTypeStr, 30, false);

                writer.WriteLine(line);
                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ADODBRecordset"></param>
        /// <param name="writer"></param>
        public static void WriteSchema(object ADODBRecordset, TextWriter writer)
        {
            WriteSchema((Recordset) ADODBRecordset, writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="maxRowCount"></param>
        /// <param name="writer"></param>
        [CLSCompliant(false)]
        public static void WriteRows(
            _Recordset rs,
            int maxRowCount,
            TextWriter writer)
        {
            var recordCount = rs.RecordCount;
            writer.WriteLine("RecordCount: " + recordCount);

            if (!rs.EOF)
            {
                var rsStr = rs.GetString(StringFormatEnum.adClipString, maxRowCount, "\t", "\r\n", "<NULL>");
                writer.WriteLine(rsStr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ADODBRecordset"></param>
        /// <param name="writer"></param>
        public static void WriteRows(object ADODBRecordset, TextWriter writer)
        {
            WriteRows((Recordset) ADODBRecordset, int.MaxValue, writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ADODBRecordset"></param>
        /// <param name="maxRowCount"></param>
        /// <param name="writer"></param>
        public static void Write(
            object ADODBRecordset,
            int maxRowCount,
            TextWriter writer)
        {
            if (ADODBRecordset != null)
            {
                var rs = (Recordset) ADODBRecordset;
                WriteSchema(rs, writer);
                WriteRows(rs, maxRowCount, writer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ADODBRecordset"></param>
        /// <param name="writer"></param>
        public static void Write(
            object ADODBRecordset,
            TextWriter writer)
        {
            if (ADODBRecordset != null)
            {
                var rs = (Recordset) ADODBRecordset;
                WriteSchema(rs, writer);
                WriteRows(rs, int.MaxValue, writer);
            }
        }
    }
}