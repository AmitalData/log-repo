using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.VendorSearchServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class VE_MSG051_VendorSearchByCustomsAgentRequestService:RequestServiceBase<VE_MSG051_VendorSearchByCustomsAgentMessage,VE_MSG051_VendorSearchByCustomsAgentRequestParams>
    {
        public override VE_MSG051_VendorSearchByCustomsAgentMessage GetRequest(VE_MSG051_VendorSearchByCustomsAgentRequestParams requestParams)
        {
            var realCustomReq = new VE_MSG051_VendorSearchByCustomsAgentMessage();
            realCustomReq.VendorSearchParameters = new VE_MSG051_VendorSearchByCustomsAgentMessageVendorSearchParameters()
            {
                //vendorName = requestParams.VendorName,
                dunsNumber = requestParams.DunsNumber,
                dunsNumberSpecified = requestParams.DunsNumber > 0 ? true : false,
                EnglishAddress = new EnglishAddress() { englishCityName = requestParams.CityName, englishCountry = requestParams.CountryCode, englishMainAddressLine = requestParams.MainAddressLine, englishPostalCode = requestParams.PostalCode, englishSubCountry = requestParams.SubCountryCode, },
                isSearchPreviousName = requestParams.isSearchPreviousName,
                isSearchPreviousNameSpecified = requestParams.isSearchPreviousName != null ? true : false,
                licensedDealerNumber = requestParams.LicensedDealerNumber,
                vendorID = requestParams.VendorNumber,
                vendorIDSpecified = requestParams.VendorNumber > 0  ? true : false,
                vendorTypeID = requestParams.VendorTypeCode,
                vendorTypeIDSpecified = requestParams.VendorTypeCode > 0 ? true : false,
                isPalestinian = requestParams.IsPalestinian, 
            };
            if (!string.IsNullOrWhiteSpace(requestParams.VendorName))
            {
                realCustomReq.VendorSearchParameters.vendorName = requestParams.VendorName;
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsVendor");
            this.MyRequestSheetParam.RequestDescription = "חיפוש ספק " + requestParams.VendorNumber;
            
            return realCustomReq;
        }
    }
}
