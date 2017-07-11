using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraigslistMapsGoogleAPIParser.Components
{
    class RooftopResultPlace
    {
        public string street_number { get; set; } = String.Empty;
        public string route { get; set; } = String.Empty;
        public string locality                  {get; set; } = String.Empty;
        public string county                    {get; set; } = String.Empty;
        public int postal_code               {get; set; } = -1;
        public string state                     {get; set;}
        public int postal_code_short         {get; set; } = -1;
        public string county_short              {get; set; } = String.Empty;
        public string locality_short            {get; set; } = String.Empty;
        public string route_short               {get; set; } = String.Empty;
        public string street_number_short       {get; set; } = String.Empty;
        public string state_short               {get; set; } = String.Empty;
        public string formatted_address         {get; set; } = String.Empty;
        public int postal_code_suffix        {get; set; } = -1;
        public int postal_code_suffix_short  {get; set; } = -1;
        public string country                   {get; set; } = String.Empty;
        public string country_short { get; set; } = String.Empty;
        public long ID { get; set; } = -1;

        public RooftopResultPlace(GeocodeJsonObject o,long ID_)
        {
            if(o.status != Constants.GoogleMapsGeocodingStatusCodes.OK)
            {
                Console.WriteLine(o.status+" "+o.error_message);
                return;
            }
            GeocodeJsonObject.Result res = o.results.Find(i => i.geometry.location_type == Constants.GoogleLocationTypes.ROOFTOP || i.geometry.location_type == Constants.GoogleLocationTypes.RANGE_INTERPOLATED);
            if(res == null)
            {
                Console.WriteLine("Нет точных данных в результатах., {0}",ID_);
                return;
            }
            res.address_components.ForEach(i =>
            {
                switch (i.types[0])
                {
                    case Constants.GoogleAddressComponentsTypes.country:
                        country = i.long_name;
                        country_short = i.short_name;
                        break;
                    case Constants.GoogleAddressComponentsTypes.county:
                        county = i.long_name;
                        county_short = i.short_name;
                        break;
                    case Constants.GoogleAddressComponentsTypes.locality:
                        locality = i.long_name;
                        locality_short = i.short_name;
                        break;
                    case Constants.GoogleAddressComponentsTypes.postal_code:
                        try
                        {
                            postal_code = Convert.ToInt32(i.long_name);
                            postal_code_short = Convert.ToInt32(i.short_name);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        
                        break;
                    case Constants.GoogleAddressComponentsTypes.postal_code_suffix:
                        postal_code_suffix = Convert.ToInt32(i.long_name);
                        postal_code_suffix_short = Convert.ToInt32(i.short_name);
                        break;
                    case Constants.GoogleAddressComponentsTypes.route:
                        route = i.long_name;
                        route_short = i.short_name;
                        break;
                    case Constants.GoogleAddressComponentsTypes.street_number:
                        street_number =       i.long_name ;
                        street_number_short = i.short_name;
                        break;
                    case Constants.GoogleAddressComponentsTypes.state:
                        state = i.long_name;
                        state_short = i.short_name;
                        break;
                }

            });
            formatted_address = res.formatted_address;
            ID = ID_;
        }
        public void InsertToDb()
        {
            if (ID == -1)
            {
                return; //если это пустой объект
            }
                
            SqlCommand insertJsonInfo = DataProviders.DataProvider.Instance.CreateSQLCommandForSP(Resources.SP_SetParsedJsonInfo, Resources.DbCraigslistPlacesConnectionString);
            insertJsonInfo.Parameters.AddWithValue("@street_number", street_number != String.Empty ? (object)street_number : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@route", route != String.Empty ? (object)route : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@locality"                      , locality                 !=String.Empty ? (object) locality                : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@county"                        ,county                    !=String.Empty ?  (object)county                   : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@postal_code"                   ,postal_code               !=-1 ?            (object)postal_code              : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@state"                         ,state                     !=String.Empty ?  (object)state                    : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@postal_code_short"             ,postal_code_short         !=-1 ?            (object)postal_code_short        : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@county_short"                  ,county_short              !=String.Empty ?  (object)county_short             : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@locality_short"                ,locality_short            !=String.Empty ?  (object)locality_short           : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@route_short"                   ,route_short               !=String.Empty ?  (object)route_short              : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@street_number_short"           ,street_number_short       != String.Empty ?            (object)street_number_short      : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@state_short"                   ,state_short               !=String.Empty ?  (object)state_short              : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@formatted_address"             ,formatted_address         !=String.Empty ?  (object)formatted_address        : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@postal_code_suffix"            ,postal_code_suffix        !=-1 ?            (object)postal_code_suffix       : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@postal_code_suffix_short"      ,postal_code_suffix_short  !=-1 ?            (object)postal_code_suffix_short : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@country", country == String.Empty ? (object)country : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@country_short", country_short != String.Empty ? (object)country_short : DBNull.Value);
            insertJsonInfo.Parameters.AddWithValue("@ID"                             , ID);
            DataProviders.DataProvider.Instance.ExecureSP(insertJsonInfo);
        }
    }
}
