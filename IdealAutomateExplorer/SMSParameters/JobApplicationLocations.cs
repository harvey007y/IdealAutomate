using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
public class JobApplicationLocations : INotifyPropertyChanged
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
    private String _LocationName;
    // Property to Get/Set the LocationName
    public String LocationName
    {
        get
        {
            return _LocationName;
        }
        set
        {
            //   if (value == null || value == string.Empty)
            //       throw new Exception("LocationName cannot be null or empty");
            //   if (!string.IsNullOrEmpty(value))
            _LocationName = value;
        }
    } // end of LocationName
    private String _LinkedInGeoId;
    // Property to Get/Set the LinkedInGeoId
    public String LinkedInGeoId
    {
        get
        {
            return _LinkedInGeoId;
        }
        set
        {
            //   if (value == null || value == string.Empty)
            //       throw new Exception("LinkedInGeoId cannot be null or empty");
            //   if (!string.IsNullOrEmpty(value))
            _LinkedInGeoId = value;
        }
    } // end of LinkedInGeoId
    private String _GlassdoorLocation;
    // Property to Get/Set the GlassdoorLocation
    public String GlassdoorLocation
    {
        get
        {
            return _GlassdoorLocation;
        }
        set
        {
            //   if (value == null || value == string.Empty)
            //       throw new Exception("GlassdoorLocation cannot be null or empty");
            //   if (!string.IsNullOrEmpty(value))
            _GlassdoorLocation = value;
        }
    } // end of GlassdoorLocation
    private String _DiceLatitude;
    // Property to Get/Set the DiceLatitude
    public String DiceLatitude
    {
        get
        {
            return _DiceLatitude;
        }
        set
        {
            //   if (value == null || value == string.Empty)
            //       throw new Exception("DiceLatitude cannot be null or empty");
            //   if (!string.IsNullOrEmpty(value))
            _DiceLatitude = value;
        }
    } // end of DiceLatitude
    private String _DiceLongitude;
    // Property to Get/Set the DiceLongitude
    public String DiceLongitude
    {
        get
        {
            return _DiceLongitude;
        }
        set
        {
            //   if (value == null || value == string.Empty)
            //       throw new Exception("DiceLongitude cannot be null or empty");
            //   if (!string.IsNullOrEmpty(value))
            _DiceLongitude = value;
        }
    } // end of DiceLongitude
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
