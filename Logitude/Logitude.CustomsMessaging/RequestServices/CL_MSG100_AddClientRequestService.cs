using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientAddMessageServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class CL_MSG100_AddClientRequestService : RequestServiceBase<CL_MSG100_AddClientMessage, CreateClientRequestParams>
    {
        public override CL_MSG100_AddClientMessage GetRequest(CreateClientRequestParams requestParams)
        {
            //Build request 3600 - Add Client + Address 
            var myAddClient = new CL_MSG100_AddClientMessage();
            int externalID = 0;
            int passportType = 0;
            int tempInt = 0;
            string requestClient = requestParams.LoggingEntityId;
            if (string.IsNullOrWhiteSpace(requestParams.LoggingEntityId))
            {
                requestClient = requestParams.PassportNumber;
            }
            string requestDescription = "הוספת לקוח " + requestClient;

            myAddClient.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            myAddClient.GeneralCustomer = new CL_MSG100_AddClientMessageGeneralCustomer();
            if (!requestParams.IsExternalId)
            {
                //Foreign Resident 
                myAddClient.GeneralCustomer.clientIdentificationType = 2;
                myAddClient.GeneralCustomer.ClientForeignResidentIdentification = new ClientForeignResidentIdentification();
                myAddClient.GeneralCustomer.ClientForeignResidentIdentification.passportNumber = requestParams.PassportNumber;
                myAddClient.GeneralCustomer.ClientForeignResidentIdentification.passportCountry = requestParams.PassportCountryCode;
                int.TryParse(requestParams.PassportTypeCode, out passportType);
                myAddClient.GeneralCustomer.ClientForeignResidentIdentification.passportType = passportType;
                myAddClient.GeneralCustomer.ClientForeignResidentIdentification.passportFirstName = requestParams.PassportFirstName;
                myAddClient.GeneralCustomer.ClientForeignResidentIdentification.passportLastName = requestParams.PassportLastName;
                if (requestParams.PassportIssueDate.HasValue)
                {
                    myAddClient.GeneralCustomer.ClientForeignResidentIdentification.passportIssueDate = requestParams.PassportIssueDate.GetValueOrDefault();
                }
                if (requestParams.PassportExpirationDate.HasValue)
                {
                    myAddClient.GeneralCustomer.ClientForeignResidentIdentification.passportExpirationDate = requestParams.PassportExpirationDate.GetValueOrDefault();
                }
                myAddClient.GeneralCustomer.ClientForeignResidentIdentification.englishBirthPlace = requestParams.EnglishBirthPlace;
                myAddClient.GeneralCustomer.ClientForeignResidentIdentification.englishFatherName = requestParams.EnglishFatherName;
                //myAddClient.GeneralCustomer.ClientForeignResidentIdentification.NationalIdentificatioNumber = requestParams.NationalIdentificationNumber;
            }
            else
            {
                //Local Client
                myAddClient.GeneralCustomer.clientIdentificationType = 1;
                int.TryParse(requestParams.LoggingEntityReference, out externalID);
                myAddClient.GeneralCustomer.externalID = externalID;
                myAddClient.GeneralCustomer.externalIDSpecified = externalID > 0 ? true : false;
            }

            myAddClient.GeneralCustomer.isActive = requestParams.IsActive;
            myAddClient.GeneralCustomer.isExporter = requestParams.IsExporter;
            myAddClient.GeneralCustomer.isImporter = requestParams.IsImporter;
            myAddClient.GeneralCustomer.GeneralDetails = new GeneralDetails();
            //myAddClient.GeneralCustomer.GeneralDetails.birthDate = Convert.ToDateTime(DateTime.Now.AddYears(-20));
            if (requestParams.BirthDate != null)
            {
                myAddClient.GeneralCustomer.GeneralDetails.birthDate = requestParams.BirthDate;
                myAddClient.GeneralCustomer.GeneralDetails.birthDateSpecified = true;
            }

            if (string.IsNullOrWhiteSpace(requestParams.DunsNumber))
            {
                myAddClient.GeneralCustomer.GeneralDetails.dunsNumberSpecified = false;
            }
            else
            {
                int.TryParse(requestParams.DunsNumber, out tempInt);
                myAddClient.GeneralCustomer.GeneralDetails.dunsNumber = tempInt;
                myAddClient.GeneralCustomer.GeneralDetails.dunsNumberSpecified = true;
            }
            myAddClient.GeneralCustomer.GeneralDetails.englishCorporationName = requestParams.EnglishCorporationName;
            myAddClient.GeneralCustomer.GeneralDetails.englishFirstName = requestParams.EnglishFirstName;
            myAddClient.GeneralCustomer.GeneralDetails.englishLastName = requestParams.EnglishLastName;
            myAddClient.GeneralCustomer.GeneralDetails.localCorporationName = requestParams.LocalCorporationName;
            myAddClient.GeneralCustomer.GeneralDetails.localFirstName = requestParams.LocalFirstName;
            myAddClient.GeneralCustomer.GeneralDetails.localLastName = requestParams.LocalLastName;
            //AddressDetails
            myAddClient.GeneralCustomer.AddressDetails = GetClientAddressDetails(requestParams);
            //DrivingLicenseDetails
            myAddClient.GeneralCustomer.DrivingLicenseDetails = GetClientDrivingLicenseDetails(requestParams);

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Client");
            this.MyRequestSheetParam.EntityId1 = requestClient;
            this.MyRequestSheetParam.RequestDescription = requestDescription;

            return myAddClient;
        }

        private DrivingLicenseDetails[] GetClientDrivingLicenseDetails(CreateClientRequestParams requestParams)
        {
            var myDrivingLicenseDetailsList = new List<DrivingLicenseDetails>();
            foreach (ClientDrivingLicenseParams clientDrivingLicenses in requestParams.ClientDrivingLicenses)
            {
                var drivingLicenseDetails = new DrivingLicenseDetails();
                drivingLicenseDetails.DrivingLicenseNumber = clientDrivingLicenses.DrivingLicenseNumber;
                drivingLicenseDetails.DrivingLicenseValidityDate = clientDrivingLicenses.DriverLicenseValidityDate;
                drivingLicenseDetails.DrivingLicenseCountryID = clientDrivingLicenses.DrivingLicenseCountryID;

                var myDrivingLicenseTypeList = new List<string>();
                foreach (var drivingLicenseTypeItem in clientDrivingLicenses.ClientDrivingLicenseTypes)
                {
                    myDrivingLicenseTypeList.Add(drivingLicenseTypeItem.DriversLicenseTypeCode);
                }
                drivingLicenseDetails.DrivingLicenseTypes = myDrivingLicenseTypeList.ToArray();

                myDrivingLicenseDetailsList.Add(drivingLicenseDetails);
            }
            return myDrivingLicenseDetailsList.ToArray();
        }

        private AddressDetails[] GetClientAddressDetails(CreateClientRequestParams requestParams)
        {
            var myAddressList = new List<AddressDetails>();
            foreach (ClientAdressParams clientAddress in requestParams.ClientAddresses)
            {

                var Address = new AddressDetails();

                Address = GetAddressDetails(clientAddress);

                myAddressList.Add(Address);
            }
            return myAddressList.ToArray();
        }

        private AddressDetails GetAddressDetails(ClientAdressParams clientAddress)
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
            int.TryParse(clientAddress.ContactStateCode, out addressContactState);
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
                if (clientAddress.LocalApartment != null)
                {
                    localApartment = (int)(clientAddress.LocalApartment);
                    addressDetails.LocalAddress.localApartment = localApartment;
                }
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
            foreach (var communicationDeviceItem in clientAddress.ClientAddressCommunicationType)
            {
                CommunicationDevice communicationDevice = new CommunicationDevice();
                communicationDevice.communicationAddress = communicationDeviceItem.CommunicationAddress;
                communicationDevice.communicationType = communicationDeviceItem.CommunicationTypeCode;

                myCommunicationDeviceList.Add(communicationDevice);
            }
            addressDetails.CommunicationDevice = myCommunicationDeviceList.ToArray();

            return addressDetails;
        }
    }
}
