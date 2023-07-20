using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using GestoreDBMS.Tools;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Data.Common;
using System.ComponentModel.DataAnnotations;
using GestoreDBMS.Context;

namespace GestoreDBMS.Pages
{
    public class SqLiteImportCsvModel : PageModel
    {
        public readonly SqLiteContext _context;
        private readonly ILogger<SqLiteImportCsvModel> _logger;
        public SqLiteImportCsvModel(SqLiteContext context, ILogger<SqLiteImportCsvModel> logger)
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

        //Ottiene il contenuto del file ed esegue un insert

        public async Task<IActionResult> OnPostAsync()
        {
            string[] path = filePath.Split("/");
            string tabella = path[path.Length - 1].Remove(path[path.Length - 1].Length - 4);
            string[]? csv = FileReader.ReadCsv(filePath);
            if (csv == null)
            {
                return RedirectToPage("./Errors/ErrorFile");
            }

            DbCommand command = _context.Database.GetDbConnection().CreateCommand();
            _context.Database.GetDbConnection().Open();
            string colonne = csv[0]; //contiene le colonne

            foreach (string line in csv)
            {
                if (!line.Equals(colonne)) //ottiene i valori da inserire nel database, scartando la prima riga
                {
                    LinkedList<string> listValue = new LinkedList<string>();
                    string[] values = line.Split(',');

                    foreach (string value in values)
                    {
                        if (value == "")
                        {
                            listValue.AddLast("NULL");
                        }
                        else if (!Regex.IsMatch(value, @"^\d+$")) //se il dato non è un numero
                        { 
                            listValue.AddLast($"\"{value}\""); 
                        }
                        else
                        {
                            listValue.AddLast(value);
                        }
                    }

                    //costruisce una stringa con tutti i valori da inserire separati da una virgola
                    string v = "";

                    foreach (string value in listValue)
                    {
                        v = v + value + ",";
                    }
                    v = v.Remove(v.Length - 1);

                    //esegue la query di insert
                    string sql = $"INSERT INTO {tabella} ({colonne}) VALUES({v});";
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToPage("./TableInfoLite");
        }

    }
}
