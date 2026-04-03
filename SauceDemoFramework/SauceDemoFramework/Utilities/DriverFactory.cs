using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SauceDemoFramework.Utilities
{
    // Crear, entrega y cierra la instancia del driver
    public static class DriverFactory
    {
        private static IWebDriver? _driver;

        public static IWebDriver GetDriver()
        {
            if (_driver == null)
            {
                var options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                options.AddArgument("--disable-notifications");
                options.AddArgument("--disable-infobars");
                options.AddArgument("--headless=new");
                options.AddArgument("--window-size=1920,1080");

                // --- AGREGA ESTO PARA QUITAR LA ALERTA DE LA IMAGEN ---
                options.AddUserProfilePreference("credentials_enable_service", false);
                options.AddUserProfilePreference("profile.password_manager_enabled", false);
                // -----------------------------------------------------


                _driver = new ChromeDriver(options);
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ConfigManager.ImplicitWait);
            }
            return _driver;
        }

        // Llamar a este método al finalizar las pruebas para cerrar el navegador y liberar recursos
        public static void QuitDriver()
        {
            if (_driver != null)
            {
                _driver.Quit(); // Cierra el navegador y libera los recursos asociados
                _driver = null;
            }
        }
    }
}