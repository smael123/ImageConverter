# ImageConverter

A series of .NET 6 image converter applications that use a common .NET Standard 2.0 library. All applications allow you to convert an image from one format to another in your desired width and height (unless it is impossible). Uses the [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) library. 

Applications

| Name                      | Tech Stack               |
| ------------------------- | ------------------------ |
| ImageConverterConsole     | .NET 6                   |
| ImageConverterRazor       | .NET 6, ASP.NET Core MVC |

## ImageConverterConsole

A command line utility. Argument names are prefixed with "-" following the argument value.

### Arguments

#### -source

Required: Yes

File path of the image you want to convert.

#### -dest

Required: No

Destination path of your converted image. If not given then the converted image is saved in the same folder and file name as the source path.

#### -dest-type

Required: Yes

File type of your converted image.

Accepted values:

- jpg
- png
- bmp
- gif
- tga

#### -dest-width

Required: No

Width in pixels of your converted image. If no value is given then the width of your converted image will be the same as your source image.

#### -dest-height

Required: No

Height in pixels of your converted image. If no value is given then the height of your converted image will be the same as your source image.

#### -uncompressed

Required : No

If this argument is present then the converted image will be uncompressed. You don't need to specify a value (true or false) for this argument.

### Example

Converts an image "before.jpg" to an uncompressed PNG file after.png with the dimensions 400x500.

`dotnet ImageConverterConsole.dll -source "C:\Pictures\before.jpg" -dest "C:\Pictures\after.png" -dest-type "png" -dest-width 400 -dest-height 500 -uncompressed`

## ImageConverterRazor

On the image converter page `http://localhost:xxxx/Home/Index` upload your image and then enter your desired values in the fields. You must specify the conversion type. When you click on the submit button, you will be prompted to download your converted image.