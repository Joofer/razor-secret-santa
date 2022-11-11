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

        public int id { get; set; }

        public DeleteGiftModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (_context.GiftModels.Count() > 0)
            {
                var gift = _context.GiftModels.Where(g => g.id == id).First();
                if (gift != null) _context.GiftModels.Remove(gift);
                _context.SaveChanges();
            }
            return RedirectToPage("/Control");
        }
    }
}
