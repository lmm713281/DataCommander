namespace DataCommander.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof (IAsyncDataAdapter))]
    internal abstract class IAsyncDataAdapterContract : IAsyncDataAdapter
    {
        IResultWriter IAsyncDataAdapter.ResultWriter
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        long IAsyncDataAdapter.RowCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        int IAsyncDataAdapter.TableCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        void IAsyncDataAdapter.BeginFill(IProvider provider, IEnumerable<AsyncDataAdapterCommand> commands, int maxRecords, int rowBlockSize, IResultWriter resultWriter,
            Action<IAsyncDataAdapter, Exception> endFill, Action<IAsyncDataAdapter> writeEnd)
        {
            Contract.Requires<ArgumentNullException>(provider != null);
            Contract.Requires<ArgumentOutOfRangeException>(maxRecords >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(rowBlockSize >= 0);
            Contract.Requires<ArgumentNullException>(resultWriter != null);
            Contract.Requires<ArgumentNullException>(endFill != null);
            Contract.Requires<ArgumentNullException>(writeEnd != null);
        }

        void IAsyncDataAdapter.Cancel()
        {
            throw new NotImplementedException();
        }
    }
}