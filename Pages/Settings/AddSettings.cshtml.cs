using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_secret_santa.Data;

namespace razor_secret_santa.Pages.Control
{
    [Authorize]
    [BindProperties(SupportsGet = true)]
    public class AddSettingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Models.SettingsModel settingsModel { get; set; }

        public AddSettingsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            var settingsModel = new Models.SettingsModel()
            {
                name = this.settingsModel!.name,
                value = this.settingsModel!.value
            };

            try
            {
                _context.SettingModels.Add(settingsModel);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return RedirectToPage("/Settings", new { state = "error", message = String.Format("Ошибка во время очистки распределения. Подробнее: {0}", ex.Message) });
            }

            return RedirectToPage("/Settings", new { state = "success" });
        }
    }
}
