namespace Framework.Core.PageObject
{
    using System;
    using System.Collections.Generic;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public abstract class BasePage : IPage
    {
        // Webdriver should not be available to the outside world (even to the concrete Page Object classes)
        private IWebDriver driver;

        protected BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        protected IWebElement FindElement(By by, int waitInSec = 30)
        {
            try
            {
                return GetElement(by, waitInSec);
            }
            catch (WebDriverTimeoutException)
            {
                // Let it throw the original exception. ie., NoSuchElementException
                return driver.FindElement(by);
            }
        }

        protected IList<IWebElement> FindElements(By by, int waitInSec = 30)
        {
            try
            {
                return GetElements(by, waitInSec);
            }
            catch (WebDriverTimeoutException)
            {
                // Let it throw the original exception. ie., NoSuchElementException
                return driver.FindElements(by);
            }
        }

        protected bool IsElementAvailable(By by, int waitInSec)
        {
            try
            {
                GetElement(by, waitInSec);
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        private IWebElement GetElement(By by, int waitInSec)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitInSec));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            return wait.Until(driver => driver.FindElement(by));
        }

        private IList<IWebElement> GetElements(By by, int waitInSec)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitInSec));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            return wait.Until(driver => driver.FindElements(by));
        }
    }
}
