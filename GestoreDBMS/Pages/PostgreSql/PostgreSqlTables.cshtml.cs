using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using GestoreDBMS.Context;
using GestoreDBMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GestoreDBMS.Pages
{
    public class PostgreSqlTablesModel : PageModel
    {
        private readonly PostgreSqlContext _context;
        private readonly ILogger<PostgreSqlTablesModel> _logger;

        public PostgreSqlTablesModel(PostgreSqlContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Tables tables { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.Database.CanConnect())
            {
                LinkedList<string> names = new LinkedList<string>();

                DbCommand command = _context.Database.GetDbConnection().CreateCommand();
                _context.Database.GetDbConnection().Open();
                command.CommandText = "SELECT table_name FROM information_schema.tables WHERE table_type = 'BASE TABLE' AND table_schema = 'public'";
                DbDataReader reader = command.ExecuteReader();

                tables = new Tables();

                while (reader.Read())
                {
                    names.AddLast(reader[0].ToString());
                }

                reader.Close();


                foreach (string name in names)
                {

                    command.CommandText = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME= '{name}'";
                    reader = command.ExecuteReader();

                    var table = new Table();
                    table.name = name;

                    while (reader.Read())
                    {
                        var notNull = "NULL";
                        if (reader[6].ToString().Equals("1"))
                        {
                            notNull = "NOT NULL";
                        }

                        table.addColumn(reader[3].ToString(), reader[7].ToString(), notNull);
                    }

                    reader.Close();

                    command.CommandText = $"EXEC sp_pkeys '{name}'";
                    reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            table.primaryKeysNames.AddLast(reader[3].ToString());
                        }
                        reader.Close();
                    }

                    reader.Close();

                    command.CommandText = $"EXEC sp_fkeys '{name}'";
                    reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            table.addForeignKey(reader[6].ToString(), reader[7].ToString(), reader[3].ToString());
                        }
                    }

                    reader.Close();

                    command.CommandText = $"EXEC sp_helpindex '{name}'";
                    reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            table.addIndex(reader[0].ToString(), reader[2].ToString());
                        }
                    }

                    reader.Close();
                    tables.addTable(table);
                }

                _context.Database.GetDbConnection().Close();
                return Page();
            }
            else
            {
                return RedirectToPage("./Errors/ErrorConnection");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _context.Database.CloseConnection();
            PostgreSqlConnectionString.removeConnectionString();
            return RedirectToPage("./../Index");
        }
    }
}