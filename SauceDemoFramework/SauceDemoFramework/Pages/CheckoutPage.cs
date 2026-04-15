using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SauceDemoFramework.Utilities;
using SeleniumExtras.WaitHelpers;

namespace SauceDemoFramework.Pages
{
    public class CheckoutPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public CheckoutPage(IWebDriver driver)
        {
            _driver = driver;
            // Usamos el tiempo configurado en el JSON
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigManager.ExplicitWait));
        }

        // --- LOCATORS COMO IWEBELEMENT ---
        // Usamos "=>" para que se busquen en tiempo de ejecución

        private IWebElement TitleLabelCheckout => _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("title")));

        private IWebElement FirstNameField => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("first-name")));
        private IWebElement LastNameField => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("last-name")));
        private IWebElement PostalCodeField => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("postal-code")));

        private IWebElement ContinueButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("continue")));
        private IWebElement FinishButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("finish")));
        private IWebElement SuccessMessage => _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("complete-header")));

        private IWebElement ErrorMessage => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-test='error']")));

        public string GetErrorMessage()
        {
            return ErrorMessage.Text;
        }

        // Método para obtener el título de la página de checkout
        public string GetPageTitleCheckout()
        {
            return TitleLabelCheckout.Text;
        }

        // --- MÉTODOS DE ACCIÓN ---
        public void FillInformation(string firstName, string lastName, string postalCode, bool expectNavigation = true)
        {
            var js = (IJavaScriptExecutor)_driver;

            void SetFieldValue(string fieldId, string value)
            {
                var field = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(fieldId)));
                field.Click();
                field.Clear();
                if (!string.IsNullOrEmpty(value))
                {
                    js.ExecuteScript(@"
                var field = arguments[0];
                var value = arguments[1];
                var nativeInputValueSetter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set;
                nativeInputValueSetter.call(field, value);
                field.dispatchEvent(new Event('input', { bubbles: true }));
                field.dispatchEvent(new Event('change', { bubbles: true }));
                field.dispatchEvent(new Event('blur', { bubbles: true }));
            ", field, value);
                }
            }

            SetFieldValue("first-name", firstName);
            SetFieldValue("last-name", lastName);
            SetFieldValue("postal-code", postalCode);

            var continueBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("continue")));
            js.ExecuteScript("arguments[0].click();", continueBtn);

            if (expectNavigation)
                _wait.Until(ExpectedConditions.UrlContains("checkout-step-two.html"));
        }

        // Método para hacer clic en el botón "Finish"
        public void FinishCheckout()
        {
            var js = (IJavaScriptExecutor)_driver;
            var finishBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("finish")));
            js.ExecuteScript("arguments[0].click();", finishBtn);
            Console.WriteLine($"URL después de FinishButton.Click(): {_driver.Url}");
            _wait.Until(ExpectedConditions.UrlContains("checkout-complete.html"));
        }

        //Método para obtener el mensaje de éxito después de finalizar la compra
        public string GetSuccessMessage()
        {
            return SuccessMessage.Text;
        }
    }
}