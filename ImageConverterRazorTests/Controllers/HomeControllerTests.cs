using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImageConverterLibrary;
using ImageConverterRazor.Controllers;
using ImageConverterRazor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ImageConverterRazorTests.Controllers;

public class HomeControllerTests
{
    private Mock<ILogger<HomeController>> _logger;
    private Mock<IImageConverter> _imageConverter;

    private HomeController _controller;

    //Convert Tests
    private Mock<IFormFile> _formFile;
    private ConvertImageViewModel _validViewModel;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<HomeController>>();

        //Convert Tests
        _imageConverter = new Mock<IImageConverter>();
        _formFile = new Mock<IFormFile>();
        _formFile.Setup(c => c.CopyToAsync(It.IsAny<FileStream>(), default));

        _validViewModel = new()
        {
            SourceFile = _formFile.Object,
            DestinationFileType = "png",
            DestinationPixelWidth = 200,
            DestinationPixelHeight = 200,
            Uncompressed = true
        };

        _imageConverter.Setup(c => c.ConvertImage(It.IsAny<ImageConverterSettings>()));

        _controller = new HomeController(_logger.Object, _imageConverter.Object);
    }

    [Test]
    public void Index_ReturnsCorrectView()
    {

        ViewResult result = _controller.Index(null) as ViewResult;


        Assert.AreEqual("Index", result.ViewName);
    }

    [Test]
    public void Index_ReturnsViewWithCorrectViewModel()
    {
        ViewResult result = _controller.Index(null) as ViewResult;

        Assert.IsInstanceOf<ConvertImageViewModel>(result.Model);
    }

    [Test]
    public async Task Convert_InvalidModelState_RedirectsToAnotherPage()
    {
        _controller.ModelState.AddModelError("1", "Fake Error");

        RedirectToActionResult result = (RedirectToActionResult)await _controller.Convert(_validViewModel);

        Assert.IsInstanceOf<RedirectToActionResult>(result);
    }

    [Test]
    public async Task Convert_InvalidModelState_RedirectsToIndex()
    {
        _controller.ModelState.AddModelError("1", "Fake Error");

        RedirectToActionResult result = (RedirectToActionResult)await _controller.Convert(_validViewModel);

        Assert.AreEqual("Index", result.ActionName);
    }

    [Test]
    public async Task Convert_InvalidModelState_RedirectsToIndexWithErrors()
    {
        _controller.ModelState.AddModelError("1", "Fake Error");

        RedirectToActionResult result = (RedirectToActionResult)await _controller.Convert(_validViewModel);

        List<string> errorMessages = (List<string>)result.RouteValues["errorMessages"];

        Assert.AreEqual("Fake Error", errorMessages[0]);
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("exe")]
    public async Task Convert_InvalidDestinationType_RedirectsToAnotherPage(string destinationType)
    {
        _validViewModel.DestinationFileType = destinationType;

        RedirectToActionResult result = (RedirectToActionResult)await _controller.Convert(_validViewModel);

        Assert.IsInstanceOf<RedirectToActionResult>(result);
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("exe")]
    public async Task Convert_InvalidDestinationType_RedirectsToIndex(string destinationType)
    {
        _validViewModel.DestinationFileType = destinationType;

        RedirectToActionResult result = (RedirectToActionResult)await _controller.Convert(_validViewModel);

        Assert.AreEqual("Index", result.ActionName);
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("exe")]
    public async Task Convert_InvalidDestinationType_RedirectsToIndexWithErrors(string destinationType)
    {
        _validViewModel.DestinationFileType = destinationType;

        RedirectToActionResult result = (RedirectToActionResult)await _controller.Convert(_validViewModel);

        List<string> errorMessages = (List<string>)result.RouteValues["errorMessages"];

        Assert.AreEqual("Internal Server Error", errorMessages[0]);
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("exe")]
    public async Task Convert_InvalidDestinationType_LogsError(string destinationType)
    {
        _validViewModel.DestinationFileType = destinationType;

        await _controller.Convert(_validViewModel);

        _logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.Is<Exception>(ex => ex.Message == "Value given for DestinationFileType was invalid."),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ));
    }

    [Test]
    public async Task Convert_NullSourceFile_RedirectsToAnotherPage()
    {
        _validViewModel.SourceFile = null;

        RedirectToActionResult result = (RedirectToActionResult)await _controller.Convert(_validViewModel);

        Assert.IsInstanceOf<RedirectToActionResult>(result);
    }

    [Test]
    public async Task Convert_NullSourceFile_RedirectsToIndex()
    {
        _validViewModel.SourceFile = null;

        RedirectToActionResult result = (RedirectToActionResult)await _controller.Convert(_validViewModel);

        Assert.AreEqual("Index", result.ActionName);
    }

    [Test]
    public async Task Convert_NullSourceFile_RedirectsToIndexWithErrors()
    {
        _validViewModel.SourceFile = null;

        RedirectToActionResult result = (RedirectToActionResult)await _controller.Convert(_validViewModel);

        List<string> errorMessages = (List<string>)result.RouteValues["errorMessages"];

        Assert.AreEqual("Internal Server Error", errorMessages[0]);
    }

    [Test]
    public async Task Convert_NullSourceFile_LogsError()
    {
        _validViewModel.SourceFile = null;

        await _controller.Convert(_validViewModel);

        _logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.Is<Exception>(ex => ex.Message == "SourceFile is null."),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ));
    }

    [Test]
    public async Task Convert_EverythingOk_ReturnsPhysicalFileResult()
    {
        PhysicalFileResult result = (PhysicalFileResult)await _controller.Convert(_validViewModel);

        Assert.IsInstanceOf<PhysicalFileResult>(result);
    }

    [Test]
    public void Privacy_ReturnsCorrectView()
    {
        ViewResult result = _controller.Privacy() as ViewResult;

        Assert.AreEqual("Privacy", result.ViewName);
    }
}
