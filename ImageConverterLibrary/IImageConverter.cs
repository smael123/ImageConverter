using System.Threading.Tasks;

namespace ImageConverterLibrary
{
    public interface IImageConverter
    {
        Task ConvertImage(ImageConverterSettings settings);
    }
}