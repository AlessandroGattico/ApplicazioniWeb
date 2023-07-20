using GestoreDBMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace GestoreDBMS.Context
{
    public class PostgreSqlContext : DbContext
    {
        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options) { }
    }

    public static class PostgreSqlConnectionString
    {
        public static LinkedList<string> connectionStringsList = new LinkedList<string>();

        public static void addConnectionString(string host, int port, string name, string username, string password)
        {
            connectionStringsList.Append(buildConnectionString(host, port, name, username, password));
        }

        // Metodo per aggiungere una nuova stringa di connessione all'elenco
        private static string buildConnectionString(string host, int port, string name, string username, string password)
        {
            // Costruisci la stringa di connessione utilizzando NpgsqlConnectionStringBuilder
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder
            {
                Host = host,
                Port = port,
                Database = name,
                Username = username,
                Password = password,
            };

            // Restituisci la stringa di connessione risultante.
            return builder.ConnectionString;
        }

        // Metodo per rimuovere l'ultima stringa di connessione dall'elenco
        public static void removeConnectionString()
        {
            connectionStringsList.RemoveLast();
        }
    }
}
