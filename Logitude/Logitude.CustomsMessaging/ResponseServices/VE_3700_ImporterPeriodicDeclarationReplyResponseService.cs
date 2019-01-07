using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Vendor;
using Logitude.Server.Tools.Helpers;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class VE_3700_ImporterPeriodicDeclarationReplyResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData, VE_MSG032_ImporterPeriodicDeclarationReplyMessage, GenericRequestParams>
    {
        public override void Update(VE_MSG032_ImporterPeriodicDeclarationReplyMessage customResponse, GenericRequestParams requestParams)
        {

            this.MyResponseData = this.MyResponseData ?? new INF_MSG_GenericResponseData();
            //Analyze Message 3700 - Reply To Importer Declaration (DCA)
            ICustomContext commonContext = CustomContext.GetContext(requestParams.Tenant);
            var importerDespositionQueryService = new ImporterDespositionQueryService(requestParams.Tenant);
            var importerDespositionUpdateService = new ImporterDespositionUpdateService(commonContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var clientQueryService = new ClientQueryService(requestParams.Tenant);
            var vendorQueryService = new CustomsVendorQueryService(requestParams.Tenant);

            ImporterDespositionPM myImporterDespositionPM = new ImporterDespositionPM();
            EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel();

            //add checks here, task 44761
            string importerFromResponse;
            string vendorFromResponse;
            if (!string.IsNullOrEmpty(customResponse.ImporterPeriodicDeclarationReplyMessage.importerExternalId.ToString()))
            {
                importerFromResponse = clientQueryService.GetIdByCode(customResponse.ImporterPeriodicDeclarationReplyMessage.importerExternalId.ToString(), requestParams.Tenant, true);
                if (String.IsNullOrWhiteSpace(importerFromResponse))
                {
                    var errMess = "Importer: " + customResponse.ImporterPeriodicDeclarationReplyMessage.importerExternalId.ToString() + " doesn't exists as a Client in DB";
                    LogMessagingUtil.Instance.AppendLine(errMess);
                    this.MyResponseData.UserMessage = errMess;
                    //??? this.MyResponseData.HasException = true;
                    return;
                }
            }
            else
            {
                var errMess = "No Importer received";
                LogMessagingUtil.Instance.AppendLine(errMess);
                this.MyResponseData.UserMessage = errMess;
                //??? this.MyResponseData.HasException = true;
                return;
            }
            if (!string.IsNullOrEmpty(customResponse.ImporterPeriodicDeclarationReplyMessage.vendorID.ToString()))
            {
                vendorFromResponse = vendorQueryService.GetIdByVendorNumber(customResponse.ImporterPeriodicDeclarationReplyMessage.vendorID.ToString(), requestParams.Tenant);
                if (String.IsNullOrWhiteSpace(vendorFromResponse))
                {
                    var errMess = "Vendor: " + customResponse.ImporterPeriodicDeclarationReplyMessage.vendorID.ToString() + " doesn't exists as a Vendor in DB";
                    LogMessagingUtil.Instance.AppendLine(errMess);
                    this.MyResponseData.UserMessage = errMess;
                    //??? this.MyResponseData.HasException = true;
                    return;
                }
            }
            else
            {
                var errMess = "No Vendor received";
                LogMessagingUtil.Instance.AppendLine(errMess);
                this.MyResponseData.UserMessage = errMess;
                //??? this.MyResponseData.HasException = true;
                return;
            }

            string myImporterDespositionId = importerDespositionQueryService.GetImporterDespositionByDepositionNumber(customResponse.ImporterPeriodicDeclarationReplyMessage.declarationID, requestParams.Tenant);
            if (!string.IsNullOrWhiteSpace(myImporterDespositionId))
            {
                myImporterDespositionPM = importerDespositionQueryService.GetSingle(myImporterDespositionId, false, false);
                myImporterDespositionPM.ChangeSetOp = ChangeSetOperation.Update;
                myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.VE_3700_ImporterPeriodicDeclarationReplyResponseServiceUpdate;
                myInsertEventContextTagModel.EventRemarks = "Importer Declaration Updated";
            }
            else
            {
                myImporterDespositionPM.ChangeSetOp = ChangeSetOperation.Insert;
                myImporterDespositionPM.Tenant = requestParams.Tenant;
                myImporterDespositionPM.DepositionNumber = customResponse.ImporterPeriodicDeclarationReplyMessage.declarationID;
                myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.VE_3700_ImporterPeriodicDeclarationReplyResponseServiceNew;
                myInsertEventContextTagModel.EventRemarks = "New Importer Declaration";
            }

            myImporterDespositionPM.CurrentContextTag = myInsertEventContextTagModel;
            myImporterDespositionPM.ImporterDepositionStatusCode = customResponse.ImporterPeriodicDeclarationReplyMessage.importerPeriodicDeclarationStatusID.ToString();
            myImporterDespositionPM.ImporterlId = importerFromResponse;// clientQueryService.GetIdByCode(customResponse.ImporterPeriodicDeclarationReplyMessage.importerExternalId.ToString(), requestParams.Tenant,true);
            if (!String.IsNullOrWhiteSpace(myImporterDespositionPM.ImporterlId))
            {
                var imporetePM = clientQueryService.GetSingle(myImporterDespositionPM.ImporterlId, false, false);
                myImporterDespositionPM.ImporterName = imporetePM.FullName;
                myImporterDespositionPM.ImporterCode = customResponse.ImporterPeriodicDeclarationReplyMessage.importerExternalId.ToString();
            }
            myImporterDespositionPM.VendorID = vendorFromResponse;// vendorQueryService.GetIdByVendorNumber(customResponse.ImporterPeriodicDeclarationReplyMessage.vendorID.ToString(), requestParams.Tenant);
            if (!String.IsNullOrWhiteSpace(myImporterDespositionPM.VendorID))
            {
                var vendorPM = vendorQueryService.GetSingle(myImporterDespositionPM.VendorID, false, false);
                myImporterDespositionPM.VendorName = vendorPM.VendorName;
            }
            myImporterDespositionPM.StartDate = customResponse.ImporterPeriodicDeclarationReplyMessage.startDate;
            myImporterDespositionPM.EndDate = customResponse.ImporterPeriodicDeclarationReplyMessage.endDate;
            myImporterDespositionPM.NotesToAgent = customResponse.ImporterPeriodicDeclarationReplyMessage.notes;
            myImporterDespositionPM.ErrorMessage = customResponse.ImporterPeriodicDeclarationReplyMessage.message;

            importerDespositionUpdateService.Update(myImporterDespositionPM, true);

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.ImporterDesposition");
            this.MyRequestSheetParam.EntityId1 = myImporterDespositionPM.Id;
            this.MyRequestSheetParam.RequestDescription = "פרטי תצהיר יבואן לסוכן " + customResponse.ImporterPeriodicDeclarationReplyMessage.declarationID;

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.ApplicationID = myImporterDespositionPM.Id;
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "פרטי תצהיר יבואן לסוכן " + customResponse.ImporterPeriodicDeclarationReplyMessage.declarationID;
        }

        public override INF_MSG_GenericResponseData GetResponse(VE_MSG032_ImporterPeriodicDeclarationReplyMessage customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
