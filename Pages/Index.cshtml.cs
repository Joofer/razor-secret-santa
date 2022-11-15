using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit.Text;
using MimeKit;
using razor_secret_santa.Data;
using razor_secret_santa.Models;
using razor_secret_santa.Controllers;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;

namespace razor_secret_santa.Pages
{
    [BindProperties(SupportsGet = true)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public GiftModel giftModel { get; set; }
        public UserModel userModel { get; set; }
        public string state { get; set; }
        public string error { get; set; }

        public string email { get; set; }
        public UserModel? foundUser { get; set; }
        public UserDetails? foundDetails { get; set; }
        public string? foundFriend { get; set; }
        public string? foundGift { get; set; }

        public int phase { get; set; } // DEV
        public int userCount { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
            userCount = _context.UserDetails.Count();

            if (email != null)
            {
                foundUser = _context.UserModels.Where(u => u.email == email).FirstOrDefault();
                if (foundUser == null)
                {
                    state = "error";
                    return;
                }
                foundDetails = _context.UserDetails.Where(d => d.userID == foundUser.id).FirstOrDefault();
                if (foundDetails == null)
                {
                    state = "error";
                    return;
                }
                foundFriend = _context.UserModels.Where(u => u.id == foundDetails.friendID).FirstOrDefault().name;
                if (foundFriend == null)
                {
                    state = "error";
                    return;
                }
                foundGift = _context.GiftModels.Where(g => g.id == foundDetails.giftID).FirstOrDefault().name;
                if (foundGift == null)
                {
                    state = "error";
                    return;
                }

                state = "success";
            }
        }

        public IActionResult OnPost()
        {
            // Defining form used

            if (giftModel.name != null)
            {
                if (_context.GiftModels.Where(g => g.name == giftModel.name).Count() > 0)
                    return RedirectToPage("/Index", new { state = "error", error = "exists" });

                // Updating context

                try
                {
                    _context.GiftModels.Add(giftModel);
                    _context.SaveChanges();
                }               
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return RedirectToPage("/Error");
                }
            }
            else if (userModel.name != null)
            {
                if (_context.UserModels.Where(u => u.email == userModel.email).Count() > 0)
                    return RedirectToPage("/Index", new { state = "error", error = "exists" });

                // Updating context

                try
                {
                    userModel.group = userModel.group!.ToUpper().Trim();
                    _context.UserModels.Add(userModel);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return RedirectToPage("/Error");
                }

                // Sending welcome email

                /*try
                {
                    SendHello(userModel.id, userModel.name, userModel.group, userModel.email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return RedirectToPage("/Error");
                }*/
                
            }
            else if (!string.IsNullOrEmpty(Request.Form["email"]))
            {
                return RedirectToPage("/Index", new { email = Request.Form["email"] });
            }

            return RedirectToPage("/Index", new { state = "success" });
        }

        public void SendHello(int id, string name, string group, string address)
        {
            string subject = "Тайный Санта - Подтверждение участия";
            TextPart body = new TextPart(TextFormat.Html) { Text = String.Format("Привет, {0}! Ты успешно зарегистрирован в игре <b>Тайный Санта</b>.<br /><br />Данные:<br />Группа: {1}<br />Email: {2}<br /><br />Обратная связь: aw.works@ya.ru<br /><a href=\"https://secret-santa-aw.azurewebsites.net/Unsubscribe?userID={3}\">Отписаться от рассылки</a>", name, group, address, id) };
            EmailController.Send(address, subject, body);
        }
    }
}