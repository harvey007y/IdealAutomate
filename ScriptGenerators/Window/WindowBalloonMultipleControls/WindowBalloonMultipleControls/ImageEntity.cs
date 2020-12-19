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
        public int Attempts { get; set; }
        public int Occurrence { get; set; }
        public int Sleep { get; set; }
        public int RelativeX { get; set; }
        public int RelativeY { get; set; }       
        public int Tolerance { get; set; }
        public bool UseGrayScale { get; set; }
        public ImageEntity()
        {
            Attempts = 1;
            Occurrence = 1;
            Sleep = 100;
            RelativeX = 10;
            RelativeY = 10;
            Tolerance = 90;
            UseGrayScale = false;
        }
    }
}
