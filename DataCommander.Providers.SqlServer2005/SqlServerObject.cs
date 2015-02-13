namespace DataCommander.Providers.SqlServer2005
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using DataCommander.Foundation.Data.SqlClient;

    internal static class SqlServerObject
    {
        public static string GetDatabases()
        {
            return "select name from sys.databases (nolock) order by name";
        }

        public static string GetSchemas()
        {
            return "select name from sys.schemas (nolock) order by name";
        }

        public static string GetSchemas(string database)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(database));

            return string.Format(@"if exists(select * from sys.databases (nolock) where name = '{0}')
begin
    select null,name
    from [{0}].sys.schemas (nolock)
    order by name
end", database);
        }

        public static string GetObjects(string schema, IEnumerable<string> objectTypes)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(schema));
            Contract.Requires(objectTypes != null && objectTypes.Any());

            return string.Format(@"declare @schema_id int

select @schema_id = schema_id
from sys.schemas (nolock)
where name = '{0}'

if @schema_id is not null
begin
    select o.name
    from sys.all_objects o (nolock)
    where
        o.schema_id = @schema_id
        and o.type in({1})
    order by o.name
end", schema, string.Join(",", objectTypes.Select(o=>o.ToTSqlVarChar())));
        }

        public static string GetObjects(
            string database,
            string schema,
            IEnumerable<string> objectTypes)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(database));
            Contract.Requires(!string.IsNullOrWhiteSpace(schema));
            Contract.Requires(objectTypes!=null && objectTypes.Any());

            return string.Format(@"if exists(select * from sys.databases (nolock) where name = '{0}')
begin
    declare @schema_id int

    select @schema_id = schema_id
    from [{0}].sys.schemas (nolock)
    where name = '{1}'

    if @schema_id is not null
    begin
        select o.name
        from [{0}].sys.all_objects o (nolock)
        where
            o.schema_id = @schema_id
            and o.type in({2})
        order by o.name
    end
end", database, schema, string.Join(",", objectTypes.Select(t => t.ToTSqlVarChar())));
        }
    }
}