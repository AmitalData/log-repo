using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.VendorSearchServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class VE_MSG052_VendorSearchResultsForCustomsAgentResponseService : ResponseServiceBase<VE_MSG052_VendorSearchResultsForCustomsAgentResponseData, VE_MSG052_VendorSearchResultsForCustomsAgentMessage, VE_MSG051_VendorSearchByCustomsAgentRequestParams>
    {
        CustomsVendorPM _CustomsVendorPM;
        public override VE_MSG052_VendorSearchResultsForCustomsAgentResponseData GetResponse(VE_MSG052_VendorSearchResultsForCustomsAgentMessage customResponse, VE_MSG051_VendorSearchByCustomsAgentRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(VE_MSG052_VendorSearchResultsForCustomsAgentMessage customResponse, VE_MSG051_VendorSearchByCustomsAgentRequestParams requestParams)
        {
            //Analyze message 3660- Vendor Search Results
            this.MyResponseData = GetResponsData(customResponse, requestParams);
            if (!requestParams.RecallSuppliersFromFileRequest) return;

            if (MyResponseData.VendorResults.Count > 1)
            {
                LogMessagingUtil.Instance.AppendLine("RecallSuppliersFromFileRequest expected 1 result due  search by VendorNumber. MyResponseData.VendorResults.Count:" + customResponse.NumberOfResult);
                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "RecallSuppliersFromFileRequest expected 1 result due  search by VendorNumber.  MyResponseData.VendorResults.Count:" + customResponse.NumberOfResult;
                return;
            }
            var vendorResult = MyResponseData.VendorResults.FirstOrDefault();
            if (vendorResult == null)
            {
                LogMessagingUtil.Instance.AppendLine("RecallSuppliersFromFileRequest expected at least 1 result due search by VendorNumber");
                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "RecallSuppliersFromFileRequest expected at least 1 result due search by VendorNumber";
                return;
            }

            Upsert(vendorResult, requestParams);
        }

        private static VE_MSG052_VendorSearchResultsForCustomsAgentResponseData GetResponsData(VE_MSG052_VendorSearchResultsForCustomsAgentMessage customResponse, VE_MSG051_VendorSearchByCustomsAgentRequestParams requestParams)
        {
            VE_MSG052_VendorSearchResultsForCustomsAgentResponseData responseData = new VE_MSG052_VendorSearchResultsForCustomsAgentResponseData();
            responseData.NumberOfResult = customResponse.NumberOfResult;
            List<VendorResult> vendorResultList = new List<VendorResult>();
            CustomsVendorQueryService vendorQueryService = new CustomsVendorQueryService(requestParams.Tenant);
            VendorTypeQueryService vendorTypeQueryService = new VendorTypeQueryService(requestParams.Tenant);
            
            foreach (VE_MSG052_VendorSearchResultsForCustomsAgentMessageVendorResult vend in customResponse.VendorResult)
            {
                List<VendorCommunicationResult> vendorCommunicationResultList = new List<VendorCommunicationResult>();
                //bool exists = vendorQueryService.DoesVendorExist(vend.vendorID.ToString(), requestParams.Tenant);
                bool exists = false;
                bool inActive = false;
                string customsVendorId = vendorQueryService.GetIdByVendorNumber(vend.vendorID.ToString(), requestParams.Tenant);
                if (!String.IsNullOrWhiteSpace(customsVendorId))
                {
                    CustomsVendorPM customsVendorPM = vendorQueryService.GetSingle(customsVendorId,false,false);
                    exists = true;
                    inActive = customsVendorPM.InActive;
                }
                VendorTypePM vendortype = vendorTypeQueryService.GetSingle(vend.vendorTypeID.ToString(), false, false);
                VendorResult vendor = new VendorResult()
                {
                    Id = Guid.NewGuid().ToString(),
                    StatusCode = vend.statusID.ToString(),
                    CityName = vend.EnglishAddress.englishCityName,
                    CountryCode = vend.EnglishAddress.englishCountry,
                    DunsNumber = vend.dunsNumber.ToString(),
                    MainAddressLine = vend.EnglishAddress.englishMainAddressLine,
                    PostalCode = vend.EnglishAddress.englishPostalCode,
                    SubCountryCode = vend.EnglishAddress.englishSubCountry,
                    Tenant = requestParams.Tenant,
                    VendorName = vend.vendorName,
                    VendorTypeCode = vend.vendorTypeID.ToString(),
                    VendorTypeName = vendortype.LocalName != null ? vendortype.LocalName : vendortype.EnglishName,
                    VendorNumber = vend.vendorID.ToString(),
                    Exists = exists,
                    InActive = inActive,
                    VATNumber = vend.licensedDealerNumber,
                    IsPalestinian = vend.EnglishAddress.englishCountry == "PS" ? true : false,
                };
                foreach (CommunicationDevice device in vend.CommunicationDevice)
                {
                    VendorCommunicationResult vendorComm = new VendorCommunicationResult()
                    {
                        CommunicationAddress = device.communicationAddress,
                        CommunicationType = device.communicationType,

                    };
                    vendorCommunicationResultList.Add(vendorComm);
                }
                vendor.VendorCommunications = vendorCommunicationResultList;
                vendorResultList.Add(vendor);
            }

            responseData.VendorResults = vendorResultList;
            responseData.NumberOfResult = customResponse.NumberOfResult;
            responseData.Succeeded = true;
            responseData.HasException = false;
            responseData.UserMessage = "Vendor Search Results- Found: " + customResponse.NumberOfResult + " vendors";

            return responseData;
        }

        private void Upsert(VendorResult vendorResult, VE_MSG051_VendorSearchByCustomsAgentRequestParams requestParams)
        {
            ICustomContext customContext = CustomContext.GetContext(requestParams.Tenant);
            var queryService = new CustomsVendorQueryService(requestParams.Tenant);
            var updateService = new CustomsVendorUpdateService(customContext, new Dictionary<string, IContext>(), requestParams.Tenant);

            string customsVendorId = queryService.GetIdByVendorNumber(vendorResult.VendorNumber, requestParams.Tenant);
            if (!string.IsNullOrWhiteSpace(customsVendorId))
            {
                this._CustomsVendorPM = queryService.GetSingle(customsVendorId,true,false);
                this._CustomsVendorPM.ChangeSetOp = ChangeSetOperation.Update;
                DeleteVendorCommunications();
            }
            else
            {
                this._CustomsVendorPM = new CustomsVendorPM();
                this._CustomsVendorPM.ChangeSetOp = ChangeSetOperation.Insert;
                this._CustomsVendorPM.Tenant = requestParams.Tenant;
            }
                    
            this._CustomsVendorPM.VendorName = vendorResult.VendorName;
            this._CustomsVendorPM.CityName = vendorResult.CityName;
            this._CustomsVendorPM.CountryCode = vendorResult.CountryCode;
            this._CustomsVendorPM.DunsNumber = vendorResult.DunsNumber;
            this._CustomsVendorPM.MainAddressLine = vendorResult.MainAddressLine;
            this._CustomsVendorPM.PostalCode = vendorResult.PostalCode;
            this._CustomsVendorPM.StatusCode = vendorResult.StatusCode;
            this._CustomsVendorPM.SubCountryCode = vendorResult.SubCountryCode;
            this._CustomsVendorPM.VendorNumber = vendorResult.VendorNumber;
            this._CustomsVendorPM.VendorTypeCode = vendorResult.VendorTypeCode;
            this._CustomsVendorPM.VATNumber = vendorResult.VATNumber;

            foreach (var vendorCommunicationResult in vendorResult.VendorCommunications)
            {
                var newVendorCommunication = new VendorCommunicationPM()
                {
                    ChangeSetOp = ChangeSetOperation.Insert,
                    CommunicationAddress = vendorCommunicationResult.CommunicationAddress,
                    CommunicationTypeCode = vendorCommunicationResult.CommunicationType,
                    Tenant = requestParams.Tenant,
                };
                this._CustomsVendorPM.VendorCommunications.Add(newVendorCommunication);
            }

            updateService.Update(this._CustomsVendorPM, true);
        }

        private void DeleteVendorCommunications()
        {
            foreach (var vendorCommunicationItem in this._CustomsVendorPM.VendorCommunications)
            {
                vendorCommunicationItem.ChangeSetOp = ChangeSetOperation.Delete;
                this._CustomsVendorPM.DeletedVendorCommunications.Add(vendorCommunicationItem);
            }
        }
    }
}
