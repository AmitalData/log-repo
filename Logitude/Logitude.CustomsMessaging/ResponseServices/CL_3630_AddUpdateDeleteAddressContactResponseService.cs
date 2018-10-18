using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientChangeAddressContactPhoneServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CL_3630_AddUpdateDeleteAddressContactResponseService : ResponseServiceBase
       <INF_MSG_GenericResponseData, INF_MSG_Generic, AddAddressContactForClient>
    {
        ClientAddressPM _MyClientAddressPM;
        public override void Update(INF_MSG_Generic customResponse, AddAddressContactForClient requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var ClientSearchQueryService = new ClientQueryService(dbContext);
            var clientAddressQueryService = new ClientAddressQueryService(dbContext);
            var clientAddressUpdateService = new ClientAddressUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            String clientId = null;
            string requestDescription = null;

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception[0].ExeptionDescription;
                return;
            }

            if (requestParams.ExternalId != null)
            {
                clientId = ClientSearchQueryService.GetIdByCode(requestParams.ExternalId, requestParams.Tenant);
            }
            else
            {
                if (requestParams.PassportNumber != null & requestParams.PassportCountryCode != null)
                {
                    clientId = ClientSearchQueryService.GetIdByPassportNumberOrCountry(requestParams.PassportNumber, requestParams.PassportCountryCode, requestParams.Tenant);
                }
            }

            if (!String.IsNullOrWhiteSpace(clientId))
            {

                this._MyClientAddressPM = clientAddressQueryService.GetSingle(clientId, requestParams.AddressCode.AddressId, true, false);
                if (this._MyClientAddressPM == null && requestParams.OperationType != AddAddressContactForClient.OperationTypes.Delete)
                {
                    requestParams.OperationType = AddAddressContactForClient.OperationTypes.Add;
                }


                switch (requestParams.OperationType)
                {
                    case AddAddressContactForClient.OperationTypes.Delete:
                        DeleteClientsAddressCommunicationList();
                        this._MyClientAddressPM.ChangeSetOp = ChangeSetOperation.Delete;
                        requestDescription = "ביטול כתובת לקוח בוצע בהצלחה";
                        break;
                    case AddAddressContactForClient.OperationTypes.Update:
                        DeleteClientsAddressCommunicationList();
                        this._MyClientAddressPM.ChangeSetOp = ChangeSetOperation.Update;
                        this._MyClientAddressPM.CustomAddressCode = customResponse.ResponseContentHeader.ApplicationID.ToString();
                        UpdateAddressContactForClient(requestParams.AddressCode);
                        requestDescription = "עדכון כתובת לקוח בוצע בהצלחה";
                        break;
                    case AddAddressContactForClient.OperationTypes.Add:
                        this._MyClientAddressPM = new ClientAddressPM();
                        this._MyClientAddressPM.ChangeSetOp = ChangeSetOperation.Insert;
                        this._MyClientAddressPM.ClientId = clientId;
                        this._MyClientAddressPM.Tenant = requestParams.Tenant;
                        if (customResponse.ResponseContentHeader.ApplicationID != null)
                        {
                            this._MyClientAddressPM.CustomAddressCode = customResponse.ResponseContentHeader.ApplicationID.ToString(); //Yuval Chalup 09.11.2016 TASK-24081
                        }
                        UpdateAddressContactForClient(requestParams.AddressCode);
                        requestDescription = "הוספת כתובת לקוח בוצע בהצלחה";
                        break;
                }

                clientAddressUpdateService.Update(this._MyClientAddressPM, true);
                requestParams.LoggingEntityId = this._MyClientAddressPM.ClientId;
                requestParams.LoggingEntityReference = this._MyClientAddressPM.AddressId;
            }

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.ApplicationID = this._MyClientAddressPM.ClientId;
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = requestDescription;

        }

        private void DeleteClientsAddressCommunicationList()
        {
            if (this._MyClientAddressPM == null || this._MyClientAddressPM.ClientsAddressCommTypes == null)
            {
                return;
            }

            foreach (var communicationItem in this._MyClientAddressPM.ClientsAddressCommTypes)
            {
                communicationItem.ChangeSetOp = ChangeSetOperation.Delete;
                this._MyClientAddressPM.DeletedClientsAddressCommTypes.Add(communicationItem);
            }
        }

        private void UpdateAddressContactForClient(AddAddressContactForClient.ClientAddress clientAddress)
        {
            //Get Client Address Details
            this._MyClientAddressPM.ContactStateCode = clientAddress.AddressContactState;
            this._MyClientAddressPM.AddressTypeCode = clientAddress.AddressTypeCode;
            this._MyClientAddressPM.AddressPurposeCode = clientAddress.AddressPurposeCode;
            this._MyClientAddressPM.IsPalestinianCity = clientAddress.IsPalestinianCity;
            this._MyClientAddressPM.IsHebrewAddress = clientAddress.IsHebrewAddress;
            this._MyClientAddressPM.BranchName = clientAddress.BranchName;
            this._MyClientAddressPM.ContactIdentifier = clientAddress.ContactIdentifier;
            this._MyClientAddressPM.ContactFirstName = clientAddress.ContactFirstName;
            this._MyClientAddressPM.ContactLastName = clientAddress.ContactLastName;
            this._MyClientAddressPM.ContactRoleTypeCode = clientAddress.ContactRoleTypeCode;
            this._MyClientAddressPM.AuthorizedSignerPermit1 = clientAddress.AuthorizedSignerPermit1;
            this._MyClientAddressPM.AuthorizedSignerPermit2 = clientAddress.AuthorizedSignerPermit2;
            this._MyClientAddressPM.AuthorizedSignerPermit3 = clientAddress.AuthorizedSignerPermit3;
            if (clientAddress.IsHebrewAddress == true)
            {
                this._MyClientAddressPM.LocalCityCode = clientAddress.LocalCityCode;
                this._MyClientAddressPM.LocalSecondLine = clientAddress.LocalSecondLine;
                this._MyClientAddressPM.LocalStreetName = clientAddress.LocalStreetName;
                this._MyClientAddressPM.LocalHouseNumber = clientAddress.LocalHouseNumber;
                this._MyClientAddressPM.LocalHouseLetter = clientAddress.LocalHouseLetter;
                this._MyClientAddressPM.LocalEntrance = clientAddress.LocalEntrance;
                Decimal localApartment;
                decimal.TryParse(clientAddress.LocalApartment, out localApartment);
                this._MyClientAddressPM.LocalApartment = localApartment;
                this._MyClientAddressPM.LocalPOBox = clientAddress.LocalPOBox;
                this._MyClientAddressPM.LocalPostalCode = clientAddress.LocalPostalCode;
            }
            else
            {
                this._MyClientAddressPM.EnglishCountryCode = clientAddress.EnglishCountryCode;
                this._MyClientAddressPM.EnglishSubCountryCode = clientAddress.EnglishSubCountryCode;
                this._MyClientAddressPM.EnglishCityName = clientAddress.EnglishCityName;
                this._MyClientAddressPM.EnglishMainAddressLine = clientAddress.EnglishMainAddressLine;
                this._MyClientAddressPM.EnglishPostalCode = clientAddress.EnglishPostalCode;
            }

            //Get Client Communication Details
            if (clientAddress.ClientsAddressCommunication != null)
            {
                this._MyClientAddressPM.ClientsAddressCommTypes = GetClientCommunicationDetails(clientAddress.ClientsAddressCommunication);
            }
        }



        private List<ClientsAddressCommTypePM> GetClientCommunicationDetails(List<AddAddressContactForClient.ClientAddress.ClientsAddressCommunicationResult> communicationDeviceList)
        {
            List<ClientsAddressCommTypePM> clientAddressCommunicationDetailsPMList = new List<ClientsAddressCommTypePM>();
            int counter = 0;

            foreach (var communicationItem in communicationDeviceList)
            {
                counter++;
                var clientAddressCommunicationDetailsPM = new ClientsAddressCommTypePM();
                clientAddressCommunicationDetailsPM.ChangeSetOp = ChangeSetOperation.Insert;
                clientAddressCommunicationDetailsPM.ClientId = this._MyClientAddressPM.ClientId;
                clientAddressCommunicationDetailsPM.AddressId = this._MyClientAddressPM.AddressId;
                clientAddressCommunicationDetailsPM.Line = counter;
                clientAddressCommunicationDetailsPM.Tenant = this._MyClientAddressPM.Tenant;
                clientAddressCommunicationDetailsPM.CommunicationTypeCode = communicationItem.CommunicationType;
                clientAddressCommunicationDetailsPM.CommunicationAddress = communicationItem.CommunicationAddress;

                clientAddressCommunicationDetailsPMList.Add(clientAddressCommunicationDetailsPM);
            }
            return clientAddressCommunicationDetailsPMList;
        }

        private ClientsAddressCommTypePM FindAddressCommunicationInList(string communicationType, string communicationAddress)
        {

            if (this._MyClientAddressPM.ClientsAddressCommTypes.Count == 0)
            {
                return null;
            }

            List<ClientsAddressCommTypePM> clientsAddressCommunication = 
                                    (from a in this._MyClientAddressPM.ClientsAddressCommTypes
                                     where (a.CommunicationTypeCode == communicationType && a.CommunicationAddress == communicationAddress)
                                     select a).ToList();

            if (clientsAddressCommunication.Count > 0)
            {
                return clientsAddressCommunication[0];
            }
            else
            {
                return null;
            }
        }

        public override INF_MSG_GenericResponseData GetResponse(INF_MSG_Generic customResponse, AddAddressContactForClient requestParams)
        {
            return this.MyResponseData;
        }
    }
}
