namespace Framework.Core.DataAccess
{
    using System.Data;

    using Framework.Core.Constant;

    public abstract class BaseDatabaseExecutor : IDatabaseExecutor
    {
        public string ConnectionString { get; set; }

        public DatabaseType DatabaseType { get; set; }

        public abstract DataTable ExecuteQuery(string query);
    }
}
