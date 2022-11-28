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

        public string userID { get; set; }

        public UnsubscribeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var userModel = _context.UserModels.SingleOrDefault(u => u.id == userID);
            if (userModel == null)
                return RedirectToPage("/Error", new { message = "Пользователь #" + userID + " не найден." });

            try
            {
                userModel!.email = "null@null.null";
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return RedirectToPage("/Error", new { message = "Ошибка во время выполнения запроса. Подробнее: " + ex.Message });
            }

            return Page();
        }
    }
}
