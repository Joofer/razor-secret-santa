using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace razor_secret_santa.Models
{
    public class SettingsModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        [Required]
        [Display(Name = "Setting name")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Value")]
        public string value { get; set; }
    }
}
