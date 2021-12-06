using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Gif;
using System.IO;

namespace ImageConverterLibrary
{
    public class ImageConverter
    {
        public async Task ConvertImage(ImageConverterSettings settings)
        {
            string sourceFilePath = settings.SourceFilePath;
            string destinationFilePath = settings.DestinationFilePath;
            int? width = settings.DestinationPixelWidth;
            int? height = settings.DestinationPixelHeight;

            if (!File.Exists(settings.SourceFilePath))
            {
                throw new Exception($"File: {settings.SourceFilePath} does not exist.");
            }

            using (var image = await Image.LoadAsync(sourceFilePath))
            {
                if (width.HasValue && height.HasValue)
                {
                    image.Mutate(c => c.Resize(width.Value, height.Value));
                }

                var encoder = GetEncoder(settings);
                await image.SaveAsync(destinationFilePath, encoder);
            }
        }

        private static IImageEncoder GetEncoder(ImageConverterSettings settings)
        {
            switch (settings.DestinationFileType)
            {
                case "jpg":
                    return new JpegEncoder
                    {
                        Quality = settings.Uncompressed ? 100 : 75
                    };
                case "png":
                    return new PngEncoder();
                case "bmp":
                    return new BmpEncoder();
                case "gif":
                    return new GifEncoder();
                case "tga":
                    return new TgaEncoder { Compression = settings.Uncompressed ? TgaCompression.None : TgaCompression.RunLength };
                default:
                    throw new Exception("No encoder for specified type: " + settings.DestinationFileType + ".");
            }
        }
    }
}
