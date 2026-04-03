using AutomationPracticeDemo.Tests.Tests.Login.Asserts;
using NUnit.Framework;
using OpenQA.Selenium;
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
        public void AddProductTest()
        {
            inventoryPage.AddProductsToCart();

            string actualBadgeCount = inventoryPage.GetCartCount();

            // Validación (Prueba 3 del proyecto: Validar contador)
            Assert.That(actualBadgeCount, Is.EqualTo("3"), "El contador del carrito (badge) no muestra la cantidad esperada de productos.");
        }


        [Test]
        public void EliminarProductoYValidarContador()
        {
            // 1. Preparación: Agregamos 3 productos primero
            inventoryPage.AddProductsToCart(); // Agrega 3 productos, pero solo verificaremos el contador después de eliminar uno

            // Verificación intermedia (opcional pero recomendada): El contador debe ser "3"
            Assert.That(inventoryPage.GetCartCount(), Is.EqualTo("3"), "El carrito debería haber iniciado con 3 productos.");

            // 2. Acción: Eliminamos uno de los productos (la mochila)
            inventoryPage.RemoveProductFromCart();

            // 3. Validación final: El contador debe haber bajado a "2"
            string actualCount = inventoryPage.GetCartCount();

            Assert.That(actualCount, Is.EqualTo("2"), "El contador no disminuyó después de eliminar el producto.");
        }

    }
}