using System;
namespace ImageConverterLibrary
{
	public class ImageConverterResult
	{
        public string SourceFilePath { get; set; }
        public string DestinationFilePath { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Uncompressed { get; set; }
    }
}