namespace Framework.Core.Function
{
    using Framework.Core.TestData;
    using Framework.Core.Factory;

    public abstract class BaseWebKeyword<TTestData> : IWebKeyword<TTestData> where TTestData : ITestData
    {
        protected readonly PageObjectFactory PageObjectFactory;

        public BaseWebKeyword(PageObjectFactory pageObjectFactory)
        {
            PageObjectFactory = pageObjectFactory;
        }

        public virtual void LoginAndExecute(TTestData testData)
        {
            Execute(testData);
        }

        protected abstract void Execute(TTestData testData);
    }
}
