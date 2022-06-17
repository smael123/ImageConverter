using ImageConverterLibrary;
using ImageConverterRazor.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ImageConverterRazor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageConverter _imageConverter;

        public HomeController(ILogger<HomeController> logger, IImageConverter imageConverter)
        {
            _logger = logger;
            _imageConverter = imageConverter;
        }

        public IActionResult Index(List<string> errorMessages)
        {
            return View("Index", new ConvertImageViewModel() { ErrorMessages = errorMessages });
        }

        public async Task<IActionResult> Convert(ConvertImageViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    List<string> errors = ModelState.Values.SelectMany(x => x.Errors).Select(c => c.ErrorMessage).ToList();
                    return RedirectToAction("Index", new { errorMessages = errors });
                }

                string? mimeType = viewModel.DestinationFileType switch
                {
                    "jpg" => "image/jpeg",
                    "png" => "image/png",
                    "bmp" => "image/bmp",
                    "gif" => "image/gif",
                    "tga" => "image/tga",
                    _ => null
                };

                if (mimeType == null)
                {
                    throw new Exception("Value given for DestinationFileType was invalid.");
                }

                if (viewModel.SourceFile == null)
                {
                    throw new NullReferenceException("SourceFile is null.");
                }

                //we could abstract this for testing but 0 byte temp files aren't dangerous
                string sourceLocalFilePath = Path.GetTempFileName();
                string destinationLocalFilePath = Path.GetTempFileName();

                ImageConverterSettings imageConverterSettings = new(sourceLocalFilePath, destinationLocalFilePath, viewModel.DestinationFileType)
                {
                    DestinationPixelWidth = viewModel.DestinationPixelWidth,
                    DestinationPixelHeight = viewModel.DestinationPixelHeight,
                    Uncompressed = viewModel.Uncompressed
                };

                using (FileStream stream = System.IO.File.Create(sourceLocalFilePath))
                {
                    await viewModel.SourceFile.CopyToAsync(stream);
                }

                await _imageConverter.ConvertImage(imageConverterSettings);

                DateTime currentDateTime = DateTime.Now;
                
                return PhysicalFile(destinationLocalFilePath, mimeType, $"converted-image-{currentDateTime.Year}-{currentDateTime.Month}-{currentDateTime.Day}T{currentDateTime.Hour}_{currentDateTime.Minute}_{currentDateTime.Second}.{viewModel.DestinationFileType}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SourceFile Length: {SourceFile}, DestinationFileType: {DestinationFileType}, DestinationPixelWidth: {DestinationPixelWidth}, DestinationPixelHeight: {DestinationPixelHeight}, Uncompressed: {Uncompressed}",  
                    viewModel.SourceFile?.Length,
                    viewModel.DestinationFileType,
                    viewModel.DestinationPixelWidth,
                    viewModel.DestinationPixelHeight,
                    viewModel.Uncompressed
                );

                return RedirectToAction("Index", new { errorMessages = new List<string> { "Internal Server Error" } });
            }
        }

        public IActionResult Privacy()
        {
            return View("Privacy");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}