using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SauceDemoFramework.Utilities
{
    // Clase base para las pruebas: inicializa el driver, navega a la URL base
    // y captura screenshot al finalizar cada test.
    public class BaseTest
    {
        protected IWebDriver driver = null!;
        protected WebDriverWait _wait = null!;

        [SetUp]
        public void Setup()
        {
            driver = DriverFactory.GetDriver();
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigManager.ExplicitWait));
            driver.Navigate().GoToUrl(ConfigManager.BaseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                TakeScreenshot();
            }

            try
            {
                DriverFactory.QuitDriver(driver); // Cierra el navegador y limpia _driver = null
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: Error al cerrar el driver: {ex.Message}");
            }
            finally
            {
                driver?.Dispose();
                driver = null!;
            }
        }

        private void TakeScreenshot()
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string testName = Path.GetInvalidFileNameChars()
                .Aggregate(TestContext.CurrentContext.Test.Name, (current, c) => current.Replace(c, '_'));

            var filePath = Path.Combine(folder, $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            screenshot.SaveAsFile(filePath);
        }
    }
}