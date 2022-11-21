using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_secret_santa.Data;

namespace razor_secret_santa.Pages.Control
{
    [Authorize]
    [BindProperties(SupportsGet = true)]
    public class DeleteSettingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Models.SettingsModel? _settingsModel { get; set; }
        public int? id { get; set; }

        public DeleteSettingsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var settings = _context.SettingModels.Where(s => s.id == id).FirstOrDefault();

            if (settings == null)
                return RedirectToPage("/Settings", new { state = "error", message = "Не найдено значение #" + id + "." });

            try
            {
                _context.SettingModels.Remove(settings);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return RedirectToPage("/Settings", new { state = "error", message = String.Format("Ошибка во время удаленмя значения. Подробнее: {0}", ex.Message) });
            }

            return RedirectToPage("/Settings", new { state = "success" });
        }
    }
}
