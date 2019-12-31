namespace Framework.Core.Utilities
{
    using System;
    using System.Data;
    using System.IO;
    using System.Reflection;

    public static class CsvHelper
    {

        private static DataTable ReadHeader(string csvFile)
        {
            var dataTable = new DataTable();

            using (var fs = new FileStream(csvFile, FileMode.Open, FileAccess.Read))
            {
                using (var tReader = new StreamReader(fs))
                {
                    string exception;
                    if (tReader.EndOfStream)
                    {
                        exception = "Looks like the csv file '"+csvFile+"' is already blank";
                    }
                    var line = tReader.ReadLine();
                    var columnsData = line.Split(',');

                    if (columnsData.Length == 0)
                    {
                        exception = "There are no columns found on the Header.";
                        Console.WriteLine(exception);
                        throw new InvalidDataException(exception);
                    }

                    foreach (var data in columnsData)
                    {
                        dataTable.Columns.Add(data.Trim());
                    }
                }
            }

            return dataTable;
        }

        public static DataTable ReadData(string csvFile, bool header=true)
        {
            var dataTable = new DataTable();
            
            using (var fs = new FileStream(csvFile, FileMode.Open, FileAccess.Read))
            {
                using (var tReader = new StreamReader(fs))
                {
                    short atRow = 0;
                    short numOfColumns = 0;

                    while (!tReader.EndOfStream)
                    {
                        string exception = null;
                        string line = tReader.ReadLine();
                        string[] columnsData = line.Split(',');
						
                        if (atRow == 0 && header)
                        {
                            numOfColumns = (short)columnsData.Length;
                            if (numOfColumns == 0)
                            {
                                exception = "There are no columns found on the Header.";
                                Console.WriteLine(exception);
                                throw new InvalidDataException(exception);
                            }
                            foreach (string data in columnsData)
                            {
                                dataTable.Columns.Add(data.Trim());
                            }
                            atRow++;
                            continue;
                        }
                        else if (atRow == 0)
                        {
                            numOfColumns = (short)columnsData.Length;
                        }
						
                        if (numOfColumns != columnsData.Length)
                        {
                            exception = "Number of columns are inconsistent at row: "+ atRow;
                            Console.WriteLine(exception);
                            throw new InvalidDataException(exception);
                        }
						
                        DataRow dRow = dataTable.NewRow();
                        for (int i = 0; i < numOfColumns; i++)
                        {
                            dRow[i] = columnsData[i].Trim();
                        }
                        dataTable.Rows.Add(dRow);
                        atRow++;
                    }
                }
            }
            
            return dataTable;
        }

        public static void InsertData(string csvFile, DataRow row)
        {
            using (var fs = new FileStream(csvFile, FileMode.Append, FileAccess.Write))
            {
                using (var tWriter = new StreamWriter(fs))
                {
                    short numOfColumns = (short)row.ItemArray.Length;

                    for (int i=0; i < numOfColumns; i++ )
                    {
                        tWriter.Write(row[i]);

                        if(i<numOfColumns-1)
                        {
                            tWriter.Write(",");
                        }
                    }
                    tWriter.Write("\n");
                }
            }
            
        }

        public static void InsertData<T>(string csvFile, T item) where T : new ()
        {
            var dataTable = ReadHeader(csvFile);
            var dataRow = dataTable.NewRow();
            var dataColumns = dataTable.Columns;

            foreach (var column in dataColumns)
            {
                var p = item.GetType().GetProperty(column.ToString().Trim());
                if (p != null)
                {
                    dataRow[column.ToString()] = p.GetValue(item, null);
                }
            }

            InsertData(csvFile, dataRow);
        }

        public static void InsertDynamicData(string csvFile, dynamic item)
        {
            var dataTable = ReadHeader(csvFile);
            var dataRow = dataTable.NewRow();
            var dataColumns = dataTable.Columns;

            foreach (var column in dataColumns)
            {
                if (item.Contains(column.ToString()))
                {
                    dataRow[column.ToString()] = item.GetMember(column.ToString());
                }
            }

            InsertData(csvFile, dataRow);
        }

        public static void UpdateData(string csvFile, DataRow inputRow, string primaryKey)
        {
            DataTable dTable = ReadData(csvFile);
            dTable.PrimaryKey = new DataColumn[1] { dTable.Columns[primaryKey] };
            DataRow dRowToUpdate = dTable.Rows.Find(inputRow[primaryKey]);

            short numOfColumns = (short)dTable.Columns.Count;
            
            for(int i=0; i<numOfColumns;i++)
            {
                dRowToUpdate[i] = inputRow[i];
            }


            using (var fs = new FileStream(csvFile, FileMode.Truncate, FileAccess.Write))
            {
                using (var tWriter = new StreamWriter(fs))
                {
                    

                    string header = "";
                    for (int i = 0; i < numOfColumns; i++)
                    {
                        header = header + dTable.Columns[i];

                        if (i < numOfColumns - 1)
                        {
                            header = header + ",";
                        }
                    }
                    tWriter.WriteLine(header);

                    foreach (DataRow dRow in dTable.Rows)
                    {
                        string line = "";
                        for (int i = 0; i < numOfColumns; i++)
                        {
                             line = line+ dRow[i];

                            if (i < numOfColumns - 1)
                            {
                                line = line + ",";
                            }
                        }
                        tWriter.WriteLine(line);
                    }
                    
                }
                
            }
            
        }

        public static void UpdateData<T>(string csvFile, PropertyInfo primaryKey, T item) where T : new()
        {
            var dataTable = ReadHeader(csvFile);
            var dataRow = dataTable.NewRow();
            var dataColumns = dataTable.Columns;

            foreach (var column in dataColumns)
            {
                var p = item.GetType().GetProperty(column.ToString().Trim());
                if (p != null)
                {
                    dataRow[column.ToString()] = p.GetValue(item, null);
                }
            }

            UpdateData(csvFile, dataRow, primaryKey.Name);
        }

        public static void UpdateDynamicData(string csvFile, string primaryKey, dynamic item)
        {
            var dataTable = ReadHeader(csvFile);
            var dataRow = dataTable.NewRow();
            var dataColumns = dataTable.Columns;

            foreach (var column in dataColumns)
            {
                if (item.Contains(column.ToString()))
                {
                    dataRow[column.ToString()] = item.GetMember(column.ToString());
                }
            }

            UpdateData(csvFile, dataRow, primaryKey);
        }
    }
}
