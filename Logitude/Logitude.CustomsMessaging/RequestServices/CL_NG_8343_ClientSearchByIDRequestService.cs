using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSearchByIDServiceReference;
using UnifreightIIG.Common.ClientSearchServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class CL_NG_8343_ClientSearchByIDRequestService : RequestServiceBase<CL_NG_8343_Web01_ClientSearchByIDParam, ClientSearchRequestParams>
    {
        public override CL_NG_8343_Web01_ClientSearchByIDParam GetRequest(ClientSearchRequestParams requestParams)
        {
            //Build request 3610- Ask for Clients Details
            var myCL_NG_8343_Web01_ClientSearchByIDParam = new CL_NG_8343_Web01_ClientSearchByIDParam();
            myCL_NG_8343_Web01_ClientSearchByIDParam.CustomerIdentification = new UnifreightIIG.Common.ClientSearchByIDServiceReference.CustomerIdentification();

            int externalID = 0;
            int passportType = 0;

            int.TryParse(requestParams.ExternalId, out externalID);
            myCL_NG_8343_Web01_ClientSearchByIDParam.CustomerIdentification.externalID = externalID;
            myCL_NG_8343_Web01_ClientSearchByIDParam.CustomerIdentification.externalIDSpecified = externalID > 0 ? true : false;
            myCL_NG_8343_Web01_ClientSearchByIDParam.CustomerIdentification.passportNumber = requestParams.PassportNumber;
            myCL_NG_8343_Web01_ClientSearchByIDParam.CustomerIdentification.passportCountry = requestParams.PassportCountryCode;
            int.TryParse(requestParams.PassportTypeCode, out passportType);
            myCL_NG_8343_Web01_ClientSearchByIDParam.CustomerIdentification.passportType = passportType;
            myCL_NG_8343_Web01_ClientSearchByIDParam.CustomerIdentification.passportTypeSpecified = passportType > 0 ? true : false;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Client");

            if (!string.IsNullOrWhiteSpace(requestParams.ExternalId))
            {
                this.MyRequestSheetParam.RequestDescription = "נתונים נוספים ליבואן/יצואן " + requestParams.ExternalId;
            }
            else
            {
                this.MyRequestSheetParam.RequestDescription = "נתונים נוספים ליבואן/יצואן " + requestParams.PassportNumber;
            }

            return myCL_NG_8343_Web01_ClientSearchByIDParam;
        }
    }
}

