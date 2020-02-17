namespace Framework.Core.DataAccess
{
    using System.Data;
    using Framework.Core.Constant;
    
    public interface IDatabaseExecutor
    {
        string ConnectionString { get; set; }

        DatabaseType DatabaseType { get; set; }

        DataTable ExecuteQuery(string query);
    }
}
