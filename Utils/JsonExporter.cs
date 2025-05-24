using System.Text.Json;
using Mi_IA.Models;

namespace Mi_IA.Utils
{
    public class JsonExporter
    {
        public void ExportarComoJson(List<Resultado> resultados, string rutaDestino)
        {
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true // Para que el JSON sea legible
            };
            var json = JsonSerializer.Serialize(resultados, opciones);
            File.WriteAllText(rutaDestino, json);
            System.IO.File.WriteAllText("core_response.json", json);
            Console.WriteLine(json);
        }
    }
}