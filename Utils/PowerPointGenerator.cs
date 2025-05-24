using System;
using System.Runtime.InteropServices;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;

namespace Mi_IA.Utils
{
    public class PowerPointGenerator
    {
        /// <summary>
        /// Genera una presentación de PowerPoint con 4 diapositivas básicas.
        /// </summary>
        /// <param name="ruta">Ruta donde se guardará el archivo .pptx</param>
        /// <param name="tema">Título principal</param>
        /// <param name="contenido">Contenido general</param>
        /// <param name="resumen">Resumen IA</param>
        /// <param name="fuentes">Fuentes</param>
        public void Generar(string ruta, string tema, string contenido, string resumen, string fuentes)
        {
            PowerPoint.Application app = null;
            PowerPoint.Presentation pres = null;
            try
            {
                app = new PowerPoint.Application();
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
                if (pres != null)
                {
                    pres.Close();
                    Marshal.ReleaseComObject(pres);
                }
                if (app != null)
                {
                    app.Quit();
                    Marshal.ReleaseComObject(app);
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}