// See https://aka.ms/new-console-template for more information

using ImageConverterConsole;
using ImageConverterLibrary;

//create settings object
try
{
    ImageConverterConsoleRunner runner = new(new ImageConverter());
    ImageConverterResult result = await runner.RunConverter(args);

    Console.WriteLine($"Done\nSource:\t{result.SourceFilePath}\nDestination:\t{result.DestinationFilePath}\nDimensions:\t{result.Width} by {result.Height}\nUncompressed:\t{(result.Uncompressed ? "Yes" : "No")}\n");
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}