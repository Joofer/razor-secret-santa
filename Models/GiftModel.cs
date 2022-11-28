using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace razor_secret_santa.Models
{
	public class GiftModel
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        [Required]
		[Display(Name = "Gift name")]
		public string name { get; set; }
	}
}
