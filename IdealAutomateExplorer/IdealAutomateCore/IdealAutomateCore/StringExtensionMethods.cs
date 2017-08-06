using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdealAutomate.Core {
  public static class StringExtensionMethods {
    /// <summary>
    /// ReplaceFirst provides an easy way to only replace the first occurrence of a string
    /// Example: myOriginalString = myOriginalString.ReplaceFirst(mySearchString,myReplacementString)
    /// </summary>
    /// <param name="text"></param>
    /// <param name="search"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    public static string ReplaceFirst(this string text, string search, string replace) {
      int pos = text.IndexOf(search);
      if (pos < 0) {
        return text;
      }
      return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }
    /// <summary>
    /// GetNthIndex allows you to specify a reference starting point within a string and then find the
    /// index of the nth occurrence of a search string.  If no reference starting point is supplied,
    /// you start at the beginning.
    /// </summary>
    /// <param name="pOriginalString"></param>
    /// <param name="pReferencePlaceString"></param>
    /// <param name="pOccurrenceString"></param>
    /// <param name="pOccurrence"></param>
    /// <returns></returns>
    public static int GetNthIndex(this string pOriginalString, string pReferencePlaceString, string pOccurrenceString, int pOccurrence) {
      int intCurrentIndex = -1;
      int intCumulativeIndex = 0;
      if (pReferencePlaceString != "") {
        intCurrentIndex = pOriginalString.IndexOf(pReferencePlaceString);
        intCumulativeIndex = intCurrentIndex + pReferencePlaceString.Length;
        pOriginalString = pOriginalString.Substring(intCumulativeIndex);
      }
      bool boolFirstTime = true;
      for (int i = 0; i < pOccurrence; i++) {
        intCurrentIndex = pOriginalString.IndexOf(pOccurrenceString);
        if (intCurrentIndex == -1) {
          return intCurrentIndex;
        }
        if (boolFirstTime) {
          boolFirstTime = false;
          intCumulativeIndex += intCurrentIndex;
        } else {
          intCumulativeIndex += intCurrentIndex + 1;
        }
        pOriginalString = pOriginalString.Substring(intCurrentIndex + 1);
      }
      return intCumulativeIndex;
    }

    public static string SubstringBetweenIndexes(this string value, int startIndex, int endIndex) {
      return value.Substring(startIndex, endIndex - startIndex);
    }

    public static string RemoveBetweenIndexes(this string value, int startIndex, int endIndex) {
      return value.Remove(startIndex, endIndex - startIndex);
    }

  }
}
