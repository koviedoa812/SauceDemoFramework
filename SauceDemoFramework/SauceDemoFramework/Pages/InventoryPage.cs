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

        //Locators
        //private IWebElement product1 => _wait.Until(d => d.FindElement(By.Id("add-to-cart-sauce-labs-backpack")));

        private IReadOnlyCollection<IWebElement> products => _driver.FindElements(By.CssSelector("[data-test^='add-to-cart']"));


        // Método para obtener la URL actual de la página
        public string GetCurrentUrl()
        {
            return _driver.Url;
        }


        //public void AddProductToCart()
        //{
        //    product1.Click();
        //}

        // Agregar producto por índice
        public void AddProductToCartByIndex(int index)
        {
            var buttons = _driver.FindElements(By.CssSelector("[data-test^='add-to-cart']"));
            buttons[index].Click();
        }

        // Agregar N productos sin repetir
        public List<string> AddRandomProductsToCart(int count)
        {
            var addedProducts = new List<string>();
            // Elegir índice aleatorio
            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                // Esperar que los botones estén disponibles
                _wait.Until(d => d.FindElements(By.CssSelector("[data-test^='add-to-cart']")).Count > 0);

                // Obtener botones disponibles frescos del DOM
                var buttons = _driver.FindElements(By.CssSelector("[data-test^='add-to-cart']"));

                if (buttons.Count == 0)
                    throw new Exception("No hay más productos disponibles.");

                
                var index = random.Next(0, buttons.Count);
                var button = buttons[index];

                // Obtener nombre del producto
                var productName = button.FindElement(By.XPath("./ancestor::div[@data-test='inventory-item']" + "//div[@data-test='inventory-item-name']")).Text;


                button.Click();

                addedProducts.Add(productName);
                TestContext.WriteLine($"Producto {i + 1} agregado: {productName}");
            }

            return addedProducts;
        }

    }
}