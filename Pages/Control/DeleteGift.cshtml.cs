using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor_secret_santa.Data;
using System.Data;

namespace razor_secret_santa.Pages.Control
{
    [Authorize]
    [BindProperties(SupportsGet = true)]
    public class DeleteGiftModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public string id { get; set; }

        public DeleteGiftModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var gift = _context.GiftModels.Where(g => g.id == id).FirstOrDefault();

            if (gift == null) 
                return RedirectToPage("/Control", new { message = String.Format("Подарок #{0} не найден.", id) });

            try
            {
                _context.GiftModels.Remove(gift);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return RedirectToPage("/Control", new { state = "error", message = String.Format("Ошибка во время удаления подарка. Подробнее: {0}", ex.Message) });
            }

            return RedirectToPage("/Control", new { state = "success" });
        }
    }
}
