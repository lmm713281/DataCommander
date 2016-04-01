namespace DataCommander.Foundation.Data.PTypes
{
    using System;
    using System.Data.SqlTypes;

    /// <summary>
    /// 
    /// </summary>
    public struct PDouble : INullable
    {
        private SqlDouble sql;

        /// <summary>
        /// 
        /// </summary>
        public static readonly PDouble Null = new PDouble( PValueType.Null );

        /// <summary>
        /// 
        /// </summary>
        public static readonly PDouble Default = new PDouble( PValueType.Default );

        /// <summary>
        /// 
        /// </summary>
        public static readonly PDouble Empty = new PDouble( PValueType.Empty );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public PDouble( Double value )
        {
            this.sql = value;
            this.ValueType = this.sql.IsNull ? PValueType.Null : PValueType.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public PDouble( SqlDouble value )
        {
            this.sql = value;
            this.ValueType = value.IsNull ? PValueType.Null : PValueType.Value;
        }

        private PDouble( PValueType type )
        {
            this.ValueType = type;
            this.sql = SqlDouble.Null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator PDouble( Double value )
        {
            return new PDouble( value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator PDouble( SqlDouble value )
        {
            return new PDouble( value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Double( PDouble value )
        {
            return (Double) value.sql;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==( PDouble x, PDouble y )
        {
            bool isEqual = x.ValueType == y.ValueType;

            if (isEqual)
            {
                if (x.ValueType == PValueType.Value)
                {
                    isEqual = x.sql.Value == y.sql.Value;
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
        public static bool operator !=( PDouble x, PDouble y )
        {
            return !(x == y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PDouble Parse( string s, PValueType type )
        {
            PDouble sp;

            if (s == null || s.Length == 0)
            {
                sp = new PDouble( type );
            }
            else
            {
                sp = SqlDouble.Parse( s );
            }

            return sp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool Equals( object y )
        {
            bool equals = y is PDouble;

            if (equals)
            {
                equals = this == (PDouble) y;
            }

            return equals;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = this.sql.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        public PValueType ValueType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNull => this.ValueType == PValueType.Null;

        /// <summary>
        /// 
        /// </summary>
        public bool IsValue => this.ValueType == PValueType.Value;

        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty => this.ValueType == PValueType.Empty;

        /// <summary>
        /// 
        /// </summary>
        public object Value
        {
            get
            {
                object value;

                switch (this.ValueType)
                {
                    case PValueType.Value:
                    case PValueType.Null:
                        value = this.sql;
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
                    this.ValueType = PValueType.Default;
                    this.sql = SqlDouble.Null;
                }
                else if (value == DBNull.Value)
                {
                    this.ValueType = PValueType.Null;
                    this.sql = SqlDouble.Null;
                }
                else
                {
                    this.sql = (SqlDouble) value;
                    this.ValueType = this.sql.IsNull ? PValueType.Null : PValueType.Value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.sql.ToString();
        }
    }
}