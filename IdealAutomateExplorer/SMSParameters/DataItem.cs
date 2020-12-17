using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DataGridExtensionsSample
{
   
    using System.ComponentModel;
    using System.Windows;

    public class DataItem
    {


        public DataItem(int index)
        {
               Index = index;

        }


        public int Index { get; set; }
        public string str_jobapplications_JobUrl { get; set; }
        public string str_jobapplications_JobBoard { get; set; }
        public string str_jobapplications_JobTitle { get; set; }
        public string str_jobapplications_CompanyTitle { get; set; }
        public DateTime? dt_jobapplications_DateAdded { get; set; }
        public DateTime? dt_jobapplications_DateLastModified { get; set; }
        public DateTime? dt_jobapplications_DateApplied { get; set; }
        public string str_jobapplications_ApplicationStatus { get; set; }
        public string str_jobapplications_Keyword { get; set; }
        public string str_jobapplications_Location { get; set; }
        public string str_jobapplications_Comments { get; set; }

    
    }
}
