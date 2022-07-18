using ImageConverterLibrary;

namespace ImageConverterConsole
{
    public interface IImageConverterConsoleRunner
    {
        Task<ImageConverterResult> RunConverter(string[] args);
        Task<ImageConverterResult> RunConverter(ImageConverterSettings settings);
    }
}