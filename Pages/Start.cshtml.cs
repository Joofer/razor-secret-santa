using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_secret_santa.Data;
using razor_secret_santa.Models;
using System.Linq;

namespace razor_secret_santa.Pages
{
    [Authorize]
    public class StartModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Dictionary<string, List<UserModel>> users { get; set; }
        public List<string?> groups { get; set; }

        public int? userCount { get; set; }
        public int? groupCount { get; set; }
        public int? giftCount { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? groupSent { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? state { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? message { get; set; }

        public StartModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            groups = _context.UserModels.Select(u => u.group).Distinct().ToList();
            users = _context.UserModels.GroupBy(u => u.group).ToDictionary(u => u.Key, u => u.ToList());
        }
    }
}
