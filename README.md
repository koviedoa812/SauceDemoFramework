# Proyecto de Automatización SauceDemo

Este proyecto utiliza **C#**, **Selenium WebDriver** y **SpecFlow** para automatizar el flujo completo de compra de la tienda virtual [SauceDemo](https://www.saucedemo.com/). Se enfoca en la implementación de buenas prácticas de QA y la estabilidad de las pruebas.

## Requisitos Previos

- **Visual Studio 2022** con la carga de trabajo ".NET Desktop Development".
- **Extensión de SpecFlow** para Visual Studio 2022.
- **SDK de .NET 6.0** o superior.
- Navegador **Google Chrome** actualizado.

## Instalación y Configuración

1. **Clonar el repositorio:**

   ```bash
   git clone [URL_DE_TU_REPOSITORIO]

   ```

2. Abrir la solución: Abrir el archivo .sln en Visual Studio.

3. Restaurar los paquetes NuGet:

Clic derecho en la Solución > Restaurar paquetes NuGet.

4. Compilar el proyecto:

Menú Compilar > Recompilar solución (Rebuild).

Ejecución de Pruebas
Abrir el Explorador de Pruebas (Prueba > Explorador de Pruebas).

Hacer clic en el botón Ejecutar todas las pruebas.

También puedes ejecutar desde consola con:
Bash
dotnet test

Estructura del Proyecto
El proyecto implementa el patrón de diseño Page Object Model (POM) y una arquitectura escalable:

Features/: Escenarios de prueba definidos en lenguaje Gherkin.

StepDefinitions/: Lógica en C# que conecta los pasos de Gherkin con las acciones del navegador.

Pages/: Clases que representan las páginas del sitio, encapsulando selectores (locators) y métodos de interacción.

Hooks/: Gestión del ciclo de vida de las pruebas (inicialización y cierre del WebDriver mediante [BeforeScenario] y [AfterScenario]).

Notas de Estabilidad y Sincronización
Para garantizar que las pruebas sean robustas y evitar errores de tipo WebDriverTimeoutException, se implementaron las siguientes estrategias:

Esperas Explícitas: Uso de WebDriverWait para aguardar condiciones específicas (como que un elemento sea clickeable) antes de interactuar.

JavaScript Click: En pasos críticos del flujo de Checkout, se utiliza la ejecución de scripts de JavaScript para forzar la interacción, evitando bloqueos por carga asíncrona de elementos en el DOM.

Validaciones Post-Acción: Cada transición importante entre páginas es validada mediante aserciones (Assert) para confirmar que el estado de la aplicación es el esperado antes de continuar.
