namespace Framework.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
        
    public static class DataConverter
    {
        
        public static List<T> CreateListFromTable<T>(DataTable tbl) where T : new()
        {
            List<T> lst = new List<T>();
            
            foreach (DataRow r in tbl.Rows)
            {
                lst.Add(CreateItemFromRow<T>(r));
            }
            
            return lst;
        }
        
        public static T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            T item = new T();
            SetItemFromRow(item, row);
            return item;
        }
        
        private static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            foreach (DataColumn c in row.Table.Columns)
            {
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName);
                
                if (p != null && row[c] != DBNull.Value)
                {
                    string type = p.PropertyType.ToString();

                    switch(type)
                    {
                        case "System.Int16":
                            p.SetValue(item, Convert.ToInt16(row[c]), null);
                            break;
                        case "System.Int32":
                            p.SetValue(item, Convert.ToInt32(row[c]), null);
                            break;
                        case "System.Int64":
                            p.SetValue(item, Convert.ToInt64(row[c]), null);
                            break;
                        case "System.Double":
                            p.SetValue(item, Convert.ToDouble(row[c]), null);
                            break;
                        case "System.Decimal":
                            p.SetValue(item, Convert.ToDecimal(row[c]), null);
                            break;

                        default:
                            p.SetValue(item, row[c], null);
                            break;
                    }
                }
            }
        }
        
        public static DataTable InsertItemIntoDataTable<T>(T item, DataTable dataTable) where T : new()
        {
            string[] arr = new string[dataTable.Columns.Count];
            //T value = dataList[dataList.Count - 1];
            int i = 0;
            foreach (DataColumn column in dataTable.Columns)
            {
                System.Reflection.PropertyInfo p = item.GetType().GetProperty(column.ColumnName);
                if (p != null)
                {
                    arr[i] = p.GetValue(item).ToString();
                }
                i++;
            }
            dataTable.Rows.Add(arr);
            return dataTable;
        }
        
        public static List<dynamic> CreateListFromTable(DataTable tbl)
        {
            List<dynamic> lst = new List<dynamic> ();
            foreach (DataRow r in tbl.Rows)
            {
                lst.Add(CreateItemFromRow(r));
            }
            return lst;
        }
        
        public static dynamic CreateItemFromRow(DataRow row)
        {
            DDModel item = new DDModel();
            SetItemFromRow(item, row);
            return item;
        }
        
        private static void SetItemFromRow(DDModel item, DataRow row)
        {
            foreach (DataColumn c in row.Table.Columns)
            {
                item.AddMember(c.ColumnName, row[c]);
            }
        }

    }
}
