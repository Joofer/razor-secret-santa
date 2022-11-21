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
            user = _context.UserModels.Where(u => u.id == id).FirstOrDefault();

            if (user == null)
                return RedirectToPage("/Control", new { state = "error", message = String.Format("Пользователь #{0} не найден.", id) });

            return Page();
        }

        public IActionResult OnPost()
        {
            var userModel = _context.UserModels.Where(u => u.id == id).FirstOrDefault();

            if (userModel == null)
                return RedirectToPage("/Control", new { state = "error", message = String.Format("Пользователь #{0} не найден.", id) });

            userModel!.name = user!.name;
            userModel!.group = user!.group;
            userModel!.email = user!.email;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return RedirectToPage("/Control", new { state = "error", message = String.Format("Ошибка во время редактирования пользователя. Подробнее: {0}", ex.Message) });
            }

            return RedirectToPage("/Control", new { state = "success" });
        }
    }
}
