using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_secret_santa.Data;

namespace razor_secret_santa.Pages.Control
{
    [Authorize]
    public class ClearContextModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ClearContextModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var userDetails = _context.UserDetails;

            try
            {
                foreach (var details in userDetails) _context.UserDetails.Remove(details);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return RedirectToPage("/Control", new { state = "error", message = String.Format("Ошибка во время очистки распределения. Подробнее: {0}", ex.Message) });
            }

            return RedirectToPage("/Control", new { state = "success" });
        }
    }
}
