﻿using System;
using System.Collections.Generic;
using System.Data;
using Foundation.Diagnostics;
using Foundation.Diagnostics.Assertions;
using Foundation.Diagnostics.Contracts;

namespace Foundation.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DataReader : IDisposable
    {
        #region Private Fields

        private readonly IDbCommand _command;
        private readonly IDataReader _dataReader;
        private bool _nextResultCalled = true;

        #endregion

        private DataReader(IDbCommand command, IDataReader dataReader)
        {
            Assert.IsNotNull(command);
            Assert.IsNotNull(dataReader);

            _command = command;
            _dataReader = dataReader;
        }

        internal static DataReader Create(IDbTransactionScope transactionScope, CommandDefinition commandDefinition, CommandBehavior commandBehavior)
        {
            Assert.IsNotNull(transactionScope);
            Assert.IsNotNull(commandDefinition);

            IDbCommand command = null;
            IDataReader dataReader = null;

            try
            {
                command = transactionScope.CreateCommand(commandDefinition);
                dataReader = command.ExecuteReader(commandBehavior);
                return new DataReader(command, dataReader);
            }
            catch
            {
                if (dataReader != null)
                    dataReader.Dispose();

                if (command != null)
                    command.Dispose();

                throw;
            }
        }

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="read"></param>
        /// <returns></returns>
        public IEnumerable<T> Read<T>(Func<IDataRecord, T> read)
        {
            Assert.IsNotNull(read);

            PrivateNextResult();

            while (_dataReader.Read())
                yield return read(_dataReader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="read"></param>
        public void Read(Action<IDataRecord> read)
        {
            Assert.IsNotNull(read);

            PrivateNextResult();

            while (_dataReader.Read())
                read(_dataReader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="read"></param>
        public void Read(Func<IDataRecord, bool> read)
        {
            Assert.IsNotNull(read);

            PrivateNextResult();

            while (_dataReader.Read())
            {
                var succeeded = read(_dataReader);
                if (!succeeded)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool NextResult()
        {
            FoundationContract.Assert(_dataReader != null);
            FoundationContract.Assert(!_nextResultCalled);

            var nextResult = _dataReader.NextResult();
            _nextResultCalled = true;

            return nextResult;
        }

        #endregion

        void IDisposable.Dispose()
        {
            _dataReader.Dispose();
            _command.Dispose();
        }

        private void PrivateNextResult()
        {
            if (_nextResultCalled)
                _nextResultCalled = false;
            else
            {
                var nextResult = _dataReader.NextResult();
                _nextResultCalled = true;

                if (!nextResult)
                    throw new InvalidOperationException();
            }
        }
    }
}