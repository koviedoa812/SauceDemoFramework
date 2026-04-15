using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using SauceDemoFramework.Pages;

namespace SauceDemoFramework.StepDefinitions.AddCart
{
    [Binding]
    public class AddCartStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IWebDriver _driver;
        private LoginPage _loginPage;
        private InventoryPage _inventoryPage;

        public AddCartStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _driver = _scenarioContext.Get<IWebDriver>();
            _loginPage = new LoginPage(_driver);
            _inventoryPage = new InventoryPage(_driver);
        }

        [Given(@"que el usuario inicia sesión con usuario ""(.*)"" y contraseña ""(.*)""")]
        public void GivenQueElUsuarioIniciaSesion(string username, string password)
        {
            _loginPage.Login(username, password);

            Assert.That(_driver.Url, Does.Contain("inventory.html"),
                "El login no redirigió a la página de inventario.");
        }

        [When(@"agrega 3 productos al carrito")]
        public void WhenAgrega3ProductosAlCarrito()
        {
            _inventoryPage.AddBackpackToCart();
        }

        [When(@"elimina un producto del carrito")]
        public void WhenEliminaUnProductoDelCarrito()
        {
            _inventoryPage.RemoveBackpackFromCart();
        }

        [Then(@"el contador del carrito debe mostrar ""(.*)""")]
        public void ThenElContadorDebeMostrar(string cantidadEsperada)
        {
            string cantidadReal = _inventoryPage.GetCartCount();

            Assert.That(cantidadReal, Is.EqualTo(cantidadEsperada),
                $"El contador muestra '{cantidadReal}' pero se esperaba '{cantidadEsperada}'.");
        }
    }
}