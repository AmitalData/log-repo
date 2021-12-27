using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSearchByIDServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CL_NG_8344_ClientSearchByIDResponseService : ResponseServiceBase<ClientSearchByIDResponseData, CL_NG_8344_Web02_ClientSearchByIDDetail, ClientSearchRequestParams>
    {
        public override ClientSearchByIDResponseData GetResponse(CL_NG_8344_Web02_ClientSearchByIDDetail customResponse, ClientSearchRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(CL_NG_8344_Web02_ClientSearchByIDDetail customResponse, ClientSearchRequestParams requestParams)
        {
            //Analyze message 8344- Client Search By ID Detail

            var clientQueryService = new ClientQueryService(requestParams.Tenant);
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var clientUpdateService = new ClientUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var clientsPoaUpdateService = new ClientsPoaUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var setting = CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant);
            string authorizedId = setting?.CustomsAgentId;
            string authorizerId = customResponse.GeneralDetails.externalID.Value.ToString();

            this.MyResponseData = new ClientSearchByIDResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                return;
            }

            if (customResponse.GeneralDetails == null)
            {
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "אין נתונים להצגה ליבואן המבוקש";
                return;
            }

            //GeneralDetails
            this.MyResponseData.ExternalID = customResponse.GeneralDetails.externalID.ToString();
            this.MyResponseData.IsActive = customResponse.GeneralDetails.isActive;
            this.MyResponseData.MerkavahNumber = customResponse.GeneralDetails.merkavahNumber;

            //AddressContactPhone
            if (customResponse.AddressContactPhone != null && customResponse.AddressContactPhone.Length > 0)
            {
                this.MyResponseData.AddressContactPhoneList = new List<AddressContactPhone>();
                foreach (var addressContactPhoneItem in customResponse.AddressContactPhone)
                {
                    var addressContactPhone = new AddressContactPhone();
                    addressContactPhone.Address = addressContactPhoneItem.address;
                    if (addressContactPhoneItem.addressPurposeIDSpecified == true)
                    {
                        addressContactPhone.AddressPurposeID = addressContactPhoneItem.addressPurposeID.ToString();
                    }
                    addressContactPhone.AddressPurposeName = addressContactPhoneItem.addressPurposeName;
                    addressContactPhone.AddressTypeID = addressContactPhoneItem.addressTypeID.ToString();
                    addressContactPhone.AddressTypeName = addressContactPhoneItem.addressTypeName;

                    if (addressContactPhoneItem.ContactPhoneList != null && addressContactPhoneItem.ContactPhoneList.Length > 0)
                    {
                        addressContactPhone.ContactPhoneList = new List<ContactPhone>();
                        foreach (var contactPhoneItem in addressContactPhoneItem.ContactPhoneList)
                        {
                            var contactPhone = new ContactPhone();
                            contactPhone.CommunicationAddress = contactPhoneItem.communicationAddress;
                            contactPhone.CommunicationTypeID = contactPhoneItem.communicationTypeID;
                            contactPhone.CommunicationTypeName = contactPhoneItem.communicationTypeName;
                            addressContactPhone.ContactPhoneList.Add(contactPhone);
                        }
                    }

                    this.MyResponseData.AddressContactPhoneList.Add(addressContactPhone);
                }
            }

            //Authorized
            if (customResponse.AuthorizedList != null && customResponse.AuthorizedList.Length > 0)
            {
                this.MyResponseData.AuthorizedList = new List<Authorized>();
                foreach (var authorizedItem in customResponse.AuthorizedList)
                {
                    var authorized = new Authorized();
                    authorized.AuthorizedName = authorizedItem.authorizedName;
                    authorized.CustomerActivityTypeID = authorizedItem.CustomerActivityTypeID.ToString();
                    authorized.CustomerActivityTypeName = authorizedItem.CustomerActivityTypeName;
                    if (authorizedItem.endDate != null && authorizedItem.endDateSpecified == true)
                    {
                        authorized.EndDate = authorizedItem.endDate.Value.ToString("dd/MM/yyyy");
                    }
                    authorized.PoaAuthorizationTypeID = authorizedItem.PoaAuthorizationTypeID.ToString();
                    authorized.PoaAuthorizationTypeName = authorizedItem.PoaAuthorizationTypeName;
                    authorized.PoaID = authorizedItem.poaID.ToString();
                    authorized.PoaStatus = authorizedItem.poaStatus.ToString();
                    authorized.PoaStatusName = authorizedItem.poaStatusName;
                    authorized.StartDate = authorizedItem.startDate.Date.ToString("dd/MM/yyyy");

                    this.MyResponseData.AuthorizedList.Add(authorized);
                }
            }

            //Authorizer
            if (customResponse.AuthorizerList != null && customResponse.AuthorizerList.Length > 0)
            {
                this.MyResponseData.AuthorizerList = new List<Authorized>();
                foreach (var authorizerItem in customResponse.AuthorizerList)
                {
                    var authorized = new Authorized();
                    authorized.AuthorizedName = authorizerItem.authorizerName;
                    if (authorizerItem.endDate != null && authorizerItem.endDateSpecified == true)
                    {
                        authorized.EndDate = authorizerItem.endDate.Value.ToString("dd/MM/yyyy");
                    }
                    authorized.PoaAuthorizationTypeID = authorizerItem.PoaAuthorizationTypeID.ToString();
                    authorized.PoaAuthorizationTypeName = authorizerItem.PoaAuthorizationTypeName;
                    authorized.PoaID = authorizerItem.poaID.ToString();
                    authorized.PoaStatus = authorizerItem.poaStatus.ToString();
                    authorized.PoaStatusName = authorizerItem.poaStatusName;
                    authorized.StartDate = authorizerItem.startDate.Date.ToString("dd/MM/yyyy");

                    this.MyResponseData.AuthorizerList.Add(authorized);
                }
            }

            //CustomerActivity
            if (customResponse.CustomerActivity != null && customResponse.CustomerActivity.Length > 0)
            {
                this.MyResponseData.CustomerActivityList = new List<CustomerActivity>();
                foreach (var customerActivityItem in customResponse.CustomerActivity)
                {
                    var customerActivity = new CustomerActivity();
                    customerActivity.CustomerActivityTypeID = customerActivityItem.CustomerActivityTypeID.ToString();
                    customerActivity.CustomerActivityTypeName = customerActivityItem.CustomerActivityTypeName;
                    customerActivity.IsActive = customerActivityItem.isActive;
                    customerActivity.LogisticIdentification = customerActivityItem.logisticIdentification;
                    customerActivity.StartDate = customerActivityItem.startDate.Date.ToString("dd/MM/yyyy");

                    //CustomerIndication
                    if (customerActivityItem.CustomerIndication != null && customerActivityItem.CustomerIndication.Length > 0)
                    {
                        customerActivity.CustomerIndicationList = new List<CustomerIndication>();
                        foreach (var customerIndicationItem in customerActivityItem.CustomerIndication)
                        {
                            var customerIndication = new CustomerIndication();
                            customerIndication.CustomerIndicationTypeID = customerIndicationItem.CustomerIndicationTypeID.ToString();
                            customerIndication.CustomerIndicationTypeName = customerIndicationItem.CustomerIndicationTypeName;
                            if (customerIndicationItem.endDate != null && customerIndicationItem.endDateSpecified == true)
                            {
                                customerIndication.EndDate = customerIndicationItem.endDate.Value.ToString("dd/MM/yyyy");
                            }
                            customerIndication.IsActive = customerIndicationItem.isActive;
                            customerIndication.StartDate = customerIndicationItem.startDate.Date.ToString("dd/MM/yyyy");

                            customerActivity.CustomerIndicationList.Add(customerIndication);
                        }
                    }
                    this.MyResponseData.CustomerActivityList.Add(customerActivity);
                }
            }

            //ExportRequest
            if (customResponse.ExportRequest != null && customResponse.ExportRequest.Length > 0)
            {
                this.MyResponseData.ExportRequestList = new List<ExportRequest>();
                foreach (var exportRequestItem in customResponse.ExportRequest)
                {
                    var exportRequest = new ExportRequest();
                    if (exportRequestItem.approvementEndDate != null && exportRequestItem.approvementEndDateSpecified == true)
                    {
                        exportRequest.ApprovementEndDate = exportRequestItem.approvementEndDate.Value.ToString("dd/MM/yyyy");
                    }
                    if (exportRequestItem.approvementStartDate != null && exportRequestItem.approvementStartDateSpecified == true)
                    {
                        exportRequest.ApprovementStartDate = exportRequestItem.approvementStartDate.Value.ToString("dd/MM/yyyy");
                    }
                    if (exportRequestItem.createDate != null && exportRequestItem.createDateSpecified == true)
                    {
                        exportRequest.CreateDate = exportRequestItem.createDate.Value.ToString("dd/MM/yyyy");
                    }
                    exportRequest.CustomerRequestStatusID = exportRequestItem.CustomerRequestStatusID.ToString();
                    exportRequest.CustomerRequestStatusName = exportRequestItem.CustomerRequestStatusName;
                    exportRequest.OrganizationialUnitID = exportRequestItem.organizationialUnitID;
                    exportRequest.RequestID = exportRequestItem.requestID.ToString();
                    exportRequest.RequestTypeID = exportRequestItem.requestTypeID.ToString();
                    exportRequest.RequestTypeName = exportRequestItem.requestTypeName;
                    exportRequest.StationName = exportRequestItem.stationName;

                    this.MyResponseData.ExportRequestList.Add(exportRequest);
                }
            }

            //IndicationPerClassification
            if (customResponse.IndicationPerClassification != null && customResponse.IndicationPerClassification.Length > 0)
            {
                this.MyResponseData.IndicationPerClassificationList = new List<IndicationPerClassification>();
                foreach (var indicationPerClassificationtItem in customResponse.IndicationPerClassification)
                {
                    var IndicationPerClassification = new IndicationPerClassification();
                    IndicationPerClassification.ClassificationID = indicationPerClassificationtItem.classificationID;
                    IndicationPerClassification.EndDate = indicationPerClassificationtItem.endDate.Date.ToString("dd/MM/yyyy");
                    IndicationPerClassification.GoodsItemDescription = indicationPerClassificationtItem.goodsItemDescription;
                    IndicationPerClassification.IndicationPerClassificationTypeID = indicationPerClassificationtItem.IndicationPerClassificationTypeID.ToString();
                    IndicationPerClassification.IndicationPerClassificationTypeName = indicationPerClassificationtItem.IndicationPerClassificationTypeName;
                    IndicationPerClassification.StartDate = indicationPerClassificationtItem.startDate.Date.ToString("dd/MM/yyyy");

                    this.MyResponseData.IndicationPerClassificationList.Add(IndicationPerClassification);
                }
            }

            this.MyResponseData.UserMessage = "התקלנו נתונים ליבואן";


            if (!string.IsNullOrEmpty(authorizerId) && !string.IsNullOrEmpty(authorizedId))
            {
                string clientId = clientQueryService.GetIdByCode(authorizerId.ToString(), requestParams.Tenant);

                if (!string.IsNullOrEmpty(clientId))
                {
                    var client = clientQueryService.GetSingle(clientId, true, false);

                    if (client != null)
                    {
                        client.ClientPoas
                            .Where(cp => cp.AuthorizedExternalId == authorizedId && cp.AuthorizerExternalId == authorizerId).ToList()
                            .ForEach(entity => entity.ChangeSetOp = ChangeSetOperation.Delete);

                        customResponse.AuthorizedList.ToList().ForEach(poa =>
                        {
                            var entity = new ClientsPoaPM
                            {
                                AuthorizerPassportCountry = "",
                                AuthorizedExternalId = authorizedId,
                                AuthorizerExternalId = authorizerId,
                                AuthorizerPassportNumber = "",
                                AuthorizerPassportType = "",
                                PoaID = poa.poaID.ToString(),
                                StartDate = poa.startDate,
                                EndDate = poa.endDate.GetValueOrDefault(),
                                PoaAuthorizationType = poa.PoaAuthorizationTypeID.ToString(),
                                PoaStatus = poa.poaStatus.ToString(),
                                ClientId = clientId,
                            };

                            entity.ChangeSetOp = ChangeSetOperation.Insert;
                            client.ClientPoas.Add(entity);
                        });

                        client.ChangeSetOp = ChangeSetOperation.Update;
                        clientUpdateService.Update(client, true);
                    }
                }
            }
        }
    }
}
