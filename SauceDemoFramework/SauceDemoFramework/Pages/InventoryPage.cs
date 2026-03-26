using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SauceDemoFramework.Utilities;

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

        // Locators
        private IWebElement CartIcon => _driver.FindElement(By.ClassName("shopping_cart_link"));
        private IWebElement CartBadge => _driver.FindElement(By.ClassName("shopping_cart_badge"));
        private IWebElement SortDropdown => _driver.FindElement(By.ClassName("product_sort_container"));

        //Método para verificar que estamos en la página de inventario, la consume LoginTests.cs
        public string GetCurrentUrl()
        {
            return _driver.Url;
        }

        public bool IsOnInventoryPage()
        {
            return _driver.Url.Contains("inventory");
        }
        //public string AddRandomProductToCart()
        //{
        //    // Buscar botones frescos del DOM cada vez
        //    var addButtons = _driver.FindElements(
        //        By.CssSelector("[data-test^='add-to-cart']"));

        //    if (addButtons.Count == 0)
        //        throw new Exception("No hay más productos disponibles.");

        //    var random = new Random();
        //    var selectedButton = addButtons[random.Next(0, addButtons.Count)];

        //    // Obtener nombre del producto desde el atributo data-test
        //    // "add-to-cart-sauce-labs-backpack" → buscar nombre real
        //    var productNameElement = selectedButton
        //        .FindElement(By.XPath("./ancestor::div[@data-test='inventory-item']//div[@data-test='inventory-item-name']"));

        //    var productName = productNameElement.Text;
        //    selectedButton.Click();

        //    return productName;
        //}

        //public int GetCartCount()
        //{
        //    var badges = _driver.FindElements(
        //        By.CssSelector("[data-test='shopping-cart-badge']"));
        //    return badges.Count > 0 ? int.Parse(badges[0].Text) : 0;
        //}

        public string AddRandomProductToCart()
        {
            var addButtons = _driver.FindElements(
                By.CssSelector("[data-test^='add-to-cart']"));

            if (addButtons.Count == 0)
                throw new Exception("No hay más productos disponibles.");

            var random = new Random();
            var selectedButton = addButtons[random.Next(0, addButtons.Count)];

            var productName = selectedButton
                .FindElement(By.XPath(
                    "./ancestor::div[@data-test='inventory-item']//div[@data-test='inventory-item-name']"))
                .Text;

            // Guardar count actual antes del click
            var currentCount = GetCartCount();

            selectedButton.Click();

            // Esperar que el contador se incremente en 1
            _wait.Until(d => GetCartCount() == currentCount + 1);

            return productName;
        }

        public int GetCartCount()
        {
            var badges = _driver.FindElements(
                By.CssSelector("[data-test='shopping-cart-badge']"));
            return badges.Count > 0 ? int.Parse(badges[0].Text) : 0;
        }
        

        public void RemoveProductFromCart(string productName)
        {
            var button = _driver.FindElement(
                By.XPath($"//div[text()='{productName}']/ancestor::div[@class='inventory_item']//button"));
            button.Click();
        }

        public void GoToCart()
        {
            CartIcon.Click();
        }

        public void SortBy(string option)
        {
            var select = new SelectElement(SortDropdown);
            select.SelectByText(option);
        }

        public List<string> GetProductNames()
        {
            var products = _driver.FindElements(By.ClassName("inventory_item_name"));
            return products.Select(p => p.Text).ToList();
        }

        public List<double> GetProductPrices()
        {
            var prices = _driver.FindElements(By.ClassName("inventory_item_price"));
            return prices.Select(p => double.Parse(p.Text.Replace("$", ""))).ToList();
        }
    }
}