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
using Microsoft.EntityFrameworkCore;

namespace razor_secret_santa.Pages
{
    [BindProperties(SupportsGet = true)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public bool serviceEnabled { 
            get 
            {
                try
                {
                    var serviceEnabledStr = _context.SettingModels.Where(s => s.name == "serviceEnabled").FirstOrDefault();
                    return (serviceEnabledStr == null ? true : (serviceEnabledStr.value == "true" ? true : false));
                }
                catch
                {
                    return true;
                }
            }
        }
        public bool devModeEnabled { 
            get
            {
                try
                {
                    var devModeEnabledStr = _context.SettingModels.Where(s => s.name == "devModeEnabled").FirstOrDefault();
                    return (devModeEnabledStr == null ? false : (devModeEnabledStr.value == "true" ? true : false));
                }
                catch
                {
                    return false;
                }
            }
        }

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
                try
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
                    var foundFriendTmp = _context.UserModels.Where(u => u.id == foundDetails.friendID).FirstOrDefault();
                    if (foundFriendTmp == null)
                    {
                        state = "error";
                        return;
                    }
                    foundFriend = foundFriendTmp.name;
                    var foundGiftTmp = _context.GiftModels.Where(g => g.id == foundDetails.giftID).FirstOrDefault();
                    if (foundGiftTmp == null)
                    {
                        state = "error";
                        return;
                    }
                    foundGift = foundGiftTmp.name;
                }
                catch
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
                    return RedirectToPage("/Index", new { state = "gift-error", error = "exists" });

                // Updating context

                try
                {
                    _context.GiftModels.Add(giftModel);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return RedirectToPage("/Error", new { message = ex.Message });
                }

                // DEV

                if (devModeEnabled)
                    return RedirectToPage("/Index", new { phase = 1, state = "gift-success" });
                else
                    return RedirectToPage("/Index", new { state = "gift-success" });
            }
            else if (userModel.name != null)
            {
                if (_context.UserModels.Where(u => u.email == userModel.email).Count() > 0)
                    return RedirectToPage("/Index", new { state = "registration-error", error = "exists" });

                // Updating context

                try
                {
                    userModel.group = "default";
                    // userModel.group = userModel.group!.ToUpper().Trim();
                    _context.UserModels.Add(userModel);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return RedirectToPage("/Error", new { message = ex.Message });
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

                // DEV
                if (devModeEnabled)
                    return RedirectToPage("/Index", new { phase = 1, state = "registration-success" });
                else
                    return RedirectToPage("/Index", new { state = "registration-success" });
            }
            else if (!string.IsNullOrEmpty(Request.Form["email"]))
            {
                // DEV
                if (devModeEnabled)
                    return RedirectToPage("/Index", new { phase = 2, email = Request.Form["email"] });
                else
                    return RedirectToPage("/Index", new { email = Request.Form["email"] });
            }

            return RedirectToPage("/Index");
        }

        public void SendHello(int id, string name, string group, string address)
        {
            string subject = "Тайный Санта - Подтверждение участия";
            TextPart body = new TextPart(TextFormat.Html) { Text = String.Format("Привет, {0}! Ты успешно зарегистрирован в игре <b>Тайный Санта</b>.<br /><br />Данные:<br />Группа: {1}<br />Email: {2}<br /><br />Обратная связь: aw.works@ya.ru<br /><a href=\"https://secret-santa-aw.azurewebsites.net/Unsubscribe?userID={3}\">Отписаться от рассылки</a>", name, group, address, id) };
            EmailController.Send(address, subject, body);
        }
    }
}