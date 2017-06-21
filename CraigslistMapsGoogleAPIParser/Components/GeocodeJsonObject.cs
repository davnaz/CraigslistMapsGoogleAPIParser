using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraigslistMapsGoogleAPIParser.Components
{
    public class GeocodeJsonObject
    {
        
        public List<Result> results ;
        public string status ;
        public string error_message;
        public class AddressComponent
        {
            public string long_name ;
            public string short_name ;
            public List<string> types ;
        }

        public class Northeast
        {
            public double lat ;
            public double lng ;
        }

        public class Southwest
        {
            public double lat ;
            public double lng ;
        }

        public class Bounds
        {
            public Northeast northeast ;
            public Southwest southwest ;
        }

        public class Location
        {
            public double lat ;
            public double lng ;
        }

        public class Northeast2
        {
            public double lat ;
            public double lng ;
        }

        public class Southwest2
        {
            public double lat ;
            public double lng ;
        }

        public class Viewport
        {
            public Northeast2 northeast ;
            public Southwest2 southwest ;
        }

        public class Geometry
        {
            public Bounds bounds ;
            public Location location ;
            public string location_type ;
            public Viewport viewport ;
        }

        public class Result
        {
            public List<AddressComponent> address_components ;
            public string formatted_address ;
            public Geometry geometry ;
            public bool partial_match ;
            public string place_id ;
            public List<string> types ;
        }        
    }
}
