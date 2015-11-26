namespace DataCommander.Providers.Odp
{
    using System;
    using Oracle.ManagedDataAccess.Types;

    internal sealed class OracleTimeStampTZField
    {
        private OracleTimeStampTZ value;

        public OracleTimeStampTZField( OracleTimeStampTZ value )
        {
            this.value = value;
        }

        public override string ToString()
        {
            DateTime dateTime = this.value.Value;
            return dateTime.ToString( "yyyy-MM-dd HH:mm:ss.ffffff" );
        }
    }
}