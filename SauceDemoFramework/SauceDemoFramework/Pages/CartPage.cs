using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SauceDemoFramework.Utilities;

namespace SauceDemoFramework.Pages
{
    public class CartPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Locators
        private IWebElement CheckoutButton => _driver.FindElement(By.Id("checkout"));
        private IReadOnlyCollection<IWebElement> CartItems => _driver.FindElements(By.CssSelector("[data-test='inventory-item']"));
        private IWebElement RemoveButton(string productName) => _driver.FindElement(By.Id("remove-" + productName.ToLower().Replace(" ", "-")));

        public CartPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver,
                TimeSpan.FromSeconds(ConfigManager.ExplicitWait));
        }

        public bool IsOnCartPage()
        {
            return _driver.Url.Contains("cart.html");
        }

        public void RemoveProduct(string productName)
        {
            RemoveButton(productName).Click();
        }

        public int GetItemCount()
        {
            return _driver.FindElements(By.CssSelector("[data-test='inventory-item']")).Count;
        }

        public void GoToCheckout()
        {
            CheckoutButton.Click();
        }
    }
}