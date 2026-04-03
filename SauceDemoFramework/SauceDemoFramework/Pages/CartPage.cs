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
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        // Locator para el botón de checkout
        private IWebElement Checkout => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("checkout")));

        private IWebElement ContinueShoppingButton => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("continue-shopping")));

        // Locator para el título "Your Cart" (opcional pero recomendado)
        private IWebElement PageTitle => _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("title")));

        // Método robusto para validar la página
        public bool IsAtCartPage()
        {
            try
            {
                // Verificamos dos cosas: la URL y que el título diga "Your Cart"
                return _driver.Url.Contains("cart.html") && PageTitle.Text == "Your Cart";
            }
            catch (Exception)
            {
                return false;
            }
        }


        // Método para saber si estamos en la página del carrito
        public bool IsContinueShoppingDisplayed()
        {
            try
            {
                return ContinueShoppingButton.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        // Método para dar click en checkout
        public void ClickCheckout()
        {
            // 1. Esperamos a que el botón sea visible y habilitado
            var btn = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("checkout")));

            try
            {
                // 2. Intentamos el clic normal primero
                btn.Click();
            }
            catch (Exception)
            {
                // 3. Si el normal falla o no hace nada, usamos JavaScript
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("arguments[0].click();", btn);
            }

            Console.WriteLine("Acción: Clic en Checkout ejecutado (Reforzado).");
        }



    }
}