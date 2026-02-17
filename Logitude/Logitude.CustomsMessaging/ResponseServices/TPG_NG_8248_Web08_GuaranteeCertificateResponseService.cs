using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Storage;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using UnifreightIIG.Common.MessageLib.Docs;
using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Utils;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using System.Collections;
using Unifreight.Data.AmitalModel;
using UnifreightIIG.Common.GuaranteeCertificateFilterParamServiceReference;


namespace Logitude.CustomsMessaging.ResponseServices
{

    public class TPG_NG_8248_Web08_GuaranteeCertificateResponseService : ResponseServiceBase<
         GuaranteeCertificateResponseData, TPG_NG_8248_Web08_GuaranteeCertificateDetail, GuaranteeCertificateRequestParams>
    {
        public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption;
        public bool _IsSubmitDeclarationResponse { get; set; }
        DeclarationPM _MyDeclarationPM;
        ConsignmentPM _MyConsignmentPM;
        private GTRTRANQueryService _GTRTRANQueryService;
        private Dictionary<string, IList> _MyLocalCache = new Dictionary<string, IList>();
        private AmitalContext _AmitalContext;

        public override void Update(TPG_NG_8248_Web08_GuaranteeCertificateDetail customResponse, GuaranteeCertificateRequestParams requestParams)
        {
            //
            string xml = null;
            var responseName = requestParams.ResponseName;
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();

            this.MyResponseData = new GuaranteeCertificateResponseData();

            //Checking foe Exceptions
            if (customResponse.ResponseContentHeader != null && customResponse.ResponseContentHeader.Exception != null || _ResponseHeaderExeption != null)
            {
                if (customResponse.ResponseContentHeader.Exception != null)
                {
                    this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                }
                else
                {
                    this.MyResponseData.UserMessage = _ResponseHeaderExeption.ErrorDescription;
                }
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage);
                return;
            }

            if (customResponse.GeneralDetails == null && customResponse.GuaranteeCertificateAllocation == null && customResponse.GuaranteeCertificateRequest == null)
            {
                string errMess = "No details in the Response ";
                if (customResponse.ResponseContentHeader.Exception != null && !String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription))
                {
                    errMess = errMess + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                }

                LogMessagingUtil.Instance.AppendLine(errMess);
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = errMess;
                this.MyResponseData.HasException = true;
                return;
            }

            GetResponseData(customResponse);

            LogMessagingUtil.Instance.AppendLine("Analyze Manifest Status Query response " + requestParams.certificateID);
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage);
        }

        private void GetResponseData(TPG_NG_8248_Web08_GuaranteeCertificateDetail customResponse)
        {
            this.MyResponseData.GeneralDetailsData = new GuaranteeCertificateResponseData.GeneralDetails();
            this.MyResponseData.GeneralDetailsData.guaranteeType = customResponse.GeneralDetails.guaranteeType;
            this.MyResponseData.GeneralDetailsData.guaranteeTypeName = customResponse.GeneralDetails.guaranteeTypeName;
            this.MyResponseData.GeneralDetailsData.GuaranteeCertificateStatus = customResponse.GeneralDetails.GuaranteeCertificateStatus;
            this.MyResponseData.GeneralDetailsData.GuaranteeCertificateStatusName = customResponse.GeneralDetails.GuaranteeCertificateStatusName;
            this.MyResponseData.GeneralDetailsData.guaranteedId = customResponse.GeneralDetails.guaranteedId;
            this.MyResponseData.GeneralDetailsData.guaranteedName = customResponse.GeneralDetails.guaranteedName;
            this.MyResponseData.GeneralDetailsData.certificateID = customResponse.GeneralDetails.certificateID;
            this.MyResponseData.GeneralDetailsData.guaranteeExternalCertificateNumebr = customResponse.GeneralDetails.guaranteeExternalCertificateNumebr;
            this.MyResponseData.GeneralDetailsData.guaranteeValidityDate = customResponse.GeneralDetails.guaranteeValidityDate.Date.ToString("dd/MM/yyyy");
            this.MyResponseData.GeneralDetailsData.guaranteeAmount = customResponse.GeneralDetails.guaranteeAmount.ToString("N2");
            this.MyResponseData.GeneralDetailsData.totalCertificateAllocation = customResponse.GeneralDetails.totalCertificateAllocation.ToString("N2");
            this.MyResponseData.GeneralDetailsData.certificateAvailableAmount = customResponse.GeneralDetails.certificateAvailableAmount.ToString("N2");

            this.MyResponseData.AllocationList = new List<GuaranteeCertificateResponseData.Allocation>();
            if (customResponse.GuaranteeCertificateAllocation != null && customResponse.GuaranteeCertificateAllocation.Length > 0)
            {
                foreach (var guaranteeAllocationItem in customResponse.GuaranteeCertificateAllocation)
                {
                    GuaranteeCertificateResponseData.Allocation allocation = new GuaranteeCertificateResponseData.Allocation();
                    allocation.fileType = guaranteeAllocationItem.fileType;
                    allocation.fileTypeName = guaranteeAllocationItem.fileTypeName;
                    allocation.fileNumber = guaranteeAllocationItem.fileNumber;
                    allocation.Numeral = guaranteeAllocationItem.Numeral;
                    allocation.displayFileNumber = guaranteeAllocationItem.displayFileNumber;
                    allocation.certificateAllocationAmount = guaranteeAllocationItem.certificateAllocationAmount;
                    allocation.validity = guaranteeAllocationItem.validity;
                    allocation.updateDate = guaranteeAllocationItem.updateDate;
                    this.MyResponseData.AllocationList.Add(allocation);
                }
            }

            this.MyResponseData.RequestList = new List<GuaranteeCertificateResponseData.Request>();
            if (customResponse.GuaranteeCertificateRequest != null && customResponse.GuaranteeCertificateRequest.Length > 0)
            {
                foreach (var guaranteeRequestItem in customResponse.GuaranteeCertificateRequest)
                {
                    GuaranteeCertificateResponseData.Request request = new GuaranteeCertificateResponseData.Request();
                    request.createTime = guaranteeRequestItem.createTime;
                    request.displayFileNumber = guaranteeRequestItem.displayFileNumber;
                    request.fileNumber = guaranteeRequestItem.fileNumber;
                    request.guaranteeStatus = guaranteeRequestItem.guaranteeStatus;
                    request.guaranteeStatusName = guaranteeRequestItem.guaranteeStatusName;
                    request.guarenteeRequestNumber = guaranteeRequestItem.guarenteeRequestNumber;
                    request.Numeral = guaranteeRequestItem.Numeral;
                    request.requestDescription = guaranteeRequestItem.requestDescription;
                    request.requestedExecutionValueSpecified = guaranteeRequestItem.requestedExecutionValueSpecified;
                    request.requestedValidityDate = guaranteeRequestItem.requestedValidityDate;

                    this.MyResponseData.RequestList.Add(request);
                }
            }
        }

        private string GetDummyXml(string message)
        {
            var myDummyXml = new GeneralMessage() { Message = message };
            var xml = XmlGenericUtil<GeneralMessage>.SerializeObject(myDummyXml);
            return xml;
        }

        private string GetDummyXml(string message, TPG_NG_8248_Web08_GuaranteeCertificateDetail response)
        {
            var myDummyXml = new GeneralMessage() { Message = message };
            if (response != null)
            {
                myDummyXml.Response = new TPG_NG_8248_Web08_GuaranteeCertificateDetail();
                myDummyXml.Response = response;
            }
            var xml = XmlGenericUtil<GeneralMessage>.SerializeObject(myDummyXml);
            return xml;
        }


        private string GetTranslationP2L(string partnerID, string tableID, string partnerCode)
        {
            if (partnerID == null || tableID == null || partnerCode == null)
            {
                return ("");
            }

            if (_GTRTRANQueryService == null)
            {
                _GTRTRANQueryService = new GTRTRANQueryService(_AmitalContext);
            }
            return GetTranslationP2LFromCache(partnerID, tableID, partnerCode);
        }

        private string GetTranslationP2LFromCache(string partnerID, string tableID, string partnerCode)
        {
            var key = partnerID + "'," + tableID;
            if (!_MyLocalCache.ContainsKey(key))
            {
                _MyLocalCache[key] = _GTRTRANQueryService.GetMulti(partnerID, tableID) ?? new List<GTRTRANPM>();
            }
            var myList = _MyLocalCache[key] as List<GTRTRANPM>;
            var recordTR = myList.FirstOrDefault(rec => rec.PARTNERID == partnerID && rec.TABLEID == tableID && rec.PARTNERCODE == partnerCode);
            if (recordTR == null)
            {
                return "";
            }
            return recordTR.LOCALCODE;
        }

        public override GuaranteeCertificateResponseData GetResponse(TPG_NG_8248_Web08_GuaranteeCertificateDetail customResponse, GuaranteeCertificateRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private string GetErrosXmlFromResponseHeaderExeption()
        {
            return _ResponseHeaderExeption.ErrorDescription;
        }

        public class GeneralMessage
        {
            public string Message { get; set; }
            public TPG_NG_8248_Web08_GuaranteeCertificateDetail Response { get; set; }
        }
    }

}




