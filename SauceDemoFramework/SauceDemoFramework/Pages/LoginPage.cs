using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SauceDemoFramework.Utilities;
using SeleniumExtras.WaitHelpers;

namespace SauceDemoFramework.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Locators con IWebElement
        private IWebElement UsernameField => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("user-name")));
        private IWebElement PasswordField => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("password")));
        private IWebElement LoginButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("login-button")));
        private IWebElement ErrorMessage => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("h3[data-test='error']")));

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver,TimeSpan.FromSeconds(ConfigManager.ExplicitWait));
        }

        public void EnterUsername(string username)
        {
            _wait.Until(d => d.FindElement(By.Id("user-name")));
            UsernameField.Clear();
            UsernameField.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            PasswordField.SendKeys(password);
        }

        public void ClickLogin()
        {
            LoginButton.Click();
        }

        // Método de login completo
        public void Login(string Username, string Password)
        {
            EnterUsername(Username);
            EnterPassword(Password);
            ClickLogin();
        }

        // Método para obtener el mensaje de error con login inválido
        public string GetErrorMessage()
        {
            string ErrorMessageLogin = ErrorMessage.Text;
            return ErrorMessageLogin;
        }
    }
}