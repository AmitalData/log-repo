using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class ContinuousResponseOnClaimFileResponseData : ResponseDataBase
    {
        public string CustomsLegalDemandsList { get; set; }
        public string CountriesExclusionList { get; set; }
    }
}
