using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PersonalPhotos.Models
{
    public class PhotoUploadViewModel
    {
        [Required(ErrorMessage = "Please enter a description.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please choose a photo!")]
        public IFormFile File { get; set; }
    }
}