using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace razor_secret_santa.Models
{
	public class GiftModel
	{
		public int id { get; set; }
		[Required]
		[Display(Name = "Gift name")]
		public string? name { get; set; }
	}
}
