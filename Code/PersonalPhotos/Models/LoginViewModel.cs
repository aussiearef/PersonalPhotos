using System.ComponentModel.DataAnnotations;

namespace PersonalPhotos.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please provide the Email address")]
    [EmailAddress]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Please provide password")]
    public required string Password { get; set; }

    public required string ReturnUrl { get; set; }
}