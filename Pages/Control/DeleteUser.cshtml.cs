using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_secret_santa.Data;
using System.Data;

namespace razor_secret_santa.Pages.Control
{
    [Authorize]
    [BindProperties(SupportsGet = true)]
    public class DeleteUserModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public int id { get; set; }

        public DeleteUserModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (_context.UserModels.Count() > 0)
            {
                var user = _context.UserModels.Where(u => u.id == id).First();
                if (user != null) _context.UserModels.Remove(user);
                if (_context.UserDetails.Count() > 0)
                {
                    var details = _context.UserDetails.Where(d => d.userID == id).First();
                    if (details != null) _context.UserDetails.Remove(details);
                }
                _context.SaveChanges();
            }
            return RedirectToPage("/Control");
        }
    }
}
