using System.Data.Common;
using GestoreDBMS.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using GestoreDBMS.Models;

namespace GestoreDBMS.Pages
{
    public class SqLiteQueryModel : PageModel
    {
        private readonly SqLiteContext _context;
        private readonly ILogger<SqLiteQueryModel> _logger;

        [BindProperty]
        public string Query { get; set; } = default!;
        public Query values { get; set; } = default!;

        public SqLiteQueryModel(SqLiteContext context, ILogger<SqLiteQueryModel> logger)
        {
            _context = context;
            _logger = logger;
            values = new Query();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        //Analizza ed esegue la query
        public async Task<IActionResult> OnPostAsync()
        {
            if (_context.Database.CanConnect())
            {
                string[] querySplit = this.Query.Split(" ");
              
                DbCommand command = _context.Database.GetDbConnection().CreateCommand();
                _context.Database.GetDbConnection().Open();
                values.status = null;

                switch (querySplit[0])
                {
                    case "SELECT":
                        command.CommandText = Query;

                        try
                        {
                            //esegue la query
                            DbDataReader reader = command.ExecuteReader();

                            if (reader.HasRows)
                            {
                                //ottiene le righe
                                for (int k = 1; reader.Read(); k++)
                                {
                                    LinkedList<string> list = new LinkedList<string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        list.AddLast(reader[i].ToString());
                                    }

                                    values.addRow(k, list);
                                }

                                //nomi delle colonne 
                                reader.Close();

                                command.CommandText = $"PRAGMA table_info({querySplit[3]})";
                                reader=command.ExecuteReader();

                                LinkedList<string> list2 = new LinkedList<string>();

                                while (reader.Read())
                                {
                                    list2.AddLast(reader[1].ToString());
                                }

                                values.addFirstRow(0,list2);
                                reader.Close();
                            }
                            else
                            {
                                //nel caso non siano state trovate corrispondenze
                                values.status = "NO VALUES FOUND";
                            }
                        }
                        catch (SqliteException e)
                        { 
                            values.status = e.Message;
                        }
                        break;

                    case "INSERT":
                        command.CommandText = Query;

                        try
                        {
                            //esegue la query
                            DbDataReader reader = command.ExecuteReader();
                            if(reader!= null)
                            {
                                //numero di righe
 
                                reader.Close();
                                command.CommandText = "SELECT changes()";
                                reader=command.ExecuteReader();
                                if (reader != null)
                                {
                                    reader.Read();
                                    values.status = $"{reader[0]} row/s added successfully";
                                    reader.Close();
                                }

                                //ottiene la tabella
                                command.CommandText = "SELECT * FROM " + querySplit[2].Split("(")[0];
                                reader = command.ExecuteReader();

                                for (int k = 1; reader.Read(); k++)
                                {
                                    LinkedList<string> list = new LinkedList<string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        list.AddLast(reader[i].ToString());
                                    }

                                    values.addRow(k, list);
                                }

                                //nomi delle colonne
                                reader.Close();
                                command.CommandText = $"PRAGMA table_info({querySplit[2].Split("(")[0]})";
                                reader = command.ExecuteReader();
                                LinkedList<string> list2 = new LinkedList<string>();
                                while (reader.Read())
                                {
                                    list2.AddLast(reader[1].ToString());
                                }

                                values.addFirstRow(0, list2);
                                reader.Close();
                            }
                        }
                        catch (SqliteException e)
                        {
                            values.status = e.Message;
                        }
                        break;

                    case "UPDATE":
                        command.CommandText = Query;
                        try
                        {
                            //esegue la query
                            DbDataReader reader = command.ExecuteReader();

                            if (reader != null)
                            {
                                //numero di righe
                                reader.Close();

                                command.CommandText = "SELECT changes()";
                                reader = command.ExecuteReader();

                                if (reader != null)
                                {
                                    reader.Read();
                                    values.status = $"{reader[0]} row/s updated successfully";
                                    reader.Close();
                                }

                                //ottiene la tabella
                                command.CommandText = "SELECT * FROM " + querySplit[1];
                                reader = command.ExecuteReader();

                                for (int k = 1; reader.Read(); k++)
                                {
                                    LinkedList<string> list = new LinkedList<string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        list.AddLast(reader[i].ToString());
                                    }

                                    values.addRow(k, list);
                                }

                                //nomi delle colonne
                                reader.Close();
                                command.CommandText = $"PRAGMA table_info({querySplit[1]})";
                                reader = command.ExecuteReader();

                                LinkedList<string> list2 = new LinkedList<string>();

                                while (reader.Read())
                                {
                                    list2.AddLast(reader[1].ToString());
                                }

                                values.addFirstRow(0, list2);
                                reader.Close();
                            }
                        }
                        catch (SqliteException e)
                        {
                            values.status = e.Message;
                        }
                        break;

                    case "DELETE":
                        command.CommandText = Query;
                        try
                        {
                            //esegue la query
                            DbDataReader reader = command.ExecuteReader();

                            if (reader != null)
                            {
                                //ottiene il numero di righe
                                reader.Close();
                                command.CommandText = "SELECT changes()";
                                reader = command.ExecuteReader();

                                if (reader != null)
                                {
                                    reader.Read();
                                    values.status = $"{reader[0]} row/s deleted successfully";
                                    reader.Close();
                                }

                                //ottiene la tabella
                                command.CommandText = "SELECT * FROM " + querySplit[2];
                                reader = command.ExecuteReader();

                                for (int k = 1; reader.Read(); k++)
                                {
                                    LinkedList<string> list = new LinkedList<string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        list.AddLast(reader[i].ToString());
                                    }

                                    values.addRow(k, list);
                                }

                                //nomi delle colonne
                                reader.Close();
                                command.CommandText = $"PRAGMA table_info({querySplit[2].Split("(")[0]})";
                                reader = command.ExecuteReader();
                                LinkedList<string> list2 = new LinkedList<string>();

                                while (reader.Read())
                                {
                                    list2.AddLast(reader[1].ToString());
                                }

                                values.addFirstRow(0, list2);
                                reader.Close();
                            }
                        }
                        catch (SqliteException e)
                        {
                            values.status = e.Message;
                        }
                        break;

                    default:
                        values.status = $"ERROR: unknown statement: '{querySplit[0]}'";
                        break;
                }

                return Page();
            }
            else
            {
                return RedirectToPage("./Errors/ErrorConnection");
            }
        }
    }
}
