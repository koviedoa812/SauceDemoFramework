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

            // Navegar primero a la URL base para que las operaciones sobre cookies/localStorage se apliquen al dominio correcto
            driver.Navigate().GoToUrl(ConfigManager.BaseUrl);

            // Asegurar sesión limpia entre tests: eliminar cookies y almacenamiento local/session
            try
            {
                driver.Manage().Cookies.DeleteAllCookies();
                if (driver is IJavaScriptExecutor js)
                {
                    js.ExecuteScript("window.localStorage.clear(); window.sessionStorage.clear();");
                }

                // Refrescar para que la app cargue en estado limpio
                driver.Navigate().Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: No se pudo limpiar la sesión antes del test: {ex.Message}");
            }
        }

        [TearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;

            if (status != NUnit.Framework.Interfaces.TestStatus.Skipped)
            {
                TakeScreenshot();
            }

            // Cerrar y liberar la instancia de driver de este test únicamente
            try
            {
                driver.Quit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: Error al cerrar el driver: {ex.Message}");
            }

            try
            {
                driver.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: Error al disponer el driver: {ex.Message}");
            }
            finally
            {
                driver = null!;
            }
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