using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_secret_santa.Data;
using razor_secret_santa.Models;

namespace razor_secret_santa.Pages
{
    [BindProperties(SupportsGet = true)]
    public class UnsubscribeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public int userID { get; set; }

        public UnsubscribeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var userModel = _context.UserModels.SingleOrDefault(u => u.id == userID);
            userModel!.email = "null@null.null";
            _context.SaveChanges();
            return Page();
        }
    }
}
