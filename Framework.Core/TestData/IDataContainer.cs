namespace Framework.Core.TestData
{
    using System.Reflection;

    public interface IDataContainer<T> where T: class, new()
    {
        void ImportData();

        void AddData(T item);

        void UpdateData(T item, PropertyInfo primaryKey);
    }
}
