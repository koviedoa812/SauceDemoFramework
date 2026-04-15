using NUnit.Framework;
using OpenQA.Selenium;
using SauceDemoFramework.Pages;
using SauceDemoFramework.Tests.WebTests.Inventory.Asserts;
using SauceDemoFramework.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SauceDemoFramework.Tests.WebTests.Inventory
{
    [TestFixture]
    public class InventoryTests : BaseTest
    {

        private LoginPage _loginPage;
        private InventoryPage _inventoryPage;

        [SetUp]
        public void SetUpInventory()
        {
            string user = ConfigManager.ValidUsername;
            TestContext.WriteLine($"Iniciando prueba con usuario: {ConfigManager.ValidUsername}");

            _loginPage = new LoginPage(driver);
            _inventoryPage = new InventoryPage(driver);
            _loginPage.Login(user, ConfigManager.ValidPassword);
        }

        private static IEnumerable<SortTestData> GetSortTestCases()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "sortTestData.json");
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<SortTestData>>(json)!;
        }

        [Test, Category("Inventory")]
        [TestCaseSource(nameof(GetSortTestCases))]
        public void SortProducts_ByName_ShouldBeCorrectOrder(SortTestData data)
        {
            TestContext.WriteLine($"Ejecutando caso: {data.TestCase}");

            // 1. Ordenar productos
            _inventoryPage.SortBy(data.SortOption);

            // 2. Obtener nombres actuales en pantalla
            List<string> actualNames = _inventoryPage.GetProductNames();

            // 3. Generar lista esperada ordenando localmente
            List<string> expectedNames = data.IsAscending
                ? actualNames.OrderBy(n => n).ToList()
                : actualNames.OrderByDescending(n => n).ToList();

            // 4. Validar
            Assert.That(actualNames, Is.EqualTo(expectedNames),
                $"Error en caso '{data.TestCase}': El orden de productos no es correcto.");
        }

        [Test, Category("Inventory")]
        public void AddProductToCart_ShouldDisplayBadge()
        {
            //Acción: Agregar un producto (ej: el primero de la lista)
            _inventoryPage.AddBackpackToCart();

            // 4. Validación: Verificar que el badge del carrito diga "1"
            string cartCount = _inventoryPage.GetCartCount();
            Assert.That(cartCount, Is.EqualTo("1"), "El contador del carrito no aumentó a 1.");
        }

        [Test]
        public void RemoveProductFromCart_ShouldHideBadge()
        {
            // 1. Preparar: Agregar el producto
            _inventoryPage.AddBackpackToCart();

            // 2. Actuar: Eliminar el producto (el método ya tiene su propio Wait)
            _inventoryPage.RemoveBackpackFromCart();

            // 3. Verificar: El contador debe ser "0"
            string count = _inventoryPage.GetCartCount();

            Assert.That(count, Is.EqualTo("0"), $"Error: Se esperaba 0 productos en el carrito pero hay {count}");
        }



    }
}