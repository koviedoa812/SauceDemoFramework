using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SauceDemoFramework.Tests.API.Jsonplaceholder.Dtos
{
    [TestFixture]
    public class PlaceholderApiTest
    {
        [Test]
        public async Task GetUsers_SimpleTest()
        {
            // 1. Configuración del cliente (URL de JSONPlaceholder)
            var client = new RestClient("https://jsonplaceholder.typicode.com");
            var request = new RestRequest("/users", Method.Get);

            // 2. Ejecución
            var response = await client.ExecuteAsync(request);

            // 3. Validación de estado (OK = 200)
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "El código de estado no fue OK");

            // 4. Deserialización (JSONPlaceholder devuelve una LISTA directamente)
            var apiResponse = JsonSerializer.Deserialize<List<PlaceholderUser>>(response.Content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // 5. Validaciones de datos
            Assert.That(apiResponse, Is.Not.Null, "La respuesta es nula");
            Assert.That(apiResponse, Is.Not.Empty, "La lista de usuarios está vacía");

            // Validamos el primer usuario de la lista
            Assert.That(apiResponse[0].Name, Is.Not.Null.And.Not.Empty);

            // Imprime en la consola de la prueba (Test Output)
            TestContext.WriteLine($"El primer usuario es: {apiResponse[0].Name}");
            TestContext.WriteLine($"Su email es: {apiResponse[0].Email}");
            TestContext.WriteLine($"Su ciudad es: {apiResponse[0].Address.City}");
        }


        [Test]
        public async Task CreatePost_ShouldReturnCreated()
        {
            // 1. Configuración del cliente (Usando JSONPlaceholder)
            var client = new RestClient("https://jsonplaceholder.typicode.com");
            var request = new RestRequest("/posts", Method.Post);

            // 2. Definimos el objeto a enviar (Payload)
            var newPost = new
            {
                title = "Prueba de Kenneth",
                body = "Esta es una prueba de POST exitosa",
                userId = 1
            };

            request.AddJsonBody(newPost);

            // 3. Ejecución
            var response = await client.ExecuteAsync(request);

            // 4. Validación de estado (201 = Created)
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "El post no fue creado correctamente");

            // 5. Verificación en consola
            TestContext.WriteLine($"Respuesta del servidor: {response.Content}");
            Assert.That(response.Content, Does.Contain("101"), "El servidor no devolvió el ID esperado (101)");
        }

        [Test]
        public async Task UpdateUser_PutTest()
        {
            // 1. Configuración: Apuntamos al usuario con ID 1
            var client = new RestClient("https://jsonplaceholder.typicode.com");
            var request = new RestRequest("/users/1", Method.Put);

            // 2. Definimos los datos actualizados
            var updatedData = new
            {
                name = "Kenneth Updated",
                email = "kenneth_new@test.com",
                username = "ken_qa_pro"
            };

            request.AddJsonBody(updatedData);

            // 3. Ejecución
            var response = await client.ExecuteAsync(request);

            // 4. Validación de estado (200 OK es lo estándar para PUT)
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "La actualización falló");

            // 5. Deserialización para comprobar que los datos cambiaron
            var result = JsonSerializer.Deserialize<PlaceholderUser>(response.Content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Validamos que el nombre en la respuesta sea el nuevo
            Assert.That(result.Name, Is.EqualTo("Kenneth Updated"));

            TestContext.WriteLine($"PUT Exitoso. Usuario 1 ahora se llama: {result.Name}");
            TestContext.WriteLine($"Cuerpo de respuesta: {response.Content}");
        }


        [Test]
        public async Task DeleteUser_Test()
        {
            // 1. Configuración: Queremos borrar al usuario con ID 1
            var client = new RestClient("https://jsonplaceholder.typicode.com");
            var request = new RestRequest("/users/1", Method.Delete);

            // 2. Ejecución
            var response = await client.ExecuteAsync(request);

            // 3. Validación de estado
            // Nota: Algunas APIs devuelven 204 (No Content), pero JSONPlaceholder devuelve 200.
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "El borrado falló");

            // 4. Validación del contenido (Debe estar vacío o ser un JSON vacío)
            Assert.That(response.Content, Is.EqualTo("{}").Or.Empty);

            TestContext.WriteLine("DELETE Exitoso: El servidor confirmó el borrado del usuario 1.");
        }


    }




    // --- MODELOS (Adaptados a JSONPlaceholder) ---

    public class PlaceholderUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public string City { get; set; }
    }
}