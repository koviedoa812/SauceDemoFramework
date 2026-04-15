using OpenQA.Selenium;
using Reqnroll;
using SauceDemoFramework.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemoFramework.Tests.BDDTests.Features
{
    [Binding]
    public class LoginStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IWebDriver _driver;
        private LoginPage _loginPage;
        private InventoryPage _inventoryPage;

        public LoginStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _driver = _scenarioContext.Get<IWebDriver>();
            _loginPage = new LoginPage(_driver);
            _inventoryPage = new InventoryPage(_driver);
        }

        [Given(@"que el usuario está en la página de login")]
        public void GivenQueElUsuarioEstaEnLaPaginaDeLogin()
        {
            Assert.That(_driver.Url, Does.Contain("saucedemo.com"),
                "No se cargó la página de login correctamente.");
        }

        [When(@"ingresa el usuario ""(.*)"" y la contraseña ""(.*)""")]
        public void WhenIngresaElUsuarioYLaContrasena(string username, string password)
        {
            _loginPage.EnterUsername(username);
            _loginPage.EnterPassword(password);
        }

        [When(@"hace clic en el botón de login")]
        public void WhenHaceClicEnElBotonDeLogin()
        {
            _loginPage.ClickLogin();
        }

        [Then(@"debe ser redirigido a la página de inventario")]
        public void ThenDebeSerRedirigidoALaPaginaDeInventario()
        {
            string actualUrl = _driver.Url;

            Assert.That(actualUrl, Does.Contain("inventory.html"),
                "El login exitoso no redirigió a la página de inventario.");
        }

        [Then(@"debe ver el mensaje de error ""(.*)""")]
        public void ThenDebeVerElMensajeDeError(string mensajeEsperado)
        {
            string mensajeReal = _loginPage.GetErrorMessage();

            Assert.That(mensajeReal, Does.Contain(mensajeEsperado),
                $"El mensaje de error no coincide. Se obtuvo: '{mensajeReal}'");
        }
    }
}