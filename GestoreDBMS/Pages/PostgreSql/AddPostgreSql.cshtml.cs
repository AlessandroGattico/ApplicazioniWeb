using GestoreDBMS.Context;
using GestoreDBMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestoreDBMS.Pages
{
    public class AddPostgreSqlModel : PageModel
    {
        private readonly ILogger<AddPostgreSqlModel> _logger;

        [BindProperty]
        public PostgreSqlModel postrgeSql { get; set; } = default!;

        public AddPostgreSqlModel(ILogger<AddPostgreSqlModel> logger)
        {
            _logger = logger;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            PostgreSqlConnectionString.addConnectionString(postrgeSql.host, postrgeSql.port, postrgeSql.name, postrgeSql.username, postrgeSql.password);
            return RedirectToPage("./PostgreSqlTables");
        }
    }
}
