using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;

namespace razor_secret_santa.Models
{
    public class UserModel
	{
		public int id { get; set; }
		[Required]
		[Display(Name = "User name")]
		public string? name { get; set; }
		[Required]
		[Display(Name = "Group name")]
        public string? group { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email")]
        public string? email { get; set; }
	}
}
