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
        public int id { get; set; }

        public EditGiftModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            gift = _context.GiftModels.Find(id);
            if (gift == null)
                return RedirectToPage("/Control");
            return Page();
        }

        public IActionResult OnPost()
        {
            var giftModel = _context.GiftModels.SingleOrDefault(g => g.id == id)!.name = gift!.name;
            _context.SaveChanges();
            return RedirectToPage("/Control");
        }
    }
}
