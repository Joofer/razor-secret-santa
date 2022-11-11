using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace razor_secret_santa.Models
{
    public class UserDetails
    {
        [Key]
        public int userID { get; set; }
        [ForeignKey("userID")]
        public UserModel user { get; set; }
        public int friendID { get; set; }
        [ForeignKey("friendID")]
        public UserModel friend { get; set; }
        public int giftID { get; set; }
        [ForeignKey("giftID")]
        public GiftModel gift { get; set; }
    }
}
