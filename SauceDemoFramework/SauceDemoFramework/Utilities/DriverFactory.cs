using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SauceDemoFramework.Utilities
{
    // Crear, entrega y cierra la instancia del driver
    public static class DriverFactory
    {
        //private static IWebDriver? _driver;
        public static IWebDriver GetDriver()
        {
            // Siempre crear una nueva instancia por test para evitar estado compartido entre pruebas
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-infobars");
            //options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");

            // --- AGREGA ESTO PARA QUITAR LA ALERTA DE LA IMAGEN ---
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            options.AddUserProfilePreference("profile.password_manager_leak_detection", false);
            // -----------------------------------------------------

            var driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ConfigManager.ImplicitWait);
            return driver;
        }

        // Llamar a este método al finalizar las pruebas para cerrar el navegador y liberar recursos
        public static void QuitDriver(IWebDriver? driver)
        {
            driver?.Quit();
        }
    }
}