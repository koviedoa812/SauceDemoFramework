using AutomationPracticeDemo.Tests.Tests.Login.Asserts;
using NUnit.Framework;
using OpenQA.Selenium;
using SauceDemoFramework.Pages;
using SauceDemoFramework.Tests.WebTests.Checkout.Asserts;
using SauceDemoFramework.Utilities;
using System.Linq;
using System.Text.Json;

namespace SauceDemoFramework.Tests.WebTests.Checkout
{
    [TestFixture]
    public class CheckoutTests : BaseTest
    {
        private InventoryPage _inventoryPage;
        private CheckoutPage _checkoutPage;
        private LoginPage _loginPage;
        private CartPage _cartPage;

        // Carga los casos de prueba desde el JSON
        private static IEnumerable<CheckoutTestData> GetCheckoutTestCases()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "checkoutTestData.json");
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<CheckoutTestData>>(json)!;
        }

        [SetUp]
        public void SetupCheckout()
        {
            _loginPage = new LoginPage(driver);
            _inventoryPage = new InventoryPage(driver);
            _checkoutPage = new CheckoutPage(driver);
            _cartPage = new CartPage(driver);
        }

        [Test, Category("Checkout")]
        [TestCaseSource(nameof(GetCheckoutTestCases))]
        public void FullCheckout_ShouldBeSuccessful(CheckoutTestData data)
        {
            if (!data.IsValid) Assert.Ignore("Caso inválido, ignorado en este test.");

            TestContext.WriteLine($"Ejecutando caso: {data.TestCase}");

            _loginPage.Login(data.Username, data.Password);
            _inventoryPage.AddBackpackToCart();
            _inventoryPage.GoToCart();
            _cartPage.ClickCheckout();
            _checkoutPage.FillInformation(data.FirstName, data.LastName, data.PostalCode, expectNavigation: true);
            _checkoutPage.FinishCheckout();

            string successMessage = _checkoutPage.GetSuccessMessage();
            Assert.That(successMessage, Is.EqualTo("Thank you for your order!"),
                $"Error en caso '{data.TestCase}': El mensaje de éxito no es el esperado.");
        }

        [Test, Category("Checkout")]
        [TestCaseSource(nameof(GetCheckoutTestCases))]
        public void Checkout_WithIncompleteData_ShouldShowError(CheckoutTestData data)
        {
            if (data.IsValid) Assert.Ignore("Caso válido, ignorado en este test.");

            TestContext.WriteLine($"Ejecutando caso: {data.TestCase}");

            _loginPage.Login(data.Username, data.Password);
            _inventoryPage.AddBackpackToCart();
            _inventoryPage.GoToCart();
            _cartPage.ClickCheckout();
            _checkoutPage.FillInformation(data.FirstName, data.LastName, data.PostalCode, expectNavigation: false);

            string errorMessage = _checkoutPage.GetErrorMessage();
            Assert.That(errorMessage, Is.EqualTo(data.ExpectedError),
                $"Error en caso '{data.TestCase}': El mensaje de error no es el esperado.");
        }





    }
}