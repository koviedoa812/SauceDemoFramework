using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SauceDemoFramework.Utilities;
using SeleniumExtras.WaitHelpers;
using System.Globalization;
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
            // Usamos el tiempo configurado en el JSON
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigManager.ExplicitWait));
        }

        private IWebElement TitleLabel => _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("title")));

        private IWebElement AddFirstProductButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("add-to-cart-sauce-labs-backpack")));

        private IWebElement CartBadge => _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("shopping_cart_badge")));

        private IWebElement RemoveBackpackButton => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("remove-sauce-labs-backpack")));

        private IWebElement ShoppingCartButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("shopping_cart_link")));

        private IWebElement SortDropdown => _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("product_sort_container")));

        // Seleccionar opción del dropdown de ordenamiento
        public void SortBy(string sortValue)
        {
            var dropdown = new SelectElement(SortDropdown);
            dropdown.SelectByValue(sortValue);
        }

        // Obtener lista de nombres de productos en el orden actual
        public List<string> GetProductNames()
        {
            var items = _driver.FindElements(By.ClassName("inventory_item_name"));
            return items.Select(i => i.Text).ToList();
        }

        // Método para obtener el título de la página
        public string GetPageTitle()
        {
            return TitleLabel.Text;
        }

        // Método para agregar el primer producto (Backpack) al carrito
        public void AddBackpackToCart()
        {
            AddFirstProductButton.Click();
        }

        // Método para obtener el número que aparece en el carrito
        public string GetCartCount()
        {
            var badges = _driver.FindElements(By.ClassName("shopping_cart_badge"));
            return badges.Count > 0 ? badges[0].Text : "0";
        }

        // Método para remover el producto del carrito
        public void RemoveBackpackFromCart()
        {
            RemoveBackpackButton.Click();
        }



        // Método para hacer clic en el ícono del carrito y navegar a la página de checkout
        public void GoToCart()
        {
            ShoppingCartButton.Click();
        }



    }
}