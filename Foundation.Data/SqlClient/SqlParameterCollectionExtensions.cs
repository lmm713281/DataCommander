﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text;
using Foundation.Assertions;
using Foundation.Linq;
using Foundation.Text;

namespace Foundation.Data.SqlClient
{
    public static class SqlParameterCollectionExtensions
    {
        public static void Add(this ICollection<SqlParameter> parameters, string parameterName, object value)
        {
            var parameter = new SqlParameter
            {
                ParameterName = parameterName,
                Value = value
            };
            parameters.Add(parameter);
        }

        public static void Add(this ICollection<SqlParameter> parameters, string parameterName, SqlDbType sqlDbType, object value)
        {
            var parameter = new SqlParameter();
            parameter.ParameterName = parameterName;
            parameter.SqlDbType = sqlDbType;
            parameter.Value = value;

            parameters.Add(parameter);
        }

        public static string ToLogString(this SqlParameterCollection parameters)
        {
            Assert.IsNotNull(parameters);

            var sb = new StringBuilder();
            var first = true;
            string s;
            var numberFormatInfo = NumberFormatInfo.InvariantInfo;

            foreach (SqlParameter parameter in parameters)
            {
                var value = parameter.Value;

                if (value != null)
                {
                    switch (parameter.Direction)
                    {
                        case ParameterDirection.ReturnValue:
                            break;

                        default:
                            if (first)
                                first = false;
                            else
                                sb.AppendLine(",");

                            if (value == DBNull.Value)
                                s = SqlNull.NullString;
                            else
                            {
                                var type = value.GetType();
                                var nullable = value as INullable;

                                if (nullable != null)
                                {
                                    if (nullable.IsNull)
                                        s = SqlNull.NullString;
                                    else
                                    {
                                        switch (parameter.SqlDbType)
                                        {
                                            case SqlDbType.Bit:
                                                var sqlBoolean = (SqlBoolean) value;
                                                s = sqlBoolean.ByteValue.ToString();
                                                break;

                                            case SqlDbType.Char:
                                            case SqlDbType.VarChar:
                                            case SqlDbType.Text:
                                            case SqlDbType.NChar:
                                            case SqlDbType.NVarChar:
                                            case SqlDbType.NText:
                                                s = value.ToString();
                                                s = s.Replace("\'", "''");
                                                s = $"'{s}'";
                                                break;

                                            case SqlDbType.DateTime:
                                                var sqlDateTime = (SqlDateTime) value;
                                                var dateTime = sqlDateTime.Value;
                                                s = dateTime.ToTSqlDateTime();
                                                break;

                                            case SqlDbType.Float:
                                                var sqlDouble = (SqlDouble) value;
                                                var d = sqlDouble.Value;
                                                var i = (long) d;

                                                if (i == d)
                                                    s = i.ToString(numberFormatInfo);
                                                else
                                                    s = d.ToString(numberFormatInfo);

                                                break;

                                            case SqlDbType.Real:
                                                var sqlSingle = (SqlSingle) value;
                                                s = sqlSingle.ToString();
                                                break;

                                            case SqlDbType.Decimal:
                                                var sqlDecimal = (SqlDecimal) value;
                                                s = sqlDecimal.ToString();
                                                break;

                                            case SqlDbType.Money:
                                                var sqlMoney = (SqlMoney) value;
                                                var dec = sqlMoney.Value;
                                                i = (long) dec;
                                                s = i == dec ? i.ToString(numberFormatInfo) : dec.ToString(numberFormatInfo);
                                                break;

                                            case SqlDbType.SmallDateTime:
                                                sqlDateTime = (SqlDateTime) value;
                                                dateTime = sqlDateTime.Value;
                                                s = dateTime.ToTSqlDateTime();
                                                break;

                                            default:
                                                s = value.ToString();
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (type.IsArray)
                                    {
                                        var elementType = type.GetElementType();
                                        var elementTypeCode = Type.GetTypeCode(elementType);

                                        switch (elementTypeCode)
                                        {
                                            case TypeCode.Byte:
                                                var bytes = (byte[]) value;
                                                s = "0x" + Hex.GetString(bytes, true);
                                                break;

                                            default:
                                                s = value.ToString();
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        var typeCode = Type.GetTypeCode(type);

                                        switch (typeCode)
                                        {
                                            case TypeCode.Boolean:
                                                var b = (bool) value;
                                                s = b ? "1" : "0";
                                                break;

                                            case TypeCode.String:
                                                s = (string) value;
                                                s = s.Replace("\'", "''");
                                                s = $"'{s}'";
                                                break;

                                            case TypeCode.DateTime:
                                                var dateTime = (DateTime) value;
                                                s = dateTime.ToTSqlDateTime();
                                                break;

                                            case TypeCode.Decimal:
                                                var decimalValue = (decimal) value;
                                                s = decimalValue.ToString(numberFormatInfo);
                                                break;

                                            default:
                                                s = value.ToString();
                                                break;
                                        }
                                    }
                                }
                            }

                            sb.AppendFormat("{0} = {1}", parameter.ParameterName, s);
                            break;
                    }
                }
            }

            s = sb.ToString();

            if (s.Length == 0)
                s = null;

            return s;
        }

        public static List<object> ToObjectList(this ICollection<SqlParameter> parameters)
        {
            var result = new List<object>(parameters.Count);
            result.AddRange(parameters);
            return result;
        }

        public static ReadOnlyCollection<object> ToObjectReadOnlyCollection(this ICollection<SqlParameter> parameters) => parameters.Cast<object>().ToReadOnlyCollection();
        public static ReadOnlyCollection<object> ToObjectReadOnlyCollection(this IEnumerable<SqlParameter> parameters) => parameters.Cast<object>().ToReadOnlyCollection();
    }
}