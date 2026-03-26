using Newtonsoft.Json;
public static class JsonHelper
{
    /// <summary>
    /// Carga y deserializa un archivo JSON en una lista de objetos del tipo especificado.
    /// </summary>
    /// <typeparam name="T">Clase destino para deserializar</typeparam>
    /// <param name="pathFile">nombre completo del archivo Json</param>
    /// <returns>Lista de objetos del tipo T</returns>
    public static List<T> LoadListFromJson<T>(string nameFile)
    {
        string pathJson = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\TestData\" + nameFile));
        if (string.IsNullOrWhiteSpace(pathJson))
            throw new ArgumentException("La ruta del archivo no puede estar vacía.");

        if (!File.Exists(pathJson))
            throw new FileNotFoundException($"No se encontró el archivo: {pathJson}");

        var contenidoJson = File.ReadAllText(pathJson);
        var lista = JsonConvert.DeserializeObject<List<T>>(contenidoJson);

        return lista ?? new List<T>();
    }

    /// <summary>
    /// Carga y deserializa un archivo JSON en un solo objeto del tipo especificado.
    /// </summary>
    /// <typeparam name="T">Clase destino para deserializar</typeparam>
    /// <param  name="pathFile"> Nombre completo del archivo Json</param>
    /// <returns>Objeto del tipo T</returns>
    public static T LoadObjectFromJson<T>(string nameFile)
    {
        string pathJson = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\TestData\" + nameFile));
        if (string.IsNullOrWhiteSpace(pathJson))
            throw new ArgumentException("La ruta del archivo no puede estar vacía.");

        if (!File.Exists(pathJson))
            throw new FileNotFoundException($"No se encontró el archivo: {pathJson}");

        var contenidoJson = File.ReadAllText(pathJson);
        var objeto = JsonConvert.DeserializeObject<T>(contenidoJson);

        if (objeto == null)
            throw new InvalidOperationException("No se pudo deserializar el contenido JSON.");

        return objeto;
    }
}