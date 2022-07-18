using System.Threading.Tasks;

namespace ImageConverterLibrary
{
    public interface IImageConverter
    {
        Task<ImageConverterResult> ConvertImage(ImageConverterSettings settings);
    }
}