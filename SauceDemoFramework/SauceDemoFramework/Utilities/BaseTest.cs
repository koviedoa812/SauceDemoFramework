using AventStack.ExtentReports;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SauceDemoFramework.Reporting;

namespace SauceDemoFramework.Utilities
{
    // Clase base para las pruebas: inicializa el driver, navega a la URL base
    // y captura screenshot al finalizar cada test.
    public class BaseTest
    {
        protected IWebDriver driver = null!;
        protected WebDriverWait _wait = null!;

        // ← Agregado para reportería
        private static readonly ReportManager _report = ReportManager.Instance;
        private ExtentTest _currentTest = null!;
        protected ExtentTest CurrentTest => _currentTest;

        [SetUp]
        public void Setup()
        {
            driver = DriverFactory.GetDriver();
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigManager.ExplicitWait));
            driver.Navigate().GoToUrl(ConfigManager.BaseUrl);

            // ← Crear entrada en el reporte
            _currentTest = _report.CreateTest(
                TestContext.CurrentContext.Test.FullName);
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                TakeScreenshot();
            }

            // ← Registrar resultado en el reporte
            var outcome = TestContext.CurrentContext.Result.Outcome.Status;
            switch (outcome)
            {
                case TestStatus.Passed:
                    _currentTest?.Pass("Test passed ✅");
                    break;
                case TestStatus.Failed:
                    var message = TestContext.CurrentContext.Result.Message ?? "Unknown failure";
                    var stack = TestContext.CurrentContext.Result.StackTrace ?? "";
                    _currentTest?.Fail(message);
                    if (!string.IsNullOrWhiteSpace(stack))
                        _currentTest?.Fail($"<pre>{stack}</pre>");
                    break;
                case TestStatus.Skipped:
                    _currentTest?.Skip("Test skipped.");
                    break;
            }

            // ← Flush del reporte
            _report.Flush();

            try
            {
                DriverFactory.QuitDriver(driver);
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

        // ← Método para loguear info en el reporte desde los tests
        protected void LogInfo(string message) => _currentTest?.Info(message);

        private void TakeScreenshot()
        {
            try
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string testName = Path.GetInvalidFileNameChars()
                    .Aggregate(TestContext.CurrentContext.Test.Name,
                        (current, c) => current.Replace(c, '_'));

                var filePath = Path.Combine(folder,
                    $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                screenshot.SaveAsFile(filePath);

                // ← Adjuntar screenshot al reporte
                _currentTest?.AddScreenCaptureFromPath(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: Error al tomar screenshot: {ex.Message}");
            }
        }
    }
}