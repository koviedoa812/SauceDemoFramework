using AutomationPracticeDemo.Tests.Tests.Login.Asserts;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using SauceDemoFramework.Pages;
using SauceDemoFramework.Utilities;

namespace SauceDemoFramework.Tests.WebTests.Login
{
    
    public class LoginTests : BaseTest
    {

        // PRUEBA DE LOGIN CON USUARIOS VÁLIDOS

        [Test, Category("Login"), TestCaseSource(typeof(LoginDataSource), nameof(LoginDataSource.UsersIsValid))]
        public void LoginWithValidUser(string Username, string Password)
        {
            var loginPage = new LoginPage(driver);
            var inventoryPage = new InventoryPage(driver);

            //Llenado del formulario de login
            loginPage.Login(Username, Password);

            string expectedUrl = inventoryPage.GetCurrentUrl();

            Assert.That(expectedUrl, Does.Contain("inventory.html"), "El login falló: No se redirigió a la página de inventario.");

        }

        // PRUEBA DE LOGIN CON USUARIOS INVÁLIDOS

        [Test, Category("Login"), TestCaseSource(typeof(LoginDataSource), nameof(LoginDataSource.UsersNotValid))]
        public void LoginWithInValidUser(string Username, string Password)
        {
            var loginPage = new LoginPage(driver);

            //Llenado del formulario de login
            loginPage.Login(Username, Password);            

            Assert.That(loginPage.GetErrorMessage(), Does.Contain("Epic sadface: Username and password do not match any user in this service"), "El mensaje de error no se mostró correctamente para un usuario inválido.");
        }


    }






}