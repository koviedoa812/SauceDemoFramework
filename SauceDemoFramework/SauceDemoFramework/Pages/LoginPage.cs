using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SauceDemoFramework.Utilities;

namespace SauceDemoFramework.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            // Usamos el tiempo configurado en el JSON
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigManager.ExplicitWait));
        }

        // --- LOCALIZADORES (Usando diferentes tipos para cumplir el requisito) ---

        // Uso de ID
        private IWebElement UsernameField => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("user-name")));

        // Uso de NAME (Requisito: Variar selectores)
        private IWebElement PasswordField => _wait.Until(ExpectedConditions.ElementIsVisible(By.Name("password")));

        // Uso de CLASS NAME (Requisito: Variar selectores)
        private IWebElement LoginButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("submit-button")));

        // Uso de RELATIVE LOCATORS (Requisito Obligatorio)
        // Buscamos el mensaje de error que aparece DEBAJO del campo de password
        private IWebElement ErrorMessage => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("h3[data-test='error']")));


        // --- MÉTODOS DE ACCIÓN ---

        public void EnterUsername(string username)
        {
            UsernameField.Clear();
            UsernameField.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            // PasswordField ya tiene la espera integrada en su definición
            PasswordField.Clear();
            PasswordField.SendKeys(password);
        }

        public void ClickLogin()
        {
            LoginButton.Click();
        }

        public void Login(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLogin();
        }

        public string GetErrorMessage()
        {
            // El Relative Locator ya está definido en la propiedad ErrorMessage
            return ErrorMessage.Text;
        }
    }
}