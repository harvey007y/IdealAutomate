using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
public class JobApplicationKeywords : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    // This method is called by the Set accessor of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName 
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private Int32 _Id;
    // Property to Get/Set the Id
    public Int32 Id
    {
        get
        {
            return _Id;
        }
        set
        {
            //   if (value == null || value == string.Empty)
            //       throw new Exception("Id cannot be null or empty");
            //   if (!string.IsNullOrEmpty(value))
            _Id = value;
        }
    } // end of Id
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
    private Boolean _Enabled;
    // Property to Get/Set the Enabled
    public Boolean Enabled
    {
        get
        {
            return _Enabled;
        }
        set
        {
            //   if (value == null || value == string.Empty)
            //       throw new Exception("Enabled cannot be null or empty");
            //   if (!string.IsNullOrEmpty(value))
            _Enabled = value;
        }
    } // end of Enabled
}
