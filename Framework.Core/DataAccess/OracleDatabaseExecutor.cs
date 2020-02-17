namespace Framework.Core.DataAccess
{
    using System.Data;

    using Oracle.ManagedDataAccess.Client;

    using Framework.Core.Constant;

    public sealed class OracleDatabaseExecutor : BaseDatabaseExecutor
    {
        public OracleDatabaseExecutor()
        {
            this.DatabaseType = DatabaseType.Oracle;
            this.ConnectionString = "Your connection string to the Oracle Databse";
        }

        public override DataTable ExecuteQuery(string query)
        {
            DataTable table;

            using (var connection = new OracleConnection(ConnectionString))
            {
                using (var command = new OracleCommand(query, connection))
                {
                    connection.Open();
                    using (var adapter = new OracleDataAdapter(command))
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
