using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenObservableCollection
{
    public class jobapplications : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName 
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private String _JobUrl;
        // Property to Get/Set the JobUrl
        public String JobUrl
        {
            get
            {
                return _JobUrl;
            }
            set
            {
                //   if (value == null || value == string.Empty)
                //       throw new Exception("JobUrl cannot be null or empty");
                //   if (!string.IsNullOrEmpty(value))
                _JobUrl = value;
            }
        } // end of JobUrl
        private String _JobBoard;
        // Property to Get/Set the JobBoard
        public String JobBoard
        {
            get
            {
                return _JobBoard;
            }
            set
            {
                //   if (value == null || value == string.Empty)
                //       throw new Exception("JobBoard cannot be null or empty");
                //   if (!string.IsNullOrEmpty(value))
                _JobBoard = value;
            }
        } // end of JobBoard
        private String _JobTitle;
        // Property to Get/Set the JobTitle
        public String JobTitle
        {
            get
            {
                return _JobTitle;
            }
            set
            {
                //   if (value == null || value == string.Empty)
                //       throw new Exception("JobTitle cannot be null or empty");
                //   if (!string.IsNullOrEmpty(value))
                _JobTitle = value;
            }
        } // end of JobTitle
        private String _CompanyTitle;
        // Property to Get/Set the CompanyTitle
        public String CompanyTitle
        {
            get
            {
                return _CompanyTitle;
            }
            set
            {
                //   if (value == null || value == string.Empty)
                //       throw new Exception("CompanyTitle cannot be null or empty");
                //   if (!string.IsNullOrEmpty(value))
                _CompanyTitle = value;
            }
        } // end of CompanyTitle
        private DateTime _DateAdded;
        // Property to Get/Set the DateAdded
        public DateTime DateAdded
        {
            get
            {
                return _DateAdded;
            }
            set
            {
                //   if (value == null || value == string.Empty)
                //       throw new Exception("DateAdded cannot be null or empty");
                //   if (!string.IsNullOrEmpty(value))
                _DateAdded = value;
            }
        } // end of DateAdded
        private DateTime _DateLastModified;
        // Property to Get/Set the DateLastModified
        public DateTime DateLastModified
        {
            get
            {
                return _DateLastModified;
            }
            set
            {
                //   if (value == null || value == string.Empty)
                //       throw new Exception("DateLastModified cannot be null or empty");
                //   if (!string.IsNullOrEmpty(value))
                _DateLastModified = value;
            }
        } // end of DateLastModified
        private DateTime _DateApplied;
        // Property to Get/Set the DateApplied
        public DateTime DateApplied
        {
            get
            {
                return _DateApplied;
            }
            set
            {
                //   if (value == null || value == string.Empty)
                //       throw new Exception("DateApplied cannot be null or empty");
                //   if (!string.IsNullOrEmpty(value))
                _DateApplied = value;
            }
        } // end of DateApplied
        private String _ApplicationStatus;
        // Property to Get/Set the ApplicationStatus
        public String ApplicationStatus
        {
            get
            {
                return _ApplicationStatus;
            }
            set
            {
                //   if (value == null || value == string.Empty)
                //       throw new Exception("ApplicationStatus cannot be null or empty");
                //   if (!string.IsNullOrEmpty(value))
                _ApplicationStatus = value;
            }
        } // end of ApplicationStatus
        private String _Keyword;
        // Property to Get/Set the Keyword
        public String Keyword
        {
            get
            {
                return _Keyword;
            }
            set
            {
                //   if (value == null || value == string.Empty)
                //       throw new Exception("Keyword cannot be null or empty");
                //   if (!string.IsNullOrEmpty(value))
                _Keyword = value;
            }
        } // end of Keyword
        private String _Location;
        // Property to Get/Set the Location
        public String Location
        {
            get
            {
                return _Location;
            }
            set
            {
                //   if (value == null || value == string.Empty)
                //       throw new Exception("Location cannot be null or empty");
                //   if (!string.IsNullOrEmpty(value))
                _Location = value;
            }
        } // end of Location
        private String _Comments;
        // Property to Get/Set the Comments
        public String Comments
        {
            get
            {
                return _Comments;
            }
            set
            {
                //   if (value == null || value == string.Empty)
                //       throw new Exception("Comments cannot be null or empty");
                //   if (!string.IsNullOrEmpty(value))
                _Comments = value;
            }
        } // end of Comments
    }
}
