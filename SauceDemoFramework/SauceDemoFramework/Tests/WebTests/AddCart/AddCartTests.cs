using AutomationPracticeDemo.Tests.Tests.Login.Asserts;
using NUnit.Framework;
using SauceDemoFramework.Pages;
using SauceDemoFramework.Utilities;

namespace SauceDemoFramework.Tests.WebTests.AddCart
{
    public class AddCartTests : BaseTest
    {
        private LoginPage loginPage = null!;
        private InventoryPage inventoryPage = null!;

        [SetUp]
        public void SetupPage()
        {
            // 1. Inicializar páginas
            loginPage = new LoginPage(driver);
            inventoryPage = new InventoryPage(driver);

            // 2. Leer usuario válido desde DataUser.json
            var users = JsonHelper.LoadListFromJson<LoginData>("DataUser.json");
            var validUser = users.First(u => u.IsValid);

            // 3. Login automático antes de cada test
            loginPage.Login(validUser.Username, validUser.Password);
        }

        [Test]
        public void AddProductToCartAndValidateCounter()
        {
            var productsAdded = new List<string>();

            for (int i = 0; i < 3; i++)
            {
                var productName = inventoryPage.AddRandomProductToCart();
                productsAdded.Add(productName);
                TestContext.WriteLine($"Producto {i + 1} agregado: {productName}");
            }

            Assert.That(inventoryPage.GetCartCount(), Is.EqualTo(3));
        }
    }
}