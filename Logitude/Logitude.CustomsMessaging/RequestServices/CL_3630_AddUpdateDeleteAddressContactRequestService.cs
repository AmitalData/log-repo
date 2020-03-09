using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientChangeAddressContactPhoneServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class CL_3630_AddUpdateDeleteAddressContactRequestService : RequestServiceBase
        <CL_MSG106_AddUpdateDeleteAddressContactPhoneForCustomsAgent, AddAddressContactForClient>
    {
        public override CL_MSG106_AddUpdateDeleteAddressContactPhoneForCustomsAgent GetRequest(AddAddressContactForClient requestParams)
        {
            //Build request 3630- Add\Update\Delete Address and ContactPhone For Customs Agent
            var myAddUpdateDeleteAddressContactPhoneForCustomsAgent = new CL_MSG106_AddUpdateDeleteAddressContactPhoneForCustomsAgent();
            int externalID = 0;
            int passportType = 0;
            string requestClient = requestParams.ExternalId;
            string requestDescription = "הוספת/עדכון/ביטול כתובת לקוח " + requestParams.ExternalId;

            myAddUpdateDeleteAddressContactPhoneForCustomsAgent.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } }; 
       
            myAddUpdateDeleteAddressContactPhoneForCustomsAgent.CustomerIdentification = new CustomerIdentification();
            int.TryParse(requestParams.ExternalId, out externalID);
            ClientPM clientPM = null;
            if (!String.IsNullOrWhiteSpace(requestParams.ClientId))
            {
                ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
                var ClientSearchQueryService = new ClientQueryService(dbContext);
                clientPM = ClientSearchQueryService.GetSingle(requestParams.ClientId, false, false);
            }
            myAddUpdateDeleteAddressContactPhoneForCustomsAgent.CustomerIdentification.externalID = externalID;
            myAddUpdateDeleteAddressContactPhoneForCustomsAgent.CustomerIdentification.externalIDSpecified = externalID > 0 ? true : false;
            if (clientPM != null && clientPM.PassportCountryCode != "IL")
            {
                myAddUpdateDeleteAddressContactPhoneForCustomsAgent.CustomerIdentification.externalID = null;
                myAddUpdateDeleteAddressContactPhoneForCustomsAgent.CustomerIdentification.externalIDSpecified = false;
            }

            myAddUpdateDeleteAddressContactPhoneForCustomsAgent.CustomerIdentification.passportNumber = requestParams.PassportNumber;
            myAddUpdateDeleteAddressContactPhoneForCustomsAgent.CustomerIdentification.passportCountry = requestParams.PassportCountryCode;
            int.TryParse(requestParams.PassportTypeCode, out passportType);
            myAddUpdateDeleteAddressContactPhoneForCustomsAgent.CustomerIdentification.passportType = passportType;
            myAddUpdateDeleteAddressContactPhoneForCustomsAgent.CustomerIdentification.passportTypeSpecified = passportType > 0 ? true : false;
            myAddUpdateDeleteAddressContactPhoneForCustomsAgent.AddressContactPhone = GetClientAddressContactPhone(requestParams);

            if (string.IsNullOrWhiteSpace(requestParams.ExternalId))
            {
                requestClient = requestParams.PassportNumber;
            }

            switch (requestParams.OperationType)
            {
                case AddAddressContactForClient.OperationTypes.Delete:
                    requestDescription = "ביטול כתובת לקוח " + requestClient;
                    break;
                case AddAddressContactForClient.OperationTypes.Update:
                    requestDescription = "עדכון כתובת לקוח " + requestClient;
                    break;
                case AddAddressContactForClient.OperationTypes.Add:
                    requestDescription = "הוספת כתובת לקוח " + requestClient;
                    break;
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Client");
            this.MyRequestSheetParam.EntityId1 = requestParams.ClientId;
            this.MyRequestSheetParam.RequestDescription = requestDescription;

            return myAddUpdateDeleteAddressContactPhoneForCustomsAgent;
        }

        private CL_MSG106_AddUpdateDeleteAddressContactPhoneForCustomsAgentAddressContactPhone[] GetClientAddressContactPhone(AddAddressContactForClient requestParams)
        {
            var myAddressContactPhoneList = new List<CL_MSG106_AddUpdateDeleteAddressContactPhoneForCustomsAgentAddressContactPhone>();
            var AddressContactPhone = new CL_MSG106_AddUpdateDeleteAddressContactPhoneForCustomsAgentAddressContactPhone();

            int customAddressCode = 0;
            int.TryParse(requestParams.AddressCode.CustomAddressCode, out customAddressCode);

            switch (requestParams.OperationType)
            {
                case AddAddressContactForClient.OperationTypes.Delete:
                    AddressContactPhone.oldAddressContactPhoneID = customAddressCode;
                    AddressContactPhone.oldAddressContactPhoneIDSpecified = true;
                    break;
                case AddAddressContactForClient.OperationTypes.Update:
                    AddressContactPhone.oldAddressContactPhoneID = customAddressCode;
                    AddressContactPhone.oldAddressContactPhoneIDSpecified = true;
                    AddressContactPhone.AddressDetails = GetAddressDetails(requestParams.AddressCode);
                    break;
                case AddAddressContactForClient.OperationTypes.Add:
                    AddressContactPhone.AddressDetails = GetAddressDetails(requestParams.AddressCode);
                    break;
            }      

            myAddressContactPhoneList.Add(AddressContactPhone);
            return myAddressContactPhoneList.ToArray();
        }

        private AddressDetails GetAddressDetails(AddAddressContactForClient.ClientAddress clientAddress)
        {
            AddressDetails addressDetails = new AddressDetails();
            int addressId = 0;
            int addressContactState = 0;
            int addressTypeCode = 0;
            int addressPurposeCode = 0;
            int contactIdentifier = 0;
            int contactRoleTypeCode = 0;
            int authorizedSignerPermit1 = 0;
            int authorizedSignerPermit2 = 0;
            int authorizedSignerPermit3 = 0;
            int localCityCode = 0;
            int localHouseNumber = 0;
            int localApartment = 0;
            int localPOBox = 0;
            int localPostalCode = 0;

            int.TryParse(clientAddress.AddressId, out addressId);
            addressDetails.addressId = addressId;
            addressDetails.addressIdSpecified = addressId > 0 ? true : false;
            int.TryParse(clientAddress.AddressContactState, out addressContactState);
            addressDetails.addressContactState = addressContactState;

            addressDetails.HeaderAddress = new HeaderAddress();
            int.TryParse(clientAddress.AddressTypeCode, out addressTypeCode);
            addressDetails.HeaderAddress.addressType = addressTypeCode;
            int.TryParse(clientAddress.AddressPurposeCode, out addressPurposeCode);
            addressDetails.HeaderAddress.addressPurpose = addressPurposeCode;
            addressDetails.HeaderAddress.addressPurposeSpecified = addressPurposeCode > 0 ? true : false;
            addressDetails.HeaderAddress.isPalestinianCity = clientAddress.IsPalestinianCity;
            addressDetails.HeaderAddress.isHebrewAddress = clientAddress.IsHebrewAddress;

            addressDetails.AddressGeneralDetails = new AddressGeneralDetails();
            addressDetails.AddressGeneralDetails.branchName = clientAddress.BranchName;
            int.TryParse(clientAddress.ContactIdentifier, out contactIdentifier);
            addressDetails.AddressGeneralDetails.contactId = contactIdentifier;
            addressDetails.AddressGeneralDetails.contactIdSpecified = contactIdentifier > 0 ? true : false;
            addressDetails.AddressGeneralDetails.contactFirstName = clientAddress.ContactFirstName;
            addressDetails.AddressGeneralDetails.contactLastName = clientAddress.ContactLastName;
            int.TryParse(clientAddress.ContactRoleTypeCode, out contactRoleTypeCode);
            addressDetails.AddressGeneralDetails.contactRoleType = contactRoleTypeCode;
            addressDetails.AddressGeneralDetails.contactRoleTypeSpecified = contactRoleTypeCode > 0 ? true : false;
            if (clientAddress.AuthorizedSignerPermit1 != null)
            {
                addressDetails.AddressGeneralDetails.AuthorizedSignerPermit = new int[3];
                int.TryParse(clientAddress.AuthorizedSignerPermit1, out authorizedSignerPermit1);
                addressDetails.AddressGeneralDetails.AuthorizedSignerPermit[0] = authorizedSignerPermit1;
                int.TryParse(clientAddress.AuthorizedSignerPermit2, out authorizedSignerPermit2);
                addressDetails.AddressGeneralDetails.AuthorizedSignerPermit[1] = authorizedSignerPermit2;
                int.TryParse(clientAddress.AuthorizedSignerPermit3, out authorizedSignerPermit3);
                addressDetails.AddressGeneralDetails.AuthorizedSignerPermit[2] = authorizedSignerPermit3;
            }

            if (clientAddress.IsHebrewAddress == true)
            {
                addressDetails.LocalAddress = new LocalAddress();
                int.TryParse(clientAddress.LocalCityCode, out localCityCode);
                addressDetails.LocalAddress.localCity = localCityCode;
                addressDetails.LocalAddress.localCitySpecified = localCityCode > 0 ? true : false;
                addressDetails.LocalAddress.localSecondLine = clientAddress.LocalSecondLine;
                addressDetails.LocalAddress.localStreetName = clientAddress.LocalStreetName;
                int.TryParse(clientAddress.LocalHouseNumber, out localHouseNumber);
                addressDetails.LocalAddress.localHouseNumber = localHouseNumber;
                addressDetails.LocalAddress.localHouseNumberSpecified = localHouseNumber > 0 ? true : false;
                addressDetails.LocalAddress.localHouseLetter = clientAddress.LocalHouseLetter;
                addressDetails.LocalAddress.localEntrance = clientAddress.LocalEntrance;
                int.TryParse(clientAddress.LocalApartment, out localApartment);
                addressDetails.LocalAddress.localApartment = localApartment;
                addressDetails.LocalAddress.localApartmentSpecified = localApartment > 0 ? true : false;
                int.TryParse(clientAddress.LocalPOBox, out localPOBox);
                addressDetails.LocalAddress.localPOBox = localPOBox;
                addressDetails.LocalAddress.localPOBoxSpecified = localPOBox > 0 ? true : false;
                int.TryParse(clientAddress.LocalPostalCode, out localPostalCode);
                addressDetails.LocalAddress.localPostalCode = localPostalCode;
                addressDetails.LocalAddress.localPostalCodeSpecified = localPostalCode > 0 ? true : false;
            }
            else
            {
                addressDetails.EnglishAddress = new EnglishAddress();
                addressDetails.EnglishAddress.englishCountry = clientAddress.EnglishCountryCode;
                addressDetails.EnglishAddress.englishSubCountry = clientAddress.EnglishSubCountryCode;
                addressDetails.EnglishAddress.englishCityName = clientAddress.EnglishCityName;
                addressDetails.EnglishAddress.englishMainAddressLine = clientAddress.EnglishMainAddressLine;
                addressDetails.EnglishAddress.englishPostalCode = clientAddress.EnglishPostalCode;
            }

            var myCommunicationDeviceList = new List<CommunicationDevice>();
            foreach (var communicationDeviceItem in clientAddress.ClientsAddressCommunication)
            {
                CommunicationDevice communicationDevice = new CommunicationDevice();
                communicationDevice.communicationAddress = communicationDeviceItem.CommunicationAddress;
                communicationDevice.communicationType = communicationDeviceItem.CommunicationType;

                myCommunicationDeviceList.Add(communicationDevice);
            }
            addressDetails.CommunicationDevice = myCommunicationDeviceList.ToArray();

            return addressDetails;
        }
    }
}
