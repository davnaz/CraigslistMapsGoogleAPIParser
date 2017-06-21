using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraigslistMapsGoogleAPIParser.Components
{
    public class Constants
    {

        public class WebAttrsNames
        {
            public const string href = "href";
            public const string NotFound = "no";
        }

        public class GoogleRequestParams
        {
            public const string AddressMapsQueryPattern = "http://maps.googleapis.com/maps/api/geocode/json?components=country:US&language=en&address=";
            public const string LatlngMapsQueryPattern = "http://maps.googleapis.com/maps/api/geocode/json?latlng=";
        }
        public class GoogleMapsGeocodingStatusCodes
        {
            public const string OK = "OK";
            public const string ZERO_RESULTS = "ZERO_RESULTS";
            public const string OVER_QUERY_LIMIT = "OVER_QUERY_LIMIT";
            public const string REQUEST_DENIED = "REQUEST_DENIED";
            public const string INVALID_REQUEST = "INVALID_REQUEST";
            public const string UNKNOWN_ERROR = "UNKNOWN_ERROR";
        }

        public class GoogleLocationTypes
        {   /// <summary>
            /// указывает, что отображаемый результат содержит точный геокод.
            /// </summary>
            public const string ROOFTOP = "ROOFTOP"; //самое точное приближение места, с точностью до дома
            /// <summary>
            /// указывает на возвращение геометрического центра результата, например, ломаной линии (улицы) или многоугольника (района).
            /// </summary>
            public const string GEOMETRIC_CENTER = "GEOMETRIC_CENTER";//с точностью до улицы
            /// <summary>
            /// указывает на возвращение приближенного результата.
            /// </summary>
            public const string APPROXIMATE = "APPROXIMATE"; //все остальные уровни точности
            /// <summary>
            /// Указывает, что возвращаемый результат содержит приближенное значение (обычно на дороге), полученное посредством интерполяции двух точных значений (например, перекрестков). Интерполированные результаты обычно возвращаются, 
            /// если для почтового адреса недоступны геокоды номеров зданий.
            /// </summary>
            public const string RANGE_INTERPOLATED = "RANGE_INTERPOLATED";//
        }

        public class GoogleAddressComponentsTypes
        {
            public const string street_number = "street_number";
            public const string route = "route";
            public const string locality = "locality";
            public const string county = "administrative_area_level_2";
            public const string state = "administrative_area_level_1";
            public const string country = "country";
            public const string postal_code = "postal_code";
            public const string postal_code_suffix = "postal_code_suffix";
        }

        /// <summary>
        /// Это типы ссылок, которые используются в БД CraigsList для сохранения адреса
        /// </summary>
        public class GoogleStringRequestInDb
        {
            public const string ByName = "https://maps.google.com/?q=loc";
            public const string ByLatlng = "https://maps.google.com/maps/preview/@";
        }

        public enum TypeOfMapGrabbing
        {
            ByAddress,
            ByLatlng
        }

        public class CraigsListDbAddressNulls
        {
            public const string NoPlaceName = "No Place Name";
            public const string NoLink = "No link";
        }

        public class OfferListSelectors
        {
            public const string NextPage = ".paginationContainer .mrs.bas.pvs.phm";
            public const string OfferLinks = "a.tileLink.phm";
            /// <summary>
            /// document.querySelector(".pls.typeLowlight").innerText
            /// </summary>
            public const string OffersCount = ".pls.typeLowlight"; //"document.querySelector(".pls.typeLowlight").innerText"
        }

    }
}
