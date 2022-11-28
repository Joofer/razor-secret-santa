using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_secret_santa.Data;

namespace razor_secret_santa.Pages
{
    [Authorize]
    [BindProperties(SupportsGet = true)]
    public class SettingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<Models.SettingsModel> settingsModels { get; set; }
        // public List<IdentityUserRole> identityUserRoles { get; set; }

        public string? state { get; set; }
        public string? message { get; set; }

        public SettingsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var settings = _context.SettingModels;
            if (settings.Any()) settingsModels = settings.OrderBy(s => s.name).ToList();
            return Page();
        }
    }
}
