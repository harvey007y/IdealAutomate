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
using IdealAutomateCore;


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
namespace IdealAutomate.Core {
  class Scraper {
    struct objErrors {
      public int intWrong;
      public int intRight;
      public int intYellowErrors;
      public int intSaveY;
      public int intStartLineWrong;
      public int intEndLineWrong;
      public int intNumberIgnored;


    }

        public static List<SubPositionInfo> GetSubPositions(Bitmap main,
    Bitmap sub,
    bool boolUseGrayScale,
    ref decimal highestPercentCorrect,
    int intTolerance,
    ref Dictionary<string, System.Drawing.Point> dictImagesFoundCached,
    string ImageFile,
    bool boolStopOnPerfectMatch
    )
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

            MostPopularColorSmallImage oMostPopularColorSmallImage = new MostPopularColorSmallImage();
            LeastPopularColor oLeastPopularColor = new LeastPopularColor();
            LeastPopularPattern oLeastPopularPattern = new LeastPopularPattern();
            LeastPopularPattern oLeastPopularPattern2 = new LeastPopularPattern();

            Dictionary<MyTwoColors, int[]> repeats = new Dictionary<MyTwoColors, int[]>();
            Dictionary<MyColor, int[]> repeatsMostPopular = new Dictionary<MyColor, int[]>();
            Dictionary<MyPattern, int[]> repeatsPattern = new Dictionary<MyPattern, int[]>();

            List<Point> lstPrelimBackground = new List<Point>();
            List<Point> lstPrelimForeground = new List<Point>();
            List<Point> lstHighContrast = new List<Point>();

            Dictionary<MyColor, MostPopularColorSmallImage> dictMostPopularColorSmallImage = new Dictionary<MyColor, MostPopularColorSmallImage>();

            Dictionary<Point, objErrors> dictionary = new Dictionary<Point, objErrors>();
            StringBuilder sb = new StringBuilder();

            if (boolUseGrayScale == false)
            {

                FindLeastPopularColorInSmallImage(sub, subwidth, subheight, strideSub, dataSub, ref oLeastPopularColor, repeats);

                //try
                //{
                LoopthruEachPixelInBigImageToFindMatchesOnSmallImageLeastPopularColor(main, sub, possiblepos, foundRects, mainwidth, mainheight, dataMain, strideMain, strideSub, dataSub, oLeastPopularColor, ref highestPercentCorrect, intTolerance, ref dictImagesFoundCached, ImageFile, boolStopOnPerfectMatch);
                //}
                //catch (Exception ex)
                //{
                //    System.Diagnostics.Debugger.Break();
                //    throw;
                //}

            }
            else
            {
                bool[,] boolArySmallImage = new bool[subwidth, subheight];
                //FindLeastPopularHighContrastPatternInSmallImage(sub, subwidth, subheight, strideSub, dataSub, ref oLeastPopularPattern, ref oLeastPopularPattern2, repeatsPattern, ref boolArySmallImage);
                ////   System.Diagnostics.Debugger.Break();
                //Logging.WriteLogSimple(Logging.DumpObject(oLeastPopularPattern.thePattern));
                //Logging.WriteLogSimple(Logging.DumpObject(oLeastPopularPattern.thePosition));
                //Logging.WriteLogSimple(Logging.DumpObject(oLeastPopularPattern2.thePattern));
                //Logging.WriteLogSimple(Logging.DumpObject(oLeastPopularPattern2.thePosition));
                //LoopthruEachPixelInBigImageToFindMatchesOnSmallImageLeastPopularPattern(main, sub, possiblepos, foundRects, mainwidth, mainheight, dataMain, strideMain, strideSub, dataSub, oLeastPopularPattern, boolArySmallImage, ref highestPercentCorrect, intTolerance);
                //if (foundRects.Count == 0) {
                //  LoopthruEachPixelInBigImageToFindMatchesOnSmallImageLeastPopularPattern(main, sub, possiblepos, foundRects, mainwidth, mainheight, dataMain, strideMain, strideSub, dataSub, oLeastPopularPattern2, boolArySmallImage, ref highestPercentCorrect, intTolerance);
                //}
                /*
                 * x1. In the small image, get the most popular color, which will be the background color for small picture.
                 * x2. In the small image, find 3 background pixels in a row and select the location of the middle one as 
                 *     a safe place to get the background color for big image.
                 * x3. In the small image, everytime the foreground/background change 
                 *     and there are at least 3 of the new kind in a row, add the location of the middle 
                 *     one to one of two lists (Background/Foreground)of safe preliminary points to check. 
                 * x4. Get background for big picture.
                 * x5. Do preliminary check in big picture that tests at least 5 background pixels and 5 foreground pixels
                 *     and quits when error rate rises above a certain percentage. If there are not 5 background or
                 *     5 foreground pixels in small image, just proceed to complete check.
                 * x6. Do complete check in big picture if big picture passes preliminary check.
                 */
                // System.Diagnostics.Debugger.Break();
                MyColor oDarkestColorSmallImage = new MyColor();
                List<Point> olstDarkestColorSmallImage = new List<Point>();
                boolArySmallImage = new bool[subwidth, subheight];
                // FindMostPopularColorInSmallImage is going to find the most popular colors in the small image
                // and put the location of at least five of them in lstPrelimBackground
                // It will also put the location of at least five foreground pixels in lstPrelimForeground
                // If it cannot find 5, it will show popup message saying we cannot use this strategy on this image
                Logging.WriteLogSimple("FindMostPopularColorInSmallImage");
                FindMostPopularColorInSmallImage(sub,
                  subwidth,
                  subheight,
                  strideSub,
                  dataSub,
                  ref oMostPopularColorSmallImage,
                  repeatsMostPopular,
                  ref lstPrelimBackground,
                  ref lstPrelimForeground,
                  ref dictMostPopularColorSmallImage,
                  ref lstHighContrast,
                  ref oDarkestColorSmallImage,
                  ref olstDarkestColorSmallImage);
                Logging.WriteLogSimple("LoopthruEachPixelInBigImageToFindMatchesOnSmallImageBackgroundPattern");
                LoopthruEachPixelInBigImageToFindMatchesOnSmallImageBackgroundPattern(main,
                  sub,
                  possiblepos,
                  foundRects,
                  mainwidth,
                  mainheight,
                  dataMain,
                  strideMain,
                  strideSub,
                  dataSub,
                  oMostPopularColorSmallImage,
                  boolArySmallImage,
                  ref highestPercentCorrect,
                  intTolerance,
                  lstPrelimBackground,
                  lstPrelimForeground,
                  ref dictMostPopularColorSmallImage,
                  lstHighContrast,
                  ref oDarkestColorSmallImage,
                  ref olstDarkestColorSmallImage,
                 ref dictImagesFoundCached,
                 ImageFile,
                 boolStopOnPerfectMatch
                 );
            }

            // Here is the boolUseGrayScale stuff I deleted
            //bool[,] boolArySmallImage = new bool[subwidth, subheight];
            //FindLeastPopularPatternInSmallImage(sub, subwidth, subheight, strideSub, dataSub, ref oLeastPopularPattern, ref oLeastPopularPattern2, repeatsPattern, ref boolArySmallImage);
            //LoopthruEachPixelInBigImageToFindMatchesOnSmallImageLeastPopularPattern(main, sub, possiblepos, foundRects, mainwidth, mainheight, dataMain, strideMain, strideSub, dataSub, oLeastPopularPattern, boolArySmallImage, ref highestPercentCorrect, intTolerance);
            //if (foundRects.Count == 0) {
            //  LoopthruEachPixelInBigImageToFindMatchesOnSmallImageLeastPopularPattern(main, sub, possiblepos, foundRects, mainwidth, mainheight, dataMain, strideMain, strideSub, dataSub, oLeastPopularPattern2, boolArySmallImage, ref highestPercentCorrect, intTolerance);
            //}

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
           int intTolerance,
           ref Dictionary<string, Point> dictImageFoundCached,
           string imageFile,
           bool boolStopOnPerfectMatch)
        {
            if (dictImageFoundCached.ContainsKey(imageFile) && boolStopOnPerfectMatch)
            {
                System.Drawing.Point cachedPoint = new System.Drawing.Point(0, 0);
                dictImageFoundCached.TryGetValue(imageFile, out cachedPoint);

                if (imageThere(dataMain, cachedPoint.X, cachedPoint.Y, dataSub, sub, strideMain, strideSub, ref highestPercentCorrect, intTolerance))
                {

                    SubPositionInfo mySubPositionInfo = new SubPositionInfo();
                    mySubPositionInfo.myPoint = new Point(cachedPoint.X, cachedPoint.Y);
                    mySubPositionInfo.percentcorrect = highestPercentCorrect;
                    mySubPositionInfo.strSearchMethod = "UseColorWithLeastPopularColorSearch";
                    if (mySubPositionInfo.percentcorrect > 99)
                    {
                        if (!possiblepos.Contains(mySubPositionInfo))
                        {
                            possiblepos.Add(mySubPositionInfo);
                        }
                        highestPercentCorrect = 0;
                        if (!foundRects.Contains(new Rectangle(cachedPoint.X, cachedPoint.Y, sub.Width, sub.Height)))
                        {
                            foundRects.Add(new Rectangle(cachedPoint.X, cachedPoint.Y, sub.Width, sub.Height));
                        }
                        if (dictImageFoundCached.ContainsKey(imageFile))
                        {
                            dictImageFoundCached[imageFile] = mySubPositionInfo.myPoint;
                        }
                        else
                        {
                            dictImageFoundCached.Add(imageFile, mySubPositionInfo.myPoint);
                        }
                        return;
                    }
                }
            }
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
                            if (imageThere(dataMain, sx, sy, dataSub, sub, strideMain, strideSub, ref highestPercentCorrect, intTolerance))
                            {
                                SubPositionInfo mySubPositionInfo = new SubPositionInfo();
                                mySubPositionInfo.myPoint = new Point(x - loc.X, y - loc.Y);
                                mySubPositionInfo.percentcorrect = highestPercentCorrect;
                                mySubPositionInfo.strSearchMethod = "UseColorWithLeastPopularColorSearch";
                                if (!possiblepos.Contains(mySubPositionInfo))
                                {
                                    possiblepos.Add(mySubPositionInfo);
                                }
                                highestPercentCorrect = 0;
                                if (!foundRects.Contains(new Rectangle(x, y, sub.Width, sub.Height)))
                                {
                                    foundRects.Add(new Rectangle(x, y, sub.Width, sub.Height));
                                }
                                if (mySubPositionInfo.percentcorrect > 99)
                                {
                                    if (!dictImageFoundCached.ContainsKey(imageFile))
                                    {
                                        dictImageFoundCached.Add(imageFile, mySubPositionInfo.myPoint);
                                    }
                                    break;
                                }
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// x4. Get background for big picture.
        /// x5. Do preliminary check in big picture that tests at least 5 background pixels and 5 foreground pixels and quits when error rate rises above a certain percentage. If there are not 5 background or 5 foreground pixels in small image, just proceed to complete check.
        /// x6. Do complete check in big picture if big picture passes preliminary check.
        /// </summary>
        /// <param name="main"></param>
        /// <param name="sub"></param>
        /// <param name="possiblepos"></param>
        /// <param name="foundRects"></param>
        /// <param name="mainwidth"></param>
        /// <param name="mainheight"></param>
        /// <param name="dataMain"></param>
        /// <param name="strideMain"></param>
        /// <param name="strideSub"></param>
        /// <param name="dataSub"></param>
        /// <param name="pMostPopularColorSmallImage"></param>
        /// <param name="boolArySmallImage"></param>
        /// <param name="highestPercentCorrect"></param>
        /// <param name="intTolerance"></param>
        /// <param name="lstPrelimBackground"></param>
        /// <param name="lstPrelimForeground"></param>
        private static void LoopthruEachPixelInBigImageToFindMatchesOnSmallImageBackgroundPattern(
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
            MostPopularColorSmallImage pMostPopularColorSmallImage,
            bool[,] boolArySmallImage,
            ref decimal highestPercentCorrect,
            int intTolerance,
          List<Point> lstPrelimBackground,
          List<Point> lstPrelimForeground,
          ref Dictionary<MyColor, MostPopularColorSmallImage> dictMostPopularColorSmallImage,
          List<Point> lstHighContrast,
          ref MyColor oDarkestColorSmallImage,
          ref List<Point> olstDarkestColorSmallImage,
          ref Dictionary<string, Point> dictImagesFoundCached,
          string imageFile,
          bool boolStopOnPerfectMatch
            )
        {
            int xtemp = -1;
            int ytemp = -1;
            bool boolDoPrelimCheck = true;
            bool boolNeedNewRowInBigImage = false;
            bool boolWeAreDoneWithBigImage = false;
            int intPrelimCheckBackground = lstPrelimBackground.Count;
            int intPrelimCheckForeground = lstPrelimForeground.Count;
            int intPrelimCheckTotal = intPrelimCheckBackground + intPrelimCheckForeground;
            decimal intCorrect = 0;
            decimal intWrong = 0;
            decimal intTotalCorrectWrong = 0;
            decimal decHighContrastCount = lstHighContrast.Count;
            decimal decDarkestColorCount = olstDarkestColorSmallImage.Count;
            decimal decCorrectnessWeightBackground = (decimal).49;
            decimal decCorrectnessWeightHighContrast = (decimal).56;
            decimal decCorrectnessWeightDarkestColor = (decimal).79;
            if (decHighContrastCount == 0)
            {
                decHighContrastCount = 1;
            }
            if (decDarkestColorCount == 0)
            {
                decDarkestColorCount = 1;
            }
            int intTotalPixelsInSmallImage = sub.Width * sub.Height;
            decimal decWeightForHighContrast = (intTotalPixelsInSmallImage / (lstHighContrast.Count)) * decCorrectnessWeightHighContrast;
            decimal decWeightForDarkestColor = (intTotalPixelsInSmallImage / olstDarkestColorSmallImage.Count) * decCorrectnessWeightDarkestColor;


            Dictionary<MyColor, Point> dictMostPopularColorSmallImageWithTemps = new Dictionary<MyColor, Point>();
            Dictionary<MyColor, Point> dictMostPopularColorBigImage = new Dictionary<MyColor, Point>();
            decimal intPercentCorrect = 0;
            bool boolSkipThisPixelInBigImage = false;
            bool booldebuggingmode = false;
            MyColor oDarkestColorBigImage = new MyColor();
            oDarkestColorBigImage.R = 255;
            oDarkestColorBigImage.G = 255;
            oDarkestColorBigImage.B = 255;
            if (intPrelimCheckTotal + lstHighContrast.Count < 10)
            {
                boolDoPrelimCheck = false;
            }
            // we have saved the relative position of the safe background pixel for the small image
            // and now we want to add that offset to the current pixel in the big image. If adding
            // the offset to the current pixel in the big image throws us out of the bounds of the
            // big image, we are not interested in the current pixel for the big image.

            // this forloop is for each row in the big image - when we go outside the Y boundary of rows,
            // we are done with the big image
            for (int y = 0; y < mainheight; y++)
            {
                boolNeedNewRowInBigImage = false; // we just got a new row so we need to initialize this
                int intYMax = y + sub.Height;
                if (boolWeAreDoneWithBigImage || intYMax > mainheight)
                {
                    //   System.Diagnostics.Debugger.Break();
                    break;
                }
                dictMostPopularColorSmallImageWithTemps.Clear();
                foreach (var item in dictMostPopularColorSmallImage)
                {
                    pMostPopularColorSmallImage = item.Value;
                    ytemp = y + pMostPopularColorSmallImage.thePosition.Y;
                    if (ytemp > mainheight)
                    {
                        boolWeAreDoneWithBigImage = true;
                        break;
                    }
                    Point oPoint = new Point();
                    oPoint.X = -1;
                    oPoint.Y = ytemp;
                    bool boolContains2 = false;
                    foreach (var item2 in dictMostPopularColorSmallImageWithTemps)
                    {
                        MyColor keycolor = item2.Key;
                        if (pMostPopularColorSmallImage.theColor.Equals(keycolor))
                        {
                            boolContains2 = true;
                            break;
                        }
                    }
                    if (boolContains2 == false)
                    {
                        if (!dictMostPopularColorSmallImageWithTemps.ContainsKey(pMostPopularColorSmallImage.theColor))
                        {
                            dictMostPopularColorSmallImageWithTemps.Add(pMostPopularColorSmallImage.theColor, oPoint);
                        }
                    }
                }

                // if adding the offset for the rows throws it outside of the big image, we are done
                if (boolWeAreDoneWithBigImage)
                {
                    //     System.Diagnostics.Debugger.Break();
                    break;
                }
                // x for big image represents the columns in the big image
                for (int x = 0; x < mainwidth; x++)
                {
                    //if (y == 418 && x == 1239) {
                    //  System.Diagnostics.Debugger.Break();
                    //  string abc = "abd";
                    //  booldebuggingmode = true;
                    //} else {
                    //  booldebuggingmode = false;
                    //}
                    int intXMax = x + sub.Width;
                    if (boolNeedNewRowInBigImage || intXMax > mainwidth)
                    {
                        boolNeedNewRowInBigImage = false;
                        break;
                    }
                    // everytime we get a new pixel, we need to initialize everything for that pixel
                    boolSkipThisPixelInBigImage = false;
                    intCorrect = 0;
                    intWrong = 0;
                    // we normally do prelim check unless there are fewer than
                    // 10 pixels involved in prelim check - in that case, percentages might be skewed
                    // just because we hit a couple of mismatches right at the start. When there are 
                    // less than 10 preliminary pixels to check, we go skip prelim check and just do complete check
                    boolDoPrelimCheck = true;
                    xtemp = x + pMostPopularColorSmallImage.thePosition.X;
                    Point oPoint = new Point();
                    foreach (var item in dictMostPopularColorSmallImage)
                    {
                        pMostPopularColorSmallImage = item.Value;
                        xtemp = x + pMostPopularColorSmallImage.thePosition.X;
                        if (xtemp > mainwidth)
                        {
                            boolNeedNewRowInBigImage = true;
                            break;
                        }
                        bool boolContains3 = false;
                        foreach (var item3 in dictMostPopularColorSmallImageWithTemps)
                        {
                            MyColor keycolor = item3.Key;
                            if (pMostPopularColorSmallImage.theColor.Equals(keycolor))
                            {
                                boolContains3 = true;
                                pMostPopularColorSmallImage.theColor = keycolor;
                                break;
                            }
                        }
                        if (boolContains3)
                        {
                            oPoint.X = xtemp;
                            oPoint.Y = dictMostPopularColorSmallImageWithTemps[pMostPopularColorSmallImage.theColor].Y;
                            dictMostPopularColorSmallImageWithTemps[pMostPopularColorSmallImage.theColor] = oPoint;
                        }

                    }
                    // if adding offset to column in big image causes it to go outside of columns for big
                    // picture, we are not interested in in the rest of the pixels on that line for the big image
                    if (boolNeedNewRowInBigImage)
                    {
                        break;
                    }


                    // Adding the offsets did not cause us to go out of bounds so we can now
                    // get what we think is the background color for the relative position of the
                    // small image within the big one

                    Point myPoint = new Point();
                    MyColor curcolor = new MyColor();
                    //if (y > 420 && y < 430) {
                    //  curcolor = GetColor(x, y, strideMain, dataMain);
                    //  Console.WriteLine("Big Image Loop: x=" + x + "; y=" + y + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
                    //}
                    dictMostPopularColorBigImage.Clear();
                    foreach (var item0 in dictMostPopularColorSmallImageWithTemps)
                    {
                        myPoint = item0.Value;
                        xtemp = myPoint.X;
                        ytemp = myPoint.Y;
                        curcolor = GetColor(xtemp, ytemp, strideMain, dataMain);
                        MostPopularColorBigImage oMostPopularColorBigImage = new MostPopularColorBigImage();
                        oMostPopularColorBigImage.theColor = curcolor;
                        oMostPopularColorBigImage.thePosition.X = xtemp;
                        oMostPopularColorBigImage.thePosition.Y = ytemp;
                        bool boolContains4 = false;
                        foreach (var item4 in dictMostPopularColorBigImage)
                        {
                            MyColor keycolor = item4.Key;
                            if (curcolor.Equals(keycolor))
                            {
                                boolContains4 = true;
                                break;
                            }
                        }
                        if (boolContains4 == false)
                        {
                            //if (booldebuggingmode) {
                            //  Console.WriteLine("Background color for big image: x=" + xtemp + "; y=" + ytemp + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
                            //}
                            if (!dictMostPopularColorBigImage.ContainsKey(oMostPopularColorBigImage.theColor))
                            {
                                dictMostPopularColorBigImage.Add(oMostPopularColorBigImage.theColor, oMostPopularColorBigImage.thePosition);
                            }
                        }
                    }
                    // we need to do preliminary check by going thru the offsets saved in the list for
                    // safe background pixels

                    // maximum number of prelim background checks is 10
                    if (intPrelimCheckBackground > 10)
                    {
                        intPrelimCheckBackground = 10;
                    }
                    // maximum number of prelim foreground checks is 10
                    if (intPrelimCheckForeground > 10)
                    {
                        intPrelimCheckForeground = 10;
                    }
                    if (boolDoPrelimCheck)
                    {
                        for (int i = 0; i < intPrelimCheckBackground; i++)
                        {
                            xtemp = x + lstPrelimBackground[i].X;
                            // if adding the offset to the current column in the big picture
                            // causes us to go outside of the bounds of the big picture, we
                            // know we do not want this pixel for the big image and we no 
                            // longer need to do any more preliminary checks for this pixel in the big image
                            if (xtemp > mainwidth)
                            {
                                boolSkipThisPixelInBigImage = true;
                                break;
                            }
                            ytemp = y + lstPrelimBackground[i].Y;
                            // if adding the offset to the current row in the big picture
                            // causes us to go outside of the bounds of the big picture, we
                            // know we do not want this pixel for the big image and we no 
                            // longer need to do any more preliminary checks for this pixel in the big image
                            if (ytemp > mainheight)
                            {
                                boolSkipThisPixelInBigImage = true;
                                break;
                            }
                            // the position of the preliminary check for background color was within
                            // the big image so we know want to know if the relative pixel within the
                            // big image is a background color like it should be.
                            curcolor = GetColor(xtemp, ytemp, strideMain, dataMain);
                            //if (booldebuggingmode) {
                            //  Console.WriteLine("Prelim Background check: x=" + x + "; y=" + y + "xtemp=" + xtemp + "; ytemp=" + ytemp + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
                            //}

                            bool boolContains5 = false;
                            foreach (var item5 in dictMostPopularColorBigImage)
                            {
                                MyColor keycolor = item5.Key;
                                if (curcolor.Equals(keycolor))
                                {
                                    boolContains5 = true;
                                    break;
                                }
                            }
                            if (boolContains5)
                            {
                                //if (booldebuggingmode) {
                                //  Console.WriteLine("Correct");
                                //}                
                                intCorrect++;
                                intCorrect++;
                                intCorrect++;
                            }
                            else
                            {
                                //if (booldebuggingmode) {
                                //  Console.WriteLine("InCorrect");
                                //}
                                intWrong++;
                                intWrong++;
                                intWrong++;
                            }
                        }
                        //   Logging.WriteLogSimple("Prelim background check done - intCorrect = " + intCorrect + " intWrong = " + intWrong);
                        // if we did not run into problems with the relative position for the preliminary background
                        // color being outside of the bounds of the big image, then we can proceed to do the preliminary
                        // checks for the foreground color
                        if (boolSkipThisPixelInBigImage == false)
                        {
                            for (int i = 0; i < intPrelimCheckForeground; i++)
                            {
                                xtemp = x + lstPrelimForeground[i].X;
                                if (xtemp > mainwidth)
                                {
                                    boolNeedNewRowInBigImage = true;
                                    boolSkipThisPixelInBigImage = true;
                                    break;
                                }
                                ytemp = y + lstPrelimForeground[i].Y;
                                if (ytemp > mainheight)
                                {
                                    boolWeAreDoneWithBigImage = true;
                                    boolSkipThisPixelInBigImage = true;
                                    break;
                                }
                                curcolor = GetColor(xtemp, ytemp, strideMain, dataMain);
                                //if (booldebuggingmode) {
                                //  Console.WriteLine("Prelim Foreground check: x=" + xtemp + "; y=" + ytemp + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
                                //}
                                bool boolContains6 = false;
                                foreach (var item6 in dictMostPopularColorBigImage)
                                {
                                    MyColor keycolor = item6.Key;
                                    if (curcolor.Equals(keycolor))
                                    {
                                        boolContains6 = true;
                                        break;
                                    }
                                }
                                if (boolContains6 == false)
                                {
                                    //if (booldebuggingmode) {
                                    //  Console.WriteLine("Correct");
                                    //}
                                    intCorrect++;
                                }
                                else
                                {
                                    //if (booldebuggingmode) {
                                    //  Console.WriteLine("InCorrect");
                                    //}
                                    intWrong++;
                                }
                            }
                        }
                    }
                    // if any problems were encountered with the preliminary checks, we just want to get
                    // the next pixel in the big image
                    if (boolSkipThisPixelInBigImage)
                    {
                        continue;
                    }
                    int intXBig = 0;
                    int intXBigNext = 0;
                    int intYBig = 0;
                    int intPrelimHighContrast = 0;
                    foreach (var item in lstHighContrast)
                    {
                        intPrelimHighContrast++;
                        if (intPrelimHighContrast > 10)
                        {
                            break;
                        }
                        intXBig = item.X + x;
                        intYBig = item.Y + y;
                        if (intXBig + 2 > mainwidth || intYBig > mainheight)
                        {
                            continue;
                        }
                        intXBigNext = intXBig + 1;
                        MyColor bigCurColor = GetColor(intXBig, intYBig, strideMain, dataMain);
                        MyColor bigCurColorNext = GetColor(intXBigNext, intYBig, strideMain, dataMain);
                        MyColor bigCurColorNextNext = GetColor(intXBigNext, intYBig, strideMain, dataMain);
                        if (bigCurColor.HighContrast(bigCurColorNext) || bigCurColor.HighContrast(bigCurColorNextNext))
                        {
                            intCorrect += decWeightForHighContrast;
                        }
                        else
                        {
                            intWrong += decWeightForHighContrast;
                        }
                    }

                    // we did at least 10 preliminary checks or the boolean flag for doing preliminary checks
                    // was set to false. We can now see if the percentage correct for the preliminary checks is 
                    // good enough for us to continue on to the full blown complete check
                    intTotalCorrectWrong = intCorrect + intWrong;
                    intPercentCorrect = ((intCorrect * 100) / intTotalCorrectWrong);
                    //if (booldebuggingmode) {
                    //  Console.WriteLine("Percent Correct% " + intPercentCorrect.ToString());
                    //}

                    if (boolDoPrelimCheck == true && intTolerance - 10 > intPercentCorrect)
                    {
                        continue;
                    }
                    // do complete check - x and y still have the value of the current pixel in the big image.
                    // We want to loop through every pixel of the small image and add that offset to the current
                    // pixel in the big image. We will compare each pixel in the small image to the relative
                    // position of the small image within the big image to see if there is a match with regard
                    // to background/foreground.  
                    // System.Diagnostics.Debugger.Break();
                    // Logging.WriteLogSimple("we are doing a complete check at x=" + x + ";y=" +y);
                    intCorrect = 0;
                    intWrong = 0;
                    int intCorrectBackground = 0;
                    int intWrongBackground = 0;
                    int intCorrectHighContrast = 0;
                    int intWrongHighContrast = 0;
                    int intCorrectDarkestColor = 0;
                    int intWrongDarkestColor = 0;

                    for (int sy = 0; sy < sub.Height; sy++)
                    {
                        // if we ever go out of the bounds of the big picture, we are no longer interested in this
                        // pixel in the big image
                        ytemp = y + sy;
                        if (ytemp > mainheight)
                        {
                            boolWeAreDoneWithBigImage = true;
                            boolSkipThisPixelInBigImage = true;
                            break;
                        }
                        bool boolPixelInSubIsBackground = false;
                        bool boolNextPixelInSubIsHighContrast = false;
                        MyColor myNextPixel;
                        int intXNextPixel = -1;
                        for (int sx = 0; sx < sub.Width; sx++)
                        {

                            // first we find out if the pixel in the sub is background             
                            curcolor = GetColor(sx, sy, strideSub, dataSub);
                            if (curcolor.LessThan(oDarkestColorBigImage))
                            {
                                oDarkestColorBigImage = curcolor;
                            }
                            intXNextPixel = sx + 1;
                            boolNextPixelInSubIsHighContrast = false;
                            if (intXNextPixel <= sub.Width)
                            {
                                myNextPixel = GetColor(intXNextPixel, sy, strideSub, dataSub);
                                if (curcolor.HighContrast(myNextPixel))
                                {
                                    boolNextPixelInSubIsHighContrast = true;
                                }
                            }
                            //if (booldebuggingmode) {
                            //  Console.WriteLine("Complete check - get pixel in sub: sx=" + sx + " sy=" + sy + " xtemp=" + xtemp + "; ytemp=" + ytemp + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
                            //}
                            bool boolContains7 = false;
                            foreach (var item7 in dictMostPopularColorSmallImage)
                            {
                                MyColor keycolor = item7.Key;
                                if (curcolor.Equals(keycolor))
                                {
                                    boolContains7 = true;
                                    break;
                                }
                            }
                            if (boolContains7)
                            {
                                boolPixelInSubIsBackground = true;
                            }
                            else
                            {
                                boolPixelInSubIsBackground = false;
                            }

                            // then we find if pixel in big image is the same type(background or foreground)
                            // as the one in the subimage; if it is the same, we add one to intCorrect; else add 1 to intWrong
                            xtemp = x + sx;
                            if (xtemp > mainwidth)
                            {
                                boolNeedNewRowInBigImage = true;
                                boolSkipThisPixelInBigImage = true;
                                break;
                            }
                            bool boolNextPixelInBigIsHighContrast = false;
                            bool boolPixelInBigIsBackground;
                            curcolor = GetColor(xtemp, ytemp, strideMain, dataMain);
                            intXNextPixel = xtemp + 1;
                            if (intXNextPixel <= mainwidth)
                            {
                                myNextPixel = GetColor(intXNextPixel, ytemp, strideMain, dataMain);
                                if (curcolor.HighContrast(myNextPixel))
                                {
                                    boolNextPixelInBigIsHighContrast = true;
                                }
                            }
                            //if (booldebuggingmode) {
                            //  Console.WriteLine("Complete check: sx=" + sx + " sy=" + sy + " xtemp=" + xtemp + "; ytemp=" + ytemp + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
                            //}
                            bool boolContains8 = false;
                            foreach (var item8 in dictMostPopularColorBigImage)
                            {
                                MyColor keycolor = item8.Key;
                                if (curcolor.Equals(keycolor))
                                {
                                    boolContains8 = true;
                                    break;
                                }
                            }
                            if (boolContains8)
                            {
                                boolPixelInBigIsBackground = true;
                            }
                            else
                            {
                                boolPixelInBigIsBackground = false;
                            }
                            if ((boolPixelInBigIsBackground && boolPixelInSubIsBackground) ||
                            (boolPixelInBigIsBackground == false && boolPixelInSubIsBackground == false) && boolNextPixelInSubIsHighContrast == boolNextPixelInBigIsHighContrast)
                            {
                                //if (booldebuggingmode) {
                                //  Console.WriteLine("Correct Complete check");
                                //}
                                intCorrectBackground++;
                                intCorrect += decCorrectnessWeightBackground;
                            }
                            else
                            {
                                //if (booldebuggingmode) {
                                //  Console.WriteLine("InCorrect Complete check: boolPixelInBigIsBackground=" + boolPixelInBigIsBackground + " boolPixelInSubIsBackground=" + boolPixelInSubIsBackground);
                                //}
                                intWrongBackground++;
                                intWrong += decCorrectnessWeightBackground;
                            }
                            if (boolNextPixelInSubIsHighContrast == true && boolNextPixelInBigIsHighContrast == true)
                            {
                                //if (booldebuggingmode) {
                                //  Console.WriteLine("Correct Complete check");
                                //}
                                intCorrectHighContrast++;
                                intCorrect = intCorrect + decWeightForHighContrast;
                            }
                            if (boolNextPixelInSubIsHighContrast != boolNextPixelInBigIsHighContrast)
                            {
                                //if (booldebuggingmode) {
                                //  Console.WriteLine("Correct Complete check");
                                //}
                                intWrongHighContrast++;
                                intWrong = intWrong + decWeightForHighContrast;
                            }
                            intTotalCorrectWrong = intCorrect + intWrong;
                            intPercentCorrect = ((intCorrect * 100) / intTotalCorrectWrong);
                            //if (booldebuggingmode) {
                            //  Console.WriteLine("Percent Correct Complete check" +  intPercentCorrect.ToString());
                            //}
                            int intTotal = intCorrectBackground + intWrongBackground
                              + intCorrectHighContrast + intWrongHighContrast
                              + intCorrectDarkestColor + intWrongDarkestColor;
                            if (intTolerance - 20 > intPercentCorrect && intTotal > 25)
                            {
                                //                Logging.WriteLogSimple("Complete check and early exit >25  x=" + x + " y=" + y + "intCorrectBackground=" + intCorrectBackground.ToString() +
                                //" intWrongBackground=" + intWrongBackground.ToString() +
                                //" intCorrectHighContrast=" + intCorrectHighContrast.ToString() +
                                //" intWrongHighContrast=" + intWrongHighContrast.ToString() +
                                //" intCorrectDarkestColor=" + intCorrectDarkestColor.ToString() +
                                //" intWrongDarkestColor=" + intWrongDarkestColor.ToString());
                                boolSkipThisPixelInBigImage = true;
                                break;
                            }

                        }

                    }
                    if (boolSkipThisPixelInBigImage)
                    {
                        //    System.Diagnostics.Debugger.Break();
                        continue;
                    }
                    foreach (var item in lstHighContrast)
                    {
                        intXBig = item.X + x;
                        intYBig = item.Y + y;
                        if (intXBig + 2 > mainwidth || intYBig > mainheight)
                        {
                            continue;
                        }
                        intXBigNext = intXBig + 1;
                        MyColor bigCurColor = GetColor(intXBig, intYBig, strideMain, dataMain);
                        MyColor bigCurColorNext = GetColor(intXBigNext, intYBig, strideMain, dataMain);
                        MyColor bigCurColorNextNext = GetColor(intXBigNext, intYBig, strideMain, dataMain);
                        if (bigCurColor.HighContrast(bigCurColorNext) || bigCurColor.HighContrast(bigCurColorNextNext))
                        {
                            intCorrectHighContrast++;
                            intCorrect += decWeightForHighContrast;
                        }
                        else
                        {
                            intWrongHighContrast++;
                            intWrong += decWeightForHighContrast;
                        }
                    }
                    foreach (var item in olstDarkestColorSmallImage)
                    {
                        intXBig = item.X + x;
                        intYBig = item.Y + y;
                        if (intXBig + 2 > mainwidth || intYBig > mainheight)
                        {
                            continue;
                        }
                        MyColor bigCurColor = GetColor(intXBig, intYBig, strideMain, dataMain);
                        if (bigCurColor.Equals(oDarkestColorBigImage))
                        {
                            intCorrectDarkestColor++;
                            intCorrect += decWeightForDarkestColor;
                        }
                        else
                        {
                            intWrongDarkestColor++;
                            intWrong += decWeightForDarkestColor;
                        }
                    }
                    // if percent correct is less than tolerance, we can skip this pixel in big image
                    intTotalCorrectWrong = intCorrect + intWrong;
                    intPercentCorrect = ((intCorrect * 100) / intTotalCorrectWrong);
                    //if (booldebuggingmode) {
                    //  Console.WriteLine("Percent Correct Complete check" +  intPercentCorrect.ToString());
                    //}
                    if (intTolerance > intPercentCorrect)
                    {
                        //            Logging.WriteLogSimple("Complete check and tolerance > % correct x=" + x + " y=" + y + "intCorrectBackground=" + intCorrectBackground.ToString() +
                        //" intWrongBackground=" + intWrongBackground.ToString() +
                        //" intCorrectHighContrast=" + intCorrectHighContrast.ToString() +
                        //" intWrongHighContrast=" + intWrongHighContrast.ToString() +
                        //" intCorrectDarkestColor=" + intCorrectDarkestColor.ToString() +
                        //" intWrongDarkestColor=" + intWrongDarkestColor.ToString());
                        continue;
                    }
                    Logging.WriteLogSimple("x=" + x + " y=" + y + "intCorrectBackground=" + intCorrectBackground.ToString() +
                    " intWrongBackground=" + intWrongBackground.ToString() +
                    " intCorrectHighContrast=" + intCorrectHighContrast.ToString() +
                    " intWrongHighContrast=" + intWrongHighContrast.ToString() +
                    " intCorrectDarkestColor=" + intCorrectDarkestColor.ToString() +
                   " intWrongDarkestColor=" + intWrongDarkestColor.ToString());

                    //we found a match
                    if (intPercentCorrect > intTolerance)
                    {
                        intTolerance = (int)intPercentCorrect;
                    }
                    SubPositionInfo mySubPositionInfo = new SubPositionInfo();
                    mySubPositionInfo.myPoint = new Point(x, y);
                    mySubPositionInfo.percentcorrect = intPercentCorrect;

                    if (!possiblepos.Contains(mySubPositionInfo))
                    {
                        possiblepos.Add(mySubPositionInfo);
                    }
                    highestPercentCorrect = 0;
                    mySubPositionInfo.strSearchMethod = "UsePatternBasedBackgroundForegroundPlusHighContrast";
                    if (!foundRects.Contains(new Rectangle(x, y, sub.Width, sub.Height)))
                    {
                        foundRects.Add(new Rectangle(x, y, sub.Width, sub.Height));
                    }


                }

            }

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

        public static List<SubPositionInfo> GetSubPositions(Bitmap main, Bitmap sub, bool boolUseGrayScale, ref decimal highestPercentCorrect, int intTolerance) {

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

      MostPopularColorSmallImage oMostPopularColorSmallImage = new MostPopularColorSmallImage();
      LeastPopularColor oLeastPopularColor = new LeastPopularColor();
      LeastPopularPattern oLeastPopularPattern = new LeastPopularPattern();
      LeastPopularPattern oLeastPopularPattern2 = new LeastPopularPattern();

      Dictionary<MyTwoColors, int[]> repeats = new Dictionary<MyTwoColors, int[]>();
      Dictionary<MyColor, int[]> repeatsMostPopular = new Dictionary<MyColor, int[]>();
      Dictionary<MyPattern, int[]> repeatsPattern = new Dictionary<MyPattern, int[]>();

      List<Point> lstPrelimBackground = new List<Point>();
      List<Point> lstPrelimForeground = new List<Point>();
      List<Point> lstHighContrast = new List<Point>();

      Dictionary<MyColor, MostPopularColorSmallImage> dictMostPopularColorSmallImage = new Dictionary<MyColor, MostPopularColorSmallImage>();

      Dictionary<Point, objErrors> dictionary = new Dictionary<Point, objErrors>();
      StringBuilder sb = new StringBuilder();

      if (boolUseGrayScale == false) {
        FindLeastPopularColorInSmallImage(sub, subwidth, subheight, strideSub, dataSub, ref oLeastPopularColor, repeats);
        LoopthruEachPixelInBigImageToFindMatchesOnSmallImageLeastPopularColor(main, sub, possiblepos, foundRects, mainwidth, mainheight, dataMain, strideMain, strideSub, dataSub, oLeastPopularColor, ref highestPercentCorrect, intTolerance);
      } else {
        bool[,] boolArySmallImage = new bool[subwidth, subheight];
        //FindLeastPopularHighContrastPatternInSmallImage(sub, subwidth, subheight, strideSub, dataSub, ref oLeastPopularPattern, ref oLeastPopularPattern2, repeatsPattern, ref boolArySmallImage);
        ////   System.Diagnostics.Debugger.Break();
        //Logging.WriteLogSimple(Logging.DumpObject(oLeastPopularPattern.thePattern));
        //Logging.WriteLogSimple(Logging.DumpObject(oLeastPopularPattern.thePosition));
        //Logging.WriteLogSimple(Logging.DumpObject(oLeastPopularPattern2.thePattern));
        //Logging.WriteLogSimple(Logging.DumpObject(oLeastPopularPattern2.thePosition));
        //LoopthruEachPixelInBigImageToFindMatchesOnSmallImageLeastPopularPattern(main, sub, possiblepos, foundRects, mainwidth, mainheight, dataMain, strideMain, strideSub, dataSub, oLeastPopularPattern, boolArySmallImage, ref highestPercentCorrect, intTolerance);
        //if (foundRects.Count == 0) {
        //  LoopthruEachPixelInBigImageToFindMatchesOnSmallImageLeastPopularPattern(main, sub, possiblepos, foundRects, mainwidth, mainheight, dataMain, strideMain, strideSub, dataSub, oLeastPopularPattern2, boolArySmallImage, ref highestPercentCorrect, intTolerance);
        //}
        /*
         * x1. In the small image, get the most popular color, which will be the background color for small picture.
         * x2. In the small image, find 3 background pixels in a row and select the location of the middle one as 
         *     a safe place to get the background color for big image.
         * x3. In the small image, everytime the foreground/background change 
         *     and there are at least 3 of the new kind in a row, add the location of the middle 
         *     one to one of two lists (Background/Foreground)of safe preliminary points to check. 
         * x4. Get background for big picture.
         * x5. Do preliminary check in big picture that tests at least 5 background pixels and 5 foreground pixels
         *     and quits when error rate rises above a certain percentage. If there are not 5 background or
         *     5 foreground pixels in small image, just proceed to complete check.
         * x6. Do complete check in big picture if big picture passes preliminary check.
         */
        // System.Diagnostics.Debugger.Break();
        MyColor oDarkestColorSmallImage = new MyColor();
        List<Point> olstDarkestColorSmallImage = new List<Point>();
        boolArySmallImage = new bool[subwidth, subheight];
        // FindMostPopularColorInSmallImage is going to find the most popular colors in the small image
        // and put the location of at least five of them in lstPrelimBackground
        // It will also put the location of at least five foreground pixels in lstPrelimForeground
        // If it cannot find 5, it will show popup message saying we cannot use this strategy on this image
        Logging.WriteLogSimple("FindMostPopularColorInSmallImage");
        FindMostPopularColorInSmallImage(sub, 
          subwidth, 
          subheight, 
          strideSub, 
          dataSub, 
          ref oMostPopularColorSmallImage, 
          repeatsMostPopular, 
          ref lstPrelimBackground, 
          ref lstPrelimForeground, 
          ref dictMostPopularColorSmallImage, 
          ref lstHighContrast,
          ref oDarkestColorSmallImage,
          ref olstDarkestColorSmallImage);
        Logging.WriteLogSimple("LoopthruEachPixelInBigImageToFindMatchesOnSmallImageBackgroundPattern");
        LoopthruEachPixelInBigImageToFindMatchesOnSmallImageBackgroundPattern(main, 
          sub, 
          possiblepos, 
          foundRects, 
          mainwidth, 
          mainheight, 
          dataMain, 
          strideMain, 
          strideSub, 
          dataSub, 
          oMostPopularColorSmallImage, 
          boolArySmallImage, 
          ref highestPercentCorrect, 
          intTolerance, 
          lstPrelimBackground, 
          lstPrelimForeground, 
          ref dictMostPopularColorSmallImage, 
          lstHighContrast,
          ref oDarkestColorSmallImage,
          ref olstDarkestColorSmallImage);
      }

      // Here is the boolUseGrayScale stuff I deleted
      //bool[,] boolArySmallImage = new bool[subwidth, subheight];
      //FindLeastPopularPatternInSmallImage(sub, subwidth, subheight, strideSub, dataSub, ref oLeastPopularPattern, ref oLeastPopularPattern2, repeatsPattern, ref boolArySmallImage);
      //LoopthruEachPixelInBigImageToFindMatchesOnSmallImageLeastPopularPattern(main, sub, possiblepos, foundRects, mainwidth, mainheight, dataMain, strideMain, strideSub, dataSub, oLeastPopularPattern, boolArySmallImage, ref highestPercentCorrect, intTolerance);
      //if (foundRects.Count == 0) {
      //  LoopthruEachPixelInBigImageToFindMatchesOnSmallImageLeastPopularPattern(main, sub, possiblepos, foundRects, mainwidth, mainheight, dataMain, strideMain, strideSub, dataSub, oLeastPopularPattern2, boolArySmallImage, ref highestPercentCorrect, intTolerance);
      //}

      System.Runtime.InteropServices.Marshal.Copy(dataSub, 0, Scan0Sub, bytesSub);
      sub.UnlockBits(bmSubData);

      System.Runtime.InteropServices.Marshal.Copy(dataMain, 0, Scan0Main, bytesMain);
      main.UnlockBits(bmMainData);

      return possiblepos;
    }
    /// <summary>
    /// x1. In the small image, get the most popular color, which will be the background color for small picture.
    /// x2. In the small image, find 3 background pixels in a row and select the location of the middle one as a safe place to get the background color for big image.
    /// x3. In the small image, everytime the foreground/background change and there are at least 3 of the new kind in a row, add the location of the middle one to one of two lists (Background/Foreground)of safe preliminary points to check. 
    /// </summary>
    /// <param name="sub"></param>
    /// <param name="subwidth"></param>
    /// <param name="subheight"></param>
    /// <param name="strideSub"></param>
    /// <param name="dataSub"></param>
    /// <param name="pMostPopularColorSmallImage"></param>
    /// <param name="repeatsMostPopular"></param>
    /// <param name="lstPrelimBackground"></param>
    /// <param name="lstPrelimForeground"></param>
    private static void FindMostPopularColorInSmallImage(Bitmap sub, 
      int subwidth, 
      int subheight, 
      int strideSub, 
      byte[] dataSub, 
      ref MostPopularColorSmallImage pMostPopularColorSmallImage, 
      Dictionary<MyColor, int[]> repeatsMostPopular, 
      ref List<Point> lstPrelimBackground, 
      ref List<Point> lstPrelimForeground, 
      ref Dictionary<MyColor, MostPopularColorSmallImage> dictMostPopularColorSmallImage, 
      ref List<Point> lstHighContrast,
      ref MyColor oDarkestColorSmallImage,
      ref List<Point> olstDarkestColorSmallImage) {

      int intPrevX;
      int intPrevY;
      int intBackgroundsCtr = 0;
      repeatsMostPopular.Clear();
      dictMostPopularColorSmallImage.Clear();
      int intMaxPrelimBackgrounds = 0;
      int intSaveBackgroundCtr = 0;
      oDarkestColorSmallImage.A = 255;
      oDarkestColorSmallImage.R= 255;
      oDarkestColorSmallImage.G = 255;
      oDarkestColorSmallImage.B = 255;
 
      do {
        intBackgroundsCtr++;
        repeatsMostPopular.Clear();
        lstPrelimBackground.Clear();
        lstPrelimForeground.Clear();
        pMostPopularColorSmallImage = AddAColorToSmallImageBackgrounds(sub, strideSub, dataSub, pMostPopularColorSmallImage, repeatsMostPopular, ref dictMostPopularColorSmallImage, out intPrevX, out intPrevY);
        // now we want fo find all of the locations for the middle pixel of the first set of three-in-a-row for
        // the background and foreground everytime the background changes to foreground and vice versa.
        // We want to use the safe pixels to do a preliminary check of at least 10 pixels before we do a full
        // blown check of all the pixels related to the small image at a given pixel in the big image.
        Point myPoint = new Point();
        int intThreeInARowCtrBackground = 0;
        int intThreeInARowCtrForeground = 0;
        //    System.Diagnostics.Debugger.Break();
        bool boolWantToFindBackground = true;
        bool boolWantToFindForeground = true;
        bool boolCurrentColorBackground = false;
        bool boolCurrentAboveColorBackground = false;
        bool boolCurrentBelowColorBackground = false;
        for (int y = 0; y < sub.Height; y++) {
          for (int x = 0; x < sub.Width; x++) {
            boolCurrentColorBackground = false;
            boolCurrentAboveColorBackground = false;
            boolCurrentBelowColorBackground = false;
            MyColor curcolor = GetColor(x, y, strideSub, dataSub);
            if (curcolor.LessThan(oDarkestColorSmallImage)) {
              oDarkestColorSmallImage = curcolor;
            }
            if (intBackgroundsCtr == 1) {
              int intXNextPixel = x + 1;
              if (intXNextPixel <= sub.Width) {
                MyColor myNextPixel = GetColor(intXNextPixel, y, strideSub, dataSub);
                if (curcolor.HighContrast(myNextPixel)) {
                  lstHighContrast.Add(new Point(x, y));
                }
              }
            }
            bool boolContains = false;
            foreach (var item in dictMostPopularColorSmallImage) {
              MyColor keycolor = item.Key;
              if (curcolor.Equals(keycolor)) {
                boolContains = true;
                break;
              }
            }
            if (boolContains) {
              boolCurrentColorBackground = true;
            }
            int intYAbove = y - 1;
            int intYBelow = y + 1;
            // if above or below are out of bounds, reset counters and skip it;
            if (intYAbove < 0 || intYBelow > sub.Height) {
              intThreeInARowCtrBackground = 0;
              intThreeInARowCtrForeground = 0;
              continue;
            }

            MyColor curcolorAbove = GetColor(x, intYAbove, strideSub, dataSub);
            boolContains = false;
            foreach (var item in dictMostPopularColorSmallImage) {
              MyColor keycolor = item.Key;
              if (curcolorAbove.Equals(keycolor)) {
                boolContains = true;
                break;
              }
            }
            if (boolContains) {
              boolCurrentAboveColorBackground = true;
            }
            MyColor curcolorBelow = GetColor(x, intYBelow, strideSub, dataSub);
            boolContains = false;
            foreach (var item in dictMostPopularColorSmallImage) {
              MyColor keycolor = item.Key;
              if (curcolorBelow.Equals(keycolor)) {
                boolContains = true;
                break;
              }
            }
            if (boolContains) {
              boolCurrentBelowColorBackground = true;
            }
            if (boolWantToFindForeground == false) {
              if (boolCurrentAboveColorBackground == false || boolCurrentColorBackground == false || boolCurrentBelowColorBackground == false) {
                intThreeInARowCtrBackground = 0;
                intThreeInARowCtrForeground = 0;
                continue;
              }
            }
            //if (boolWantToFindBackground == false) {
            //  if (boolCurrentAboveColorBackground == true || boolCurrentColorBackground == true || boolCurrentBelowColorBackground == true) {
            //    intThreeInARowCtrBackground = 0;
            //    intThreeInARowCtrForeground = 0;
            //    continue;
            //  }
            //}

            // if it equals the most popular color, increment the background counter and reset foreground counter to 0
            // if it does not equal the most popular color, increment the foreground counter and reset background counter to 0
            if (boolCurrentAboveColorBackground == true && boolCurrentColorBackground == true && boolCurrentBelowColorBackground == true) {
              intThreeInARowCtrBackground++;
              intThreeInARowCtrForeground = 0;
            } 
            if (boolCurrentColorBackground == false) {
           //   System.Diagnostics.Debugger.Break();
              intThreeInARowCtrForeground++;
              intThreeInARowCtrBackground = 0;
            }
            if (intThreeInARowCtrBackground == 3 && boolWantToFindBackground) {
              boolWantToFindForeground = true;
              boolWantToFindBackground = false;
              myPoint.X = intPrevX;
              myPoint.Y = intPrevY;
              //  Console.WriteLine("Background color for small image: x=" + intPrevX + "; y=" + intPrevY + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
              lstPrelimBackground.Add(myPoint);
            }
            if (intThreeInARowCtrForeground == 3 && boolWantToFindForeground) {
              boolWantToFindForeground = false;
              boolWantToFindBackground = true;
              myPoint.X = intPrevX;
              myPoint.Y = intPrevY;
              //   Console.WriteLine("Foreground color for small image: x=" + intPrevX + "; y=" + intPrevY + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
              lstPrelimForeground.Add(myPoint);
            }
            intPrevX = x;
            intPrevY = y;
          }
        }
         // System.Diagnostics.Debugger.Break();
        if (intMaxPrelimBackgrounds < lstPrelimBackground.Count) {
          intMaxPrelimBackgrounds = lstPrelimBackground.Count;
          intSaveBackgroundCtr = intBackgroundsCtr;
        }

      } while (lstPrelimBackground.Count < 10 && intBackgroundsCtr < 7);

      //=====
      repeatsMostPopular.Clear();
      dictMostPopularColorSmallImage.Clear();      
      intBackgroundsCtr = 0;
      do {
        intBackgroundsCtr++;
        repeatsMostPopular.Clear();
        lstPrelimBackground.Clear();
        lstPrelimForeground.Clear();
        pMostPopularColorSmallImage = AddAColorToSmallImageBackgrounds(sub, strideSub, dataSub, pMostPopularColorSmallImage, repeatsMostPopular, ref dictMostPopularColorSmallImage, out intPrevX, out intPrevY);
        // now we want fo find all of the locations for the middle pixel of the first set of three-in-a-row for
        // the background and foreground everytime the background changes to foreground and vice versa.
        // We want to use the safe pixels to do a preliminary check of at least 10 pixels before we do a full
        // blown check of all the pixels related to the small image at a given pixel in the big image.
        Point myPoint = new Point();
        int intThreeInARowCtrBackground = 0;
        int intThreeInARowCtrForeground = 0;
        //    System.Diagnostics.Debugger.Break();
        bool boolWantToFindBackground = true;
        bool boolWantToFindForeground = true;
        bool boolCurrentColorBackground = false;
        bool boolCurrentAboveColorBackground = false;
        bool boolCurrentBelowColorBackground = false;
        for (int y = 0; y < sub.Height; y++) {
          for (int x = 0; x < sub.Width; x++) {
            boolCurrentColorBackground = false;
            boolCurrentAboveColorBackground = false;
            boolCurrentBelowColorBackground = false;
            MyColor curcolor = GetColor(x, y, strideSub, dataSub);
            if (curcolor.Equals(oDarkestColorSmallImage)) {
              Point oPoint = new Point();
              oPoint.X = x;
              oPoint.Y = y;
              olstDarkestColorSmallImage.Add(oPoint);
            }
            bool boolContains = false;
            foreach (var item in dictMostPopularColorSmallImage) {
              MyColor keycolor = item.Key;
              if (curcolor.Equals(keycolor)) {
                boolContains = true;
                break;
              }
            }
            if (boolContains) {
              boolCurrentColorBackground = true;
            }
            int intYAbove = y - 1;
            int intYBelow = y + 1;
            // if above or below are out of bounds, reset counters and skip it;
            if (intYAbove < 0 || intYBelow > sub.Height) {
              intThreeInARowCtrBackground = 0;
              intThreeInARowCtrForeground = 0;
              continue;
            }

            MyColor curcolorAbove = GetColor(x, intYAbove, strideSub, dataSub);
            boolContains = false;
            foreach (var item in dictMostPopularColorSmallImage) {
              MyColor keycolor = item.Key;
              if (curcolorAbove.Equals(keycolor)) {
                boolContains = true;
                break;
              }
            }
            if (boolContains) {
              boolCurrentAboveColorBackground = true;
            }
            MyColor curcolorBelow = GetColor(x, intYBelow, strideSub, dataSub);
            boolContains = false;
            foreach (var item in dictMostPopularColorSmallImage) {
              MyColor keycolor = item.Key;
              if (curcolorBelow.Equals(keycolor)) {
                boolContains = true;
                break;
              }
            }
            if (boolContains) {
              boolCurrentBelowColorBackground = true;
            }
            if (boolWantToFindForeground == false) {
              if (boolCurrentAboveColorBackground == false || boolCurrentColorBackground == false || boolCurrentBelowColorBackground == false) {
                intThreeInARowCtrBackground = 0;
                intThreeInARowCtrForeground = 0;
                continue;
              }
            }
            //if (boolWantToFindBackground == false) {
            //  if (boolCurrentAboveColorBackground == true || boolCurrentColorBackground == true || boolCurrentBelowColorBackground == true) {
            //    intThreeInARowCtrBackground = 0;
            //    intThreeInARowCtrForeground = 0;
            //    continue;
            //  }
            //}

            // if it equals the most popular color, increment the background counter and reset foreground counter to 0
            // if it does not equal the most popular color, increment the foreground counter and reset background counter to 0
            if (boolCurrentAboveColorBackground == true && boolCurrentColorBackground == true && boolCurrentBelowColorBackground == true) {
              intThreeInARowCtrBackground++;
              intThreeInARowCtrForeground = 0;
            }
            if (boolCurrentColorBackground == false) {
              //   System.Diagnostics.Debugger.Break();
              intThreeInARowCtrForeground++;
              intThreeInARowCtrBackground = 0;
            }
            if (intThreeInARowCtrBackground == 3 && boolWantToFindBackground) {
              boolWantToFindForeground = true;
              boolWantToFindBackground = false;
              myPoint.X = intPrevX;
              myPoint.Y = intPrevY;
              //  Console.WriteLine("Background color for small image: x=" + intPrevX + "; y=" + intPrevY + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
              lstPrelimBackground.Add(myPoint);
            }
            if (intThreeInARowCtrForeground == 3 && boolWantToFindForeground) {
              boolWantToFindForeground = false;
              boolWantToFindBackground = true;
              myPoint.X = intPrevX;
              myPoint.Y = intPrevY;
              //   Console.WriteLine("Foreground color for small image: x=" + intPrevX + "; y=" + intPrevY + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
              lstPrelimForeground.Add(myPoint);
            }
            intPrevX = x;
            intPrevY = y;
          }
        }
        // System.Diagnostics.Debugger.Break();


      } while (intBackgroundsCtr < intSaveBackgroundCtr);
      foreach (var item in lstPrelimBackground) {
    //    Logging.WriteLogSimple("background x=" + item.X + ";y=" + item.Y);
      }
      foreach (var item in lstPrelimForeground) {
    //    Logging.WriteLogSimple("foreground x=" + item.X + ";y=" + item.Y);
      }
      foreach (var item in dictMostPopularColorSmallImage) {
    //    Logging.WriteLogSimple("dictMostPopularColorSmall=" + (MyColor)item.Key + ";value=" + (MostPopularColorSmallImage)item.Value);
      }
    //  Logging.WriteLogSimple(Logging.DumpObject(dictMostPopularColorSmallImage));
     // System.Diagnostics.Debugger.Break();
      //if (intBackgroundsCtr > 5) {
      //  MessageBox.Show("Could not use background strategy on this image: intBackgroundsCtr > 5");
      //}
    }

    private static MostPopularColorSmallImage AddAColorToSmallImageBackgrounds(Bitmap sub, int strideSub, byte[] dataSub, MostPopularColorSmallImage pMostPopularColorSmallImage, Dictionary<MyColor, int[]> repeatsMostPopular, ref Dictionary<MyColor, MostPopularColorSmallImage> dictMostPopularColorSmallImage, out int intPrevX, out int intPrevY) {
      // add the color of each pixel to a dictionary that contains the color, the x and y coordinates, and 
      // a counter of the number of times found

      for (int y = 0; y < sub.Height; y++) {
        for (int x = 0; x < sub.Width; x++) {

          MyColor curcolor = GetColor(x, y, strideSub, dataSub);
          bool boolContains = false;
          foreach (var item in dictMostPopularColorSmallImage) {
            MyColor keycolor = item.Key;
            if (curcolor.Equals(keycolor)) {
              boolContains = true;
              break;
            }
          }
          // if color is already in dictionary of most popular colors, ignoe it
          if (boolContains) {
            continue;
          }
          int intYAbove = y - 1;
          int intYBelow = y + 1;
          int intXLeft = x - 1;
          int intXRight = x + 1;
          // if above or below are out of bounds, skip it;
          if (intYAbove < 0 || intYBelow > sub.Height) {
            continue;
          }
          if (intXLeft < 0 || intXRight > sub.Width) {
            continue;
          }
          MyColor curcolorAbove = GetColor(x, intYAbove, strideSub, dataSub);
          if (curcolorAbove.Equals(curcolor) == false) {
              continue;            
          }
          MyColor curcolorBelow = GetColor(x, intYBelow, strideSub, dataSub);
          if (curcolorBelow.Equals(curcolor) == false) {
            continue;
          }
          MyColor curcolorLeft = GetColor(intXLeft, y, strideSub, dataSub);
          if (curcolorLeft.Equals(curcolor) == false) {
            continue;
          }
          MyColor curcolorRight = GetColor(intXRight, y, strideSub, dataSub);
          if (curcolorRight.Equals(curcolor) == false) {
            continue;
          }

          // The pixel value has been found before
          bool boolContains1 = false;
          MyColor KeyFound1 = new MyColor();
          foreach (var item1 in repeatsMostPopular) {
            MyColor keycolor = item1.Key;
            if (curcolor.Equals(keycolor)) {
              boolContains1 = true;
              KeyFound1 = keycolor; 
              break;
            }
          }
          if (!boolContains1) {
            // a = {number times found, x location, y location}
            int[] a = { 1, x, y };
            repeatsMostPopular.Add(curcolor, a);
          } else {
            // Increment the number of times the values been found
            ((int[])repeatsMostPopular[KeyFound1])[0]++;
          }
        }
      }

      // Find the pixel value that has been found the most
      // number of times; a[0] is count; a[1] is x; a[2] is y; image[ix, iy] 
      // is the color of the pixel
      int max = int.MinValue, ix = -1, iy = -1;
      MyColor oMyColor = new MyColor();
      foreach (var de in repeatsMostPopular) {
        int[] a = (int[])de.Value;
        if (a[0] > max) {
          max = a[0];
          ix = a[1];
          iy = a[2];
          oMyColor = de.Key;
        }
      }
      // we have found background color for small image
      pMostPopularColorSmallImage.theColor = oMyColor;
      pMostPopularColorSmallImage.thePosition.X = ix;
      pMostPopularColorSmallImage.thePosition.Y = iy;

      // now we want to find a safe position in the small image where the background pixel occurs 
      // three times in a row.  When we find that, we want to grab the previous pixel location
      // so we will have the middle pixel. It is okay if the 3 pixels wrap across two rows.

      intPrevX = -1;
      intPrevY = -1;
      int intThreeInARowCtr = 0;
      for (int y = 0; y < sub.Height; y++) {
        // this breaks us out of the y loop for rows
        if (intThreeInARowCtr == 3) {
          break;
        }
        for (int x = 0; x < sub.Width; x++) {

          MyColor curcolor = GetColor(x, y, strideSub, dataSub);

          if (curcolor.Equals(pMostPopularColorSmallImage.theColor)) {
            intThreeInARowCtr++;
          } else {
            intThreeInARowCtr = 0;
          }
          // this breaks us out of the x loop for columns
          if (intThreeInARowCtr == 3) {
            break;
          }
          intPrevX = x;
          intPrevY = y;
        }
      }

      // we now have the location of the safe background pixel that is located between
      // two background pixels in the small image.  It is a "safe" position because
      // most of the errors occur on the edge of a shape
      if (intPrevX > -1) {
        pMostPopularColorSmallImage.thePosition = new Point(intPrevX, intPrevY);
      }
      //   System.Diagnostics.Debugger.Break();
      //  Console.WriteLine("Most popular background colors for small image: x=" + pMostPopularColorSmallImage.thePosition.X +   "; y=" + pMostPopularColorSmallImage.thePosition.Y + "; theColor=R=" + pMostPopularColorSmallImage.theColor.R + " G=" + pMostPopularColorSmallImage.theColor.G + " B=" + pMostPopularColorSmallImage.theColor.B);
      bool boolContains9 = false;
      foreach (var item9 in dictMostPopularColorSmallImage) {
        MyColor keycolor = item9.Key;
        if (pMostPopularColorSmallImage.theColor.Equals(keycolor)) {
          boolContains9 = true;
          break;
        }
      }
      if (boolContains9 == false) {
        dictMostPopularColorSmallImage.Add(pMostPopularColorSmallImage.theColor, pMostPopularColorSmallImage);
      }
      return pMostPopularColorSmallImage;
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
        int intTolerance) {
      // y for big image
      for (int y = 0; y < mainheight; y++) {
        // x for big image
        for (int x = 0; x < mainwidth; x++) {
          int xtemp = x + 1;
          int ytemp = y;
          if (pLeastPopularColor.theTwoColors.IndexWrapX != -1) {
            xtemp = xtemp - sub.Width;
            ytemp = ytemp + 1;
          }
          if (pLeastPopularColor.theTwoColors.IndexWrapY != -1) {
            ytemp = ytemp - sub.Height;
            xtemp = xtemp - sub.Width;
          }

          // if the indexes are outside the bounds of 
          // the big image, we say there is no match                     
          if (xtemp > main.Width - 1 || xtemp < 0) {
            continue;
          }

          if (ytemp > main.Height - 1 || ytemp < 0) {
            continue;
          }

          MyColor curcolor = GetColor(x, y, strideMain, dataMain);
          MyColor curcolor2 = GetColor(xtemp, ytemp, strideMain, dataMain);

          // We are only looking for places where the 
          // two pixels are different in the big image
          if (curcolor.Equals(curcolor2)) {
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
          if (pLeastPopularColor.theTwoColors.Equals(_myTwoColors) && notFound(x, y, foundRects)) {
            // this finds where rectangle would start
            Point loc = pLeastPopularColor.thePosition;

            int sx = x - loc.X;
            int sy = y - loc.Y;
            // Subimage occurs in desktop image 
            // sx and sy must both be positive
            if (sx > 0 && sy > 0) {
              if (imageThere(dataMain, sx, sy, dataSub, sub, strideMain, strideSub, ref highestPercentCorrect, intTolerance)) {
                SubPositionInfo mySubPositionInfo = new SubPositionInfo();
                mySubPositionInfo.myPoint = new Point(x - loc.X, y - loc.Y);
                mySubPositionInfo.percentcorrect = highestPercentCorrect;
                mySubPositionInfo.strSearchMethod = "UseColorWithLeastPopularColorSearch";
                possiblepos.Add(mySubPositionInfo);
                highestPercentCorrect = 0;
                foundRects.Add(new Rectangle(x, y, sub.Width, sub.Height));
              }
            }
          }

        }
      }
    }
    /// <summary>
    /// x4. Get background for big picture.
    /// x5. Do preliminary check in big picture that tests at least 5 background pixels and 5 foreground pixels and quits when error rate rises above a certain percentage. If there are not 5 background or 5 foreground pixels in small image, just proceed to complete check.
    /// x6. Do complete check in big picture if big picture passes preliminary check.
    /// </summary>
    /// <param name="main"></param>
    /// <param name="sub"></param>
    /// <param name="possiblepos"></param>
    /// <param name="foundRects"></param>
    /// <param name="mainwidth"></param>
    /// <param name="mainheight"></param>
    /// <param name="dataMain"></param>
    /// <param name="strideMain"></param>
    /// <param name="strideSub"></param>
    /// <param name="dataSub"></param>
    /// <param name="pMostPopularColorSmallImage"></param>
    /// <param name="boolArySmallImage"></param>
    /// <param name="highestPercentCorrect"></param>
    /// <param name="intTolerance"></param>
    /// <param name="lstPrelimBackground"></param>
    /// <param name="lstPrelimForeground"></param>
    private static void LoopthruEachPixelInBigImageToFindMatchesOnSmallImageBackgroundPattern(
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
        MostPopularColorSmallImage pMostPopularColorSmallImage,
        bool[,] boolArySmallImage,
        ref decimal highestPercentCorrect,
        int intTolerance,
      List<Point> lstPrelimBackground,
      List<Point> lstPrelimForeground,
      ref Dictionary<MyColor, MostPopularColorSmallImage> dictMostPopularColorSmallImage,
      List<Point> lstHighContrast,
      ref MyColor oDarkestColorSmallImage,
      ref List<Point> olstDarkestColorSmallImage) {
      int xtemp = -1;
      int ytemp = -1;
      bool boolDoPrelimCheck = true;
      bool boolNeedNewRowInBigImage = false;
      bool boolWeAreDoneWithBigImage = false;
      int intPrelimCheckBackground = lstPrelimBackground.Count;
      int intPrelimCheckForeground = lstPrelimForeground.Count;
      int intPrelimCheckTotal = intPrelimCheckBackground + intPrelimCheckForeground;
      decimal intCorrect = 0;
      decimal intWrong = 0;
      decimal intTotalCorrectWrong = 0;
      decimal decHighContrastCount = lstHighContrast.Count;
      decimal decDarkestColorCount = olstDarkestColorSmallImage.Count;
      decimal decCorrectnessWeightBackground = (decimal).49;
      decimal decCorrectnessWeightHighContrast = (decimal).56;
      decimal decCorrectnessWeightDarkestColor = (decimal).79;
      if (decHighContrastCount == 0) {
        decHighContrastCount = 1;
      }
      if (decDarkestColorCount == 0) {
        decDarkestColorCount = 1;
      }
      int intTotalPixelsInSmallImage = sub.Width * sub.Height;
      decimal decWeightForHighContrast = (intTotalPixelsInSmallImage / (lstHighContrast.Count)) * decCorrectnessWeightHighContrast;
      decimal decWeightForDarkestColor = (intTotalPixelsInSmallImage / olstDarkestColorSmallImage.Count) * decCorrectnessWeightDarkestColor;
     

      Dictionary<MyColor, Point> dictMostPopularColorSmallImageWithTemps = new Dictionary<MyColor, Point>();
      Dictionary<MyColor, Point> dictMostPopularColorBigImage = new Dictionary<MyColor, Point>();
      decimal intPercentCorrect = 0;
      bool boolSkipThisPixelInBigImage = false;
      bool booldebuggingmode = false;
      MyColor oDarkestColorBigImage = new MyColor();
      oDarkestColorBigImage.R = 255;
      oDarkestColorBigImage.G = 255;
      oDarkestColorBigImage.B = 255;
      if (intPrelimCheckTotal + lstHighContrast.Count < 10) {
        boolDoPrelimCheck = false;
      }
      // we have saved the relative position of the safe background pixel for the small image
      // and now we want to add that offset to the current pixel in the big image. If adding
      // the offset to the current pixel in the big image throws us out of the bounds of the
      // big image, we are not interested in the current pixel for the big image.

      // this forloop is for each row in the big image - when we go outside the Y boundary of rows,
      // we are done with the big image
      for (int y = 0; y < mainheight; y++) {
        boolNeedNewRowInBigImage = false; // we just got a new row so we need to initialize this
        int intYMax = y + sub.Height;        
        if (boolWeAreDoneWithBigImage || intYMax > mainheight) {
          //   System.Diagnostics.Debugger.Break();
          break;
        }
        dictMostPopularColorSmallImageWithTemps.Clear();
        foreach (var item in dictMostPopularColorSmallImage) {
          pMostPopularColorSmallImage = item.Value;
          ytemp = y + pMostPopularColorSmallImage.thePosition.Y;
          if (ytemp > mainheight) {
            boolWeAreDoneWithBigImage = true;
            break;
          }
          Point oPoint = new Point();
          oPoint.X = -1;
          oPoint.Y = ytemp;
          bool boolContains2 = false;
          foreach (var item2 in dictMostPopularColorSmallImageWithTemps) {
            MyColor keycolor = item2.Key;
            if (pMostPopularColorSmallImage.theColor.Equals(keycolor)) {
              boolContains2 = true;
              break;
            }
          }
          if (boolContains2 == false) {
            dictMostPopularColorSmallImageWithTemps.Add(pMostPopularColorSmallImage.theColor, oPoint);
          }
        }

        // if adding the offset for the rows throws it outside of the big image, we are done
        if (boolWeAreDoneWithBigImage) {
          //     System.Diagnostics.Debugger.Break();
          break;
        }
        // x for big image represents the columns in the big image
        for (int x = 0; x < mainwidth; x++) {
          //if (y == 418 && x == 1239) {
          //  System.Diagnostics.Debugger.Break();
          //  string abc = "abd";
          //  booldebuggingmode = true;
          //} else {
          //  booldebuggingmode = false;
          //}
          int intXMax = x + sub.Width;
          if (boolNeedNewRowInBigImage || intXMax > mainwidth) {
            boolNeedNewRowInBigImage = false;
            break;
          }
          // everytime we get a new pixel, we need to initialize everything for that pixel
          boolSkipThisPixelInBigImage = false;
          intCorrect = 0;
          intWrong = 0;
          // we normally do prelim check unless there are fewer than
          // 10 pixels involved in prelim check - in that case, percentages might be skewed
          // just because we hit a couple of mismatches right at the start. When there are 
          // less than 10 preliminary pixels to check, we go skip prelim check and just do complete check
          boolDoPrelimCheck = true;
          xtemp = x + pMostPopularColorSmallImage.thePosition.X;
          Point oPoint = new Point();
          foreach (var item in dictMostPopularColorSmallImage) {
            pMostPopularColorSmallImage = item.Value;
            xtemp = x + pMostPopularColorSmallImage.thePosition.X;
            if (xtemp > mainwidth) {
              boolNeedNewRowInBigImage = true;
              break;
            }
            bool boolContains3 = false;
            foreach (var item3 in dictMostPopularColorSmallImageWithTemps) {
              MyColor keycolor = item3.Key;
              if (pMostPopularColorSmallImage.theColor.Equals(keycolor)) {
                boolContains3 = true;
                pMostPopularColorSmallImage.theColor = keycolor;
                break;
              }
            }
            if (boolContains3) {
              oPoint.X = xtemp;
              oPoint.Y = dictMostPopularColorSmallImageWithTemps[pMostPopularColorSmallImage.theColor].Y;
              dictMostPopularColorSmallImageWithTemps[pMostPopularColorSmallImage.theColor] = oPoint;
            }

          }
          // if adding offset to column in big image causes it to go outside of columns for big
          // picture, we are not interested in in the rest of the pixels on that line for the big image
          if (boolNeedNewRowInBigImage) {
            break;
          }


          // Adding the offsets did not cause us to go out of bounds so we can now
          // get what we think is the background color for the relative position of the
          // small image within the big one
          
          Point myPoint = new Point();
          MyColor curcolor = new MyColor();
          //if (y > 420 && y < 430) {
          //  curcolor = GetColor(x, y, strideMain, dataMain);
          //  Console.WriteLine("Big Image Loop: x=" + x + "; y=" + y + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
          //}
          dictMostPopularColorBigImage.Clear();
          foreach (var item0 in dictMostPopularColorSmallImageWithTemps) {
            myPoint = item0.Value;
            xtemp = myPoint.X;
            ytemp = myPoint.Y;
            curcolor = GetColor(xtemp, ytemp, strideMain, dataMain);
            MostPopularColorBigImage oMostPopularColorBigImage = new MostPopularColorBigImage();
            oMostPopularColorBigImage.theColor = curcolor;
            oMostPopularColorBigImage.thePosition.X = xtemp;
            oMostPopularColorBigImage.thePosition.Y = ytemp;
            bool boolContains4 = false;
            foreach (var item4 in dictMostPopularColorBigImage) {
              MyColor keycolor = item4.Key;
              if (curcolor.Equals(keycolor)) {
                boolContains4 = true;
                break;
              }
            }
            if (boolContains4 == false) {
              //if (booldebuggingmode) {
              //  Console.WriteLine("Background color for big image: x=" + xtemp + "; y=" + ytemp + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
              //}
              dictMostPopularColorBigImage.Add(oMostPopularColorBigImage.theColor, oMostPopularColorBigImage.thePosition);
            }
          }
          // we need to do preliminary check by going thru the offsets saved in the list for
          // safe background pixels

          // maximum number of prelim background checks is 10
          if (intPrelimCheckBackground > 10) {
            intPrelimCheckBackground = 10;
          }
          // maximum number of prelim foreground checks is 10
          if (intPrelimCheckForeground > 10) {
            intPrelimCheckForeground = 10;
          }
          if (boolDoPrelimCheck) {
            for (int i = 0; i < intPrelimCheckBackground; i++) {
              xtemp = x + lstPrelimBackground[i].X;
              // if adding the offset to the current column in the big picture
              // causes us to go outside of the bounds of the big picture, we
              // know we do not want this pixel for the big image and we no 
              // longer need to do any more preliminary checks for this pixel in the big image
              if (xtemp > mainwidth) {
                boolSkipThisPixelInBigImage = true;
                break;
              }
              ytemp = y + lstPrelimBackground[i].Y;
              // if adding the offset to the current row in the big picture
              // causes us to go outside of the bounds of the big picture, we
              // know we do not want this pixel for the big image and we no 
              // longer need to do any more preliminary checks for this pixel in the big image
              if (ytemp > mainheight) {
                boolSkipThisPixelInBigImage = true;
                break;
              }
              // the position of the preliminary check for background color was within
              // the big image so we know want to know if the relative pixel within the
              // big image is a background color like it should be.
              curcolor = GetColor(xtemp, ytemp, strideMain, dataMain);
              //if (booldebuggingmode) {
              //  Console.WriteLine("Prelim Background check: x=" + x + "; y=" + y + "xtemp=" + xtemp + "; ytemp=" + ytemp + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
              //}

              bool boolContains5 = false;
              foreach (var item5 in dictMostPopularColorBigImage) {
                MyColor keycolor = item5.Key;
                if (curcolor.Equals(keycolor)) {
                  boolContains5 = true;
                  break;
                }
              }
              if (boolContains5) {
                //if (booldebuggingmode) {
                //  Console.WriteLine("Correct");
                //}                
                intCorrect++;
                intCorrect++;
                intCorrect++;
              } else {
                //if (booldebuggingmode) {
                //  Console.WriteLine("InCorrect");
                //}
                intWrong++;
                intWrong++;
                intWrong++;
              }
            }
         //   Logging.WriteLogSimple("Prelim background check done - intCorrect = " + intCorrect + " intWrong = " + intWrong);
            // if we did not run into problems with the relative position for the preliminary background
            // color being outside of the bounds of the big image, then we can proceed to do the preliminary
            // checks for the foreground color
            if (boolSkipThisPixelInBigImage == false) {
              for (int i = 0; i < intPrelimCheckForeground; i++) {
                xtemp = x + lstPrelimForeground[i].X;
                if (xtemp > mainwidth) {
                  boolNeedNewRowInBigImage = true;
                  boolSkipThisPixelInBigImage = true;
                  break;
                }
                ytemp = y + lstPrelimForeground[i].Y;
                if (ytemp > mainheight) {
                  boolWeAreDoneWithBigImage = true;
                  boolSkipThisPixelInBigImage = true;
                  break;
                }
                curcolor = GetColor(xtemp, ytemp, strideMain, dataMain);
                //if (booldebuggingmode) {
                //  Console.WriteLine("Prelim Foreground check: x=" + xtemp + "; y=" + ytemp + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
                //}
                bool boolContains6 = false;
                foreach (var item6 in dictMostPopularColorBigImage) {
                  MyColor keycolor = item6.Key;
                  if (curcolor.Equals(keycolor)) {
                    boolContains6 = true;
                    break;
                  }
                }
                if (boolContains6 == false) {
                  //if (booldebuggingmode) {
                  //  Console.WriteLine("Correct");
                  //}
                  intCorrect++;
                } else {
                  //if (booldebuggingmode) {
                  //  Console.WriteLine("InCorrect");
                  //}
                  intWrong++;
                }
              }
            }
          }
          // if any problems were encountered with the preliminary checks, we just want to get
          // the next pixel in the big image
          if (boolSkipThisPixelInBigImage) {
            continue;
          }
          int intXBig = 0;
          int intXBigNext = 0;
          int intYBig = 0;
          int intPrelimHighContrast = 0;
          foreach (var item in lstHighContrast) {
            intPrelimHighContrast++;
            if (intPrelimHighContrast > 10) {
              break;
            }
            intXBig = item.X + x;
            intYBig = item.Y + y;
            if (intXBig + 2 > mainwidth || intYBig > mainheight) {
              continue;
            }
            intXBigNext = intXBig + 1;
            MyColor bigCurColor = GetColor(intXBig, intYBig, strideMain, dataMain);
            MyColor bigCurColorNext = GetColor(intXBigNext, intYBig, strideMain, dataMain);
            MyColor bigCurColorNextNext = GetColor(intXBigNext, intYBig, strideMain, dataMain);
            if (bigCurColor.HighContrast(bigCurColorNext) || bigCurColor.HighContrast(bigCurColorNextNext)) {
              intCorrect += decWeightForHighContrast;
            } else {
              intWrong += decWeightForHighContrast;
            }
          }

          // we did at least 10 preliminary checks or the boolean flag for doing preliminary checks
          // was set to false. We can now see if the percentage correct for the preliminary checks is 
          // good enough for us to continue on to the full blown complete check
          intTotalCorrectWrong = intCorrect + intWrong;
          intPercentCorrect = ((intCorrect * 100) / intTotalCorrectWrong);
          //if (booldebuggingmode) {
          //  Console.WriteLine("Percent Correct% " + intPercentCorrect.ToString());
          //}

          if (boolDoPrelimCheck == true && intTolerance - 10 > intPercentCorrect) {
            continue;
          }
          // do complete check - x and y still have the value of the current pixel in the big image.
          // We want to loop through every pixel of the small image and add that offset to the current
          // pixel in the big image. We will compare each pixel in the small image to the relative
          // position of the small image within the big image to see if there is a match with regard
          // to background/foreground.  
          // System.Diagnostics.Debugger.Break();
         // Logging.WriteLogSimple("we are doing a complete check at x=" + x + ";y=" +y);
          intCorrect = 0;
          intWrong = 0;
          int intCorrectBackground = 0;
          int intWrongBackground = 0;
          int intCorrectHighContrast = 0;
          int intWrongHighContrast = 0;
          int intCorrectDarkestColor = 0;
          int intWrongDarkestColor = 0;

          for (int sy = 0; sy < sub.Height; sy++) {
            // if we ever go out of the bounds of the big picture, we are no longer interested in this
            // pixel in the big image
            ytemp = y + sy;
            if (ytemp > mainheight) {
              boolWeAreDoneWithBigImage = true;
              boolSkipThisPixelInBigImage = true;
              break;
            }
            bool boolPixelInSubIsBackground = false;
            bool boolNextPixelInSubIsHighContrast = false;
            MyColor myNextPixel;
            int intXNextPixel = -1;
            for (int sx = 0; sx < sub.Width; sx++) {

              // first we find out if the pixel in the sub is background             
              curcolor = GetColor(sx, sy, strideSub, dataSub);
              if (curcolor.LessThan(oDarkestColorBigImage)) {
                oDarkestColorBigImage = curcolor;
              }
              intXNextPixel = sx + 1;
              boolNextPixelInSubIsHighContrast = false;
              if (intXNextPixel <= sub.Width) {
                myNextPixel = GetColor(intXNextPixel, sy, strideSub, dataSub);
                if (curcolor.HighContrast(myNextPixel)) {
                  boolNextPixelInSubIsHighContrast = true;
                }
              }
              //if (booldebuggingmode) {
              //  Console.WriteLine("Complete check - get pixel in sub: sx=" + sx + " sy=" + sy + " xtemp=" + xtemp + "; ytemp=" + ytemp + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
              //}
              bool boolContains7 = false;
              foreach (var item7 in dictMostPopularColorSmallImage) {
                MyColor keycolor = item7.Key;
                if (curcolor.Equals(keycolor)) {
                  boolContains7 = true;
                  break;
                }
              }
              if (boolContains7) {
                boolPixelInSubIsBackground = true;
              } else {
                boolPixelInSubIsBackground = false;
              }

              // then we find if pixel in big image is the same type(background or foreground)
              // as the one in the subimage; if it is the same, we add one to intCorrect; else add 1 to intWrong
              xtemp = x + sx;
              if (xtemp > mainwidth) {
                boolNeedNewRowInBigImage = true;
                boolSkipThisPixelInBigImage = true;
                break;
              }
              bool boolNextPixelInBigIsHighContrast = false;
              bool boolPixelInBigIsBackground;
              curcolor = GetColor(xtemp, ytemp, strideMain, dataMain);
              intXNextPixel = xtemp + 1;
              if (intXNextPixel <= mainwidth) {
                myNextPixel = GetColor(intXNextPixel, ytemp, strideMain, dataMain);
                if (curcolor.HighContrast(myNextPixel)) {
                  boolNextPixelInBigIsHighContrast = true;
                }
              }
              //if (booldebuggingmode) {
              //  Console.WriteLine("Complete check: sx=" + sx + " sy=" + sy + " xtemp=" + xtemp + "; ytemp=" + ytemp + "; curcolor R=" + curcolor.R + " G=" + curcolor.G + " B=" + curcolor.B);
              //}
              bool boolContains8 = false;
              foreach (var item8 in dictMostPopularColorBigImage) {
                MyColor keycolor = item8.Key;
                if (curcolor.Equals(keycolor)) {
                  boolContains8 = true;
                  break;
                }
              }
              if (boolContains8) {
                boolPixelInBigIsBackground = true;
              } else {
                boolPixelInBigIsBackground = false;
              }
              if ((boolPixelInBigIsBackground && boolPixelInSubIsBackground) ||
              (boolPixelInBigIsBackground == false && boolPixelInSubIsBackground == false) && boolNextPixelInSubIsHighContrast == boolNextPixelInBigIsHighContrast) {
                //if (booldebuggingmode) {
                //  Console.WriteLine("Correct Complete check");
                //}
                intCorrectBackground++;
                intCorrect += decCorrectnessWeightBackground;
              } else {
                //if (booldebuggingmode) {
                //  Console.WriteLine("InCorrect Complete check: boolPixelInBigIsBackground=" + boolPixelInBigIsBackground + " boolPixelInSubIsBackground=" + boolPixelInSubIsBackground);
                //}
                intWrongBackground++;
                intWrong += decCorrectnessWeightBackground;
              }
              if (boolNextPixelInSubIsHighContrast == true && boolNextPixelInBigIsHighContrast == true) {
                //if (booldebuggingmode) {
                //  Console.WriteLine("Correct Complete check");
                //}
                intCorrectHighContrast++;
                intCorrect = intCorrect + decWeightForHighContrast;
              }
              if (boolNextPixelInSubIsHighContrast != boolNextPixelInBigIsHighContrast) {
                //if (booldebuggingmode) {
                //  Console.WriteLine("Correct Complete check");
                //}
                intWrongHighContrast++;
                intWrong = intWrong + decWeightForHighContrast;
              }
              intTotalCorrectWrong = intCorrect + intWrong;
              intPercentCorrect = ((intCorrect * 100) / intTotalCorrectWrong);
              //if (booldebuggingmode) {
              //  Console.WriteLine("Percent Correct Complete check" +  intPercentCorrect.ToString());
              //}
              int intTotal = intCorrectBackground + intWrongBackground
                + intCorrectHighContrast + intWrongHighContrast
                + intCorrectDarkestColor + intWrongDarkestColor;
              if (intTolerance - 20 > intPercentCorrect && intTotal > 25) {
//                Logging.WriteLogSimple("Complete check and early exit >25  x=" + x + " y=" + y + "intCorrectBackground=" + intCorrectBackground.ToString() +
//" intWrongBackground=" + intWrongBackground.ToString() +
//" intCorrectHighContrast=" + intCorrectHighContrast.ToString() +
//" intWrongHighContrast=" + intWrongHighContrast.ToString() +
//" intCorrectDarkestColor=" + intCorrectDarkestColor.ToString() +
//" intWrongDarkestColor=" + intWrongDarkestColor.ToString());
                boolSkipThisPixelInBigImage = true;
                break;
              }

            }

          }
          if (boolSkipThisPixelInBigImage) {
            //    System.Diagnostics.Debugger.Break();
            continue;
          }
          foreach (var item in lstHighContrast) {
            intXBig = item.X + x;
            intYBig = item.Y + y;
            if (intXBig + 2 > mainwidth || intYBig > mainheight) {
              continue;
            }
            intXBigNext = intXBig + 1;
            MyColor bigCurColor = GetColor(intXBig, intYBig, strideMain, dataMain);
            MyColor bigCurColorNext = GetColor(intXBigNext, intYBig, strideMain, dataMain);
            MyColor bigCurColorNextNext = GetColor(intXBigNext, intYBig, strideMain, dataMain);
            if (bigCurColor.HighContrast(bigCurColorNext) || bigCurColor.HighContrast(bigCurColorNextNext) ) {
              intCorrectHighContrast++;
              intCorrect += decWeightForHighContrast;
            } else {
              intWrongHighContrast++;
              intWrong += decWeightForHighContrast;
            }
          }
          foreach (var item in olstDarkestColorSmallImage) {
            intXBig = item.X + x;
            intYBig = item.Y + y;
            if (intXBig + 2 > mainwidth || intYBig > mainheight) {
              continue;
            }           
            MyColor bigCurColor = GetColor(intXBig, intYBig, strideMain, dataMain);            
            if (bigCurColor.Equals(oDarkestColorBigImage)) {
              intCorrectDarkestColor++;
              intCorrect += decWeightForDarkestColor;
            } else {
              intWrongDarkestColor++;
              intWrong += decWeightForDarkestColor;
            }
          }
          // if percent correct is less than tolerance, we can skip this pixel in big image
          intTotalCorrectWrong = intCorrect + intWrong;
          intPercentCorrect = ((intCorrect * 100) / intTotalCorrectWrong);
          //if (booldebuggingmode) {
          //  Console.WriteLine("Percent Correct Complete check" +  intPercentCorrect.ToString());
          //}
          if (intTolerance > intPercentCorrect) {
//            Logging.WriteLogSimple("Complete check and tolerance > % correct x=" + x + " y=" + y + "intCorrectBackground=" + intCorrectBackground.ToString() +
//" intWrongBackground=" + intWrongBackground.ToString() +
//" intCorrectHighContrast=" + intCorrectHighContrast.ToString() +
//" intWrongHighContrast=" + intWrongHighContrast.ToString() +
//" intCorrectDarkestColor=" + intCorrectDarkestColor.ToString() +
//" intWrongDarkestColor=" + intWrongDarkestColor.ToString());
            continue;
          }
          Logging.WriteLogSimple("x=" + x + " y=" + y + "intCorrectBackground=" + intCorrectBackground.ToString() +
          " intWrongBackground=" + intWrongBackground.ToString() +
          " intCorrectHighContrast=" + intCorrectHighContrast.ToString() +
          " intWrongHighContrast=" + intWrongHighContrast.ToString() +
          " intCorrectDarkestColor=" + intCorrectDarkestColor.ToString() +
         " intWrongDarkestColor=" + intWrongDarkestColor.ToString());

          //we found a match
          if (intPercentCorrect > intTolerance) {
            intTolerance = (int)intPercentCorrect;
          }
          SubPositionInfo mySubPositionInfo = new SubPositionInfo();
          mySubPositionInfo.myPoint = new Point(x, y);
          mySubPositionInfo.percentcorrect = intPercentCorrect;
          possiblepos.Add(mySubPositionInfo);
          highestPercentCorrect = 0;
          mySubPositionInfo.strSearchMethod = "UsePatternBasedBackgroundForegroundPlusHighContrast";
          foundRects.Add(new Rectangle(x, y, sub.Width, sub.Height));

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
  int intTolerance) {

      for (int y = 0; y < mainheight; y++) {
        // x for big image
        for (int x = 0; x < mainwidth; x++) {
          if (y == 589 && x == 434) {
            // System.Diagnostics.Debugger.Break();
            string abc = "abd";
          }
          MyPattern _myBigPattern = MyPattern.GetHighContrastPatternInBigImage(x, y, pLeastPopularPattern, sub.Width, sub.Height, strideMain, dataMain, mainwidth, mainheight);

          // Pixle value from subimage in desktop image
          if (_myBigPattern.discard == false
              && pLeastPopularPattern.thePattern.Equals(_myBigPattern)
              && notFound(x, y, foundRects)) {
            // this finds where rectangle would start
            Point loc = pLeastPopularPattern.thePosition;

            int sx = x - loc.X;
            int sy = y - loc.Y;
            // Subimage occurs in desktop image 
            // sx and sy must both be positive
            if (sx > -1 && sy > -1) {
              if (imageTherePattern(sx, sy, boolArySmallImage, sub, strideMain, strideSub, dataMain, mainwidth, mainheight, ref highestPercentCorrect, intTolerance)) {
                SubPositionInfo mySubPositionInfo = new SubPositionInfo();
                mySubPositionInfo.myPoint = new Point(x - loc.X, y - loc.Y);
                mySubPositionInfo.percentcorrect = highestPercentCorrect;
                mySubPositionInfo.strSearchMethod = "UsePatternBasedOnHighContrast";
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
        Dictionary<MyTwoColors, int[]> repeats) {
      int IndexWrapColorX = -1;
      int IndexWrapColorY = -1;
      bool[,] boolArySmallImage = new bool[subwidth, subheight];
      for (int y = 0; y < sub.Height; y++) {
        for (int x = 0; x < sub.Width; x++) {
          IndexWrapColorX = -1;
          IndexWrapColorY = -1;
          int xtemp = x + 1;
          int ytemp = y;
          if (xtemp > sub.Width - 1) {
            IndexWrapColorX = 1;
            xtemp = 0;
            ytemp = y + 1;
            if (ytemp > sub.Height - 1) {
              IndexWrapColorY = 1;
              ytemp = 0;
            }
          }
          MyColor curcolor = GetColor(x, y, strideSub, dataSub);
          MyColor curcolor2 = GetColor(xtemp, ytemp, strideSub, dataSub);

          if (curcolor.Equals(curcolor2)) {
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
          if (!repeats.ContainsKey(_myTwoColors)) {
            // a = {number times found, x location, y location}
            int[] a = { 1, x, y };
            repeats.Add(_myTwoColors, a);
          } else {
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
      foreach (var de in repeats) {
        int[] a = (int[])de.Value;
        if (a[0] < min) {
          min = a[0];
          ix = a[1];
          iy = a[2];
          oMyTwoColors = de.Key;
        }
      }

      pLeastPopularColor.theTwoColors = oMyTwoColors;
      pLeastPopularColor.thePosition = new Point(ix, iy);

    }


    private static void FindLeastPopularHighContrastPatternInSmallImage(
        Bitmap sub,
        int subwidth,
        int subheight,
        int strideSub,
        byte[] dataSub,
        ref LeastPopularPattern pLeastPopularPattern,
        ref LeastPopularPattern pLeastPopularPattern2,
        Dictionary<MyPattern, int[]> repeatsPattern,
        ref bool[,] boolArySmallImage) {
      // boolArySmallImage is an array that contains
      // true for an element if the corresponding pixel
      // is identical to the next pixel; otherwise, it is false

      for (int y = 0; y < sub.Height; y++) {
        for (int x = 0; x < sub.Width; x++) {
          int xtemp = x + 1;
          int ytemp = y;
          if (xtemp > sub.Width - 1) {
            xtemp = 0;
            ytemp = y + 1;
            if (ytemp > sub.Height - 1) {
              ytemp = 0;
            }
          }
          MyColor curcolor = GetColor(x, y, strideSub, dataSub);
          MyColor curcolor2 = GetColor(xtemp, ytemp, strideSub, dataSub);

          if (curcolor.HighContrast(curcolor2)) {
            boolArySmallImage[x, y] = true;
            continue;
          } else {
            boolArySmallImage[x, y] = false;
          }
        }
      }

      // this is used if we are trying to find patterns
      for (int y1 = 0; y1 < sub.Height; y1++) {
        for (int x1 = 0; x1 < sub.Width; x1++) {

          MyPattern _myPattern = MyPattern.GetHighContrastPatternInBoolArySmallImage(x1, y1, boolArySmallImage, sub.Width, sub.Height);

          // The pixel value has been found before
          if (!repeatsPattern.ContainsKey(_myPattern)) {
            // a = {number times found, x location, y location}
            int[] a = { 1, x1, y1 };
            repeatsPattern.Add(_myPattern, a);
          } else {
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
      oMyPattern.discard = false;
      foreach (var de in repeatsPattern) {
        int[] a = (int[])de.Value;
        if (a[0] < minP) {
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
      foreach (var de in repeatsPattern) {
        MyPattern oMyPattern1 = de.Key;
        int[] a = (int[])de.Value;
        ixP = a[1];
        iyP = a[2];
        if (ixP >= intholdixP && ixP <= maxixP && iyP == holdiyP) {
          myKeys.Add(de.Key);
        }
        if (ixP < 10 && iyP == holdiyP + 1 && oMyPattern1.IndexWrapX > -1) {
          myKeys.Add(de.Key);
        }
      }
      foreach (var item in myKeys) {
        repeatsPattern.Remove(item);
      }


      minP = int.MaxValue;
      ixP = -1;
      iyP = -1;
      oMyPattern = new MyPattern();
      oMyPattern.ColorWrapX = -1;
      oMyPattern.ColorWrapY = -1;
      oMyPattern.discard = false;
      foreach (var de in repeatsPattern) {
        int[] a = (int[])de.Value;
        if (a[0] < minP) {
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


    private static void CreateDataSubByteArray(Bitmap sub, int subwidth, int subheight, out BitmapData bmSubData, out int bytesSub, out int strideSub, out System.IntPtr Scan0Sub, out byte[] dataSub) {
      bmSubData = sub.LockBits(new Rectangle(0, 0, subwidth, subheight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
      bytesSub = Math.Abs(bmSubData.Stride) * subheight;
      strideSub = bmSubData.Stride;
      Scan0Sub = bmSubData.Scan0;
      dataSub = new byte[bytesSub];
      System.Runtime.InteropServices.Marshal.Copy(Scan0Sub, dataSub, 0, bytesSub);
    }

    private static void CreateDataMainByteArray(Bitmap main, int mainwidth, int mainheight, out byte[] dataMain, out BitmapData bmMainData, out int bytesMain, out int strideMain, out System.IntPtr Scan0Main) {
      bmMainData = main.LockBits(new Rectangle(0, 0, mainwidth, mainheight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
      bytesMain = Math.Abs(bmMainData.Stride) * mainheight;
      strideMain = bmMainData.Stride;
      Scan0Main = bmMainData.Scan0;
      dataMain = new byte[bytesMain];
      System.Runtime.InteropServices.Marshal.Copy(Scan0Main, dataMain, 0, bytesMain);
    }

    private static MyColor GetColor(Point point, int stride, byte[] data) {
      return GetColor(point.X, point.Y, stride, data);
    }

    private static MyColor GetColor(int x, int y, int stride, byte[] data) {
      int pos = y * stride + x * 4;
      if (pos + 3 > data.Length) {
        return MyColor.FromARGB(0x00, 0x00, 0x00, 0x00);
      }
      byte a = data[pos + 3];
      byte r = data[pos + 2];
      byte g = data[pos + 1];
      byte b = data[pos + 0];
      return MyColor.FromARGB(a, r, g, b);
    }

    struct MyColor {
      public byte A;
      public byte R;
      public byte G;
      public byte B;

      public static MyColor FromARGB(byte a, byte r, byte g, byte b) {
        MyColor mc = new MyColor();
        mc.A = a;
        mc.R = r;
        mc.G = g;
        mc.B = b;
        return mc;
      }

      public  bool LessThan(object obj) {
        // this is big image color and obj is small image color
        if (!(obj is MyColor))
          return false;
        MyColor color = (MyColor)obj;
        int intColorDiff = (this.R - color.R) +
            (this.G - color.G) +
             (this.B - color.B);
        if (intColorDiff < 0) {
          return true;
        }
        return false;
      }

      public override bool Equals(object obj) {
        // this is big image color and obj is small image color
        if (!(obj is MyColor))
          return false;
        MyColor color = (MyColor)obj;
        int intColorDiff = (color.R - this.R) +
            (color.G - this.G) +
             (color.B - this.B);
        if (intColorDiff < 0) {
          intColorDiff = intColorDiff * -1;
        }
        if (intColorDiff < 11)
          return true;
        return false;
      }
      public bool HighContrast(object obj) {
        // this is big image color and obj is small image color
        if (!(obj is MyColor))
          return false;
        MyColor color = (MyColor)obj;
        int intColorDiff = (color.R - this.R) +
            (color.G - this.G) +
             (color.B - this.B);
        if (intColorDiff < 0) {
          intColorDiff = intColorDiff * -1;
        }
        if (intColorDiff > 225)
          return true;
        return false;
      }
    }
    struct MyPattern {
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
      public bool discard;

      public static MyPattern GetHighContrastPatternInBoolArySmallImage(int x, int y, bool[,] myBoolArySmallImage, int smallWidth, int smallHeight) {
        MyPattern mp = new MyPattern();
        mp.ColorWrapX = -1;
        mp.ColorWrapY = -1;
        mp.discard = false;
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

      public static MyPattern GetBackgroundPatternInBigImage(
      int x,
      int y,
      LeastPopularPattern pLeastPopularPattern,
      int smallWidth,
      int smallHeight,
      int strideMain,
      byte[] dataMain,
      int bigWidth,
      int bigHeight
      ) {
        // First get two colors for the current pixel
        // and the next one; if they are the same,
        // the bool result for that pixel is true; otherwise,
        // it is false. This is very similar to what I did when
        // creating boolarray for small image
        MyPattern mp = new MyPattern();
        mp.ColorWrapX = -1;
        mp.ColorWrapY = -1;
        mp.discard = false;
        int intOrigX = x;
        int intOrigY = y;
        mp.IndexWrapX = -1;
        mp.IndexWrapY = -1;
        int patternpixelposition;
        patternpixelposition = 1;
        mp.bool1 = GetBoolForPixelinBigImageForPixelBackgroundPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        patternpixelposition = 2;
        if (mp.discard == false) {
          mp.bool2 = GetBoolForPixelinBigImageForPixelBackgroundPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 3;
        if (mp.discard == false) {
          mp.bool3 = GetBoolForPixelinBigImageForPixelBackgroundPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 4;
        if (mp.discard == false) {
          mp.bool4 = GetBoolForPixelinBigImageForPixelBackgroundPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 5;
        if (mp.discard == false) {
          mp.bool5 = GetBoolForPixelinBigImageForPixelBackgroundPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 6;
        if (mp.discard == false) {
          mp.bool6 = GetBoolForPixelinBigImageForPixelBackgroundPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 7;
        if (mp.discard == false) {
          mp.bool7 = GetBoolForPixelinBigImageForPixelBackgroundPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 8;
        if (mp.discard == false) {
          mp.bool8 = GetBoolForPixelinBigImageForPixelBackgroundPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 9;
        if (mp.discard == false) {
          mp.bool9 = GetBoolForPixelinBigImageForPixelBackgroundPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 10;
        if (mp.discard == false) {
          mp.bool10 = GetBoolForPixelinBigImageForPixelBackgroundPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }

        return mp;
      }
      public static MyPattern GetHighContrastPatternInBigImage(
          int x,
          int y,
          LeastPopularPattern pLeastPopularPattern,
          int smallWidth,
          int smallHeight,
          int strideMain,
          byte[] dataMain,
          int bigWidth,
          int bigHeight
          ) {
        // First get two colors for the current pixel
        // and the next one; if they are the same,
        // the bool result for that pixel is true; otherwise,
        // it is false. This is very similar to what I did when
        // creating boolarray for small image
        MyPattern mp = new MyPattern();
        mp.ColorWrapX = -1;
        mp.ColorWrapY = -1;
        mp.discard = false;
        int intOrigX = x;
        int intOrigY = y;
        mp.IndexWrapX = -1;
        mp.IndexWrapY = -1;
        int patternpixelposition;
        patternpixelposition = 1;
        mp.bool1 = GetBoolForPixelinBigImageForPixelHighContrastPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        patternpixelposition = 2;
        if (mp.discard == false) {
          mp.bool2 = GetBoolForPixelinBigImageForPixelHighContrastPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 3;
        if (mp.discard == false) {
          mp.bool3 = GetBoolForPixelinBigImageForPixelHighContrastPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 4;
        if (mp.discard == false) {
          mp.bool4 = GetBoolForPixelinBigImageForPixelHighContrastPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 5;
        if (mp.discard == false) {
          mp.bool5 = GetBoolForPixelinBigImageForPixelHighContrastPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 6;
        if (mp.discard == false) {
          mp.bool6 = GetBoolForPixelinBigImageForPixelHighContrastPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 7;
        if (mp.discard == false) {
          mp.bool7 = GetBoolForPixelinBigImageForPixelHighContrastPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 8;
        if (mp.discard == false) {
          mp.bool8 = GetBoolForPixelinBigImageForPixelHighContrastPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 9;
        if (mp.discard == false) {
          mp.bool9 = GetBoolForPixelinBigImageForPixelHighContrastPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }
        patternpixelposition = 10;
        if (mp.discard == false) {
          mp.bool10 = GetBoolForPixelinBigImageForPixelHighContrastPatternPosition(ref x, ref y, ref pLeastPopularPattern, smallWidth, smallHeight, strideMain, dataMain, bigWidth, bigHeight, ref mp, patternpixelposition);
        }

        return mp;
      }

      private static bool GetBoolForPixelinBigImageForPixelBackgroundPatternPosition(ref int x, ref int y, ref LeastPopularPattern pLeastPopularPattern, int smallWidth, int smallHeight, int strideMain, byte[] dataMain, int bigWidth, int bigHeight, ref MyPattern mp, int patternpixelposition) {
        if (pLeastPopularPattern.thePattern.IndexWrapX == patternpixelposition) {
          mp.IndexWrapX = patternpixelposition;
          x = x - smallWidth;
          y = y + 1;
        }
        if (pLeastPopularPattern.thePattern.IndexWrapY == patternpixelposition) {
          mp.IndexWrapY = patternpixelposition;
          x = x - smallWidth;
          y = y - smallHeight;
        }
        // if the indexes are outside the bounds of 
        // the big image, we say there is no match                     
        if (x > bigWidth - 1 || x < 0) {
          mp.discard = true;
          return false;
        }

        if (y > bigHeight - 1 || y < 0) {
          mp.discard = true;
          return false;
        }
        int xtemp = x + 1;
        int ytemp = y;
        if (pLeastPopularPattern.thePattern.ColorWrapX == patternpixelposition) {
          xtemp = xtemp - smallWidth;
          ytemp = ytemp + 1;
        }
        if (pLeastPopularPattern.thePattern.ColorWrapY == patternpixelposition) {
          xtemp = (x + 1) - smallWidth;
          ytemp = (y + 1) - smallHeight;
        }

        if (xtemp > bigWidth - 1 || xtemp < 0) {
          mp.discard = true;
          return false;
        }

        if (ytemp > bigHeight - 1 || ytemp < 0) {
          mp.discard = true;
          return false;
        }

        MyColor curcolor = GetColor(x, y, strideMain, dataMain);
        MyColor curcolor2 = GetColor(xtemp, ytemp, strideMain, dataMain);

        if (curcolor.Equals(curcolor2)) {
          x = x + 1;
          return true;
        } else {
          x = x + 1;
          return false;
        }
      }
      private static bool GetBoolForPixelinBigImageForPixelHighContrastPatternPosition(ref int x, ref int y, ref LeastPopularPattern pLeastPopularPattern, int smallWidth, int smallHeight, int strideMain, byte[] dataMain, int bigWidth, int bigHeight, ref MyPattern mp, int patternpixelposition) {
        if (pLeastPopularPattern.thePattern.IndexWrapX == patternpixelposition) {
          mp.IndexWrapX = patternpixelposition;
          x = x - smallWidth;
          y = y + 1;
        }
        if (pLeastPopularPattern.thePattern.IndexWrapY == patternpixelposition) {
          mp.IndexWrapY = patternpixelposition;
          x = x - smallWidth;
          y = y - smallHeight;
        }
        // if the indexes are outside the bounds of 
        // the big image, we say there is no match                     
        if (x > bigWidth - 1 || x < 0) {
          mp.discard = true;
          return false;
        }

        if (y > bigHeight - 1 || y < 0) {
          mp.discard = true;
          return false;
        }
        int xtemp = x + 1;
        int ytemp = y;
        if (pLeastPopularPattern.thePattern.ColorWrapX == patternpixelposition) {
          xtemp = xtemp - smallWidth;
          ytemp = ytemp + 1;
        }
        if (pLeastPopularPattern.thePattern.ColorWrapY == patternpixelposition) {
          xtemp = (x + 1) - smallWidth;
          ytemp = (y + 1) - smallHeight;
        }

        if (xtemp > bigWidth - 1 || xtemp < 0) {
          mp.discard = true;
          return false;
        }

        if (ytemp > bigHeight - 1 || ytemp < 0) {
          mp.discard = true;
          return false;
        }

        MyColor curcolor = GetColor(x, y, strideMain, dataMain);
        MyColor curcolor2 = GetColor(xtemp, ytemp, strideMain, dataMain);

        if (curcolor.HighContrast(curcolor2)) {
          x = x + 1;
          return true;
        } else {
          x = x + 1;
          return false;
        }
      }

      public override bool Equals(object obj) {
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
          ) {
        x++;
        if (x == smallWidth - 1) {
          mp.ColorWrapX = patternpixel;
        }
        if (x == smallWidth - 1 && y == smallHeight - 1) {
          mp.ColorWrapY = patternpixel;
        }
        if (x > smallWidth - 1) {
          mp.IndexWrapX = patternpixel;
          x = 0;
          y++;
          if (y > smallHeight - 1) {
            mp.IndexWrapY = patternpixel;
            y = 0;
          }

        }
      }
    }
    struct LeastPopularColor {
      public MyTwoColors theTwoColors;
      public Point thePosition;
    }
    struct MostPopularColorSmallImage {
      public MyColor theColor;
      public Point thePosition;
    }
    struct MostPopularColorBigImage {
      public MyColor theColor;
      public Point thePosition;
    }
    struct LeastPopularPattern {
      public MyPattern thePattern;
      public Point thePosition;
    }
    struct MyTwoColors {
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

      public static MyTwoColors FromARGB2(byte a, byte r, byte g, byte b, byte a2, byte r2, byte g2, byte b2, int X, int Y) {
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

      public override bool Equals(object obj) {
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
        if (intColorDiff < 0) {
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
    public static Bitmap getDesktopBitmap(bool UseGrayScale) {
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
      if (UseGrayScale) {
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
    private static bool imageThere(Byte[] dataMain, int sx, int sy, Byte[] dataSub, Bitmap sub, int strideMain, int strideSub, ref decimal highestPercentCorrect, int intTolerance) {
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
      for (ySub = 0; ySub < sub.Height; ySub++) {
        // Horizontal line of pixles in the main bitmap data               
        yMain = sy + ySub;
        for (xSub = 0; xSub < sub.Width; xSub++) {
          xMain = (sx + xSub);
          MyColor myBigColor = GetColor(xMain, yMain, strideMain, dataMain);
          MyColor mySmallColor = GetColor(xSub, ySub, strideSub, dataSub);

          int intColorDiff = (mySmallColor.R - myBigColor.R) +
              (mySmallColor.G - myBigColor.G) +
              (mySmallColor.B - myBigColor.B);
          if (intColorDiff < 0) {
            intColorDiff = intColorDiff * -1;
          }
          // I want to ignore everything that is wrong on a row 
          // from the first yellow pixel on that row until the last one

          // this identifies the first yellow pixel on a row
          if (ySub != intSaveY && mySmallColor.B == 0 && mySmallColor.R > 0) {
            intSaveY = ySub;
            intStartLineWrong = intWrong;
          }

          // this identifies the last yellow pixel on a row
          if (ySub == intSaveY && mySmallColor.B == 0 && mySmallColor.R > 0) {
            intSaveY = ySub;
            intEndLineWrong = intWrong;
            intNumberIgnored += 1;
          }

          // we are changing rows and found some yellow on previous row
          // we need to adjust wrong and reset intStartLineWrong
          if (ySub != intSaveY && intStartLineWrong != -1) {
            intWrong = intWrong - (intEndLineWrong - intStartLineWrong) - 1;
            intStartLineWrong = -1;
          }
          if (intColorDiff > 10) {

            intWrong += 1;
          } else {
            intRight += 1;
          }
          int intTotalCorrectWrong = intRight + intWrong;
          int intPercentCorrect = ((intRight * 100) / intTotalCorrectWrong);
          //if (booldebuggingmode) {
          //  Console.WriteLine("Percent Correct Complete check" +  intPercentCorrect.ToString());
          //}
          if (intTolerance - 20 > intPercentCorrect && intTotalCorrectWrong > 25) {           
            break;
          }
        }
        xSub = 0;
      }

      int intTotal = intWrong + intRight;
      decimal intPercentRight = (intRight * 100) / intTotal;
      if (intPercentRight > highestPercentCorrect) {
        highestPercentCorrect = intPercentRight;
      }
      if (intWrong < intRight && intWrong < 10 || (intTolerance != 0 && intPercentRight > intTolerance)) {
        //Console.WriteLine(strSubImageFileName + " intRight =" + intRight.ToString() + " intWrong =" + intWrong.ToString() + " yMain=" + yMain.ToString() + " xMain=" + xMain.ToString() + "Found it");
        return true;
      } else {
        //if ((intRight / (intRight + intWrong)) * 100 > 75)
        //{
        //    Console.WriteLine(strSubImageFileName + " intRight =" + intRight.ToString() + " intWrong =" + intWrong.ToString() + " yMain=" + yMain.ToString() + " xMain=" + xMain.ToString());
        //}
        return false;
      }
    }
    private static bool imageTherePattern(int sx, int sy, bool[,] boolArySmallImage, Bitmap sub, int strideMain, int strideSub, byte[] dataMain, int pMainWidth, int pMainHeight, ref decimal highestPercentCorrect, int intTolerance) {
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
      for (ySub = 0; ySub < sub.Height; ySub++) {
        // Horizontal line of pixles in the main bitmap data               
        yMain = sy + ySub;
        for (xSub = 0; xSub < sub.Width - 1; xSub++) {
          xMain = (sx + xSub);
          if (xMain > pMainWidth) {
            return false;
          }
          if (yMain > pMainHeight) {
            return false;
          }
          int xtemp = xMain + 1;
          int ytemp = yMain;
          if (xSub == sub.Width - 1) {
            xtemp = xtemp - sub.Width;
            ytemp = ytemp + 1;
            if (ySub == sub.Height - 1) {
              ySub = (ySub + 1) - sub.Height;
            }
          }
          MyColor curcolor = GetColor(xMain, yMain, strideMain, dataMain);
          MyColor curcolor2 = GetColor(xtemp, ytemp, strideMain, dataMain);

          if (curcolor.HighContrast(curcolor2)) {
            boolArrayForRectangleInBigImage[xSub, ySub] = true;
            continue;
          } else {
            boolArrayForRectangleInBigImage[xSub, ySub] = false;
          }
        }
      }
      for (ySub = 0; ySub < sub.Height; ySub++) {
        for (xSub = 0; xSub < sub.Width; xSub++) {

          if (boolArrayForRectangleInBigImage[xSub, ySub] ==
              boolArySmallImage[xSub, ySub]) {
            //if (sy > 730)
            //{
            //    Console.WriteLine("Right xSub=" + xSub.ToString() + "ySub=" + ySub.ToString() + "bigImage=" + boolArrayForRectangleInBigImage[xSub, ySub] + "smallImage=" + boolArySmallImage[xSub, ySub]);
            //}
            intRight += 1;
          } else {
            //if (sy > 730)
            //{
            //    Console.WriteLine("Wrong xSub=" + xSub.ToString() + "ySub=" + ySub.ToString() + "bigImage=" + boolArrayForRectangleInBigImage[xSub, ySub] + "smallImage=" + boolArySmallImage[xSub, ySub]);
            //}
            intWrong += 1;
          }
          if (intWrong > 2500) {
            //    Console.WriteLine(strSubImageFileName + " intRight =" + intRight.ToString() + " intWrong =" + intWrong.ToString() + " yMain=" + yMain.ToString() + " xMain=" + xMain.ToString());
            return false;
          }
        }
        xSub = 0;
      }
      int intTotal = intWrong + intRight;
      decimal intPercentRight = (intRight * 100) / intTotal;
      if (intPercentRight > highestPercentCorrect) {
        highestPercentCorrect = intPercentRight;
      }
      if (intPercentRight > 86 || (intTolerance != 0 && intPercentRight > intTolerance)) {
        // Console.WriteLine(strSubImageFileName + " intRight =" + intRight.ToString() + " intWrong =" + intWrong.ToString() + " yMain=" + yMain.ToString() + " xMain=" + xMain.ToString() + "PercentRight=" + intPercentRight.ToString() + "Found it");
        return true;
      } else {
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
    private static bool notFound(int x, int y, List<Rectangle> foundRects) {
      foreach (Rectangle r in foundRects) {
        if (r.Contains(new Point(x, y)))
          return false;
      }

      return true;
    }
    public static Image ConvertToGrayscale(Image image) {
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
      using (Graphics g = Graphics.FromImage(grayscaleImage)) {
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
