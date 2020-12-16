namespace TestProject.StepDefinitions
{
    using OpenQA.Selenium;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TechTalk.SpecFlow;

    [Binding]
    public sealed class WikipediaTitleValidationSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly IWebDriver _webdriver;

        public WikipediaTitleValidationSteps(IWebDriver webdriver)
        {
            _webdriver = webdriver;
        }

        [Given(@"I want to validate the title of ""(.*)"" wikipedia page")]
        public void GivenIWantToValidateTheTitleOfWikipediaPage(string name)
        {
            Console.WriteLine($"To validate {name} title page");
        }

        [When(@"I navigate to the ""(.*)"" page on wikipedia")]
        public void WhenINavigateToThePageOnWikipedia(string name)
        {
            _webdriver.Url = $"https://en.wikipedia.org/wiki/{name}";
            _webdriver.Navigate();
        }

        [Then(@"the the title of the page should be ""(.*)""")]
        public void ThenTheTheTitleOfThePageShouldBe(string title)
        {
            Xunit.Assert.Equal(title, _webdriver.Title);
        }


    }
}
