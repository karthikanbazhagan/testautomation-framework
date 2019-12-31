namespace Framework.Core.Utilities
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    
    public static class SqlUtility
    {
        public static DataTable ExecuteSelectQuery(string connectionString, string queryString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable result = new DataTable();
                    result.Load(reader);
                    return result;
                }
                catch(Exception e)
                {
                    throw new InvalidOperationException(e.Message);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }
        
        public static void ExecuteUpdateQuery(string connectionString, string queryString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                }
                catch(Exception e)
                {
                    throw new InvalidOperationException(e.Message);
                }
                finally
                {
                    connection.Close();
                }

            }
        }
    }
}
