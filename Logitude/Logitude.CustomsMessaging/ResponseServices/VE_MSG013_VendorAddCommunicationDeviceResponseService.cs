using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.VendorAddCommunicationDeviceServiceReference;


namespace Logitude.CustomsMessaging.ResponseServices
{
    public class VE_MSG013_VendorAddCommunicationDeviceResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, INF_MSG_Generic, VE_MSG013_VendorAddCommunicationDeviceRequestParams>
    {

        public override INF_MSG_GenericResponseData GetResponse(INF_MSG_Generic customResponse, VE_MSG013_VendorAddCommunicationDeviceRequestParams requestParams)
        {
            bool succeeded = false;
            string applicationId = null;
            bool hasException = false;
            string exceptionMessage = null;
            if (customResponse.ResponseContentHeader.Exception == null)
            {
                succeeded = true;
                applicationId = customResponse.ResponseContentHeader.ApplicationID.ToString();
            }
            else
            {
                string ExceptionDescription = "";
                var ExeptionDescription = "";
                if (customResponse.ResponseContentHeader.Exception != null)
                {
                    foreach (var rec in customResponse.ResponseContentHeader.Exception)
                    {

                        if (!String.IsNullOrWhiteSpace(ExeptionDescription))
                        {
                            ExeptionDescription += Environment.NewLine;
                        }
                        ExeptionDescription += rec.ExeptionDescription;

                    }
                    ExceptionDescription = ExeptionDescription;
                    hasException = true;
                    exceptionMessage = ExceptionDescription;
                }
            }
            INF_MSG_GenericResponseData responseData = new INF_MSG_GenericResponseData() { Succeeded = succeeded, ApplicationID = applicationId, HasException = hasException, UserMessage = exceptionMessage };
            return responseData;
        }

        public override void Update(INF_MSG_Generic customResponse, VE_MSG013_VendorAddCommunicationDeviceRequestParams requestParams)
        {
            //mohammad just for test
            ICustomContext context=CustomContext.GetContext(requestParams.Tenant);
            CustomsVendorQueryService vendorQueryService = new CustomsVendorQueryService(context);
            string id=vendorQueryService.GetIdByVendorNumber(requestParams.VendorNumber.ToString(),requestParams.Tenant);
            CustomsVendorPM vendor = vendorQueryService.GetSingle(id, true, false);
            vendor.ChangeSetOp = ChangeSetOperation.Update;
            foreach (VendorCommunicationResult commResult in requestParams.CommunicationDevices)
            {
                VendorCommunicationPM communication = new VendorCommunicationPM() { CommunicationAddress=commResult.CommunicationAddress,CommunicationTypeCode=commResult.CommunicationType , ChangeSetOp=ChangeSetOperation.Insert, Tenant=requestParams.Tenant,};
                vendor.VendorCommunications.Add(communication);
            }
            CustomsVendorUpdateService updateService = new CustomsVendorUpdateService(context,new Dictionary<string,Simplog.Server.Infrastructure.IContext>(),requestParams.Tenant);
            updateService.Update(vendor, true);
        }
    }
}
