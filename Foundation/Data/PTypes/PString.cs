﻿using System;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace Foundation.Data.PTypes
{
    /// <summary>
    /// 
    /// </summary>
    public struct PString : INullable
    {
        private SqlString _sql;

        /// <summary>
        /// 
        /// </summary>
        public static readonly PString Null = new PString( PValueType.Null );

        /// <summary>
        /// 
        /// </summary>
        public static readonly PString Default = new PString( PValueType.Default );

        /// <summary>
        /// 
        /// </summary>
        public static readonly PString Empty = new PString( PValueType.Empty );

        private PString( PValueType type )
        {
            ValueType = type;
            _sql = SqlString.Null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public PString( char value )
        {
            _sql = value.ToString();
            ValueType = PValueType.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [DebuggerStepThrough]
        public PString( string value )
        {
            _sql = value;
            ValueType = PValueType.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public PString( SqlString value )
        {
            _sql = value;
            ValueType = value.IsNull ? PValueType.Null : PValueType.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator PString( char value )
        {
            return new PString( value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static implicit operator PString( string value )
        {
            return new PString( value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator PString( SqlString value )
        {
            return new PString( value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator string( PString value )
        {
            return (string) value._sql;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==( PString x, PString y )
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
        public static bool operator !=( PString x, PString y )
        {
            return !(x == y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PString Parse( string s, PValueType type )
        {
            PString sp;

            if (string.IsNullOrEmpty(s))
            {
                sp = new PString( type );
            }
            else
            {
                sp = new PString( s );
            }

            return sp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals( object obj )
        {
            var equals = obj is PString;

            if (equals)
            {
                equals = this == (PString) obj;
            }

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
        public PValueType ValueType { get; private set; }

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
                    _sql = SqlString.Null;
                }
                else if (value == DBNull.Value)
                {
                    ValueType = PValueType.Null;
                    _sql = SqlString.Null;
                }
                else
                {
                    _sql = (SqlString) value;
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
    }
}