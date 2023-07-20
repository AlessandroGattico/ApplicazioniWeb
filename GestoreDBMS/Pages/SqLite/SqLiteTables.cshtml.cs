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
    public class SqLiteTablesModel : PageModel
    {
        private readonly SqLiteContext _context;
        private readonly ILogger<SqLiteTablesModel> _logger;

        [BindProperty]
        public Tables tables { get; set; } = new Tables();

        public SqLiteTablesModel(SqLiteContext context, ILogger<SqLiteTablesModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.Database.CanConnect())
            {
                DbCommand command = _context.Database.GetDbConnection().CreateCommand();
                _context.Database.GetDbConnection().Open();
                command.CommandText = "SELECT name FROM sqlite_schema WHERE type='table'";
                DbDataReader reader = command.ExecuteReader();

                tables = new Tables();

                while (reader.Read())
                {
                    var table = new Table
                    {
                        name = reader[0].ToString()
                    };

                    DbCommand command2 = _context.Database.GetDbConnection().CreateCommand();
                    command2.CommandText = $"PRAGMA table_info({reader[0]})";
                    DbDataReader reader2 = command2.ExecuteReader();

                    if (reader2 != null)
                    {
                        while (reader2.Read())
                        {
                            var notNull = "NULL";
                            if (reader2[3].ToString().Equals("1"))
                            {
                                notNull = "NOT NULL";
                            }

                            table.addColumn(reader2[1].ToString(), reader2[2].ToString(), notNull);

                            if (!reader2[5].ToString().Equals("0"))
                            {
                                table.addPrimaryKey(reader2[1].ToString());
                            }
                        }

                        reader2.Close();
                    }

                    command2.CommandText = $"PRAGMA foreign_key_list({reader[0]})";
                    reader2 = command2.ExecuteReader();

                    if (reader2 != null)
                    {
                        while (reader2.Read())
                        {
                            table.addForeignKey(reader2[2].ToString(), reader2[3].ToString(), reader2[4].ToString());
                        }

                        reader2.Close();
                    }

                    command2.CommandText = $"PRAGMA index_list('{reader[0]}')";
                    reader2 = command2.ExecuteReader();

                    if (reader2 != null)
                    {
                        while (reader2.Read())
                        {
                            DbCommand command3 = _context.Database.GetDbConnection().CreateCommand();
                            command3.CommandText = $"PRAGMA index_info('{reader2[1]}')";
                            DbDataReader reader3 = command3.ExecuteReader();
                            if (reader3 != null)
                            {
                                LinkedList<string> columns = new LinkedList<string>();
                                while (reader3.Read())
                                {
                                    columns.AddLast(reader3[2].ToString());
                                }

                                table.addIndex(reader2[1].ToString(), columns);
                            }
                        }

                        reader2.Close();
                    }

                    tables.addTable(table);
                }

                reader.Close();
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
            SqLiteConnectionString.removeConnectionString();
            return RedirectToPage("./../Index");
        }
    }
}
