using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SauceDemoFramework.Pages
{
    public class CheckoutPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public CheckoutPage(IWebDriver driver)
        {
            _driver = driver;
            // Usamos el tiempo de espera configurado en tu ConfigManager
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        // --- LOCALIZADORES (IWebElement) ---

        // Paso 1: Formulario de información
        private IWebElement FirstNameField => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("first-name")));
        private IWebElement LastNameField => _driver.FindElement(By.Id("last-name"));
        private IWebElement ZipCodeField => _driver.FindElement(By.Id("postal-code"));

        //private IWebElement CartBadge => _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("shopping_cart_badge")));

        // Localizador para el título de la cabecera
        private IWebElement PageTitle => _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("title")));

        // Asegúrate de tener este localizador para el título
        private IWebElement PageTitleFinish => _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("title")));

        // Locator para el botón Finish
        private IWebElement FinishButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("finish")));

        // Locator para el mensaje de éxito (Thank you for your order!)
        private IWebElement SuccessHeader => _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("complete-header")));

        public void ClickFinish()
        {
            FinishButton.Click();
            Console.WriteLine("Acción: Clic en el botón Finish ejecutado.");
        }

        public string GetSuccessMessage()
        {
            // Retorna el texto del encabezado final
            return SuccessHeader.Text;
        }

        public bool IsAtCheckoutStepOne()
        {
            try
            {
                // 1. Esperamos a que la URL contenga el texto esperado
                _wait.Until(d => d.Url.Contains("checkout-step-one.html"));

                // 2. Comparamos URL y Texto del Header
                // Usamos .Trim() para evitar errores por espacios invisibles
                return _driver.Url.Contains("checkout-step-one.html") &&
                       PageTitle.Text.Trim() == "Checkout: Your Information";
            }
            catch (Exception)
            {
                // Si hay un timeout o el elemento no carga, devuelve false
                return false;
            }
        }


        /// <summary>
        /// Llena los campos de nombre, apellido y código postal y continúa.
        /// </summary>
        public void FillCustomerInfo(string firstName, string lastName, string zipCode)
        {
            // 1. Limpiar y llenar Nombre
            var firstNameInput = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("first-name")));
            firstNameInput.Clear();
            firstNameInput.SendKeys(firstName);

            // 2. Limpiar y llenar Apellido
            var lastNameInput = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("last-name")));
            lastNameInput.Clear();
            lastNameInput.SendKeys(lastName);

            // 3. Limpiar y llenar Código Postal
            var zipCodeInput = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("postal-code")));
            zipCodeInput.Clear();
            zipCodeInput.SendKeys(zipCode);

            // 4. Clic en el botón Continue
            var continueBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("continue")));
            continueBtn.Click();

            Console.WriteLine($"Acción: Datos de {firstName} enviados y clic en Continue.");
        }
        public bool IsAtCheckoutStepTwo()
        {
            try
            {
                // 1. Sincronización: Esperamos a que la URL cambie (Es lo más rápido)
                _wait.Until(d => d.Url.Contains("checkout-step-two.html"));

                // 2. Validación de Contenido: Esperamos a que el título sea visible y tenga el texto
                var title = _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("title")));

                // 3. Comparación robusta (usando Trim para evitar espacios invisibles)
                return _driver.Url.Contains("checkout-step-two.html") &&
                       title.Text.Trim() == "Checkout: Overview";
            }
            catch (Exception ex)
            {
                // Log opcional para saber qué falló en el Debug
                Console.WriteLine($"DEBUG: No se pudo validar Checkout Step Two. Error: {ex.Message}");
                return false;
            }
        }


    }
}