using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using SauceDemoFramework.Pages;
using SauceDemoFramework.Utilities;

namespace SauceDemoFramework.StepDefinitions.Checkout
{
    [Binding]
    public class CheckoutStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private InventoryPage _inventoryPage;
        private CartPage _cartPage;
        private CheckoutPage _checkoutPage;

        public CheckoutStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _driver = _scenarioContext.Get<IWebDriver>();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(ConfigManager.ExplicitWait));
            _inventoryPage = new InventoryPage(_driver);
            _cartPage = new CartPage(_driver);
            _checkoutPage = new CheckoutPage(_driver);
        }

        [When(@"navega al carrito de compras")]
        public void WhenNavegaAlCarrito()
        {
            _inventoryPage.GoToCart();

            Assert.That(_cartPage.GetPageTitleCart(), Is.EqualTo("Your Cart"),
                "No se pudo navegar a la página del carrito.");
        }

        //[When(@"procede al checkout")]
        //public void WhenProcedeAlCheckout()
        //{
        //    _cartPage.ClickCheckout();

        //    //_wait.Until(d => d.Url.Contains("checkout-step-one.html"));

        //    Assert.That(_checkoutPage.GetPageTitleCheckout(), Is.EqualTo("Checkout: Your Information"),
        //        "No se pudo navegar al paso 1 del checkout.");
        //}


        [When(@"procede al checkout")]
        public void WhenProcedeAlCheckout()
        {
            // Acción fuerte con JS
            var checkoutBtn = _driver.FindElement(By.Id("checkout"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", checkoutBtn);

            // Esperar a que el elemento exista/esté listo
            _wait.Until(d => _checkoutPage.GetPageTitleCheckout() == "Checkout: Your Information");

            // El Assert es tu "seguro de vida"
            Assert.That(_checkoutPage.GetPageTitleCheckout(), Is.EqualTo("Checkout: Your Information"),
                "Error: El título de la página de checkout no es el esperado.");
        }

        [When(@"completa el formulario con nombre ""(.*)"" apellido ""(.*)"" y código postal ""(.*)""")]
        public void WhenCompletaElFormulario(string nombre, string apellido, string cp)
        {
            _checkoutPage.FillInformation(nombre, apellido, cp);

            Assert.That(_driver.Url, Does.Contain("checkout-step-two.html"),
                "No se pudo navegar al resumen de la compra.");
        }

        [When(@"confirma la compra")]
        public void WhenConfirmaLaCompra()
        {
            _checkoutPage.FinishCheckout();
        }

        [Then(@"debe ver el mensaje de éxito ""(.*)""")]
        public void ThenDebeVerElMensajeDeExito(string mensajeEsperado)
        {
            string mensajeReal = _checkoutPage.GetSuccessMessage();

            Assert.That(mensajeReal, Is.EqualTo(mensajeEsperado),
                $"El mensaje final no coincide. Se obtuvo: '{mensajeReal}'");
        }
    }
}