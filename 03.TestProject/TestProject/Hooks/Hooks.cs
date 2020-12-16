namespace TestProject.Hooks
{
    using BoDi;
    using Framework.Core.Factory;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public class Hooks
    {
        private readonly WebDriverFactory _webdriverFactory;
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _webdriver;

        public Hooks(IObjectContainer objectContainer, WebDriverFactory webDriverFactory)
        {
            _objectContainer = objectContainer;
            _webdriverFactory = webDriverFactory;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _webdriver = _webdriverFactory.GetWebDriver;
            _objectContainer.RegisterInstanceAs<IWebDriver>(_webdriver);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _webdriver.Quit();
        }
    }
}
