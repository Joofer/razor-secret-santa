using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_secret_santa.Data;
using razor_secret_santa.Models;

namespace razor_secret_santa.Pages.Control
{
    [Authorize]
    [BindProperties(SupportsGet = true)]
    public class EditGiftModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GiftModel? gift { get; set; }
        public string id { get; set; }

        public EditGiftModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            gift = _context.GiftModels.Where(g => g.id == id).FirstOrDefault();

            if (gift == null)
                return RedirectToPage("/Control", new { state = "error", message = String.Format("Подарок #{0} не найден.", id) });

            return Page();
        }

        public IActionResult OnPost()
        {
            var giftModel = _context.GiftModels.Where(g => g.id == id).FirstOrDefault();

            if (giftModel == null)
                return RedirectToPage("/Control", new { state = "error", message = String.Format("Подарок #{0} не найден.", id) });

            giftModel.name = gift!.name;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return RedirectToPage("/Control", new { state = "error", message = String.Format("Ошибка во время редактирования подарка. Подробнее: {0}", ex.Message) });
            }

            return RedirectToPage("/Control", new { state = "success" });
        }
    }
}
