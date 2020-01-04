namespace Framework.Core.Factory
{
    using System;
    using System.Configuration;
    using System.Threading;
    
    using OpenQA.Selenium;

    using Framework.Core.Utilities;

    /// <summary>
    /// Web driver factory to create and dispose an instance of the IWebdriver interface
    /// </summary>
    public class WebDriverFactory
    {
        private IWebDriver WebDriver;
        
        public IWebDriver GetWebDriver
        {
            get
            {
                return WebDriver ?? (WebDriver = CreateWebDriver());
            }
        }

        public void DisposeWebDriver()
        {
            WebDriver.Quit();
            WebDriver = null;
        }

        private IWebDriver CreateWebDriver()
        {
            using (var mutex = new Mutex(false, "CreateWebDriver"))
            {
                var mutexWaitTime = Convert.ToDouble(ConfigurationManager.AppSettings["MutexWaitTime"]);

                mutex.WaitOne(TimeSpan.FromSeconds(mutexWaitTime));

                try
                {
                    WebDriver = WebDriverUtilities.StartWebDriver();
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }

            return WebDriver;
        }
    }
}
