using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SauceDemoFramework.Utilities;
using SeleniumExtras.WaitHelpers;
using System;

namespace SauceDemoFramework.Pages
{
    public class CartPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;


        public CartPage(IWebDriver driver)
        {
            _driver = driver;
            // Usamos el tiempo configurado en el JSON
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigManager.ExplicitWait));
        }

        private IWebElement CheckoutButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("checkout")));

        private IWebElement TitleLabelCart => _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("title")));

        // Método para obtener el título de la página del carrito
        public string GetPageTitleCart()
        {
            return TitleLabelCart.Text;
        }


        // Método para hacer clic en el botón "Checkout"
        public void ClickCheckout()
        {
            var checkoutButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("checkout"))); //Ok

            CheckoutButton.Click();
        }

    }
}