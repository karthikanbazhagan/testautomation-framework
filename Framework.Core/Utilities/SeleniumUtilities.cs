namespace Framework.Core.Utilities
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Internal;
    using OpenQA.Selenium.Support.UI;

    public static class SeleniumUtilities
    {
        public static void ForceClearAndSendKeys(this IWebElement element, string text)
        {
            element.SendKeys(Keys.Home + Keys.Shift + Keys.End);
            element.SendKeys(text);
        }

        public static void ClearAndSendKeys(this IWebElement element, string text)
        {
            element.Clear();
            element.SendKeys(text);
        }

        public static void SelectDropdown(this IWebElement element, SelectBy selectBy, string value)
        {
            var driver = ((IWrapsDriver)element).WrappedDriver;

            driver.ScrollToView(element);
            switch (selectBy)
            {
                case SelectBy.Text:
                    new SelectElement(element).SelectByText(value);
                    break;
                case SelectBy.Value:
                    new SelectElement(element).SelectByValue(value);
                    break;
                case SelectBy.Index:
                    new SelectElement(element).SelectByIndex(Convert.ToInt32(value));
                    break;
            }
        }

        public static IWebElement ScrollToView(this IWebDriver driver, By selector)
        {
            var element = driver.FindElement(selector);
            driver.ScrollToView(element);
            return element;
        }

        public static void ScrollToView(this IWebDriver driver, IWebElement element)
        {
            if (element.Location.Y > 200)
            {
                driver.ScrollTo(0, element.Location.Y - 100); // Make sure element is in the view but below the top navigation pane
            }
        }

        public static void ScrollTo(this IWebDriver driver, int xPosition = 0, int yPosition = 0)
        {
            var jsExecutor = (IJavaScriptExecutor)driver;
            var js = $"window.scrollTo({xPosition}, {yPosition})";
            jsExecutor.ExecuteScript(js);
        }

        public static void ScrollDown(this IWebDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        }

        public static void ScrollRight(this IWebDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(document.body.scrollWidth, 0);");
        }

        public static void Hover(this IWebElement element)
        {
            IWebDriver driver = ((RemoteWebElement)element).WrappedDriver;

            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
        }

        public static void HoverAndClick(this IWebElement elementToHover, IWebElement elementToClick)
        {
            IWebDriver driver = ((RemoteWebElement)elementToHover).WrappedDriver;

            Actions action = new Actions(driver);
            action.MoveToElement(elementToHover).Click(elementToClick).Build().Perform();
        }

        public static bool IsElementVisible(this IWebElement element)
        {
            IWebDriver driver = ((RemoteWebElement)element).WrappedDriver;

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                return (bool)js.ExecuteScript("return $(arguments[0]).is(':visible')", element);
            }
            catch (StaleElementReferenceException e)
            {
                return false;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
        }

        public static bool IsElementEnabled(this IWebElement element)
        {
            IWebDriver driver = ((RemoteWebElement)element).WrappedDriver;

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            return (bool)js.ExecuteScript("return $(arguments[0]).is(':enabled')", element);
        }

        public static bool IsElementDisabled(this IWebElement element)
        {
            IWebDriver driver = ((RemoteWebElement)element).WrappedDriver;

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            return (bool)js.ExecuteScript("return $(arguments[0]).is(':disabled')", element);
        }

        public static bool IsChecked(this IWebElement element)
        {
            IWebDriver driver = ((RemoteWebElement)element).WrappedDriver;

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            return (bool)js.ExecuteScript("return arguments[0].checked", element);
        }
    }

    public enum SelectBy
    {
        /// <summary>
        /// To select by Text
        /// </summary>
        Text,

        /// <summary>
        /// To select by Value
        /// </summary>
        Value,

        /// <summary>
        /// To select by Index
        /// </summary>
        Index
    }
}
