using GestoreDBMS.Context;
using GestoreDBMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestoreDBMS.Pages
{
    public class AddSqLiteModel : PageModel
    {
        private readonly ILogger<AddSqLiteModel> _logger;

        [BindProperty]
        public SqLiteModel sqLite { get; set; } = default!;

        public AddSqLiteModel(ILogger<AddSqLiteModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
       

        public async Task<IActionResult> OnPostAsync()
        {
            SqLiteConnectionString.addConnectionString(sqLite.path);

            return RedirectToPage("./SqLiteTables");
        }
    }
}
