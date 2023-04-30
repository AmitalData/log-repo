using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.VendorInsertUpdateDeleteServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class VE_MSG010_VendorInsertUpdateDeleteMessageResponseService : ResponseServiceBase<VE_MSG010_VendorInsertUpdateDeleteResponseData, INF_MSG_Generic, VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams>
    {
        CustomsVendorPM _CustomsVendorPM;
        public override VE_MSG010_VendorInsertUpdateDeleteResponseData GetResponse(INF_MSG_Generic customResponse, VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(INF_MSG_Generic customResponse, VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams requestParams)
        {
            ICustomContext customContext = CustomContext.GetContext(requestParams.Tenant);
            CustomsVendorQueryService customsVendorQueryService = new CustomsVendorQueryService(requestParams.Tenant);
            CustomsVendorUpdateService customsVendorUpdateService = new CustomsVendorUpdateService(customContext, new Dictionary<string, IContext>(), requestParams.Tenant);

            bool isCustomWarning = false;
            int exeptionType = 0;
            string responseDescription = null;
            string localDescription = null;

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                foreach (var exceptionItem in customResponse.ResponseContentHeader.Exception)
                {
                    if (exceptionItem.ExeptionType == 5532)
                    {
                        isCustomWarning = true;
                        exeptionType = 5532;
                    }

                    if (!String.IsNullOrWhiteSpace(responseDescription))
                    {
                        responseDescription += Environment.NewLine;
                    }
                    responseDescription += exceptionItem.ExeptionDescription;
                }

                this.MyResponseData = new VE_MSG010_VendorInsertUpdateDeleteResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = responseDescription;
                this.MyResponseData.IsCustomWarning = isCustomWarning;
                this.MyResponseData.ExeptionType = exeptionType;
                return;
            }

            switch (requestParams.OperationType)
            {
                case Logitude.CustomsMessaging.Common.RequestParams.VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams.OperationTypes.Add:
                    this._CustomsVendorPM = new CustomsVendorPM();
                    this._CustomsVendorPM.ChangeSetOp = ChangeSetOperation.Insert;
                    SaveVendorDetails(requestParams);
                    localDescription = "הוספת ספק ";
                    this._CustomsVendorPM.VendorNumber = customResponse.ResponseContentHeader.ApplicationID.ToString();
                    this._CustomsVendorPM.InActive = false;
                    break;
                case Logitude.CustomsMessaging.Common.RequestParams.VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams.OperationTypes.Update:
                    this._CustomsVendorPM = customsVendorQueryService.GetSingle(requestParams.LoggingEntityId, true, false);
                    if (this._CustomsVendorPM == null)
                    {
                        this.MyResponseData = new VE_MSG010_VendorInsertUpdateDeleteResponseData();
                        this.MyResponseData.Succeeded = true;
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.UserMessage = "לא נמצא ספק מתאים במערכת " + requestParams.LoggingEntityId;
                        this.MyResponseData.IsCustomWarning = false;
                        return;
                    }
                    this._CustomsVendorPM.ChangeSetOp = ChangeSetOperation.Update;
                    DeleteVendorCommunications();
                    SaveVendorDetails(requestParams);
                    localDescription = "עדכון ספק ";
                    break;
                case Logitude.CustomsMessaging.Common.RequestParams.VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams.OperationTypes.Delete: // not for dev yet
                    this._CustomsVendorPM = customsVendorQueryService.GetSingle(requestParams.LoggingEntityId, true, false);
                    if (this._CustomsVendorPM == null)
                    {
                        this.MyResponseData = new VE_MSG010_VendorInsertUpdateDeleteResponseData();
                        this.MyResponseData.Succeeded = true;
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.UserMessage = "לא נמצא ספק מתאים במערכת " + requestParams.LoggingEntityId;
                        this.MyResponseData.IsCustomWarning = false;
                        return;
                    }
                    //DeleteVendorCommunications();
                    //this._CustomsVendorPM.ChangeSetOp = ChangeSetOperation.Delete;
                    this._CustomsVendorPM.ChangeSetOp = ChangeSetOperation.Update;
                    if (customResponse.ResponseContentHeader.ApplicationID != 0)
                    {
                        this._CustomsVendorPM.InActive = true;
                    }
                    localDescription = "מחיקת ספק ";
                    break;
            }

            customsVendorUpdateService.Update(this._CustomsVendorPM, true);

            responseDescription = string.Concat(localDescription, this._CustomsVendorPM.VendorNumber + "-" + this._CustomsVendorPM.VendorName + " בוצע בהצלחה");
            this.MyResponseData = new VE_MSG010_VendorInsertUpdateDeleteResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = responseDescription;
            this.MyResponseData.ApplicationID = this._CustomsVendorPM.Id;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = string.Concat(localDescription, this._CustomsVendorPM.VendorNumber + "-" + this._CustomsVendorPM.VendorName);
            this.MyRequestSheetParam.EntityId1 = this._CustomsVendorPM.Id;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsVendor");
        }

        private void SaveVendorDetails(VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams requestParams)
        {
            this._CustomsVendorPM.Tenant = requestParams.Tenant;
            this._CustomsVendorPM.VendorTypeCode = requestParams.VendorTypeCode;
            this._CustomsVendorPM.VendorName = requestParams.VendorName;
            this._CustomsVendorPM.CityName = requestParams.CityName;
            this._CustomsVendorPM.CountryCode = requestParams.CountryCode;
            this._CustomsVendorPM.SubCountryCode = requestParams.SubCountryCode;
            this._CustomsVendorPM.MainAddressLine = requestParams.MainAddressLine;
            this._CustomsVendorPM.PostalCode = requestParams.PostalCode;
            this._CustomsVendorPM.DunsNumber = requestParams.DunsNumber;
            this._CustomsVendorPM.StatusCode = requestParams.StatusCode;
            this._CustomsVendorPM.VATNumber = requestParams.VATNumber;
            this._CustomsVendorPM.TransactionTypeID = requestParams.TransactionTypeID;
            this._CustomsVendorPM.InActive = requestParams.InActive;
            this._CustomsVendorPM.IsPalestinian = requestParams.IsPalestinian;
            this._CustomsVendorPM.ExternalId = requestParams.ExternalId;
            this._CustomsVendorPM.ConcurrencyGUID = requestParams.ConcurrencyGUID;

            foreach (var vendorCommunicationResult in requestParams.CommunicationDevices)
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
        }

        public void InsertVendor(CustomsVendorPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.CustomsVendor", "NEW", entityPm.Tenant);

            //if (customContext == null)
            //{
            //    customContext = CustomContext.GetContext(entityPm.Tenant);
            //}
            ICustomContext context = CustomContext.GetContext(entityPm.Tenant);
            CustomsVendorUpdateService service = new CustomsVendorUpdateService(context, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            foreach (VendorCommunicationPM vendorcomm in entityPm.VendorCommunications)
            {
                vendorcomm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }
            service.Update(entityPm, true);

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
