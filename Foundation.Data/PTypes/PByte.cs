﻿using System;
using System.Data.SqlTypes;

namespace Foundation.Data.PTypes
{
    /// <summary>
    /// 
    /// </summary>
    public struct PByte : INullable
    {
        private SqlByte _sql;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public PByte(byte value)
        {
            _sql = value;
            ValueType = PValueType.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public PByte(SqlByte value)
        {
            _sql = value;
            ValueType = value.IsNull ? PValueType.Null : PValueType.Value;
        }

        private PByte(PValueType type)
        {
            ValueType = type;
            _sql = SqlByte.Null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator PByte(byte value)
        {
            return new PByte(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator PByte(byte? value)
        {
            return value != null ? new PByte(value.Value) : Null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator PByte(SqlByte value)
        {
            return new PByte(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator byte(PByte value)
        {
            return (byte) value._sql;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(PByte x, PByte y)
        {
            var isEqual = x.ValueType == y.ValueType;

            if (isEqual)
            {
                if (x.ValueType == PValueType.Value)
                {
                    isEqual = x._sql.Value == y._sql.Value;
                }
            }

            return isEqual;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(PByte x, PByte y)
        {
            return !(x == y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PByte Parse(string s, PValueType type)
        {
            var sp = string.IsNullOrEmpty(s) ? new PByte(type) : SqlByte.Parse(s);
            return sp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool Equals(object y)
        {
            var equals = y is PByte;

            if (equals)
                equals = this == (PByte) y;

            return equals;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = _sql.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        public PValueType ValueType
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNull => ValueType == PValueType.Null;

        /// <summary>
        /// 
        /// </summary>
        public bool IsValue => ValueType == PValueType.Value;

        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty => ValueType == PValueType.Empty;

        /// <summary>
        /// 
        /// </summary>
        public object Value
        {
            get
            {
                object value;

                switch (ValueType)
                {
                    case PValueType.Value:
                    case PValueType.Null:
                        value = _sql;
                        break;

                    default:
                        value = null;
                        break;
                }

                return value;
            }

            set
            {
                if (value == null)
                {
                    ValueType = PValueType.Default;
                    _sql = SqlByte.Null;
                }
                else if (value == DBNull.Value)
                {
                    ValueType = PValueType.Null;
                    _sql = SqlByte.Null;
                }
                else
                {
                    _sql = (SqlByte) value;
                    ValueType = _sql.IsNull ? PValueType.Null : PValueType.Value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _sql.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly PByte Null = new PByte(PValueType.Null);

        /// <summary>
        /// 
        /// </summary>
        public static readonly PByte Default = new PByte(PValueType.Default);

        /// <summary>
        /// 
        /// </summary>
        public static readonly PByte Empty = new PByte(PValueType.Empty);
    }
}