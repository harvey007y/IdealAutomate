using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.Generic;

using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;


/// <summary>Class for finding the locations of a subimage within the screen</summary>
/// <remarks>
/// <para>
/// This code is available under the GNU GPL (http://www.gnu.org/copyleft/gpl.html).
/// </para>
/// <para>Author: Chris Gorecki (chris.k.gorecki@gmail.com)</para>
/// </remarks>
/// 
/*
 * Steps for pattern recognition:
1. CreateSmallBoolArray - Walk thru small image and create boolArray that indicates if next pixel is same as current pixel.
2. FindLeastCommonPatternInSmallBoolArray - Walk thru boolArray and find least common pattern of 10 pixels - 
 * the pattern will need to indicate patternpixelpos for xwrap and ywrap.
3. LookForLeastCommonPatternInBigImage - Walk thru big image comparing each pixel to least common pattern
4. ImagePatternThere - When you find a match in the big image for the least common pattern,  you have to 
 * find orig x and orig y by subtracting x and y offset for least common pattern from current pixel in big image.  
 * Starting with orig x and orig y, you have to see if big image contains the
 * pattern for the small image.  You have to wrap within big image at same width 
 * and height as what the small image is. If you exceed bounds of big image for 
 * x or y, you say that the small image is not found there.
*/
namespace IdealAutomate.Core
{
    class Scraper
    {
       struct objErrors
        {
            public int intWrong;
            public int intRight;
            public int intYellowErrors;
            public int intSaveY;
            public int intStartLineWrong;
            public int intEndLineWrong;
            public int intNumberIgnored;
           
        }
         /// <summary>
         /// GetSubPositions takes a bitmap of a larger image (usually, the desktop) and a 
         /// bitmap of a smaller image. It returns a list of subpositions
         /// within the larger image where the smaller image was found.
         /// You can specify grayscale matching to match images based
         /// on grayscale. If you specify a tolerance percentage, it
         /// allows you to still consider an image matched even if
         /// only a certain percentage of the pixels match. 
         /// </summary>
         /// <param name="main">Larger Image - usually desktop</param>
         /// <param name="sub">Smaller Image that you want to find in the larger one</param>
         /// <param name="strSubImageFileName">I do not think this is used</param>
         /// <param name="boolUseGrayScale"></param>
         /// <param name="highestPercentCorrect"></param>
         /// <param name="intTolerance"></param>
         /// <returns></returns>
      
        public static List<SubPositionInfo> GetSubPositions(Bitmap main, Bitmap sub,  bool boolUseGrayScale, ref decimal highestPercentCorrect, int intTolerance)
        {
            
            List<SubPositionInfo> possiblepos = new List<SubPositionInfo>();
            List<Rectangle> foundRects = new List<Rectangle>();    // The areas of images already found

            int mainwidth = main.Width;
            int mainheight = main.Height;
            byte[] dataMain;

            BitmapData bmMainData;
            int bytesMain;
            int strideMain;
            System.IntPtr Scan0Main;
            CreateDataMainByteArray(
                main,
                mainwidth,
                mainheight,
                out dataMain,
                out bmMainData,
                out bytesMain,
                out strideMain,
                out Scan0Main);

            int subwidth = sub.Width;
            int subheight = sub.Height;

            BitmapData bmSubData;
            int bytesSub;
            int strideSub;
            System.IntPtr Scan0Sub;
            byte[] dataSub;
            CreateDataSubByteArray(
                sub,
                subwidth,
                subheight,
                out bmSubData,
                out bytesSub,
                out strideSub,
                out Scan0Sub,
                out dataSub);


            LeastPopularColor oLeastPopularColor = new LeastPopularColor();
            LeastPopularPattern oLeastPopularPattern = new LeastPopularPattern();
            LeastPopularPattern oLeastPopularPattern2 = new LeastPopularPattern();

            Dictionary<MyTwoColors, int[]> repeats = new Dictionary<MyTwoColors, int[]>();
            Dictionary<MyPattern, int[]> repeatsPattern = new Dictionary<MyPattern, int[]>();

            Dictionary<Point, objErrors> dictionary = new Dictionary<Point, objErrors>();
            StringBuilder sb = new StringBuilder();

            if (boolUseGrayScale == false)
            {
                FindLeastPopularColorInSmallImage(sub, subwidth, subheight, strideSub, dataSub, ref oLeastPopularColor, repeats);
                LoopthruEachPixelInBigImageToFindMatchesOnSmallImageLeastPopularColor(main, sub,  possiblepos, foundRects, mainwidth, mainheight, dataMain, strideMain, strideSub, dataSub, oLeastPopularColor, ref highestPercentCorrect, intTolerance);
            }
            else
            {
                bool[,] boolArySmallImage = new bool[subwidth, subheight];
                FindLeastPopularPatternInSmallImage(sub, subwidth, subheight, strideSub, dataSub, ref oLeastPopularPattern, ref oLeastPopularPattern2, repeatsPattern, ref boolArySmallImage);
                LoopthruEachPixelInBigImageToFindMatchesOnSmallImageLeastPopularPattern(main, sub,  possiblepos, foundRects, mainwidth, mainheight, dataMain, strideMain, strideSub, dataSub, oLeastPopularPattern, boolArySmallImage, ref highestPercentCorrect, intTolerance);
                if (foundRects.Count == 0)
                {
                    LoopthruEachPixelInBigImageToFindMatchesOnSmallImageLeastPopularPattern(main, sub,  possiblepos, foundRects, mainwidth, mainheight, dataMain, strideMain, strideSub, dataSub, oLeastPopularPattern2, boolArySmallImage, ref highestPercentCorrect, intTolerance);
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(dataSub, 0, Scan0Sub, bytesSub);
            sub.UnlockBits(bmSubData);

            System.Runtime.InteropServices.Marshal.Copy(dataMain, 0, Scan0Main, bytesMain);
            main.UnlockBits(bmMainData);

            return possiblepos;
        }

        private static void LoopthruEachPixelInBigImageToFindMatchesOnSmallImageLeastPopularColor(
            Bitmap main,
            Bitmap sub,           
            List<SubPositionInfo> possiblepos,
            List<Rectangle> foundRects,
            int mainwidth,
            int mainheight,
            byte[] dataMain,
            int strideMain,
            int strideSub,
            byte[] dataSub,
            LeastPopularColor pLeastPopularColor,
            ref decimal highestPercentCorrect,
            int intTolerance)
        {
            // y for big image
            for (int y = 0; y < mainheight; y++)
            {
                // x for big image
                for (int x = 0; x < mainwidth; x++)
                {
                    int xtemp = x + 1;
                    int ytemp = y;
                    if (pLeastPopularColor.theTwoColors.IndexWrapX != -1)
                    {
                        xtemp = xtemp - sub.Width;
                        ytemp = ytemp + 1;
                    }
                    if (pLeastPopularColor.theTwoColors.IndexWrapY != -1)
                    {
                        ytemp = ytemp - sub.Height;
                        xtemp = xtemp - sub.Width;
                    }

                    // if the indexes are outside the bounds of 
                    // the big image, we say there is no match                     
                    if (xtemp > main.Width - 1 || xtemp < 0)
                    {
                        continue;
                    }

                    if (ytemp > main.Height - 1 || ytemp < 0)
                    {
                        continue;
                    }

                    MyColor curcolor = GetColor(x, y, strideMain, dataMain);
                    MyColor curcolor2 = GetColor(xtemp, ytemp, strideMain, dataMain);

                    // We are only looking for places where the 
                    // two pixels are different in the big image
                    if (curcolor.Equals(curcolor2))
                    {
                        continue;
                    }

                    MyTwoColors _myTwoColors;
                    _myTwoColors.A = curcolor.A;
                    _myTwoColors.R = curcolor.R;
                    _myTwoColors.G = curcolor.G;
                    _myTwoColors.B = curcolor.B;

                    _myTwoColors.A2 = curcolor2.A;
                    _myTwoColors.R2 = curcolor2.R;
                    _myTwoColors.G2 = curcolor2.G;
                    _myTwoColors.B2 = curcolor2.B;
                    _myTwoColors.IndexWrapX = pLeastPopularColor.theTwoColors.IndexWrapX;
                    _myTwoColors.IndexWrapY = pLeastPopularColor.theTwoColors.IndexWrapY;
                    Byte[] myColor = { curcolor.A, curcolor.R, curcolor.G, curcolor.B };

                    // Pixle value from subimage in desktop image
                    if (pLeastPopularColor.theTwoColors.Equals(_myTwoColors) && notFound(x, y, foundRects))
                    {
                        // this finds where rectangle would start
                        Point loc = pLeastPopularColor.thePosition;

                        int sx = x - loc.X;
                        int sy = y - loc.Y;
                        // Subimage occurs in desktop image 
                        // sx and sy must both be positive
                        if (sx > 0 && sy > 0)
                        {
                            if (imageThere(dataMain, sx, sy, dataSub, sub, strideMain, strideSub,  ref highestPercentCorrect, intTolerance))
                            {
                                SubPositionInfo mySubPositionInfo = new SubPositionInfo();
                                mySubPositionInfo.myPoint =  new Point(x - loc.X, y - loc.Y);
                                mySubPositionInfo.percentcorrect = highestPercentCorrect;
                                possiblepos.Add(mySubPositionInfo);
                                highestPercentCorrect = 0;
                                foundRects.Add(new Rectangle(x, y, sub.Width, sub.Height));
                            }
                        }
                    }

                }
            }
        }

        private static void LoopthruEachPixelInBigImageToFindMatchesOnSmallImageLeastPopularPattern(
            Bitmap main,
            Bitmap sub,
           
            List<SubPositionInfo> possiblepos,
            List<Rectangle> foundRects,
            int mainwidth,
            int mainheight,
            byte[] dataMain,
            int strideMain,
            int strideSub,
            byte[] dataSub,
            LeastPopularPattern pLeastPopularPattern,
            bool[,] boolArySmallImage,
            ref decimal highestPercentCorrect,
            int intTolerance)
        {

            for (int y = 0; y < mainheight; y++)
            {
                // x for big image
                for (int x = 0; x < mainwidth; x++)
                {
                    if (y == 735 && x == 101)
                    {
                        string abc = "abd";
                    }
                    MyPattern _myBigPattern = MyPattern.GetPatternInBigImage(x, y, pLeastPopularPattern, sub.Width, sub.Height, strideMain, dataMain, mainwidth, mainheight);

                    // Pixle value from subimage in desktop image
                    if (_myBigPattern.disgard == false
                        && pLeastPopularPattern.thePattern.Equals(_myBigPattern)
                        && notFound(x, y, foundRects))
                    {
                        // this finds where rectangle would start
                        Point loc = pLeastPopularPattern.thePosition;

                        int sx = x - loc.X;
                        int sy = y - loc.Y;
                        // Subimage occurs in desktop image 
                        // sx and sy must both be positive
                        if (sx > -1 && sy > -1)
                        {
                            if (imageTherePattern(sx, sy, boolArySmallImage, sub, strideMain, strideSub,  dataMain, mainwidth, mainheight, ref highestPercentCorrect, intTolerance))
                            {
                                SubPositionInfo mySubPositionInfo = new SubPositionInfo();
                                mySubPositionInfo.myPoint = new Point(x - loc.X, y - loc.Y);
                                mySubPositionInfo.percentcorrect = highestPercentCorrect;
                                possiblepos.Add(mySubPositionInfo);
                                highestPercentCorrect = 0;
                                foundRects.Add(new Rectangle(x, y, sub.Width, sub.Height));
                            }
                        }
                    }
                }

            }
        }

        private static void FindLeastPopularColorInSmallImage(
            Bitmap sub,
            int subwidth,
            int subheight,
            int strideSub,
            byte[] dataSub,
            ref LeastPopularColor pLeastPopularColor,
            Dictionary<MyTwoColors, int[]> repeats)
        {
            int IndexWrapColorX = -1;
            int IndexWrapColorY = -1;
            bool[,] boolArySmallImage = new bool[subwidth, subheight];
            for (int y = 0; y < sub.Height; y++)
            {
                for (int x = 0; x < sub.Width; x++)
                {
                    IndexWrapColorX = -1;
                    IndexWrapColorY = -1;
                    int xtemp = x + 1;
                    int ytemp = y;
                    if (xtemp > sub.Width - 1)
                    {
                        IndexWrapColorX = 1;
                        xtemp = 0;
                        ytemp = y + 1;
                        if (ytemp > sub.Height - 1)
                        {
                            IndexWrapColorY = 1;
                            ytemp = 0;
                        }
                    }
                    MyColor curcolor = GetColor(x, y, strideSub, dataSub);
                    MyColor curcolor2 = GetColor(xtemp, ytemp, strideSub, dataSub);

                    if (curcolor.Equals(curcolor2))
                    {
                        continue;
                    }

                    MyTwoColors _myTwoColors;
                    _myTwoColors.A = curcolor.A;
                    _myTwoColors.R = curcolor.R;
                    _myTwoColors.G = curcolor.G;
                    _myTwoColors.B = curcolor.B;

                    _myTwoColors.A2 = curcolor2.A;
                    _myTwoColors.R2 = curcolor2.R;
                    _myTwoColors.G2 = curcolor2.G;
                    _myTwoColors.B2 = curcolor2.B;
                    _myTwoColors.IndexWrapX = IndexWrapColorX;
                    _myTwoColors.IndexWrapY = IndexWrapColorY;

                    // The pixel value has been found before
                    if (!repeats.ContainsKey(_myTwoColors))
                    {
                        // a = {number times found, x location, y location}
                        int[] a = { 1, x, y };
                        repeats.Add(_myTwoColors, a);
                    }
                    else
                    {
                        // Increment the number of times the values been found
                        ((int[])repeats[_myTwoColors])[0]++;
                    }
                }
            }

            // Find the pixel value that has been found the least
            // number of times; a[0] is count; a[1] is x; a[2] is y; image[ix, iy] 
            // is the color of the pixel
            int min = int.MaxValue, ix = -1, iy = -1;
            MyTwoColors oMyTwoColors = new MyTwoColors();
            foreach (var de in repeats)
            {
                int[] a = (int[])de.Value;
                if (a[0] < min)
                {
                    min = a[0];
                    ix = a[1];
                    iy = a[2];
                    oMyTwoColors = de.Key;
                }
            }

            pLeastPopularColor.theTwoColors = oMyTwoColors;
            pLeastPopularColor.thePosition = new Point(ix, iy);

        }


        private static void FindLeastPopularPatternInSmallImage(
            Bitmap sub,
            int subwidth,
            int subheight,
            int strideSub,
            byte[] dataSub,
            ref LeastPopularPattern pLeastPopularPattern,
            ref LeastPopularPattern pLeastPopularPattern2,
            Dictionary<MyPattern, int[]> repeatsPattern,
            ref bool[,] boolArySmallImage)
        {
            // boolArySmallImage is an array that contains
            // true for an element if the corresponding pixel
            // is identical to the next pixel; otherwise, it is false

            for (int y = 0; y < sub.Height; y++)
            {
                for (int x = 0; x < sub.Width; x++)
                {
                    int xtemp = x + 1;
                    int ytemp = y;
                    if (xtemp > sub.Width - 1)
                    {
                        xtemp = 0;
                        ytemp = y + 1;
                        if (ytemp > sub.Height - 1)
                        {
                            ytemp = 0;
                        }
                    }
                    MyColor curcolor = GetColor(x, y, strideSub, dataSub);
                    MyColor curcolor2 = GetColor(xtemp, ytemp, strideSub, dataSub);

                    if (curcolor.Equals(curcolor2))
                    {
                        boolArySmallImage[x, y] = true;
                        continue;
                    }
                    else
                    {
                        boolArySmallImage[x, y] = false;
                    }
                }
            }

            // this is used if we are trying to find patterns
            for (int y1 = 0; y1 < sub.Height; y1++)
            {
                for (int x1 = 0; x1 < sub.Width; x1++)
                {

                    MyPattern _myPattern = MyPattern.GetPatternInBoolArySmallImage(x1, y1, boolArySmallImage, sub.Width, sub.Height);

                    // The pixel value has been found before
                    if (!repeatsPattern.ContainsKey(_myPattern))
                    {
                        // a = {number times found, x location, y location}
                        int[] a = { 1, x1, y1 };
                        repeatsPattern.Add(_myPattern, a);
                    }
                    else
                    {
                        // Increment the number of times the values been found
                        ((int[])repeatsPattern[_myPattern])[0]++;
                    }
                }
            }

            // Find the pattern that has been found the least
            // number of times; a[0] is count; a[1] is x; a[2] is y; image[ix, iy] 
            // is the color of the pixel
            int minP, ixP, iyP;
            minP = int.MaxValue;
            ixP = -1;
            iyP = -1;
            MyPattern oMyPattern = new MyPattern();
            oMyPattern.ColorWrapX = -1;
            oMyPattern.ColorWrapY = -1;
            oMyPattern.disgard = false;
            foreach (var de in repeatsPattern)
            {
                int[] a = (int[])de.Value;
                if (a[0] < minP)
                {
                    minP = a[0];
                    ixP = a[1];
                    iyP = a[2];
                    oMyPattern = de.Key;
                }
            }

            pLeastPopularPattern.thePattern = oMyPattern;
            pLeastPopularPattern.thePosition = new Point(ixP, iyP);
            int intholdixP = iyP;
            int maxixP = ixP + 10;
            int holdiyP = iyP;
            List<MyPattern> myKeys = new List<MyPattern>();
            foreach (var de in repeatsPattern)
            {
                MyPattern oMyPattern1 = de.Key;
                int[] a = (int[])de.Value;
                ixP = a[1];
                iyP = a[2];
                if (ixP >= intholdixP && ixP <= maxixP && iyP == holdiyP)
                {
                    myKeys.Add(de.Key);
                }
                if (ixP < 10 &&  iyP == holdiyP + 1 && oMyPattern1.IndexWrapX > -1)
                {
                    myKeys.Add(de.Key);
                }
            }
            foreach (var item in myKeys)
            {
                repeatsPattern.Remove(item);
            }
            

            minP = int.MaxValue;
            ixP = -1;
            iyP = -1;
            oMyPattern = new MyPattern();
            oMyPattern.ColorWrapX = -1;
            oMyPattern.ColorWrapY = -1;
            oMyPattern.disgard = false;
            foreach (var de in repeatsPattern)
            {
                int[] a = (int[])de.Value;
                if (a[0] < minP)
                {
                    minP = a[0];
                    ixP = a[1];
                    iyP = a[2];
                    oMyPattern = de.Key;
                }
            }

            pLeastPopularPattern2.thePattern = oMyPattern;
            pLeastPopularPattern2.thePosition = new Point(ixP, iyP);
            repeatsPattern.Remove(oMyPattern);
        }


        private static void CreateDataSubByteArray(Bitmap sub, int subwidth, int subheight, out BitmapData bmSubData, out int bytesSub, out int strideSub, out System.IntPtr Scan0Sub, out byte[] dataSub)
        {
            bmSubData = sub.LockBits(new Rectangle(0, 0, subwidth, subheight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            bytesSub = Math.Abs(bmSubData.Stride) * subheight;
            strideSub = bmSubData.Stride;
            Scan0Sub = bmSubData.Scan0;
            dataSub = new byte[bytesSub];
            System.Runtime.InteropServices.Marshal.Copy(Scan0Sub, dataSub, 0, bytesSub);
        }

        private static void CreateDataMainByteArray(Bitmap main, int mainwidth, int mainheight, out byte[] dataMain, out BitmapData bmMainData, out int bytesMain, out int strideMain, out System.IntPtr Scan0Main)
        {
            bmMainData = main.LockBits(new Rectangle(0, 0, mainwidth, mainheight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            bytesMain = Math.Abs(bmMainData.Stride) * mainheight;
            strideMain = bmMainData.Stride;
            Scan0Main = bmMainData.Scan0;
            dataMain = new byte[bytesMain];
            System.Runtime.InteropServices.Marshal.Copy(Scan0Main, dataMain, 0, bytesMain);
        }

        private static MyColor GetColor(Point point, int stride, byte[] data)
        {
            return GetColor(point.X, point.Y, stride, data);
        }

        private static MyColor GetColor(int x, int y, int stride, byte[] data)
        {
            int pos = y * stride + x * 4;
            if (pos + 3 > data.Length)
            {
                return MyColor.FromARGB(0x00, 0x00, 0x00, 0x00);
            }
            byte a = data[pos + 3];
            byte r = data[pos + 2];
            byte g = data[pos + 1];
            byte b = data[pos + 0];
            return MyColor.FromARGB(a, r, g, b);
        }

        struct MyColor
        {
            public byte A;
            public byte R;
            public byte G;
            public byte B;

            public static MyColor FromARGB(byte a, byte r, byte g, byte b)
            {
                MyColor mc = new MyColor();
                mc.A = a;
                mc.R = r;
                mc.G = g;
                mc.B = b;
                return mc;
            }

            public override bool Equals(object obj)
            {
                // this is big image color and obj is small image color
                if (!(obj is MyColor))
                    return false;
                MyColor color = (MyColor)obj;
                int intColorDiff = (color.R - this.R) +
                    (color.G - this.G) +
                     (color.B - this.B);
                if (intColorDiff < 0)
                {
                    intColorDiff = intColorDiff * -1;
                }
                if (intColorDiff < 11)
                    return true;
                return false;
            }
        }
        struct MyPattern
        {
            public bool bool1;
            public bool bool2;
            public bool bool3;
            public bool bool4;
            public bool bool5;
            public bool bool6;
            public bool bool7;
            public bool bool8;
            public bool bool9;
            public bool bool10;
            public int IndexWrapX;
            public int IndexWrapY;
            public int ColorWrapX;
            public int ColorWrapY;
            public bool disgard;

            public static MyPattern GetPatternInBoolArySmallImage(int x, int y, bool[,] myBoolArySmallImage, int smallWidth, int smallHeight)
            {
                MyPattern mp = new MyPattern();
                mp.ColorWrapX = -1;
                mp.ColorWrapY = -1;
                mp.disgard = false;
                int intOrigX = x;
                int intOrigY = y;
                mp.IndexWrapX = -1;
                mp.IndexWrapY = -1;
                mp.bool1 = myBoolArySmallImage[x, y];
                GetNextArrayElementInBoolArySmallImage(ref x, ref y, myBoolArySmallImage, smallWidth, smallHeight, intOrigX, intOrigY, 2, ref mp);
                mp.bool2 = myBoolArySmallImage[x, y];
                GetNextArrayElementInBoolArySmallImage(ref x, ref y, myBoolArySmallImage, smallWidth, smallHeight, intOrigX, intOrigY, 3, ref mp);
                mp.bool3 = myBoolArySmallImage[x, y];
                GetNextArrayElementInBoolArySmallImage(ref x, ref y, myBoolArySmallImage, smallWidth, smallHeight, intOrigX, intOrigY, 4, ref mp);
                mp.bool4 = myBoolArySmallImage[x, y];
                GetNextArrayElementInBoolArySmallImage(ref x, ref y, myBoolArySmallImage, smallWidth, smallHeight, intOrigX, intOrigY, 5, ref mp);
                mp.bool5 = myBoolArySmallImage[x, y];
                GetNextArrayElementInBoolArySmallImage(ref x, ref y, myBoolArySmallImage, smallWidth, smallHeight, intOrigX, intOrigY, 6, ref mp);
                mp.bool6 = myBoolArySmallImage[x, y];
                GetNextArrayElementInBoolArySmallImage(ref x, ref y, myBoolArySmallImage, smallWidth, smallHeight, intOrigX, intOrigY, 7, ref mp);
                mp.bool7 = myBoolArySmallImage[x, y];
                GetNextArrayElementInBoolArySmallImage(ref x, ref y, myBoolArySmallImage, smallWidth, smallHeight, intOrigX, intOrigY, 8, ref mp);
                mp.bool8 = myBoolArySmallImage[x, y];
                GetNextArrayElementInBoolArySmallImage(ref x, ref y, myBoolArySmallImage, smallWidth, smallHeight, intOrigX, intOrigY, 9, ref mp);
                mp.bool9 = myBoolArySmallImage[x, y];
                GetNextArrayElementInBoolArySmallImage(ref x, ref y, myBoolArySmallImage, smallWidth, smallHeight, intOrigX, intOrigY, 10, ref mp);
                mp.bool10 = myBoolArySmallImage[x, y];
                return mp;
            }

            public static MyPattern GetPatternInBigImage(
                int x,
                int y,
                LeastPopularPattern pLeastPopularPattern,
                int smallWidth,
                int smallHeight,
                int strideMain,
                byte[] dataMain,
                int bigWidth,
                int bigHeight
                )
            {
                // First get two colors for the current pixel
                // and the next one; if they are the same,
                // the bool result for that pixel is true; otherwise,
                // it is false. This is very similar to what I did when
                // creating boolarray for small image
                MyPattern mp = new MyPattern();
                mp.ColorWrapX = -1;
                mp.ColorWrapY = -1;
                mp.disgard = false;
                int intOrigX = x;
                int intOrigY = y;
                mp.IndexWrapX = -1;
                mp.IndexWrapY = -1;
                int patternpixelposition;
                patternpixelposition = 1;
                mp.bool1 = GetBoolForPixelinBigImageForPixelPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
                patternpixelposition = 2;
                if (mp.disgard == false)
                {
                    mp.bool2 = GetBoolForPixelinBigImageForPixelPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
                }
                patternpixelposition = 3;
                if (mp.disgard == false)
                {
                    mp.bool3 = GetBoolForPixelinBigImageForPixelPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
                }
                patternpixelposition = 4;
                if (mp.disgard == false)
                {
                    mp.bool4 = GetBoolForPixelinBigImageForPixelPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
                }
                patternpixelposition = 5;
                if (mp.disgard == false)
                {
                    mp.bool5 = GetBoolForPixelinBigImageForPixelPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
                }
                patternpixelposition = 6;
                if (mp.disgard == false)
                {
                    mp.bool6 = GetBoolForPixelinBigImageForPixelPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
                }
                patternpixelposition = 7;
                if (mp.disgard == false)
                {
                    mp.bool7 = GetBoolForPixelinBigImageForPixelPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
                }
                patternpixelposition = 8;
                if (mp.disgard == false)
                {
                    mp.bool8 = GetBoolForPixelinBigImageForPixelPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
                }
                patternpixelposition = 9;
                if (mp.disgard == false)
                {
                    mp.bool9 = GetBoolForPixelinBigImageForPixelPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
                }
                patternpixelposition = 10;
                if (mp.disgard == false)
                {
                    mp.bool10 = GetBoolForPixelinBigImageForPixelPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
                }

                return mp;
            }

            private static bool GetBoolForPixelinBigImageForPixelPatternPosition(ref int x, ref int y, ref LeastPopularPattern pLeastPopularPattern, int smallWidth, int smallHeight, int strideMain, byte[] dataMain, int bigWidth, int bigHeight, ref MyPattern mp, int patternpixelposition)
            {
                if (pLeastPopularPattern.thePattern.IndexWrapX == patternpixelposition)
                {
                    mp.IndexWrapX = patternpixelposition;
                    x = x - smallWidth;
                    y = y + 1;
                }
                if (pLeastPopularPattern.thePattern.IndexWrapY == patternpixelposition)
                {
                    mp.IndexWrapY = patternpixelposition;
                    x = x - smallWidth;
                    y = y - smallHeight;
                }
                // if the indexes are outside the bounds of 
                // the big image, we say there is no match                     
                if (x > bigWidth - 1 || x < 0)
                {
                    mp.disgard = true;
                    return false;
                }

                if (y > bigHeight - 1 || y < 0)
                {
                    mp.disgard = true;
                    return false;
                }
                int xtemp = x + 1;
                int ytemp = y;
                if (pLeastPopularPattern.thePattern.ColorWrapX == patternpixelposition)
                {
                    xtemp = xtemp - smallWidth;
                    ytemp = ytemp + 1;
                }
                if (pLeastPopularPattern.thePattern.ColorWrapY == patternpixelposition)
                {
                    xtemp = (x + 1) - smallWidth;
                    ytemp = (y + 1) - smallHeight;
                }

                if (xtemp > bigWidth - 1 || xtemp < 0)
                {
                    mp.disgard = true;
                    return false;
                }

                if (ytemp > bigHeight - 1 || ytemp < 0)
                {
                    mp.disgard = true;
                    return false;
                }

                MyColor curcolor = GetColor(x, y, strideMain, dataMain);
                MyColor curcolor2 = GetColor(xtemp, ytemp, strideMain, dataMain);

                if (curcolor.Equals(curcolor2))
                {
                    x = x + 1;
                    return true;
                }
                else
                {
                    x = x + 1;
                    return false;
                }
            }

            public override bool Equals(object obj)
            {
                // this is big image color and obj is small image color
                if (!(obj is MyPattern))
                    return false;
                MyPattern myBool = (MyPattern)obj;

                if (myBool.bool1 == this.bool1
                  && myBool.bool2 == this.bool2
                  && myBool.bool3 == this.bool3
                  && myBool.bool4 == this.bool4
                  && myBool.bool5 == this.bool5
                  && myBool.bool6 == this.bool6
                  && myBool.bool7 == this.bool7
                  && myBool.bool8 == this.bool8
                  && myBool.bool9 == this.bool9
                  && myBool.bool10 == this.bool10)
                    return true;
                return false;
            }

            private static void GetNextArrayElementInBoolArySmallImage(ref int x,
                ref int y,
                bool[,] myBoolArySmallImage,
                int smallWidth,
                int smallHeight,
                int origX,
                int origY,
                int patternpixel,
                ref MyPattern mp
                )
            {
                x++;
                if (x == smallWidth - 1)
                {
                    mp.ColorWrapX = patternpixel;
                }
                if (x == smallWidth - 1 && y == smallHeight - 1)
                {
                    mp.ColorWrapY = patternpixel;
                }
                if (x > smallWidth - 1)
                {
                    mp.IndexWrapX = patternpixel;
                    x = 0;
                    y++;
                    if (y > smallHeight - 1)
                    {
                        mp.IndexWrapY = patternpixel;
                        y = 0;
                    }

                }
            }
        }
        struct LeastPopularColor
        {
            public MyTwoColors theTwoColors;
            public Point thePosition;
        }
        struct LeastPopularPattern
        {
            public MyPattern thePattern;
            public Point thePosition;
        }
        struct MyTwoColors
        {
            public byte A;
            public byte R;
            public byte G;
            public byte B;
            public byte A2;
            public byte R2;
            public byte G2;
            public byte B2;
            public int IndexWrapX;
            public int IndexWrapY;

            public static MyTwoColors FromARGB2(byte a, byte r, byte g, byte b, byte a2, byte r2, byte g2, byte b2, int X, int Y)
            {
                MyTwoColors mc2 = new MyTwoColors();
                mc2.A = a;
                mc2.R = r;
                mc2.G = g;
                mc2.B = b;
                mc2.A = a2;
                mc2.R = r2;
                mc2.G = g2;
                mc2.B = b2;
                mc2.IndexWrapX = X;
                mc2.IndexWrapY = Y;
                return mc2;
            }

            public override bool Equals(object obj)
            {
                // this is big image color and obj is small image color
                if (!(obj is MyTwoColors))
                    return false;
                MyTwoColors color = (MyTwoColors)obj;
                int intColorDiff = (color.R - this.R) +
                    (color.G - this.G) +
                     (color.B - this.B) +
                     (color.R2 - this.R2) +
                    (color.G2 - this.G2) +
                     (color.B2 - this.B2);
                if (intColorDiff < 0)
                {
                    intColorDiff = intColorDiff * -1;
                }
                if (intColorDiff < 11)
                    return true;
                return false;
            }
        }
        [DllImport("user32.dll")]
        static extern IntPtr GetOpenClipboardWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowThreadProcessId(IntPtr hWnd, out int
        lpdwProcessId);
        /// <summary>
        /// Initialize global data structures
        /// </summary>
        /// <remarks>
        /// The bitmap returned is in PixelFormat.Format24bppRgb.
        /// </remarks>
        /// <returns>Current screen image</returns>
        public static Bitmap getDesktopBitmap(bool UseGrayScale)
        {
            double x = Screen.PrimaryScreen.Bounds.X;
            double y = Screen.PrimaryScreen.Bounds.Y;
            double width = Screen.PrimaryScreen.Bounds.Width;
            double height = Screen.PrimaryScreen.Bounds.Height;
            int ix, iy, iw, ih;
            ix = Convert.ToInt32(x);
            iy = Convert.ToInt32(y);
            iw = Convert.ToInt32(width);
            ih = Convert.ToInt32(height);
            Bitmap image = new Bitmap(iw, ih, System.Drawing.Imaging.PixelFormat.Format32bppArgb);


            Graphics g = Graphics.FromImage(image);
            g.CopyFromScreen(ix, iy, ix, iy, new System.Drawing.Size(iw, ih), CopyPixelOperation.SourceCopy);

            Image myImage = image;
            // myImage = Image.FromFile(@"C:\TFS\WadeHome\Applications\TreeView\Sample Application\Images\Big.png");
            if (UseGrayScale)
            {
                myImage = ConvertToGrayscale(myImage);
                int intFileCtr = 0;
                intFileCtr += 200;
                string directory = AppDomain.CurrentDomain.BaseDirectory;
                string myfile = "temp" + intFileCtr + ".bmp";
                System.IO.File.Delete(directory + myfile);
                myImage.Save(directory + myfile, System.Drawing.Imaging.ImageFormat.Bmp);
            }
            Bitmap bm = new Bitmap(myImage);
            myImage.Dispose();

            return bm;
        }


        /// <summary>
        /// Initialize global data structures
        /// </summary>
        /// <remarks>
        /// Populates image with the Argb values of bmImage and
        /// searches through the Argb values to find a pixle value
        /// that occurs least often in the image.  This value is 
        /// placed into the hashtable pixels, with a value that 
        /// corosponds to the position of the pixle in the image.
        /// </remarks>
        /// <param name="bmImage">Subimage to search for</param>

        /// <summary>
        /// Finds if the image occurs at location loc 
        /// </summary>
        /// <remarks>
        /// Does a simple pixle by pixle comparison of the image 
        /// and the area starting at sx, sy in the desktop image bmd. 
        /// </remarks>
        /// <param name="bmd">Bitmap data object</param>
        /// <param name="sx">Search start location (x component)</param>
        /// <param name="sy">Search start location (y component)</param>
        /// <returns>True if there is an occurence of the image at sx, sy</returns>
        private static bool imageThere(Byte[] dataMain, int sx, int sy, Byte[] dataSub, Bitmap sub, int strideMain, int strideSub,  ref decimal highestPercentCorrect, int intTolerance)
        {
            // Console.WriteLine("imageThere sy=" + sy.ToString() + "sx=" + sx.ToString()); 
            int yMain = 0;
            int xMain = 0;
            int xSub;            
            int ySub;
            int intRight = 0;
            int intWrong = 0;
            int intSaveY = -1;
            int intStartLineWrong = -1;
            int intEndLineWrong = 0;
            int intNumberIgnored = 0;
            for (ySub = 0; ySub < sub.Height; ySub++)
            {
                // Horizontal line of pixles in the main bitmap data               
                yMain = sy + ySub;
                for (xSub = 0; xSub < sub.Width; xSub++)
                {
                    xMain = (sx + xSub);
                    MyColor myBigColor = GetColor(xMain, yMain, strideMain, dataMain);
                    MyColor mySmallColor = GetColor(xSub, ySub, strideSub, dataSub);

                    int intColorDiff = (mySmallColor.R - myBigColor.R) +
                        (mySmallColor.G - myBigColor.G) +
                        (mySmallColor.B - myBigColor.B);
                    if (intColorDiff < 0)
                    {
                        intColorDiff = intColorDiff * -1;
                    }
                    // I want to ignore everything that is wrong on a row 
                    // from the first yellow pixel on that row until the last one

                    // this identifies the first yellow pixel on a row
                    if (ySub != intSaveY && mySmallColor.B == 0 && mySmallColor.R > 0)
                    {
                        intSaveY = ySub;
                        intStartLineWrong = intWrong;
                    }

                    // this identifies the last yellow pixel on a row
                    if (ySub == intSaveY && mySmallColor.B == 0 && mySmallColor.R > 0)
                    {
                        intSaveY = ySub;
                        intEndLineWrong = intWrong;
                        intNumberIgnored += 1;
                    }

                    // we are changing rows and found some yellow on previous row
                    // we need to adjust wrong and reset intStartLineWrong
                    if (ySub != intSaveY && intStartLineWrong != -1)
                    {
                        intWrong = intWrong - (intEndLineWrong - intStartLineWrong) - 1;
                        intStartLineWrong = -1;
                    }
                    if (intColorDiff > 10)
                    {

                        intWrong += 1;
                    }
                    else
                    {
                        intRight += 1;
                    }

                }
                xSub = 0;
            }

            int intTotal = intWrong + intRight;
            decimal intPercentRight = (intRight * 100) / intTotal;
            if (intPercentRight > highestPercentCorrect)
            {
                highestPercentCorrect = intPercentRight;
            }
            if (intWrong < intRight && intWrong < 10 || (intTolerance != 0 && intPercentRight > intTolerance))
            {
                //Console.WriteLine(strSubImageFileName + " intRight =" + intRight.ToString() + " intWrong =" + intWrong.ToString() + " yMain=" + yMain.ToString() + " xMain=" + xMain.ToString() + "Found it");
                return true;
            }
            else
            {
                //if ((intRight / (intRight + intWrong)) * 100 > 75)
                //{
                //    Console.WriteLine(strSubImageFileName + " intRight =" + intRight.ToString() + " intWrong =" + intWrong.ToString() + " yMain=" + yMain.ToString() + " xMain=" + xMain.ToString());
                //}
                return false;
            }
        }
        private static bool imageTherePattern(int sx, int sy, bool[,] boolArySmallImage, Bitmap sub, int strideMain, int strideSub,  byte[] dataMain, int pMainWidth, int pMainHeight, ref decimal highestPercentCorrect, int intTolerance)
        {
            // debugging line next
            //if (sy == 735 && sx == 101)
            //{
            //    string wade = "abc";
            //}
            //  Console.WriteLine("imageTherePattern sy=" + sy.ToString() + "sx=" + sx.ToString());
            int yMain = 0;
            int xMain = 0;
            int xSub;
            int ySub;
            int intRight = 0;
            int intWrong = 0;
            int intSaveY = -1;
            int intStartLineWrong = -1;
            int intEndLineWrong = 0;
            int intNumberIgnored = 0;
            // sx and sy tell me the original point in bigImage
            // First, I need to loop thru dimensions of smallImage 
            // within bigImage in order to create a boolArray like 
            // I did for smallImage when I was looking for LeastPopularPattern
            // After I have created a boolArrayForRectangleInBigImage,
            // I just compare boolArrayForSmallImage to 
            // boolArrayForRectangleInBigImage
            bool[,] boolArrayForRectangleInBigImage = new bool[sub.Width, sub.Height];
            for (ySub = 0; ySub < sub.Height; ySub++)
            {
                // Horizontal line of pixles in the main bitmap data               
                yMain = sy + ySub;
                for (xSub = 0; xSub < sub.Width - 1; xSub++)
                {
                    xMain = (sx + xSub);
                    if (xMain > pMainWidth)
                    {
                        return false;
                    }
                    if (yMain > pMainHeight)
                    {
                        return false;
                    }
                    int xtemp = xMain + 1;
                    int ytemp = yMain;
                    if (xSub == sub.Width - 1)
                    {
                        xtemp = xtemp - sub.Width;
                        ytemp = ytemp + 1;
                        if (ySub == sub.Height - 1)
                        {
                            ySub = (ySub + 1) - sub.Height;
                        }
                    }
                    MyColor curcolor = GetColor(xMain, yMain, strideMain, dataMain);
                    MyColor curcolor2 = GetColor(xtemp, ytemp, strideMain, dataMain);

                    if (curcolor.Equals(curcolor2))
                    {
                        boolArrayForRectangleInBigImage[xSub, ySub] = true;
                        continue;
                    }
                    else
                    {
                        boolArrayForRectangleInBigImage[xSub, ySub] = false;
                    }
                }
            }
            for (ySub = 0; ySub < sub.Height; ySub++)
            {
                for (xSub = 0; xSub < sub.Width; xSub++)
                {

                    if (boolArrayForRectangleInBigImage[xSub, ySub] ==
                        boolArySmallImage[xSub, ySub])
                    {
                        //if (sy > 730)
                        //{
                        //    Console.WriteLine("Right xSub=" + xSub.ToString() + "ySub=" + ySub.ToString() + "bigImage=" + boolArrayForRectangleInBigImage[xSub, ySub] + "smallImage=" + boolArySmallImage[xSub, ySub]);
                        //}
                        intRight += 1;
                    }
                    else
                    {
                        //if (sy > 730)
                        //{
                        //    Console.WriteLine("Wrong xSub=" + xSub.ToString() + "ySub=" + ySub.ToString() + "bigImage=" + boolArrayForRectangleInBigImage[xSub, ySub] + "smallImage=" + boolArySmallImage[xSub, ySub]);
                        //}
                        intWrong += 1;
                    }
                    if (intWrong > 2500)
                    {
                        //    Console.WriteLine(strSubImageFileName + " intRight =" + intRight.ToString() + " intWrong =" + intWrong.ToString() + " yMain=" + yMain.ToString() + " xMain=" + xMain.ToString());
                        return false;
                    }
                }
                xSub = 0;
            }
            int intTotal = intWrong + intRight;
            decimal intPercentRight = (intRight * 100) / intTotal;
            if (intPercentRight > highestPercentCorrect)
            {
                highestPercentCorrect = intPercentRight;
            }
            if (intPercentRight > 86 || (intTolerance != 0 && intPercentRight > intTolerance))
            {
               // Console.WriteLine(strSubImageFileName + " intRight =" + intRight.ToString() + " intWrong =" + intWrong.ToString() + " yMain=" + yMain.ToString() + " xMain=" + xMain.ToString() + "PercentRight=" + intPercentRight.ToString() + "Found it");
                return true;
            }
            else
            {
                //Console.WriteLine(strSubImageFileName + " intRight =" + intRight.ToString() + " intWrong =" + intWrong.ToString() + " yMain=" + yMain.ToString() + " xMain=" + xMain.ToString() + "PercentRight=" + intPercentRight.ToString() + "Did not find it");
                
                //if ((intRight / (intRight + intWrong)) * 100 > 75)
                //{
                //        Console.WriteLine(strSubImageFileName + " intRight =" + intRight.ToString() + " intWrong =" + intWrong.ToString() + " yMain=" + yMain.ToString() + " xMain=" + xMain.ToString() + " intPercentRight=" + intPercentRight.ToString());
                //}
                return false;
            }
        }

        /// <summary>
        /// Determines if point x, y is in any of the found rectangles
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>True if point x, y is not contained in any rectangle in foundRects</returns>
        private static bool notFound(int x, int y, List<Rectangle> foundRects)
        {
            foreach (Rectangle r in foundRects)
            {
                if (r.Contains(new Point(x, y)))
                    return false;
            }

            return true;
        }
        public static Image ConvertToGrayscale(Image image)
        {
            Image grayscaleImage = new Bitmap(image.Width, image.Height, image.PixelFormat);

            // Create the ImageAttributes object and apply the ColorMatrix
            ImageAttributes attributes = new System.Drawing.Imaging.ImageAttributes();
            ColorMatrix grayscaleMatrix = new ColorMatrix(new float[][]{
        new float[] {0.299f, 0.299f, 0.299f, 0, 0},
        new float[] {0.587f, 0.587f, 0.587f, 0, 0},
        new float[] {0.114f, 0.114f, 0.114f, 0, 0},
        new float[] {     0,      0,      0, 1, 0},
        new float[] {     0,      0,      0, 0, 1}
        });
            attributes.SetColorMatrix(grayscaleMatrix);

            // Use a new Graphics object from the new image.
            using (Graphics g = Graphics.FromImage(grayscaleImage))
            {
                // Draw the original image using the ImageAttributes created above.
                g.DrawImage(image,
                            new Rectangle(0, 0, grayscaleImage.Width, grayscaleImage.Height),
                            0, 0, grayscaleImage.Width, grayscaleImage.Height,
                            GraphicsUnit.Pixel,
                            attributes);
            }

            return grayscaleImage;
        }
    }
}
