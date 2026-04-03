using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SauceDemoFramework.Utilities;

namespace SauceDemoFramework.Utilities
{
    // Clase base para las pruebas que maneja la configuración del driver, navegación y captura de pantalla en caso de fallo
    public class BaseTest
    {
        protected IWebDriver driver = null!;
        protected WebDriverWait _wait;

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
            
            TakeScreenshot();
            DriverFactory.QuitDriver();
            driver.Dispose();
        }

        private void TakeScreenshot()
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // 1. Obtenemos el nombre y lo limpiamos
            string testName = TestContext.CurrentContext.Test.Name;
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                testName = testName.Replace(c, '_');
            }

            // ADEMÁS: Windows odia las comillas dobles (") que NUnit pone en los parámetros
            testName = testName.Replace("\"", "");

            // 2. CORRECCIÓN: Usamos 'testName' (limpia), NO el Contexto original
            var fileName = $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";

            var filePath = Path.Combine(folder, fileName);
            screenshot.SaveAsFile(filePath);
        }

    }
}