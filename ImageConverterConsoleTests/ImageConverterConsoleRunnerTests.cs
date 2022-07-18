using ImageConverterConsole;
using ImageConverterLibrary;
using Moq;

namespace ImageConverterConsoleTests;

[TestFixture]
public class ImageConverterConsoleRunnerTests
{
    private Mock<IImageConverter> _imageConverter;

    private ImageConverterConsoleRunner _imageConverterConsoleRunner;

    [SetUp]
    public void Setup()
    {
        _imageConverter = new();

        _imageConverterConsoleRunner = new(_imageConverter.Object);
    }

    //-source "C:\Pictures\before.jpg" -dest "C:\Pictures\after.png" -dest-type "tga" -dest-width 400 -dest-height 500 -uncompressed
    //[TestCase(@"-source ""C:\Pictures\before.jpg"" -dest ""C:\Pictures\after.png"" -dest-type ""png"" -dest-width 400 -dest-height 500 -uncompressed")]

    [Test]
    [TestCase(@"-dest ""C:\Pictures\after.png"" -dest-type ""png"" -dest-width 400 -dest-height 500 -uncompressed")]
    public void RunConverter_NoSource_ThrowsException(string argsString)
    {
        string[] args = argsString.Split(null);

        Exception ex = Assert.ThrowsAsync<Exception>(async () => await _imageConverterConsoleRunner.RunConverter(args));
        Assert.That(ex.Message, Is.EqualTo("Source file path not found."));
    }

    [Test]
    [TestCase(@"-source ""C:\Pictures\before.jpg"" -dest ""C:\Pictures\after.png"" -dest-width 400 -dest-height 500 -uncompressed")]
    public void RunConverter_NoDestType_ThrowsException(string argsString)
    {
        string[] args = argsString.Split(null);

        Exception ex = Assert.ThrowsAsync<Exception>(async () => await _imageConverterConsoleRunner.RunConverter(args));
        Assert.That(ex.Message, Is.EqualTo("You did not specify a destination file type."));
    }

    [Test]
    [TestCase(@"-source ""C:\Pictures\before.jpg"" -dest ""C:\Pictures\after.png"" -dest-type ""png"" -dest-width abc -dest-height 500 -uncompressed")]
    public void RunConverter_BadFormatForWidth_ThrowsFormatException(string argsString)
    {
        string[] args = argsString.Split(null);

        Assert.ThrowsAsync<FormatException>(async () => await _imageConverterConsoleRunner.RunConverter(args));
    }

    [Test]
    [TestCase(@"-source ""C:\Pictures\before.jpg"" -dest ""C:\Pictures\after.png"" -dest-type ""png"" -dest-width 400 -dest-height abc -uncompressed")]
    public void RunConverter_BadFormatForHeight_ThrowsFormatException(string argsString)
    {
        string[] args = argsString.Split(null);

        Assert.ThrowsAsync<FormatException>(async () => await _imageConverterConsoleRunner.RunConverter(args));
    }

    [Test]
    [TestCase(@"-source ""C:\Pictures\before.jpg"" -dest ""C:\Pictures\after.png"" -dest-type ""png"" -dest-width 400 -dest-height 500 -uncompressed")]
    public async Task RunConverter_ReturnsImageConverterResultObject(string argsString)
    {
        string[] args = argsString.Split(null);

        await _imageConverterConsoleRunner.RunConverter(args);

        Assert.Pass();
    }
}
