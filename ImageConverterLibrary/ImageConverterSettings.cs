using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageConverterLibrary
{
    public class ImageConverterSettings
    {
        public string SourceFilePath { get; set; } //-source
        public string DestinationFilePath { get; set; } //-dest
        public string DestinationFileType { get; set; } //-dest-type
        public int? DestinationPixelWidth { get; set; } //-dest-width
        public int? DestinationPixelHeight { get; set; } //-dest-height
        public bool Uncompressed { get; set; } //-uncompressed
        public bool OverwriteDestination { get; set; } //-overwrite-dest

        public ImageConverterSettings(string sourceFilePath, string destinationFilePath, string destinationFileType)
        {
            SourceFilePath = sourceFilePath;
            DestinationFilePath = destinationFilePath;
            DestinationFileType = destinationFileType;
        }
    }
}
