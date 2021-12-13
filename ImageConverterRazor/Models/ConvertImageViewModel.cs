using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ImageConverterRazor.Models
{
    public class ConvertImageViewModel
    {
        [Required]
        [Display(Name = "Source File")]
        public IFormFile SourceFile { get; set; }

        [Required]
        [Display(Name = "Conversion Type")]
        public string? DestinationFileType { get; set; }

        [Display(Name = "Width")]
        [Range(0, int.MaxValue)]
        public int? DestinationPixelWidth { get; set; }

        [Display(Name = "Height")]
        [Range(0, int.MaxValue)]
        public int? DestinationPixelHeight { get; set; }

        public bool Uncompressed { get; set; }

        public List<SelectListItem> AllImageTypes
        {
            get => new()
            {
                new SelectListItem("Please select an image type.", "", true),
                new SelectListItem("jpg", "jpg"),
                new SelectListItem("png", "png"),
                new SelectListItem("bmp", "bmp"),
                new SelectListItem("gif", "gif"),
                new SelectListItem("tga", "tga")
            };
        }
        public List<string>? ErrorMessages { get; set; }
    }
}
