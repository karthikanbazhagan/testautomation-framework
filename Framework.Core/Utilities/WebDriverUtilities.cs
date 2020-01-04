namespace Framework.Core.Utilities
{
    using System;
    using System.Configuration;
    using System.IO;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.IE;

    using Framework.Core.Constant;
       

    public static class WebDriverUtilities
    {
        private static TimeSpan TimeoutInSecs;

        public static IWebDriver StartWebDriver()
        {
            var browser = ConfigurationManager.AppSettings["SeleniumBrowserName"];
            var Browser = BrowserType.Chrome;

            if (browser != "Any")
            {
                Browser = (BrowserType)Enum.Parse(typeof(BrowserType), browser);
            }

            string driverServerDirectory;
            if (GetDriverPath(Browser, out driverServerDirectory))
            {
                return InstantiateDriver(driverServerDirectory, Browser);
            }

            throw new FileNotFoundException("Missing web driver file");
        }

        private static bool GetDriverPath(BrowserType browserType, out string currentDirectory)
        {
            string driverExecutable;

            switch (browserType)
            {
                case BrowserType.Chrome:
                    driverExecutable = "chromedriver.exe";
                    break;
                case BrowserType.InternetExplorer:
                    driverExecutable = "IEDriverServer.exe";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(browserType.ToString(), browserType, null);
            }

            currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            return File.Exists(Path.Combine(currentDirectory, driverExecutable));
        }

        private static IWebDriver InstantiateDriver(string webdriverPath, BrowserType browerType)
        {
            IWebDriver webDriver;
            switch (browerType)
            {
                case BrowserType.Chrome:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--disable-extensions");

                    webDriver = new ChromeDriver(webdriverPath, chromeOptions, TimeoutInSecs);
                    break;
                case BrowserType.InternetExplorer:
                    var internetExplorerOptions = new InternetExplorerOptions { EnsureCleanSession = true, EnablePersistentHover = false, EnableNativeEvents = true };
                    webDriver = new InternetExplorerDriver(webdriverPath, internetExplorerOptions, TimeoutInSecs);
                    break;
                default:
                    throw new InvalidOperationException("Web Driver unsupported: " + browerType);
            }

            webDriver.Manage().Cookies.DeleteAllCookies();
            return webDriver;
        }
    }
}
