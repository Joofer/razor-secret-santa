using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor_secret_santa.Data;
using razor_secret_santa.Models;
using System.Data;
using System.Runtime;

namespace razor_secret_santa.Pages
{
    [Authorize]
    [BindProperties(SupportsGet = true)]
    public class ControlModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ControlModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<UserModel> Users { get; set; }
        public List<UserDetails> Details { get; set; }
        public List<GiftModel> Gifts { get; set; }

        public string state { get; set; }
        public string message { get; set; }

        public void OnGet()
        {
            Users = _context.UserModels.ToList();
            Details = _context.UserDetails.ToList();
            Gifts = _context.GiftModels.ToList();
        }

        public void EditUser(int userID)
        {

        }

        public void DeleteUser(int userID)
        {

        }

        public void EditGift(int giftID)
        {

        }

        public void DeleteGift(int giftID)
        {

        }
    }
}
