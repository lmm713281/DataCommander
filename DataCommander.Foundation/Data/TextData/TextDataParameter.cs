﻿namespace DataCommander.Foundation.Data
{
    using System;
    using System.Data;
    using System.Data.Common;

    /// <summary>
    /// 
    /// </summary>
    public sealed class TextDataParameter : DbParameter
    {
        private string name;
        private object value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public TextDataParameter(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public override DbType DbType
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override ParameterDirection Direction
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsNullable
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ParameterName
        {
            get => this.name;

            set => this.name = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ResetDbType()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Size
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override string SourceColumn
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool SourceColumnNullMapping
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataRowVersion SourceVersion
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override object Value
        {
            get => this.value;

            set => this.value = value;
        }
    }
}