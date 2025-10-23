
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSearchServiceReference;
using UnifreightIIG.Common.MessageLib.Client;
using UnifreightIIG.Common.MessageLib.Vendor;

namespace Logitude.CustomsMessaging.ResponseServices
{
    // moran 11.1.15 - Task 9921
    public class LO_NG_3720_MSG313_PoaUpdateForCustomsAgentResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData, LO_NG_3720_MSG313_PoaUpdateForCustomsAgent, GenericRequestParams>
    {
        public override void Update(LO_NG_3720_MSG313_PoaUpdateForCustomsAgent customResponse, GenericRequestParams requestParams)
        {

            var commonContext = CommonDataContext.GetContext(requestParams.Tenant);
            var clientQueryService = new ClientQueryService(requestParams.Tenant);
            var clientPoaQueryService = new ClientsPoaQueryService(requestParams.Tenant);
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var clientUpdateService = new ClientUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var vendorQueryService = new CustomsVendorQueryService(requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData(); //moran 1.3.15 - Task 9921
            MyResponseData.Succeeded = true; //moran 1.3.15 - Task 9921

           
                    string clientId = null;
                    clientId = clientQueryService.GetIdByCodeOrPassport(customResponse.POA.authorizerExternalId.ToString(), customResponse.POA.authorizerPassportNumber, requestParams.Tenant);
                    //if (String.IsNullOrWhiteSpace(client))
                    {
                        var clientSearchResponseData = new ClientSearchResponseData();
                        var myClientSearchResponseData = new ClientSearchResponseData();
                        myClientSearchResponseData = SendGetCustomer(customResponse, requestParams);
                        if (myClientSearchResponseData.Succeeded) //moran 25.3.15 - Task 9921
                        {
                            clientId = clientQueryService.GetIdByCodeOrPassport(customResponse.POA.authorizerExternalId.ToString(), customResponse.POA.authorizerPassportNumber, requestParams.Tenant);
                        }
                    }
                    if (!string.IsNullOrEmpty(clientId) &&
                        customResponse.POA != null  
                        && (customResponse.POA.authorizerExternalId.HasValue || !string.IsNullOrEmpty(customResponse.POA.authorizerPassportNumber))
                        && customResponse.PoaAuthorization != null && customResponse.PoaAuthorization.Length > 0)
                    {
                        var client = clientQueryService.GetSingle(clientId, true, false);
                        var poaToUpdate = client.ClientPoas?.Where(x => x.PoaID == customResponse.POA.poaID.ToString());
                        foreach (var item in customResponse.PoaAuthorization)
                        {
                            var entity = poaToUpdate?.FirstOrDefault(x => x.PoaAuthorizationType == item.poaAuthorizationType.ToString());
                            if(entity != null)
                            {
                                entity.AuthorizedExternalId = customResponse.POA.authorizedExternalId.ToString();
                                entity.PoaStatus = customResponse.POA.poaStatus.ToString();
                                entity.StartDate = customResponse.POA.startDate;
                                entity.EndDate = item.endDate ?? customResponse.POA.endDate.GetValueOrDefault();
                                entity.ChangeSetOp = ChangeSetOperation.Update;
                            }
                            else
                            {
                                entity = new ClientsPoaPM
                                {
                                    AuthorizerPassportCountry = customResponse.POA.authorizerPassportCountry,
                                    AuthorizedExternalId = customResponse.POA.authorizedExternalId.ToString(),
                                    AuthorizerExternalId = customResponse.POA.authorizerExternalId.ToString(),
                                    AuthorizerPassportNumber = customResponse.POA.authorizerPassportNumber,
                                    AuthorizerPassportType = string.IsNullOrWhiteSpace(customResponse.POA.authorizerPassportType.ToString())? null: customResponse.POA.authorizerPassportType.ToString(),
                                    PoaID = customResponse.POA.poaID.ToString(),
                                    StartDate = customResponse.POA.startDate,
                                    EndDate = item.endDate ?? customResponse.POA.endDate.GetValueOrDefault(),
                                    PoaAuthorizationType = item.poaAuthorizationType.ToString(),
                                    PoaStatus = customResponse.POA.poaStatus.ToString(),
                                    ClientId = clientId
                                };
                                entity.ChangeSetOp = ChangeSetOperation.Insert;
                                client.ClientPoas.Add(entity);
                            }
                        }
                        client.ChangeSetOp = ChangeSetOperation.Update;
                        clientUpdateService.Update(client, true);
                    }
             

            if (this.MyRequestSheetParam == null)
            {
                this.MyRequestSheetParam = new RequestSheetParam();
            }
            this.MyRequestSheetParam.RequestDescription = "יפוי כח " + customResponse.POA.authorizerName; //moran 5.4.15 - Task 9921
            UpdateNotification(customResponse, requestParams);
        }

        public override INF_MSG_GenericResponseData GetResponse(LO_NG_3720_MSG313_PoaUpdateForCustomsAgent customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public ClientSearchResponseData SendGetCustomer(LO_NG_3720_MSG313_PoaUpdateForCustomsAgent customResponse, GenericRequestParams requestParams)
        {

            ClientSearchRequestParams searchParams = new ClientSearchRequestParams()
            {
                LoggingEnabled = true,
                ExternalId = customResponse.POA.authorizerExternalId.ToString(),
                PassportCountryCode = customResponse.POA.authorizerPassportCountry,
                PassportNumber = customResponse.POA.authorizerPassportNumber,
                PassportTypeCode = customResponse.POA.authorizerPassportType.ToString(),
                Tenant = requestParams.Tenant,
                RequestVIA = SendRequestVIA.WebServiceBatch
            };
            LogMessagingUtil.Instance.AppendLine("Send Request to Get Customer " + customResponse.POA.authorizerExternalId.ToString());
            searchParams.RequestVIA = SendRequestVIA.WebServiceBatch;

            var myRequestMessagingService = new CL_MSG101_GetCustomerByEntityCustomerIdentificationMassagingService();
            var resData = myRequestMessagingService.Send(searchParams);
            if (!resData.Succeeded)
            {
                LogMessagingUtil.Instance.AppendLine("Request Failed " + resData.CustomsRequestsSheetId + ", Message: " + resData.UserMessage);
                return null;
            }
            LogMessagingUtil.Instance.AppendLine("Request Succeeded " + resData.CustomsRequestsSheetId);
            return resData;
        }

        private void UpdateNotification(LO_NG_3720_MSG313_PoaUpdateForCustomsAgent customResponse, GenericRequestParams requestParams)
        {
            var contactRep = new ContactRepository(requestParams.Tenant);
            var contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.ResolveUserIdentityName(requestParams.Tenant), requestParams.Tenant);
            string loggingUserId = "";
            loggingUserId = contact.Id;

            DoUpdateNotification(loggingUserId, customResponse, requestParams);
        }

        private void DoUpdateNotification(string loggingUserId, LO_NG_3720_MSG313_PoaUpdateForCustomsAgent customResponse, GenericRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            string desc = "";
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = requestParams.Tenant;

            newNotificationPM.NotificationDefinitionCode = "3720N";
            newNotificationPM.AssigneToNotificationTypeCode = "I";
            //desc = "התקבל יפוי כוח  מלקוח" + customResponse.POA.authorizerExternalId.ToString() + " " + customResponse.POA.authorizerName;
            desc = "התקבל יפוי כוח  ";
            if (!string.IsNullOrWhiteSpace(customResponse.POA.authorizerPassportNumber))
            {
                desc += "עבור דרכון " + customResponse.POA.authorizerPassportNumber + "(" + customResponse.POA.authorizerPassportCountry + ")";
                newNotificationPM.Reference2Number = customResponse.POA.authorizerPassportNumber;
            }
            if (customResponse.POA.authorizerExternalIdSpecified)
            {
                desc += "עבור לקוח " + customResponse.POA.authorizerExternalId.ToString();
                if (string.IsNullOrWhiteSpace(newNotificationPM.Reference2Number)) newNotificationPM.Reference2Number = customResponse.POA.authorizerExternalId.ToString();
            }
            if (!string.IsNullOrWhiteSpace(customResponse.POA.authorizerName)) desc += " " + customResponse.POA.authorizerName;

            if (customResponse.POA.startDate != null) desc += @"
" + "תאריך תחילת תוקף: " + customResponse.POA.startDate.Date.ToShortDateString();
            if (customResponse.POA.endDate.HasValue) desc += @"
" + "תאריך סיום תוקף: " + customResponse.POA.endDate.Value.ToShortDateString();

            // newNotificationPM.EntityId = customResponse.POA.authorizerExternalId.ToString(); // moran 13.9.16 - Bug 22397 - commented
            // newNotificationPM.ObjectTableId = ObjectTabelRepository.GetObjectTableByName("Customs.Client"); // moran 31.8.16 - Bug 22397 - change Customs.client to Customs.Client - retrieve changed to case sensitive // moran 13.9.16 - Bug 22397 - commented
            //newNotificationPM.Reference1Number = customResponse.POA.authorizerPassportNumber;
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.DueDate = DateTime.Now;
            newNotificationPM.Description = desc;
            if (customResponse != null && !string.IsNullOrWhiteSpace(customResponse.POA.authorizerExternalId.ToString())) // newNotificationPM.CustomerId = customResponse.POA.authorizerExternalId.ToString(); // moran 20.6.16 - Task 20789
            {// moran 13.9.16 - Bug 22397 - change handle
                ICommonDataContext CommonContext = CommonDataContext.GetContext(requestParams.Tenant);
                var cardRepository = new CardRepository(CommonContext);
                Card card = cardRepository.GetSingleCardByVatNumber(customResponse.POA.authorizerExternalId.ToString(), requestParams.Tenant);
                if (card != null && !String.IsNullOrWhiteSpace(card.Id))
                {
                    newNotificationPM.CustomerId = card.Id;
                }
            }
            newNotificationPM.AssigneToId = NotificationBase.
                CalcAssigneToId(requestParams.Tenant, customResponse.POA.authorizerExternalId.ToString(), "", "3720N", "");

            NotificationBase.CloseAllRelatedNotification(dbContext, newNotificationPM, newNotificationPM.NotificationDefinitionCode);
            notificationUpdateService.Update(newNotificationPM, true);
        }
    }
}
