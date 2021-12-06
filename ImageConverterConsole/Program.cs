// See https://aka.ms/new-console-template for more information

using ImageConverterLibrary;

//create settings object
try
{
    var settings = GetSettingsFromArguments();

    ImageConverter imageConverter = new();
    await imageConverter.ConvertImage(settings);
    Console.WriteLine($"Done:\nSource:\t{settings.SourceFilePath}\nDestination:\t{settings.DestinationFilePath}\nDimensions:\t{settings.DestinationPixelWidth} by {settings.DestinationPixelHeight}\nUncompressed:\t{(settings.Uncompressed ? "Yes" : "No")}\n");
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}

ImageConverterSettings GetSettingsFromArguments()
{
    var sourceFilePathIndex = Array.FindIndex(args, a => a == "--source");
    if (sourceFilePathIndex == -1)
    {
        throw new Exception("Source file path not found.");
    }
    var sourceFilePath = args[sourceFilePathIndex + 1];

    var destinationFileTypeIndex = Array.FindIndex(args, a => a == "--dest-type");
    if (destinationFileTypeIndex == -1)
    {
        throw new Exception("You did not specify a destination file type.");
    }
    var destinationFileType = args[destinationFileTypeIndex + 1];

    var destinationFilePathIndex = Array.FindIndex(args, a => a == "--dest");
    var destinationFilePath =
        destinationFilePathIndex == -1 ?
        Path.Combine(Path.GetDirectoryName(sourceFilePath) ?? "", Path.GetFileNameWithoutExtension(sourceFilePath)) + "." + destinationFileType :
        args[destinationFilePathIndex + 1];

    var destinationWidthIndex = Array.FindIndex(args, a => a == "--dest-width");
    var destinationWidth = destinationWidthIndex == -1 ? (int?)null : int.Parse(args[destinationWidthIndex + 1]);

    var destinationHeightIndex = Array.FindIndex(args, a => a == "--dest-height");
    var destinationHeight = destinationHeightIndex == -1 ? (int?)null : int.Parse(args[destinationHeightIndex + 1]);

    var uncompressedIndex = Array.FindIndex(args, a => a == "--uncompressed");
    var uncompressed = uncompressedIndex != -1;

    return new ImageConverterSettings(sourceFilePath, destinationFilePath, destinationFileType)
    {
        DestinationPixelWidth = destinationWidth,
        DestinationPixelHeight = destinationHeight,
        Uncompressed = uncompressed
    };
}