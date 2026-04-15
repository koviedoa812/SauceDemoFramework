using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace SauceDemoFramework.StepDefinitions.Hooks
{
    [Binding]
    public sealed class WebDriverHooks
    {
        private readonly ScenarioContext _scenarioContext;

        public WebDriverHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-infobars");
            options.AddArgument("headless");

            IWebDriver driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://www.saucedemo.com");
            _scenarioContext.Set<IWebDriver>(driver);

        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (_scenarioContext.TryGetValue<IWebDriver>(out var driver))
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}
