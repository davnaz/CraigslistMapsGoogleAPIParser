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
