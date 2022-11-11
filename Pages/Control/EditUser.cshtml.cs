using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor_secret_santa.Data;
using razor_secret_santa.Models;

namespace razor_secret_santa.Pages.Control
{
    [Authorize]
    [BindProperties(SupportsGet = true)]
    public class EditUserModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public UserModel? user { get; set; }
        public int id { get; set; }

        public EditUserModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            user = _context.UserModels.Find(id);
            if (user == null)
                return RedirectToPage("/Control");
            return Page();
        }

        public IActionResult OnPost()
        {
            var userModel = _context.UserModels.SingleOrDefault(u => u.id == id);
            userModel!.name = user!.name;
            userModel!.group = user!.group;
            userModel!.email = user!.email;
            _context.SaveChanges();
            return RedirectToPage("/Control");
        }
    }
}
