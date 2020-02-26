namespace Framework.Core.TestData
{
    using System.Collections.Generic;
    using System.Reflection;

    using Framework.Core.Utilities;
    
    public abstract class BaseDataContainer<T> : IDataContainer<T> where T : class, new()
    {
        protected IList<T> Data;

        protected string FilePath;

        public void AddData(T item)
        {
            Data.Add(item);
            CsvDataHandler.InsertData(FilePath, item);
        }

        public void ImportData()
        {
            var dataTable = CsvDataHandler.ReadData(FilePath);
            Data = DataConverter.CreateListFromTable<T>(dataTable);
        }

        public void UpdateData(T item, PropertyInfo primaryKey)
        {
            CsvDataHandler.UpdateData(FilePath, primaryKey, item);
        }
    }
}
