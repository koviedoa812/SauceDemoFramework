Feature: Flujo completo de compra en SauceDemo
    Como usuario autenticado
    Quiero poder completar el proceso de compra
    Para recibir mis productos correctamente

    Scenario: Completar una compra exitosamente
        Given que el usuario inicia sesión con usuario "standard_user" y contraseña "secret_sauce"
        When agrega 3 productos al carrito
        And navega al carrito de compras
        And procede al checkout
        And completa el formulario con nombre "John" apellido "Doe" y código postal "12345"
        And confirma la compra
        Then debe ver el mensaje de éxito "Thank you for your order!"

    Scenario Outline: Completar compra con distintos datos de cliente
        Given que el usuario inicia sesión con usuario "standard_user" y contraseña "secret_sauce"
        When agrega 3 productos al carrito
        And navega al carrito de compras
        And procede al checkout
        And completa el formulario con nombre "<nombre>" apellido "<apellido>" y código postal "<cp>"
        And confirma la compra
        Then debe ver el mensaje de éxito "Thank you for your order!"

        Examples:
            | nombre  | apellido  | cp    |
            | John    | Doe       | 12345 |
            | María   | González  | 98765 |
            | Carlos  | Pérez     | 54321 |