using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json.Linq;
using razor_secret_santa.Data;
using razor_secret_santa.Models;
using System.Collections.Generic;

namespace razor_secret_santa.Pages.Control
{
    [Authorize]
    [BindProperties(SupportsGet = true)]
    public class StartGroup : PageModel
    {
        private readonly ApplicationDbContext _context;

        private static Random rng = new Random();

        public string? group { get; set; }

        public StartGroup(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var groups = _context.UserModels.Select(u => u.group).Distinct().ToList();
            if (groups.Count < 1 || !groups.Contains(group)) return RedirectToPage("/Start", new { state = "error", message = String.Format("Ошибка во время распределения для группы {0}.", group) });
            var users = _context.UserModels.Where(u => u.group == group).ToList();
            var gifts = _context.GiftModels.ToList();

            var userIDs = users.Select(u => u.id).ToList();
            var friendIDs = users.Select(u => u.id).ToList();
            var giftIDs = gifts.Select(g => g.id).ToList();

            // Storing error

            string? error;

            // Checking for any data already written to the Details table

            ClearContext(userIDs, out error);
            if (error != null)
            {
                Console.WriteLine("Error: " + error);
                return RedirectToPage("/Start", new { state = "error", message = String.Format("Ошибка во время распределения для группы {0}: ошибка проверки.\nПодробнее: {1}", group, error) });
            }

            // Shuffling lists

            ShuffleVerified(ref friendIDs);
            Shuffle(ref giftIDs);

            // Creating rows

            UpdateContext(users, gifts, userIDs, friendIDs, giftIDs, out error);
            if (error != null)
            {
                Console.WriteLine("Error: " + error);
                return RedirectToPage("/Start", new { state = "error", message = String.Format("Ошибка во время распределения для группы {0}: ошибка добавления записей.\nПодробнее: {1}", group, error) });
            }

            return RedirectToPage("/Start", new { state = "success", groupSent = group });
        }

        public void UpdateContext(List<UserModel> users, List<GiftModel> gifts, List<string> userIDs, List<string> friendIDs, List<string> giftIDs, out string? error)
        {
            List<UserDetails> _details = new List<UserDetails>();

            for (int k = 0; k < userIDs.Count; k++)
            {
                var user = users.Where(u => u.id == userIDs[k]).FirstOrDefault();
                var friend = users.Where(u => u.id == friendIDs[k]).FirstOrDefault();
                var gift = gifts.Where(g => g.id == giftIDs[k]).FirstOrDefault();

                // Checking for data correctness

                if (user == null || friend == null || gift == null)
                {
                    error = "Internal error while processing request. Please, try again.";
                    return;
                }

                // Creating instance of user details

                UserDetails userDetails = new UserDetails()
                {
                    userID = user.id,
                    user = user,
                    friendID = friend.id,
                    friend = friend,
                    giftID = gift.id,
                    gift = gift
                };

                // Adding instance to list

                _details.Add(userDetails);
            }

            // Saving context

            try
            {
                _context.UserDetails.AddRange(_details);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                error = ex.Message;
                return;
            }

            error = null;
        }

        public void ClearContext(List<string> userIDs, out string? error)
        {
            for (int i = 0; i < userIDs.Count; i++)
            {
                // Checking if user id is presented in either of the tables

                var user = _context.UserDetails.Where(d => d.userID == userIDs[i]).FirstOrDefault();
                var friend = _context.UserDetails.Where(d => d.friendID == userIDs[i]).FirstOrDefault();

                // Deleting row where user is presented as user

                if (user != default)
                {
                    try
                    {
                        _context.UserDetails.Remove(user);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        error = ex.Message;
                        return;
                    }
                }

                // Deleting row where user is presented as friend

                if (friend != default)
                {
                    try
                    {
                        _context.UserDetails.Remove(friend);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        error = ex.Message;
                        return;
                    }
                }

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    error = ex.Message;
                    return;
                }
            }

            error = null;
        }

        public static void ShuffleVerified(ref List<string> arr)
        {
            var old = arr.ToList();

            // Shuffling

            Shuffle(ref arr);

            // Verifying

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i] == old[i])
                {
                    for (int j = 0; j < arr.Count; j++)
                    {
                        if (arr[i] != old[j] && arr[j] != old[i])
                        {
                            var temp = arr[i];
                            arr[i] = arr[j];
                            arr[j] = temp;
                            break;
                        }
                    }
                }
            }
        }

        public static void Shuffle(ref List<string> arr)
        {
            arr = arr.OrderBy(a => rng.Next()).ToList();
        }

    }
}
