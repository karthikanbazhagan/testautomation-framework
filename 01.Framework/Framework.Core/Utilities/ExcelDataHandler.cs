namespace Framework.Core.Utilities
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.Reflection;

    public static class ExcelDataHandler
    {
        
        public static DataSet LoadDataFromWorkbook(string filename, bool header)
        {
            DataSet dataSet = new DataSet();
            string HDR = header ? "Yes" : "No";

            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                "Data Source=" + filename + ";Extended Properties='Excel 8.0;HDR=" + HDR + ";IMEX=1;';";

            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;
            try
            {
                cmd = new OleDbCommand();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter();

                cmd.Connection = conn;
                conn.Open();
                DataTable dtSchema;
                dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                conn.Close();

                conn.Open();
                foreach (DataRow row in dtSchema.Rows)
                {

                    string ExcelSheetName = row["TABLE_NAME"].ToString();

                    cmd.CommandText = "SELECT * From [" + ExcelSheetName + "]";
                    dataAdapter.SelectCommand = cmd;
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataTable.TableName = ExcelSheetName;
                    dataSet.Tables.Add(dataTable);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }

            return dataSet;
        }
        
        public static DataTable LoadDataFromTable(string filename, string tableName, bool header)
        {
            DataTable dataTable = new DataTable();
            string HDR = header ? "Yes" : "No";

            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                "Data Source=" + filename + ";Extended Properties='Excel 8.0;HDR=" + HDR + ";IMEX=1;';";

            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;
            try
            {
                cmd = new OleDbCommand();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter();

                cmd.Connection = conn;
                conn.Open();
                DataTable dtSchema;
                dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                conn.Close();

                conn.Open();
                cmd.CommandText = "SELECT * From [" + tableName + "]";
                dataAdapter.SelectCommand = cmd;
                dataAdapter.Fill(dataTable);
                dataTable.TableName = tableName;

            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return dataTable;
        }
        
        public static void InsertDataIntoWorkSheet(string filename, string sheetName, DataRow row)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                "Data Source=" + filename + ";Extended Properties='Excel 8.0; HDR=YES'";


            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;

            string tableName = sheetName + "$";

            string fieldsList = "";
            string valuesList = "";

            DataColumnCollection columns = row.Table.Columns;

            try
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    fieldsList = fieldsList + "[" + columns[i] + "]";
                    valuesList = valuesList + "'" + row[columns[i]].ToString() + "'";

                    if (i == columns.Count - 1)
                    {
                        continue;
                    }
                    fieldsList = fieldsList + ",";
                    valuesList = valuesList + ",";
                }

                string cmdText = "INSERT INTO [" + tableName + "] (" + fieldsList + ") VALUES( " + valuesList + " )";
                conn.Open();
                cmd = new OleDbCommand(cmdText, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }
        
        public static void InsertDataIntoTable<T>(string filename, string table, T item) where T : new()
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                "Data Source=" + filename + ";Extended Properties='Excel 8.0; HDR=YES'";

            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;

            string fieldsList = "";
            string valuesList = "";

            DataColumnCollection columns = LoadDataFromTable(filename, table, true).Columns;

            try
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    PropertyInfo p = item.GetType().GetProperty(columns[i].ColumnName);

                    if (p != null)
                    {
                        fieldsList = fieldsList + "[" + columns[i].ColumnName + "]";
                        valuesList = valuesList + "'" + p.GetValue(item).ToString() + "'";
                    }

                    if (i == columns.Count - 1)
                    {
                        continue;
                    }
                    fieldsList = fieldsList + ",";
                    valuesList = valuesList + ",";
                }

                string cmdText = "INSERT INTO [" + table + "] (" + fieldsList + ") VALUES( " + valuesList + " )";
                conn.Open();
                cmd = new OleDbCommand(cmdText, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }
        
        public static void InsertDynamicDataIntoTable(string filename, string table, dynamic item)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                "Data Source=" + filename + ";Extended Properties='Excel 8.0; HDR=YES'";

            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;

            string fieldsList = "";
            string valuesList = "";

            DataColumnCollection columns = LoadDataFromTable(filename, table, true).Columns;

            try
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    
                    if (item.Contains(columns[i].ColumnName))
                    {
                        fieldsList = fieldsList + "[" + columns[i].ColumnName + "]";
                        valuesList = valuesList + "'" + item.GetMember(columns[i].ColumnName).ToString() + "'";
                    }

                    if (i == columns.Count - 1)
                    {
                        continue;
                    }
                    fieldsList = fieldsList + ",";
                    valuesList = valuesList + ",";
                }

                string cmdText = "INSERT INTO [" + table + "] (" + fieldsList + ") VALUES( " + valuesList + " )";
                conn.Open();
                cmd = new OleDbCommand(cmdText, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }
        
        public static void UpdateWorkSheet(string filename, string sheetName, DataColumn primaryKey, DataRow row)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                "Data Source=" + filename + ";Extended Properties='Excel 8.0; HDR=YES'";


            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;

            try
            {
                string table = sheetName + "$";
                DataColumnCollection columns = row.Table.Columns;

                string cmdText = "UPDATE [" + table + "] SET ";
                for (int i = 0; i < columns.Count; i++)
                {
                    if (columns[i] == primaryKey)
                    {
                        continue;
                    }
                    cmdText = cmdText + columns[i] + " = ?";

                    if (i == columns.Count - 1)
                    {
                        continue;
                    }
                    cmdText = cmdText + ",";
                }

                cmdText = cmdText + " WHERE " + primaryKey + " = ?";
                conn.Open();
                cmd = new OleDbCommand(cmdText, conn);

                for (int i = 0; i < columns.Count; i++)
                {
                    if (columns[i] == primaryKey)
                    {
                        continue;
                    }
                    cmd.Parameters.AddWithValue("?", row[columns[i]].ToString());
                }
                cmd.Parameters.AddWithValue("?", row[primaryKey].ToString());

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }
        
        public static void UpdateTable<T>(string filename, string table, PropertyInfo primaryKey, T item) where T : new()
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                "Data Source=" + filename + ";Extended Properties='Excel 8.0; HDR=YES'";

            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;

            DataColumnCollection columns = LoadDataFromTable(filename, table, true).Columns;
            string primaryKeyStr = primaryKey.Name.ToString();

            try
            {
                string cmdText = "UPDATE [" + table + "] SET ";
                for (int i = 0; i < columns.Count; i++)
                {

                    if (columns[i].ColumnName == primaryKeyStr)
                    {
                        continue;
                    }

                    PropertyInfo pr = item.GetType().GetProperty(columns[i].ColumnName);

                    if (pr != null)
                    {
                        cmdText = cmdText + columns[i].ColumnName + " = '" + pr.GetValue(item).ToString() + "'";
                    }

                    if (i == columns.Count - 2)
                    {
                        continue;
                    }
                    cmdText = cmdText + ",";
                }

                cmdText = cmdText + " WHERE " + primaryKeyStr + " = '" + primaryKey.GetValue(item).ToString() + "'";
                conn.Open();
                cmd = new OleDbCommand(cmdText, conn);

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }
        
        public static void UpdateTableDynamic(string filename, string table, string primaryKey, dynamic item)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                "Data Source=" + filename + ";Extended Properties='Excel 8.0; HDR=YES'";

            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;

            DataColumnCollection columns = LoadDataFromTable(filename, table, true).Columns;
            //string primaryKeyStr = primaryKey.Name.ToString();

            try
            {
                string cmdText = "UPDATE [" + table + "] SET ";
                for (int i = 0; i < columns.Count; i++)
                {

                    if (columns[i].ColumnName == primaryKey)
                    {
                        continue;
                    }
                    
                    if (item.Contains(columns[i].ColumnName))
                    {
                        cmdText = cmdText + columns[i].ColumnName + " = '" + item.GetMember(columns[i].ColumnName).ToString() + "'";
                    }

                    if (i == columns.Count - 2)
                    {
                        continue;
                    }
                    cmdText = cmdText + ",";
                }

                cmdText = cmdText + " WHERE " + primaryKey + " = '" + item.GetMember(primaryKey).ToString() + "'";
                conn.Open();
                cmd = new OleDbCommand(cmdText, conn);

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }

    }
}
