using System.ComponentModel.DataAnnotations;

namespace PersonalPhotos.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please provide the Email address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please provide password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}