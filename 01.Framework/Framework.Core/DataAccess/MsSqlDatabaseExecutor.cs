namespace Framework.Core.DataAccess
{
    using System.Data;
    using System.Data.SqlClient;

    using Framework.Core.Constant;

    public sealed class MsSqlDatabaseExecutor : BaseDatabaseExecutor
    {
        public MsSqlDatabaseExecutor()
        {
            this.DatabaseType = DatabaseType.MsSql;
            this.ConnectionString = "Your connection string to the SQL Databse";
        }

        public override DataTable ExecuteQuery(string query)
        {
            DataTable table;

            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        // Command timeout - 3 minutes
                        adapter.SelectCommand.CommandTimeout = 180;
                        table = new DataTable();
                        adapter.Fill(table);
                    }
                }
            }

            return table;
        }
    }
}
