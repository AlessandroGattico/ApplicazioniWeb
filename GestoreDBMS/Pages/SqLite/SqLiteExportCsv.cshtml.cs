using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using GestoreDBMS.Tools;
using System.ComponentModel.DataAnnotations;
using GestoreDBMS.Context;

namespace GestoreDBMS.Pages
{
    public class SqLiteExportCsvModel : PageModel
    {

        public readonly SqLiteContext _context;
        private readonly ILogger<SqLiteExportCsvModel> _logger;
        public SqLiteExportCsvModel(SqLiteContext context, ILogger<SqLiteExportCsvModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        [Display(Name = "Percorso del file")]
        public string filePath { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        //Ottiene tutte le tabelle e le salva in formato csv 
        public async Task<IActionResult> OnPostAsync()
        {
            LinkedList<string> tabelle = getTables();

            DbCommand command = _context.Database.GetDbConnection().CreateCommand();
            _context.Database.GetDbConnection().Open();

            string csv = string.Empty;

            foreach (string s in tabelle) 
            {
                command.CommandText = $"PRAGMA table_info({s});";
                DbDataReader result = command.ExecuteReader();

                //costruisce la prima riga del file elencando le colonne della tabella
                while (result.Read()) 
                {
                    csv += result[1].ToString() + ',';
                }

                result.Close();
                csv = csv.Substring(0, csv.Length - 1);
                csv += "\r\n";

                command.CommandText = $"SELECT * FROM {s}";
                DbDataReader results = command.ExecuteReader();
                //inserisce i valori, separando ogni riga con un carattere di new line

                while (results.Read())
                {
                    for (int i = 0; i < results.FieldCount; i++)
                    {
                        csv += results[i].ToString() + ',';
                    }
                    csv = csv.Substring(0, csv.Length - 1);
                    csv += "\r\n";
                }
                results.Close();

                bool done = FileWriter.WriteCsv(filePath, s, csv); //crea o sovrascrive il file con i dati ottenuti in formato csv

                if (!done)
                {
                    return RedirectToPage("./Errors/ErrorFile");
                }
                csv = string.Empty;
            }

            return RedirectToPage("./TableInfoLite");
        }

        /**
         * Ottiene tutte le tabelle presenti nel database
         * **/
        private LinkedList<string> getTables()
        {
            LinkedList<string> tabelle = new LinkedList<string>();
            DbCommand command = _context.Database.GetDbConnection().CreateCommand();
            _context.Database.GetDbConnection().Open();
            command.CommandText = "SELECT name FROM sqlite_schema WHERE type='table'";

            DbDataReader res = command.ExecuteReader();

            while (res.Read())
            {
                //aggiunge alla lista tutte le tabelle tranne la tabella sqlite_sequence
                if (!res[0].ToString().Equals("sqlite_sequence"))
                {
                    tabelle.AddLast(res[0].ToString());
                }
            }

            res.Close();
            command.Dispose();
            return tabelle;
        }

    }
}
