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
using UnifreightIIG.Common.ClientAddMessageServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CL_MSG100_AddClientResponseService : ResponseServiceBase
       <INF_MSG_GenericResponseData, INF_MSG_Generic, CreateClientRequestParams>
    {
        private ClientPM _MyClientPM;
        public override void Update(INF_MSG_Generic customResponse, CreateClientRequestParams requestParams)
        {
            
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var ClientSearchQueryService = new ClientQueryService(dbContext);
            var clientUpdateService = new ClientUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var clientAddressQueryService = new ClientAddressQueryService(dbContext);
            var clientAddressUpdateService = new ClientAddressUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            String clientId = null;
            string requestDescription = null;
            string loggingEntityId = "";
            if (this.MyResponseData == null)
            {
                this.MyResponseData = new INF_MSG_GenericResponseData();
            }
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception[0].ExeptionDescription;
                return;
            }

            if (!string.IsNullOrWhiteSpace(requestParams.LoggingEntityId))
            {
                loggingEntityId = requestParams.LoggingEntityId;
                clientId = ClientSearchQueryService.GetIdByCode(requestParams.LoggingEntityId, requestParams.Tenant);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(requestParams.LoggingEntityId) && !string.IsNullOrWhiteSpace(requestParams.PassportNumber))
                {
                    loggingEntityId = requestParams.PassportNumber;
                    clientId = ClientSearchQueryService.GetIdByPassportNumberOrCountry(requestParams.PassportNumber, requestParams.PassportCountryCode, requestParams.Tenant);
                }
            }

            if (String.IsNullOrWhiteSpace(clientId))
            {

                MyResponseData.HasException = true;
                string errorMessage = "";
                if (!string.IsNullOrWhiteSpace(loggingEntityId))
                {
                    int LoggingEntityId;
                    bool isCanConvert = int.TryParse(loggingEntityId, out LoggingEntityId);
                    if (isCanConvert == false)
                    {
                        MyResponseData.HasException = true;
                        MyResponseData.UserMessage = "לא ניתן להוסיף לקוח עם תווים " + requestParams.LoggingEntityId;
                        return;
                    }
                    loggingEntityId = GetLoggingEntityId(loggingEntityId);
                }

                string userMessage = "הוספת לקוח " + loggingEntityId;

                if (customResponse.ResponseContentHeader.Exception != null && customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionType != 2266)
                {
                    MyResponseData.HasException = true;
                    MyResponseData.UserMessage = "Update Client failed: " + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription; ;
                    return;
                }

                MyResponseData.HasException = true;
                if (customResponse.ResponseContentHeader.Exception != null)
                {
                    errorMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                }

                MyResponseData.UserMessage = errorMessage;

                this._MyClientPM = new ClientPM();
                _MyClientPM.ChangeSetOp = ChangeSetOperation.Insert;
                _MyClientPM.Tenant = requestParams.Tenant;
                _MyClientPM.Code = loggingEntityId;
                _MyClientPM.PassportNumber = requestParams.PassportNumber;
                _MyClientPM.PassportTypeCode = requestParams.PassportTypeCode;
                _MyClientPM.PassportCountryCode = requestParams.PassportCountryCode;
                _MyClientPM.FullName = requestParams.FullName;
                clientId = loggingEntityId;

                MyResponseData.UserMessage = "התווסף לקוח " + loggingEntityId + " ( " + errorMessage + " )";

                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Client");
                this.MyRequestSheetParam.EntityId1 = clientId;
                this.MyRequestSheetParam.RequestDescription = "הוספת לקוח " + loggingEntityId;

            }
            else
            {
                this._MyClientPM = ClientSearchQueryService.GetSingle(clientId, true, false); 
                _MyClientPM.ChangeSetOp = ChangeSetOperation.Update;
                MyResponseData.UserMessage = "עודכן לקוח " + clientId;

                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Client");
                this.MyRequestSheetParam.EntityId1 = clientId;
                this.MyRequestSheetParam.RequestDescription = "עדכון לקוח " + loggingEntityId;
            }

            if (!String.IsNullOrWhiteSpace(clientId) && _MyClientPM != null)
            {
                if (requestParams.ClientAddresses != null)
                {
                    var myAddressList = new List<ClientAddressPM>();
                    foreach (ClientAdressParams clientAddress in requestParams.ClientAddresses)
                    {

                        ClientAddressPM _MyClientAddressPM = new ClientAddressPM();
                        _MyClientAddressPM.ChangeSetOp = ChangeSetOperation.Insert;
                        _MyClientAddressPM.ClientId = clientId;
                        _MyClientAddressPM.Tenant = requestParams.Tenant;
                        if (customResponse.ResponseContentHeader != null && customResponse.ResponseContentHeader.ApplicationID > 0)
                        {
                            _MyClientAddressPM.CustomAddressCode = customResponse.ResponseContentHeader.ApplicationID.ToString();
                        }
                        UpdateAddressContactForClient(clientAddress, _MyClientAddressPM);
                        myAddressList.Add(_MyClientAddressPM);
                        //requestDescription = "הוספת כתובת לקוח בוצע בהצלחה";

                        //clientAddressUpdateService.Update(_MyClientAddressPM, true);
                        //requestParams.LoggingEntityId = _MyClientAddressPM.ClientId;
                        //requestParams.LoggingEntityReference = _MyClientAddressPM.AddressId;
                    }

                    _MyClientPM.ClientAddresses = myAddressList;
                }

                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.ApplicationID = clientId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = false;
                this.MyResponseData.UserMessage = requestDescription;
            }

            clientUpdateService.Update(_MyClientPM, true);
        }


        

        private string GetLoggingEntityId(string LoggingEntityId)
        {
 	
            LoggingEntityId = LoggingEntityId.Trim();
            while (LoggingEntityId.Length < 9)
            {
                LoggingEntityId = LoggingEntityId.Insert(0, "0");
            }
            return LoggingEntityId;
        
        }
        
        
        void UpdateAddressContactForClient(ClientAdressParams clientAddress, ClientAddressPM _MyClientAddressPM)
        {
            //Get Client Address Details
            _MyClientAddressPM.ContactStateCode = clientAddress.ContactStateCode;
            _MyClientAddressPM.AddressTypeCode = clientAddress.AddressTypeCode;
            _MyClientAddressPM.AddressPurposeCode = clientAddress.AddressPurposeCode;
            _MyClientAddressPM.IsPalestinianCity = clientAddress.IsPalestinianCity;
            _MyClientAddressPM.IsHebrewAddress = clientAddress.IsHebrewAddress;
            _MyClientAddressPM.BranchName = clientAddress.BranchName;
            _MyClientAddressPM.ContactIdentifier = clientAddress.ContactIdentifier;
            _MyClientAddressPM.ContactFirstName = clientAddress.ContactFirstName;
            _MyClientAddressPM.ContactLastName = clientAddress.ContactLastName;
            _MyClientAddressPM.ContactRoleTypeCode = clientAddress.ContactRoleTypeCode;
            _MyClientAddressPM.AuthorizedSignerPermit1 = clientAddress.AuthorizedSignerPermit1;
            _MyClientAddressPM.AuthorizedSignerPermit2 = clientAddress.AuthorizedSignerPermit2;
            _MyClientAddressPM.AuthorizedSignerPermit3 = clientAddress.AuthorizedSignerPermit3;
            if (clientAddress.IsHebrewAddress == true)
            {
                _MyClientAddressPM.LocalCityCode = clientAddress.LocalCityCode;
                _MyClientAddressPM.LocalSecondLine = clientAddress.LocalSecondLine;
                _MyClientAddressPM.LocalStreetName = clientAddress.LocalStreetName;
                _MyClientAddressPM.LocalHouseNumber = clientAddress.LocalHouseNumber;
                _MyClientAddressPM.LocalHouseLetter = clientAddress.LocalHouseLetter;
                _MyClientAddressPM.LocalEntrance = clientAddress.LocalEntrance;
                _MyClientAddressPM.LocalApartment = clientAddress.LocalApartment;
                _MyClientAddressPM.LocalPOBox = clientAddress.LocalPOBox;
                _MyClientAddressPM.LocalPostalCode = clientAddress.LocalPostalCode;
            }
            else
            {
                _MyClientAddressPM.EnglishCountryCode = clientAddress.EnglishCountryCode;
                _MyClientAddressPM.EnglishSubCountryCode = clientAddress.EnglishSubCountryCode;
                _MyClientAddressPM.EnglishCityName = clientAddress.EnglishCityName;
                _MyClientAddressPM.EnglishMainAddressLine = clientAddress.EnglishMainAddressLine;
                _MyClientAddressPM.EnglishPostalCode = clientAddress.EnglishPostalCode;
            }

            //Get Client Communication Details
            if (clientAddress.ClientAddressCommunicationType != null)
            {
                _MyClientAddressPM.ClientsAddressCommTypes = GetClientCommunicationDetails(clientAddress.ClientAddressCommunicationType, _MyClientAddressPM);
            }
        }


        private List<ClientsAddressCommTypePM> GetClientCommunicationDetails(List<ClientAddressCommunicationType> communicationDeviceList, ClientAddressPM _MyClientAddressPM)
        {
            List<ClientsAddressCommTypePM> clientAddressCommunicationDetailsPMList = new List<ClientsAddressCommTypePM>();
            int counter = 0;

            foreach (var communicationItem in communicationDeviceList)
            {
                counter++;
                var clientAddressCommunicationDetailsPM = new ClientsAddressCommTypePM();
                clientAddressCommunicationDetailsPM.ChangeSetOp = ChangeSetOperation.Insert;
                clientAddressCommunicationDetailsPM.ClientId = _MyClientAddressPM.ClientId;
                clientAddressCommunicationDetailsPM.AddressId = _MyClientAddressPM.AddressId;
                clientAddressCommunicationDetailsPM.Line = counter;
                clientAddressCommunicationDetailsPM.Tenant = _MyClientAddressPM.Tenant;
                clientAddressCommunicationDetailsPM.CommunicationTypeCode = communicationItem.CommunicationTypeCode;
                clientAddressCommunicationDetailsPM.CommunicationAddress = communicationItem.CommunicationAddress;

                clientAddressCommunicationDetailsPMList.Add(clientAddressCommunicationDetailsPM);
            }
            return clientAddressCommunicationDetailsPMList;
        }


        public override INF_MSG_GenericResponseData GetResponse(INF_MSG_Generic customResponse, CreateClientRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
