using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace razor_secret_santa.Models
{
    public class UserDetails
    {
        [Key]
        public string userID { get; set; }
        [ForeignKey("userID")]
        public UserModel user { get; set; }
        public string friendID { get; set; }
        [ForeignKey("friendID")]
        public UserModel friend { get; set; }
        public string giftID { get; set; }
        [ForeignKey("giftID")]
        public GiftModel gift { get; set; }
    }
}
