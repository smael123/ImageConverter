using ImageConverterLibrary;
using ImageConverterRazor.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageConverterRazor.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageConverter _imageConverter;

        public ImageController(IImageConverter imageConverter)
        {
            _imageConverter = imageConverter;
        }

        public IActionResult New(List<string> errorMessages)
        {
            return View(new ConvertImageViewModel() { ErrorMessages = errorMessages });
        }

        public async Task<IActionResult> Convert(ConvertImageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors).Select(c => c.ErrorMessage).ToList();

                //return View("New", new ConvertImageViewModel() { ErrorMessages = errors });
                return RedirectToAction("New", new { errorMessages = errors });
            }

            var sourceLocalFilePath = Path.GetTempFileName();
            var destinationLocalFilePath = Path.GetTempFileName();

            using (var stream = System.IO.File.Create(sourceLocalFilePath))
            {
                await viewModel.SourceFile.CopyToAsync(stream);
            }

            var imageConverterSettings = new ImageConverterSettings(sourceLocalFilePath, destinationLocalFilePath, viewModel.DestinationFileType)
            {
                DestinationPixelWidth = viewModel.DestinationPixelWidth < 1 ? null : viewModel.DestinationPixelWidth,
                DestinationPixelHeight = viewModel.DestinationPixelHeight < 1 ? null : viewModel.DestinationPixelHeight,
                Uncompressed = viewModel.Uncompressed
            };

            await _imageConverter.ConvertImage(imageConverterSettings);

            var currentDateTime = DateTime.Now;

            return PhysicalFile(destinationLocalFilePath, GetImageMimeType(viewModel.DestinationFileType), $"converted-image-{currentDateTime.Year}-{currentDateTime.Month}-{currentDateTime.Day}T{currentDateTime.Hour}_{currentDateTime.Minute}_{currentDateTime.Second}.{viewModel.DestinationFileType}");
        }

        private static string GetImageMimeType(string fileType)
        {
            return fileType switch
            {
                "jpg" => "image/jpeg",
                "png" => "image/png",
                "bmp" => "image/bmp",
                "gif" => "image/gif",
                "tga" => "image/tga",
                _ => "",
            };
        }
    }
}
