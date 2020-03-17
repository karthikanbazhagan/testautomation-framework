namespace Framework.Core.Function
{
    using Framework.Core.TestData;

    public interface IWebKeyword<TTestData> where TTestData: ITestData
    {
        void LoginAndExecute(TTestData testData);
    }
}
