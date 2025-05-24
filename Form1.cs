using System.Data.SqlClient;
using DocumentFormat.OpenXml.Packaging;
using Mi_IA.Services;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.Windows.Forms;

namespace Mi_IA
{
    public partial class Form1 : Form
    {
        private readonly GroqService _groqService = new();
        private string _contenidoGeneral = "";
        private string _resumenIA = "";
        private string _fuentes = "";

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            string tema = txtTema.Text.Trim();
            if (string.IsNullOrWhiteSpace(tema))
            {
                MessageBox.Show("Por favor, ingresa un tema.");
                return;
            }

            btnBuscar.Enabled = false;

            string prompt = $"Investiga sobre \"{tema}\". Dame un contenido de alta calidad y completo y al final, entrégame una lista de fuentes (título, autor, año) de donde sacaste la información, en texto plano y separadas por saltos de línea.";
            string respuesta = await _groqService.GenerarResumenAsync(prompt);

            int idxFuentes = respuesta.IndexOf("Fuentes:", StringComparison.OrdinalIgnoreCase);
            if (idxFuentes >= 0)
            {
                _contenidoGeneral = respuesta.Substring(0, idxFuentes).Trim();
                _fuentes = respuesta.Substring(idxFuentes).Trim();
            }
            else
            {
                _contenidoGeneral = respuesta;
                _fuentes = "(No se detectaron fuentes)";
            }

            rtxContenidoGeneral.Text = _contenidoGeneral;
            rtxResumen.Clear();
            _resumenIA = "";

            btnBuscar.Enabled = true;
        }

        private async void btnResumen_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_contenidoGeneral))
            {
                MessageBox.Show("Primero realiza una búsqueda.");
                return;
            }

            btnResumen.Enabled = false;
            string prompt = $"Resume de manera profesional el siguiente contenido no pasandote por alto los mejores detalles:\n\n{_contenidoGeneral}";
            _resumenIA = await _groqService.GenerarResumenAsync(prompt);
            rtxResumen.Text = _resumenIA;
            btnResumen.Enabled = true;
        }

        private void btnGuardarWord_Click(object sender, EventArgs e)
        {
            GuardarEnSqlYWordOPpt("word");
        }

        private void btnGenerarPowerPoint_Click(object sender, EventArgs e)
        {
            GuardarEnSqlYWordOPpt("ppt");
        }

        private void GuardarEnSqlYWordOPpt(string tipo)
        {
            string tema = txtTema.Text.Trim();
            if (string.IsNullOrWhiteSpace(tema) || string.IsNullOrWhiteSpace(_contenidoGeneral))
            {
                MessageBox.Show("Primero realiza una búsqueda y genera el resumen.");
                return;
            }

            string connStr = "Server=DESKTOP-K63CMO6\\SQLEXPRESS;Database=Mi IA;Trusted_Connection=True;";
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Investigaciones (Tema, Contenido, ResumenIA, Fuentes, Fecha) " +
                    "VALUES (@tema, @contenido, @resumenIA, @fuentes, @fecha)", conn);
                cmd.Parameters.AddWithValue("@tema", tema);
                cmd.Parameters.AddWithValue("@contenido", _contenidoGeneral);
                cmd.Parameters.AddWithValue("@resumenIA", _resumenIA);
                cmd.Parameters.AddWithValue("@fuentes", _fuentes);
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                cmd.ExecuteNonQuery();
            }

            string carpetaWord = @"C:\Users\USUARIO\Desktop\Tercer semestre\Progra 1\Mi IA\ARCHIVOWORD\";
            string carpetaPpt = @"C:\Users\USUARIO\Desktop\Tercer semestre\Progra 1\Mi IA\ARCHIVOPOWERPOINT\";
            string nombreArchivo = $"{tema}_{DateTime.Now:yyyyMMdd_HHmmss}";

            if (tipo == "word")
            {
                string plantillaPath = System.IO.Path.Combine(carpetaWord, "Plantilla.docx");
                string outputPath = System.IO.Path.Combine(carpetaWord, $"{nombreArchivo}.docx");
                File.Copy(plantillaPath, outputPath, true);
                using (var doc = WordprocessingDocument.Open(outputPath, true))
                {
                    var body = doc.MainDocumentPart.Document.Body;
                    foreach (var text in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
                    {
                        if (text.Text.Contains("{TEMA}"))
                            text.Text = text.Text.Replace("{TEMA}", tema);
                        if (text.Text.Contains("{CONTENIDO}"))
                            text.Text = text.Text.Replace("{CONTENIDO}", _contenidoGeneral);
                        if (text.Text.Contains("{RESUMEN_IA}"))
                            text.Text = text.Text.Replace("{RESUMEN_IA}", _resumenIA);
                        if (text.Text.Contains("{FUENTES}"))
                            text.Text = text.Text.Replace("{FUENTES}", _fuentes);
                    }
                    doc.MainDocumentPart.Document.Save();
                }
                MessageBox.Show("Documento Word generado y guardado en SQL.");
            }
            else if (tipo == "ppt")
            {
                string pptPath = System.IO.Path.Combine(carpetaPpt, $"{nombreArchivo}.pptx");
                GenerarPowerPointSimple(pptPath, tema, _contenidoGeneral, _resumenIA, _fuentes);
                MessageBox.Show("Presentación PowerPoint generada y datos guardados en SQL.");
            }
        }



        private void GenerarPowerPointSimple(string ruta, string tema, string contenido, string resumen, string fuentes)
        {
            var app = new PowerPoint.Application();
            PowerPoint.Presentation pres = null;
            try
            {
                pres = app.Presentations.Add(MsoTriState.msoFalse);

                // Slide 1: Título
                var slide1 = pres.Slides.Add(1, PowerPoint.PpSlideLayout.ppLayoutTitle);
                slide1.Shapes[1].TextFrame.TextRange.Text = tema;
                slide1.Shapes[2].TextFrame.TextRange.Text = "Investigación generada por IA";

                // Slide 2: Contenido General
                var slide2 = pres.Slides.Add(2, PowerPoint.PpSlideLayout.ppLayoutText);
                slide2.Shapes[1].TextFrame.TextRange.Text = "Contenido General";
                slide2.Shapes[2].TextFrame.TextRange.Text = contenido;

                // Slide 3: Resumen IA
                var slide3 = pres.Slides.Add(3, PowerPoint.PpSlideLayout.ppLayoutText);
                slide3.Shapes[1].TextFrame.TextRange.Text = "Resumen IA";
                slide3.Shapes[2].TextFrame.TextRange.Text = resumen;

                // Slide 4: Fuentes
                var slide4 = pres.Slides.Add(4, PowerPoint.PpSlideLayout.ppLayoutText);
                slide4.Shapes[1].TextFrame.TextRange.Text = "Fuentes";
                slide4.Shapes[2].TextFrame.TextRange.Text = fuentes;

                pres.SaveAs(ruta, PowerPoint.PpSaveAsFileType.ppSaveAsDefault, MsoTriState.msoFalse);
            }
            finally
            {
                if (pres != null) pres.Close();
                if (app != null) app.Quit();
            }
        }

    }
}