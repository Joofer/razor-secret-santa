using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_secret_santa.Data;

namespace razor_secret_santa.Pages.Control
{
    [Authorize]
    [BindProperties(SupportsGet = true)]
    public class ClearContextModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public string? group { get; set; }

        public ClearContextModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var users = _context.UserModels.ToList();
            var userDetails = _context.UserDetails.ToList();

            try
            {
                if (group != null)
                {
					foreach (var details in userDetails)
						if (details != null && users.Where(u => u.id == details.userID).First().group == group)
							_context.UserDetails.Remove(details);
					_context.SaveChanges();
				}
                else
                {
					foreach (var details in userDetails)
						_context.UserDetails.Remove(details);
					_context.SaveChanges();
				}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return RedirectToPage("/Start", new { state = "error", message = String.Format("Ошибка во время очистки распределения. Подробнее: {0}", ex.Message) });
            }

            return RedirectToPage("/Start", new { state = "success" });
        }
    }
}
