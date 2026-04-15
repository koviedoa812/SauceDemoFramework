Feature: Login en SauceDemo
    Como usuario del sistema
    Quiero poder iniciar sesión
    Para acceder a las funcionalidades de la tienda

    Scenario: Login exitoso con usuario válido
        Given que el usuario está en la página de login
        When ingresa el usuario "standard_user" y la contraseña "secret_sauce"
        And hace clic en el botón de login
        Then debe ser redirigido a la página de inventario

    Scenario: Login fallido con credenciales inválidas
        Given que el usuario está en la página de login
        When ingresa el usuario "usuario_invalido" y la contraseña "password_incorrecto"
        And hace clic en el botón de login
        Then debe ver el mensaje de error "Epic sadface: Username and password do not match any user in this service"

    Scenario: Login fallido con campos vacíos
        Given que el usuario está en la página de login
        When ingresa el usuario "" y la contraseña ""
        And hace clic en el botón de login
        Then debe ver el mensaje de error "Epic sadface: Username is required"

    Scenario: Login fallido con usuario bloqueado
        Given que el usuario está en la página de login
        When ingresa el usuario "locked_out_user" y la contraseña "secret_sauce"
        And hace clic en el botón de login
        Then debe ver el mensaje de error "Epic sadface: Sorry, this user has been locked out."