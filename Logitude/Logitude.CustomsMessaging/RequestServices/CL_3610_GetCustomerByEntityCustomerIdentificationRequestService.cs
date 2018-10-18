using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSearchServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class CL_3610_GetCustomerByEntityCustomerIdentificationRequestService
        : RequestServiceBase<CL_MSG101_GetCustomerByEntityCustomerIdentification, ClientSearchRequestParams>
    {
        public override CL_MSG101_GetCustomerByEntityCustomerIdentification GetRequest(ClientSearchRequestParams requestParams)
        {
            //Build request 3610- Ask for Clients Details
            var myCL_MSG101_GetCustomerByEntityCustomerIdentificationRequest = new CL_MSG101_GetCustomerByEntityCustomerIdentification();
            myCL_MSG101_GetCustomerByEntityCustomerIdentificationRequest.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            int externalID = 0;
            int passportType = 0;

            int.TryParse(requestParams.ExternalId, out externalID);
            myCL_MSG101_GetCustomerByEntityCustomerIdentificationRequest.CustomerIdentification = new CustomerIdentification();
            myCL_MSG101_GetCustomerByEntityCustomerIdentificationRequest.CustomerIdentification.externalID = externalID;
            myCL_MSG101_GetCustomerByEntityCustomerIdentificationRequest.CustomerIdentification.externalIDSpecified = externalID > 0 ? true : false;
            myCL_MSG101_GetCustomerByEntityCustomerIdentificationRequest.CustomerIdentification.passportNumber = requestParams.PassportNumber;
            myCL_MSG101_GetCustomerByEntityCustomerIdentificationRequest.CustomerIdentification.passportCountry = requestParams.PassportCountryCode;
            int.TryParse(requestParams.PassportTypeCode, out passportType);
            myCL_MSG101_GetCustomerByEntityCustomerIdentificationRequest.CustomerIdentification.passportType = passportType;
            myCL_MSG101_GetCustomerByEntityCustomerIdentificationRequest.CustomerIdentification.passportTypeSpecified = passportType > 0 ? true : false;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Client");

            if (!string.IsNullOrWhiteSpace(requestParams.ExternalId))
            {
                this.MyRequestSheetParam.RequestDescription = "שליפת לקוח " + requestParams.ExternalId;
            }
            else
            {
                this.MyRequestSheetParam.RequestDescription = "שליפת לקוח " + requestParams.PassportNumber;
            }

            return myCL_MSG101_GetCustomerByEntityCustomerIdentificationRequest;
        }
    }
}
