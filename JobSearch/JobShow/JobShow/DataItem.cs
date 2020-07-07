using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataGridExtensionsSample
{
    using System.Windows;

    public class DataItem
    {
        private static readonly Random _rand = new Random();
        private static readonly string[] _samples = new[] {"lorem", "ipsum", "dolor", "sit", "amet"};

        public DataItem(int index)
        {
            Flag = _rand.Next(2) == 0;
            Index = index;

        }

        public bool Flag { get; private set; }
        public int Index { get; private set; }
        public String str_jobapplications_JobUrl;
        public String str_jobapplications_JobBoard;
        public String str_jobapplications_JobTitle;
        public String str_jobapplications_CompanyTitle;
        public DateTime dt_jobapplications_DateAdded;
        public DateTime dt_jobapplications_DateLastModified;
        public DateTime dt_jobapplications_DateApplied;
        public String str_jobapplications_ApplicationStatus;
        public String str_jobapplications_Keyword;
        public String str_jobapplications_Location;
        String str_jobapplications_Comments;
    }
}
