using System.ComponentModel.DataAnnotations;

namespace PersonalPhotos.Models;

public class PhotoUploadViewModel
{
    [Required(ErrorMessage = "Please enter a description.")]
    public required string Description { get; set; }

    [Required(ErrorMessage = "Please choose a photo!")]
    public required IFormFile File { get; set; }
}