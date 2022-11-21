using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_secret_santa.Data;
using razor_secret_santa.Models;

namespace razor_secret_santa.Pages.Control
{
    [Authorize]
    [BindProperties(SupportsGet = true)]
    public class EditSettingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Models.SettingsModel? settingsModel { get; set; }
        public int? id { get; set; }

        public EditSettingsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            settingsModel = _context.SettingModels.Where(s => s.id == id).FirstOrDefault();
            if (settingsModel == null)
                return RedirectToPage("/Settings", new { state = "error", message = "Не найдено значение #" + id + "." });
            return Page();
        }

        public IActionResult OnPost()
        {
            var settings = _context.SettingModels.Where(s => s.id == id).FirstOrDefault();

            if (settings == null)
                return RedirectToPage("/Settings", new { state = "error", message = "Не найдено значение #" + id + "." });

            settings!.name = settingsModel!.name;
            settings!.value = settingsModel!.value;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return RedirectToPage("/Settings", new { state = "error", message = String.Format("Ошибка во время редактирования значения. Подробнее: {0}", ex.Message) });
            }

            return RedirectToPage("/Settings", new { state = "success" });
        }
    }
}
