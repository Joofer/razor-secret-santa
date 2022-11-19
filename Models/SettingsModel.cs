using System.ComponentModel.DataAnnotations;

namespace razor_secret_santa.Models
{
    public class SettingsModel
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "Setting name")]
        public string? name { get; set; }
        [Required]
        [Display(Name = "Value")]
        public string? value { get; set; }
    }
}
