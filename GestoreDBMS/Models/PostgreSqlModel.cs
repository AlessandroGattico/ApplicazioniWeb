using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GestoreDBMS.Models
{
    public class PostgreSqlModel
    {
        // Proprietà "host": rappresenta l'indirizzo dell'host del database PostgreSQL
        [Required]
        public string host { get; set; }

        // Proprietà "port": rappresenta la porta del database PostgreSQL
        [Required]
        public int port { get; set; }

        // Proprietà "name": rappresenta il nome del database PostgreSQL a cui connettersi
        [Required] 
        public string name { get; set; }

        // Proprietà "username": rappresenta il nome utente per l'autenticazione al database PostgreSQL
        [Required] 
        public string username { get; set; }

        // Proprietà "password": rappresenta la password per l'autenticazione al database PostgreSQL
        [Required] 
        public string password { get; set; }
    }
}
