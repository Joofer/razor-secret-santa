using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit.Text;
using MimeKit;
using razor_secret_santa.Data;
using razor_secret_santa.Models;
using razor_secret_santa.Controllers;
using Microsoft.EntityFrameworkCore.Query.Internal;

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

        public int phase { get; set; } = 0; // DEV

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (giftModel.name != null)
            {
                if (_context.GiftModels.Where(g => g.name == giftModel.name).Count() > 0)
                    return RedirectToPage("/Index", new { state = "error", error = "exists" });

                try
                {
                    _context.GiftModels.Add(giftModel);
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

                try
                {
                    userModel.group = userModel.group!.ToUpper();
                    _context.UserModels.Add(userModel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return RedirectToPage("/Error");
                }

                try
                {
                    SendHello(userModel.id, userModel.name, userModel.group, userModel.email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return RedirectToPage("/Error");
                }
                
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return RedirectToPage("/Error");
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