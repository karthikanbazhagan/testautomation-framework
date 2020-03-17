namespace Framework.Core.Factory
{
    using System;
    using OpenQA.Selenium;

    using Framework.Core.PageObject;
    
    public class PageObjectFactory
    {
        private readonly IWebDriver driver;

        public PageObjectFactory(IWebDriver driver)
        {
            this.driver = driver;
        }

        public T GetInstanceOf<T>() where T : IPage
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { driver });
        }
    }
}
