namespace DataCommander.Foundation.Data.SqlClient
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    internal sealed class SqlConnectionFactory : IDbConnectionHelper
    {
        private readonly IDbConnection connection;

        public event EventHandler InfoMessage;

        public SqlConnectionFactory(
            SqlConnection sqlConnection,
            IDbConnection connection)
        {
            sqlConnection.InfoMessage += this.InfoMessageEvent;
            this.connection = connection;
        }

        private void InfoMessageEvent(object sender, SqlInfoMessageEventArgs e)
        {
            this.InfoMessage?.Invoke(this.connection, e);
        }
    }
}