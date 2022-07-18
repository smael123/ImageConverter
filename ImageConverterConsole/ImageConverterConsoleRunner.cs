using System;
using ImageConverterLibrary;

namespace ImageConverterConsole
{
    public class ImageConverterConsoleRunner : IImageConverterConsoleRunner
    {
        private readonly IImageConverter _imageConverter;

        public ImageConverterConsoleRunner(IImageConverter imageConverter)
        {
            _imageConverter = imageConverter;
        }

        public Task<ImageConverterResult> RunConverter(string[] args)
        {
            ImageConverterSettings settings = ProduceSettingsFromArgs(args);

            return RunConverter(settings);
        }

        public Task<ImageConverterResult> RunConverter(ImageConverterSettings settings)
        {
            return _imageConverter.ConvertImage(settings);
        }

        private static ImageConverterSettings ProduceSettingsFromArgs(string[] args)
        {
            int sourceFilePathIndex = Array.FindIndex(args, a => a == "-source");
            if (sourceFilePathIndex == -1)
            {
                throw new Exception("Source file path not found.");
            }
            string sourceFilePath = args[sourceFilePathIndex + 1];

            int destinationFileTypeIndex = Array.FindIndex(args, a => a == "-dest-type");
            if (destinationFileTypeIndex == -1)
            {
                throw new Exception("You did not specify a destination file type.");
            }
            string destinationFileType = args[destinationFileTypeIndex + 1];

            int destinationFilePathIndex = Array.FindIndex(args, a => a == "-dest");
            string destinationFilePath =
                destinationFilePathIndex == -1 ?
                    Path.Combine(Path.GetDirectoryName(sourceFilePath) ?? "", Path.GetFileNameWithoutExtension(sourceFilePath)) + "." + destinationFileType :
                    args[destinationFilePathIndex + 1];

            int destinationWidthIndex = Array.FindIndex(args, a => a == "-dest-width");
            int? destinationWidth = destinationWidthIndex == -1 ? null : int.Parse(args[destinationWidthIndex + 1]);

            int destinationHeightIndex = Array.FindIndex(args, a => a == "-dest-height");
            int? destinationHeight = destinationHeightIndex == -1 ? null : int.Parse(args[destinationHeightIndex + 1]);

            int uncompressedIndex = Array.FindIndex(args, a => a == "-uncompressed");
            bool uncompressed = uncompressedIndex != -1;

            int overwriteDestIndex = Array.FindIndex(args, a => a == "-overwrite-dest");
            bool overwriteDest = overwriteDestIndex != -1;

            return new ImageConverterSettings(sourceFilePath, destinationFilePath, destinationFileType)
            {
                DestinationPixelWidth = destinationWidth,
                DestinationPixelHeight = destinationHeight,
                Uncompressed = uncompressed,
                OverwriteDestination = overwriteDest
            };
        }
    }
}

