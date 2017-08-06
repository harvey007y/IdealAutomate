using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


// SearchForAColumn(string myLine, List<string> lsBeginDelim, List<string> lsEndDelim, ref string strCol, ref int intDelimFound)

public class FindDelimitedTextParms {
  public FindDelimitedTextParms(List<string> beginDelim, List<string> endDelim) {
    this.lsBeginDelim = beginDelim;
    this.lsEndDelim = endDelim;
    this.lines = new string[5000];
  }
  public string[] lines { get; set; }
  public int intStartingCol { get; set; }
  public List<String> lsBeginDelim { get; set; }
  public List<String> lsEndDelim { get; set; }
  // intLineCtr is both an input and output parameter
  public int intLineCtr { get; set; }
  // output parameters follow
  public string strDelimitedTextFound { get; set; }
  // the number in intDelimFound will be the index of the begin and end delimiter pair
  public int intDelimFound { get; set; }
  public string strResultTypeFound { get; set; }
  public int intEndDelimColPosFound { get; set; }  
  
}