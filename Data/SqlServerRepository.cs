using System.Data.SqlClient;
using Mi_IA.Models;

namespace Mi_IA.Data
{
    public class SqlServerRepository
    {
        private readonly string _connectionString = "Server=DESKTOP-K63CMO6\\SQLEXPRESS;Database=Mi IA;Trusted_Connection=True;";

        public int GuardarBusqueda(string tema, DateTime fecha)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand("INSERT INTO Busquedas (Tema, Fecha) OUTPUT INSERTED.Id VALUES (@tema, @fecha)", conn);
            cmd.Parameters.AddWithValue("@tema", tema);
            cmd.Parameters.AddWithValue("@fecha", fecha);
            return (int)cmd.ExecuteScalar();
        }

        public void GuardarResultados(List<Resultado> resultados, int busquedaId)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            foreach (var r in resultados)
            {
                var cmd = new SqlCommand("INSERT INTO Resultados (BusquedaId, Titulo, Autor, Url, TextoCompleto) VALUES (@busquedaId, @titulo, @autor, @url, @textoCompleto)", conn);
                cmd.Parameters.AddWithValue("@busquedaId", busquedaId);
                cmd.Parameters.AddWithValue("@titulo", r.Titulo);
                cmd.Parameters.AddWithValue("@autor", r.Autor);
                cmd.Parameters.AddWithValue("@url", r.Url);
                cmd.Parameters.AddWithValue("@textoCompleto", r.TextoCompleto ?? "");
                cmd.ExecuteNonQuery();
            }
        }

    }
}