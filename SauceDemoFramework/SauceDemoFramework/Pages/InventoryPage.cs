using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SauceDemoFramework.Utilities;
using SeleniumExtras.WaitHelpers;
using static NUnit.Framework.Constraints.Tolerance;

namespace SauceDemoFramework.Pages
{
    public class InventoryPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public InventoryPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigManager.ExplicitWait));
        }

        //Locators

        private IWebElement ProductOne => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("add-to-cart-sauce-labs-backpack")));
        private IWebElement ProductTwo => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("add-to-cart-sauce-labs-bike-light")));
        private IWebElement ProductThree => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("add-to-cart-sauce-labs-bolt-t-shirt")));

        // Elemento para el contador del carrito (Badge)
        private IWebElement CartBadge => _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("shopping_cart_badge")));

        // Elemento para eliminar productos del carrito
        private IWebElement RemoveBackpackButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("remove-sauce-labs-backpack")));

        // Locator para el link del carrito
        // En InventoryPage.cs
        private IWebElement CartLink => _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("shopping_cart_link")));

        public void IrAlCarrito()
        {
            // Aseguramos el clic
            CartLink.Click();

            // Log para confirmar en la consola de Visual Studio
            Console.WriteLine("Acción: Se da clic en el icono del carrito ejecutado.");
        }

        // Método para obtener la URL actual de la página
        public string GetCurrentUrl()
        {
            return _driver.Url;
        }

        // Método para agregar productos al carrito
        public void AddProductsToCart()
        {
            ProductOne.Click();
            ProductTwo.Click();
            ProductThree.Click();
        }

        // Método para obtener el texto del contador del carrito (Badge)
        public string GetCartCount()
        {
            try
            {
                return CartBadge.Text;
            }
            catch (NoSuchElementException)
            {
                return "0";
            }
        }

        // Método para eliminar un producto del carrito
        public void RemoveProductFromCart()
        {
            RemoveBackpackButton.Click();
        }
    }
}