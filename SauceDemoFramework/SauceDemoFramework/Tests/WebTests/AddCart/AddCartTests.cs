using AutomationPracticeDemo.Tests.Tests.Login.Asserts;
using NUnit.Framework;
using SauceDemoFramework.Pages;
using SauceDemoFramework.Tests.WebTests.AddCart.Asserts;
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

            CleanCart();
        }

        //[Test]

        //public void AddProductToCartTest()
        //{
        //    // 4. Agregar producto al carrito
        //    inventoryPage.AddProductToCart();
        //    // 5. Verificar que el producto se agregó correctamente
        //    string currentUrl = inventoryPage.GetCurrentUrl();

        //    Assert.IsTrue(currentUrl.Contains("inventory"), "La URL no contiene 'inventory' después de agregar al carrito.");
        //}

        [Test]
        public void AddProductToCartAndValidateCounter()
        {
            // 1. Agregar 3 productos aleatorios
            var addedProducts = inventoryPage.AddRandomProductsToCart(3);

            Assert.That(addedProducts.Count, Is.EqualTo(3), "No se agregaron 3 productos al carrito.");



        }



    }
}