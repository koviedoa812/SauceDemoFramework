using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SauceDemoFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemoFramework.Pages
{
    public class CartPage
    {

        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public CartPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigManager.ExplicitWait));
        }



    }
}
