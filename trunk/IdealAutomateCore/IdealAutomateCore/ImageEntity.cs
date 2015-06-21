using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealAutomate.Core
{
    public class ImageEntity
    {
        public string ImageFile { get; set; }
        public int ImageAttempts { get; set; }
        public int ImageOccurrence { get; set; }
        public int ImageSleep { get; set; }
        public int ImageRelativeX { get; set; }
        public int ImageRelativeY { get; set; }       
        public int ImageTolerance { get; set; }
        public ImageEntity()
        {
            ImageAttempts = 1;
            ImageOccurrence = 1;
            ImageSleep = 100;
            ImageRelativeX = 10;
            ImageRelativeY = 10;
            ImageTolerance = 90;
        }
    }
}
