using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_secret_santa.Data;
using razor_secret_santa.Models;
using System.Collections.Specialized;
using System.Linq;

namespace razor_secret_santa.Pages
{
    [Authorize]
    public class StartModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<Group> groups { get; set; } = new List<Group>();

		[BindProperty(SupportsGet = true)]
        public string? group { get; set; }
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
            var groupModels = _context.UserModels.Select(u => u.group).Distinct();
            if  (groupModels != null)
            {
                var groupList = groupModels.ToList();

                foreach (var groupStr in groupList)
                {
                    int count = 0, participatingCount = 0;

                    var tempUserModels = _context.UserModels.Where(u => u.group == groupStr);
                    if (tempUserModels != null) count = tempUserModels.Count();
                    var tempUserDetails = _context.UserDetails.Where(d => d.user.group == groupStr);
                    if (tempUserDetails != null) participatingCount = tempUserDetails.Count();

                    var group = new Group
                    {
                        name = groupStr,
                        count = count,
                        participatingCount = participatingCount
                    };

                    groups.Add(group);
				}
            }
        }
    }

    public struct Group
    {
        public string name { get; set; }
        public int count { get; set; }
        public int participatingCount { get; set; }
    }
}
