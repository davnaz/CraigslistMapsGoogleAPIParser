using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraigslistMapsGoogleAPIParser.Components
{
    /// <summary>
    /// Описывает структуру строки, получаемой из исходной таблицы OffersTable для БД CraigsList
    /// </summary>
    public class AddressInfo
    {
        

        public long ID { get; set; } = -1;
        public string PlaceName { get; set; } = Constants.CraigsListDbAddressNulls.NoPlaceName;
        public string PlaceLink { get; set; } = Constants.CraigsListDbAddressNulls.NoLink;
        public string JSON { get; protected set; } = String.Empty;
        public AddressInfo(long id, string placeName, string placeLink)
        {
            ID = id;
            PlaceName = placeName;
            PlaceLink = placeLink;
        }

        public AddressInfo(DataRow dataRow)
        {
            ID = Convert.ToInt64(dataRow[0]);
            PlaceName = dataRow[1].ToString();
            PlaceLink = dataRow[2].ToString();
        }

        public override string ToString()
        {
            return String.Format("ID: {0}, PlaceName: {1}, PlaceLink: {2}", ID, PlaceName, PlaceLink);
        }
        public void InsertIntoDb()
        {
            SqlCommand insertAddress = DataProviders.DataProvider.Instance.CreateSQLCommandForSP(Resources.SP_InsertPlaceInfo, Resources.DbCraigslistPlacesConnectionString);
            insertAddress.Parameters.AddWithValue("@ID", ID);
            insertAddress.Parameters.AddWithValue("@Address", ((object)PlaceName) ?? (DBNull.Value));
            insertAddress.Parameters.AddWithValue("@JSON", ((object)JSON) ?? (DBNull.Value));
            DataProviders.DataProvider.Instance.ExecureSP(insertAddress);
        }
        public string ParseJSON()
        {
            if(PlaceLink != Constants.CraigsListDbAddressNulls.NoLink)
            {
                if (PlaceLink.Contains("q=loc"))
                {
                    JSON = AddressParser.GetJsonMapResponse(PlaceLink.Replace(Constants.GoogleStringRequestInDb.ByName, String.Empty), Constants.TypeOfMapGrabbing.ByAddress);
                }
                else if (PlaceLink.Contains("maps/preview"))
                {
                    JSON = AddressParser.GetJsonMapResponse(PlaceLink.Replace(Constants.GoogleStringRequestInDb.ByLatlng, String.Empty).Replace(",16z",""), Constants.TypeOfMapGrabbing.ByLatlng);
                }
                else
                {
                    Console.WriteLine(PlaceLink);
                }
            }
            return JSON;
        }
    }
}
