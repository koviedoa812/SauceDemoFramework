Feature: Agregar productos al carrito en SauceDemo
    Como usuario autenticado
    Quiero poder agregar y eliminar productos del carrito
    Para gestionar mi compra antes del checkout

    Background:
        Given que el usuario inicia sesión con usuario "standard_user" y contraseña "secret_sauce"

    Scenario: Agregar un producto y validar el contador del carrito
        When agrega 3 productos al carrito
        Then el contador del carrito debe mostrar "1"

    Scenario: Eliminar un producto y validar que el contador desaparece
        When agrega 3 productos al carrito
        And elimina un producto del carrito
        Then el contador del carrito debe mostrar "0"