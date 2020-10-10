using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdealAutomate.Core;

namespace System.Windows.Forms.Samples
{
    static class RoamingFolderForTabs
    {
       

        public static string MyFolder
        {
            get {
                if (MyFolder == "")
                {
                    Methods myActions = new Methods();
                    string myTabs = myActions.GetValueByKeyGlobal("RoamingFolderForTabs");
                    if (myTabs == "")
                    {
                        myTabs = "IdealAutomateExplorer";
                    }
                    return myTabs;
                } else
                {
                    return MyFolder;
                }
            }
           
        }

    }
}
