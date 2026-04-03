using AutomationPracticeDemo.Tests.Tests.Login.Asserts;
using NUnit.Framework;
using OpenQA.Selenium;
using SauceDemoFramework.Pages;
using SauceDemoFramework.Utilities;
using System.Linq;

namespace SauceDemoFramework.Tests.WebTests.Checkout
{
    [TestFixture]
    public class CheckoutTests : BaseTest
    {
        private LoginPage _loginPage = null!;
        private InventoryPage _inventoryPage = null!;
        private CheckoutPage _checkoutPage = null!;
        private CartPage _cartPage = null!;

        [SetUp]
        public void SetupCheckout()
        {
            // Inicializamos las páginas necesarias
            _loginPage = new LoginPage(driver);
            _inventoryPage = new InventoryPage(driver);
            _checkoutPage = new CheckoutPage(driver);
            _cartPage = new CartPage(driver);
        }

        [Test]
        public void CompletarCheckoutExitoso_Test()
        {
            // 1. ARRANGUE: Datos desde JSON
            var usuarioValido = LoginData.LoadList("DataUser.json").First(u => u.IsValid);

            // 2. ACCIÓN: Flujo de navegación
            _loginPage.Login(usuarioValido.Username, usuarioValido.Password);
            _inventoryPage.AddProductsToCart();

            // Paso C: Ir al carrito
            _inventoryPage.IrAlCarrito();

            // VALIDACIÓN: ¿Realmente llegamos al carrito?
            Assert.That(_cartPage.IsAtCartPage(), Is.True, "ERROR: No se pudo cargar la página del carrito.");

            // 1. Haces el clic reforzado
            _cartPage.ClickCheckout();

            // 2. Esperas a que la URL cambie (Esto es clave antes del Assert)
            _wait.Until(d => d.Url.Contains("checkout-step-one.html"));

            // 3. El Assert ahora sí encontrará la URL correcta
            Assert.That(_checkoutPage.IsAtCheckoutStepOne(), Is.True, "ERROR: La navegación falló.");


            // Paso E: Completar formulario (Este método llena datos y hace CLICK)
            _checkoutPage.FillCustomerInfo(usuarioValido.FirstName, usuarioValido.LastName, usuarioValido.PostalCode);

            // VALIDACIÓN: El método IsAt... se encarga de esperar y confirmar.
            Assert.That(_checkoutPage.IsAtCheckoutStepTwo(), Is.True, "ERROR: No se pudo navegar a la pantalla de revisión final (Overview).");


            // Paso F: Finalizar la compra
            _checkoutPage.ClickFinish();

            // Sincronización: Esperar a que la URL cambie a la página de éxito
            _wait.Until(d => d.Url.Contains("checkout-complete.html"));

            // 3. VALIDACIÓN FINAL (ASSERT)
            string mensajeReal = _checkoutPage.GetSuccessMessage();
            string mensajeEsperado = "Thank you for your order!";

            Assert.That(mensajeReal, Is.EqualTo(mensajeEsperado), $"ERROR: El mensaje final no es el esperado. Se obtuvo: {mensajeReal}");

            Console.WriteLine("¡TEST COMPLETADO CON ÉXITO!");
        }
    }
}