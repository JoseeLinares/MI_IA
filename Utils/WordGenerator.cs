using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Mi_IA.Models;

namespace Mi_IA.Utils
{
    // Interfaz opcional para inyección de dependencias
    public interface ICoreApiService
    {
        Task<string> DescargarTextoCompletoAsync(string titulo);
    }

    public class WordGenerator
    {
        private readonly ICoreApiService? _coreApiService;

        // Constructor para uso asíncrono (con API)
        public WordGenerator(ICoreApiService coreApiService)
        {
            _coreApiService = coreApiService;
        }

        // Constructor para uso simple (sin API)
        public WordGenerator() { }

        // Método asíncrono: descarga texto si es necesario y genera el Word
        public async Task GenerarDocumentoAsync(string plantillaPath, string outputPath, Resultado resultado)
        {
            if (!File.Exists(plantillaPath))
                throw new FileNotFoundException("No se encontró la plantilla Word.", plantillaPath);

            File.Copy(plantillaPath, outputPath, true);

            // Descarga el texto completo si no lo tienes y tienes el servicio
            if (_coreApiService != null && string.IsNullOrWhiteSpace(resultado.TextoCompleto))
            {
                resultado.TextoCompleto = await _coreApiService.DescargarTextoCompletoAsync(resultado.Titulo)
                    ?? "Texto completo no disponible.";
            }

            ReemplazarPlaceholdersEnWord(outputPath, resultado);
        }

        // Método síncrono: solo genera el Word con los datos ya presentes
        public void GenerarDocumento(string plantillaPath, string outputPath, Resultado resultado)
        {
            if (!File.Exists(plantillaPath))
                throw new FileNotFoundException("No se encontró la plantilla Word.", plantillaPath);

            File.Copy(plantillaPath, outputPath, true);

            ReemplazarPlaceholdersEnWord(outputPath, resultado);
        }

        // Método privado para evitar duplicación de código
        private void ReemplazarPlaceholdersEnWord(string outputPath, Resultado resultado)
        {
            using (var doc = WordprocessingDocument.Open(outputPath, true))
            {
                var body = doc.MainDocumentPart.Document.Body;

                // Lista de placeholders y sus valores
                var replacements = new Dictionary<string, string>
                {
                    { "{TITULO}", resultado.Titulo ?? "" },
                    { "{AUTOR}", resultado.Autor ?? "" },
                    { "{URL}", resultado.Url ?? "" },
                    { "{TEXTO}", resultado.TextoCompleto ?? "" }
                };

                // Recorre todos los párrafos
                foreach (var para in body.Descendants<Paragraph>())
                {
                    // Une todos los textos del párrafo
                    var texts = para.Descendants<Text>().ToList();
                    if (!texts.Any()) continue;

                    string paragraphText = string.Concat(texts.Select(t => t.Text));

                    // Reemplaza los placeholders en el texto unido
                    bool changed = false;
                    foreach (var kvp in replacements)
                    {
                        if (paragraphText.Contains(kvp.Key))
                        {
                            paragraphText = paragraphText.Replace(kvp.Key, kvp.Value);
                            changed = true;
                        }
                    }

                    // Si hubo cambios, borra los nodos Text y crea uno nuevo con el texto reemplazado
                    if (changed)
                    {
                        foreach (var t in texts) t.Remove();
                        para.AppendChild(new Run(new Text(paragraphText)));
                    }
                }

                doc.MainDocumentPart.Document.Save();
            }
        }

        // Método para guardar resumen y fuentes en Word
        public void GuardarResumenYFuentesEnWord(string plantillaPath, string outputPath, string tema, string resumenIA, IEnumerable<Articulo> articulos)
        {
            if (!File.Exists(plantillaPath))
                throw new FileNotFoundException("No se encontró la plantilla Word.", plantillaPath);

            File.Copy(plantillaPath, outputPath, true);

            using (var doc = WordprocessingDocument.Open(outputPath, true))
            {
                var body = doc.MainDocumentPart.Document.Body;
                foreach (var text in body.Descendants<Text>())
                {
                    if (text.Text.Contains("{TEMA}"))
                        text.Text = text.Text.Replace("{TEMA}", tema.Trim());
                    if (text.Text.Contains("{RESUMEN_IA}"))
                        text.Text = text.Text.Replace("{RESUMEN_IA}", resumenIA);
                    if (text.Text.Contains("{FUENTES}"))
                    {
                        var fuentes = string.Join("\n", articulos.Select(a =>
                            $"Título: {a.Titulo}\nAutores: {a.Autores}\nAño: {a.Anio}\nURL: {a.Url}\nDOI: {a.Doi}\n"));
                        text.Text = text.Text.Replace("{FUENTES}", fuentes);
                    }
                }
                doc.MainDocumentPart.Document.Save();
            }
        }
    }
}