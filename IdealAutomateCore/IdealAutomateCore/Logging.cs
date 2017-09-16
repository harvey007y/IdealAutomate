using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace IdealAutomateCore {
    public static class Logging {
        public static void WriteLogSimple(string pMsg) {

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            directory = directory.Replace("\\bin\\Debug\\", "");
            int intLastSlashIndex = directory.LastIndexOf("\\");
            directory = directory.Substring(0, intLastSlashIndex);
          //  string strScriptName = directory.Substring(intLastSlashIndex + 1);
            // string strScriptName = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
            string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\" + ConvertFullFileNameToScriptPath(directory);
      if (!Directory.Exists(settingsDirectory)) {
                Directory.CreateDirectory(settingsDirectory);
            }
            string filePath = Path.Combine(settingsDirectory, "IdealAutomateLog.txt");
            //System.Web.HttpContext.Current.Server.MapPath("~//Trace.html")
            StreamWriter sw = null;

            if (File.Exists(filePath) == false) {
                // Create a file to write to.
                sw = File.CreateText(filePath);

                sw.WriteLine(" ");

                sw.Flush();
                sw.Close();
            }

            try {
                sw = File.AppendText(filePath);
                sw.WriteLine(System.DateTime.Now + " " + pMsg);
                sw.Flush();

                sw.Close();
            } catch (Exception Ex) {
            }
        }

    private static string ConvertFullFileNameToScriptPath(string fullFileName) {
      int intIndex = fullFileName.LastIndexOf(@"/");
      if (intIndex > -1) {
        fullFileName = fullFileName.Substring(0, intIndex);
      }
      string scriptPath = fullFileName;
      scriptPath = scriptPath.Replace(":", "+").Replace(@"\", "-");
      return scriptPath;
    }
    public static void WriteLog(string pClass, string pMethod, string pLineNumber, string pText, string pData) {

            if (Directory.Exists("C:\\Data") == false) {
                Directory.CreateDirectory("C:\\Data");
            }

            string filePath = "C:\\Data\\Trace.html";
            //System.Web.HttpContext.Current.Server.MapPath("~//Trace.html")
            StreamWriter sw = null;

            if (File.Exists(filePath) == false) {
                // Create a file to write to.
                sw = File.CreateText(filePath);
                sw.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\">");
                sw.WriteLine("<html>");
                sw.WriteLine("<head>");
                sw.WriteLine("<meta content=\"text/html; charset=ISO-8859-1\" http-equiv=\"content-type\">");
                sw.WriteLine("<title>Ideal Tracer</title>");
                sw.WriteLine("</head>");
                sw.WriteLine("<body>");
                sw.WriteLine("<table border-style=\"solid\" border=\"1px\" bordercolor=\"black\" width=\"100%\" style=\"margin-top:10px;\">");

                sw.Flush();
                sw.Close();
            }

            try {
                sw = File.AppendText(filePath);
                sw.WriteLine("<tr style=\"vertical-align: top;\"><td>" + System.DateTime.Now + "</td><td>" + pClass + "</td><td>" + pMethod + "</td><td>" + pLineNumber + "</td><td>" + pText + "</td><td>" + pData + "</td></tr>");
                sw.Flush();

                sw.Close();
            } catch (Exception Ex) {
            }
        }
        // Example usage WriteLog(DumpObject(yourcomplexobjectname)) 
        public static string DumpObject(object obj) {
            return DumpObject(obj, -1);
        }

        public static string DumpObject(object obj, int MaxLevel) {
            StringBuilder sb = null;

            sb = new StringBuilder(10000);
            if (obj == null) {
                return "Nothing";
            } else {
                PrivDump(sb, obj, "[ObjectToDump]", 0, MaxLevel);
                return sb.ToString();
            }
        }

        public static object GetFieldValue(object obj, string fieldName) {
            FieldInfo fi = null;
            Type t = null;

            t = obj.GetType();
            fi = t.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi == null) {
                return null;
            } else {
                return fi.GetValue(obj);
            }
        }


        public static void PrivDump(StringBuilder sb, object obj, string objName, int level, int MaxLevel) {

            if (obj == null) {
                return;
            }
            if (MaxLevel >= 0 && level >= MaxLevel) {
                return;
            }

            string padstr = null;
            padstr = "";
            for (int i = 0; i < level; i++) {
                if (i < level - 1) {
                    padstr += "|";
                } else {
                    padstr += "+";
                }
            }
            string str = null;
            string[] strarr = null;
            Type t = null;
            t = obj.GetType();
            strarr = new string[7];
            strarr[0] = padstr;
            strarr[1] = objName;
            strarr[2] = " AS ";
            strarr[3] = t.FullName;
            strarr[4] = " = ";
            strarr[5] = obj.ToString();
            strarr[6] = Environment.NewLine;
            sb.Append(string.Concat(strarr));
            //if (obj.GetType().BaseType == typeof(ValueType)) {
            //  return;
            //}
            DumpType(padstr, sb, obj, level, t, MaxLevel);
            Type bt = null;
            bt = t.BaseType;
            if (bt != null) {
                while (!(bt == typeof(object))) {
                    str = bt.FullName;
                    sb.Append(padstr + "(" + str + ")" + Environment.NewLine);
                    DumpType(padstr, sb, obj, level, bt, MaxLevel);
                    bt = bt.BaseType;
                    if (bt != null) {
                        continue;
                    }
                    break;
                    while (bt != typeof(object)) {

                    }
                }
            }
        }

        public static void DumpType(string InitialStr, StringBuilder sb, object obj, int level, System.Type t, int maxlevel) {
            FieldInfo[] fi = null;
            fi = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (t == typeof(System.Delegate)) {
                return;
            }
            //This for each was commented out previously
            maxlevel = 3;
            foreach (FieldInfo f in fi) {
                PrivDump(sb, f.GetValue(obj), f.Name, level + 1, maxlevel);
            }
            object[] arl = null;
            int i = 0;
            if (obj is System.Array) {
                try {
                    arl = (object[])obj;
                    for (i = 0; i < arl.GetLength(0); i++) {
                        PrivDump(sb, arl[i], "[" + i + "]", level + 1, maxlevel);
                    }
                } catch (Exception e1) {
                }
            }
        }

        // Example usage: WriteLogSimple("</table>" & Dump(oddlPrefixes) & "<table>")

        public static string Dump(object o, string name) {
            return Dump(o, name, 3);
        }

        public static string Dump(object o) {
            return Dump(o, "", 3);
        }

        //INSTANT C# NOTE: Overloaded method(s) are created above to convert the following method having optional parameters:
        //ORIGINAL LINE: Public Shared Function Dump(ByVal o As Object, Optional ByVal name As String = "", Optional ByVal depth As Integer = 3) As String
        public static string Dump(object o, string name, int depth) {
            try {
                var leafprefix = (((string.IsNullOrWhiteSpace(name)) ? name : name + " = "));

                if (null == o) {
                    return leafprefix + "null" + "<br/>";
                }

                var t = o.GetType();
                bool tempVar = depth < 1 || t.FullName == "System.String" || t.IsValueType;
                depth -= 1;
                if (tempVar) {
                    return (leafprefix + o.ToString()) + "<br/>";
                }

                var sb = new StringBuilder();
                // if you want to dump a WindowComboBox,
                // add a using to System.Web 
                // add a reference to System.Web.UI.Controls
                // System.Linq
                // System.Collections.Generic
                // uncomment this next section
                // 	var enumerable = o as IEnumerable;
                // 	if (enumerable != null)
                // 	{
                // 		if (name == "Items")
                // 		{
                //             int l_index = 0;
                // 			foreach (ListItem l_text in enumerable)
                // 			{						
                //				sb.Append(name + l_index.ToString() + " = " + l_text.Value + ": " + l_text.Text + "<br/>");
                //              l_index = l_index + 1;
                //          }
                //  	return sb.ToString() + "<br/>";
                //		}
                //		else
                //		{
                //			name = (name ?? "").TrimEnd('[', ']') + Convert.ToInt32('[');
                //		}
                //		var elements = enumerable.Cast<object>().Select((e) => Dump(e, "", depth)).ToList();
                //		var arrayInOneLine = elements.Count + "] = {" + (string.Join(",", elements) + Convert.ToInt32('}'));
                //		if (! (arrayInOneLine.Contains(Environment.NewLine))) // Single line?
                //		{
                //			return name + arrayInOneLine + "<br/>";
                //		}
                //		var i = 0;
                //		foreach (var element in elements)
                //		{
                //			var lineheader = name + (i + Convert.ToInt32(']'));
                //			i += 1;
                //			sb.Append(lineheader).AppendLine(element.Replace(Environment.NewLine, Environment.NewLine + lineheader));
                //		}
                //		return sb.ToString() + "<br/>";
                //	}
                foreach (var f in t.GetFields()) {
                    sb.AppendLine(Dump(f.GetValue(o), name + f.Name, depth));
                }
                foreach (var p in t.GetProperties()) {
                    sb.AppendLine(Dump(p.GetValue(o, null), name + p.Name, depth));
                }
                if (sb.Length == 0) {
                    return (leafprefix + o.ToString()) + "<br/>";
                }
                return sb.ToString().TrimEnd() + "<br/>";
            } catch (Exception ex) {
                return name + "???" + ex.ToString() + "<br/>";
            }
        }
    }
}
