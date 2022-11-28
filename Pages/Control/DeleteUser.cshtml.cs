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

        public string id { get; set; }

        public DeleteUserModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var user = _context.UserModels.Where(u => u.id == id).FirstOrDefault();

            if (user == null) 
                return RedirectToPage("/Control", new { state = "error", message = String.Format("Пользователь #{0} не найден.", id) });

            try
            {
                _context.UserModels.Remove(user);
                var details = _context.UserDetails.Where(d => d.userID == id).FirstOrDefault();
                if (details != null) _context.UserDetails.Remove(details);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return RedirectToPage("/Control", new { state = "error", message = String.Format("Ошибка во время удаления пользователя. Подробнее: {0}", ex.Message) });
            }

            return RedirectToPage("/Control", new { state = "success" });
        }
    }
}
