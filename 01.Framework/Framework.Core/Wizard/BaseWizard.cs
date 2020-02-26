namespace Framework.Core.Wizard
{
    using Framework.Core.PageObject;
    using OpenQA.Selenium;

    public class BaseWizard : BasePage, IWizard
    {
        public BaseWizard(IWebDriver driver) : base(driver)
        {
            // Instantiate all the PageObjects here.
        }

        public int CurrentStep { get; set; }

        protected IWebElement NextButton { get; set; }

        protected IWebElement SubmitButton { get; set; }

        public virtual void NavigateToNextPage()
        {
            NextButton.Click();
            CurrentStep++;
        }

        public virtual void SubmitWizard()
        {
            SubmitButton.Click();
        }
    }
}
