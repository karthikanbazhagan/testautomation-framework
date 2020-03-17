namespace Framework.Core.TestData
{
    using System.Reflection;

    public interface IDataContainer<T> where T: ITestData
    {
        void ImportData();

        void AddData(T item);

        void UpdateData(T item, PropertyInfo primaryKey);
    }
}
