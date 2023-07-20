using GestoreDBMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GestoreDBMS.Context
{
    public class SqLiteContext : DbContext
    {
        public SqLiteContext(DbContextOptions<SqLiteContext> options) : base(options){ }

        public DbSet<SqLiteModel>? SqLiteModel{ get; set; }
    }

    public static class SqLiteConnectionString
    {
        public static LinkedList<string> connectionStringsList = new LinkedList<string>(new[] { "empty" });

        // Metodo per aggiungere una nuova stringa di connessione all'elenco
        public static void addConnectionString(string connectionString)
        {
            connectionStringsList.AddLast($"Data Source={connectionString}");
        }

        // Metodo per rimuovere l'ultima stringa di connessione dall'elenco
        public static void removeConnectionString() 
        { 
            connectionStringsList.RemoveLast();
        }
    }
}
