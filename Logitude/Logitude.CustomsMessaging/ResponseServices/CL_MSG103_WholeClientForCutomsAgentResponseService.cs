using Logitude.AmitalMessaging.Infrastructure.SystemTable;
using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Helpers;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSearchServiceReference;
using Logitude.Customs.BL.TraceEvents;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CL_MSG103_WholeClientForCutomsAgentResponseService
        : ResponseServiceBase<ClientSearchResponseData, CL_MSG103_WholeClientForCustomsAgent, ClientSearchRequestParams>
    {
        private ClientPM _MyClientSearchPM;

        public override ClientSearchResponseData GetResponse(CL_MSG103_WholeClientForCustomsAgent customResponse, ClientSearchRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(CL_MSG103_WholeClientForCustomsAgent customResponse, ClientSearchRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var ClientSearchQueryService = new ClientQueryService(dbContext);
            var ClientSearchUpdateService = new ClientUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var ClientAddUpdateService = new ClientAddressUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var ClientAddCommunicationUpdateService = new ClientsAddressCommTypeUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            String clientId = null;
            LogMessagingUtil.Instance.AppendLine("requestParams.ExternalId"+ requestParams.ExternalId);
            string userMessage = "הוספת/עדכון/ביטול כתובת לקוח " + requestParams.ExternalId;
            if (string.IsNullOrWhiteSpace(requestParams.ExternalId) && !string.IsNullOrWhiteSpace(requestParams.PassportNumber))
            {
                userMessage = "הוספת/עדכון/ביטול כתובת לקוח " + requestParams.PassportNumber;
            }

            this.MyResponseData = new ClientSearchResponseData();
            MyResponseData.Succeeded = true;

            if (customResponse.ResponseContentHeader.Exception != null && customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionType != 2266)
            {
                MyResponseData.HasException = true;
                MyResponseData.UserMessage = "Update Client failed: " + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription; ;
                MyResponseData.ResponseStatusXML = GetDummyXml(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription, requestParams.IsAngularClient);
                return;
            }

            if (!string.IsNullOrWhiteSpace(requestParams.ExternalId))
            {
                int externalId;
                bool isCanConvert = int.TryParse(requestParams.ExternalId, out externalId);
                if (isCanConvert == false)
                {
                    MyResponseData.HasException = true;
                    MyResponseData.UserMessage = "לא ניתן להוסיף לקוח עם תווים " + requestParams.ExternalId;
                    return;
                }
            }
            var setting = CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant);

            if ((customResponse.ResponseContentHeader.Exception != null && customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionType == 2266) || customResponse.GeneralCustomerData.CostomerStatusForCA == 6)
            {
                string externalID = "";
                string errorMessage = "";
                MyResponseData.HasException = true;
                if (customResponse.GeneralCustomerData.CostomerStatusForCA == 6)
                {
                    errorMessage = "כדי להקים את הלקוח במכס, חובה לספק נתוני חובה, אולם נדרש קודם ייפוי כוח מתאים";
                }
                else
                {
                    if (customResponse.ResponseContentHeader.Exception!=null)
                    {
                        errorMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                    }
                }
                MyResponseData.UserMessage = errorMessage;
                MyResponseData.Message = errorMessage;
                MyResponseData.ResponseStatusXML = GetDummyXml(errorMessage, requestParams.IsAngularClient);

                if (!string.IsNullOrWhiteSpace(requestParams.ExternalId))
                {
                    externalID = GetExternalID(requestParams.ExternalId);
                }
                if (string.IsNullOrWhiteSpace(requestParams.ExternalId) && !string.IsNullOrWhiteSpace(requestParams.PassportNumber))
                {
                    externalID = requestParams.PassportNumber;
                }
                LogMessagingUtil.Instance.AppendLine("externalID " + externalID);
                clientId = ClientSearchQueryService.GetIdByCode(externalID, requestParams.Tenant);
                LogMessagingUtil.Instance.AppendLine("clientId" + clientId);
                if (string.IsNullOrWhiteSpace(clientId))
                {
                    LogMessagingUtil.Instance.AppendLine("string.IsNullOrWhiteSpace(clientId)==true");
                    this._MyClientSearchPM = new ClientPM();
                    _MyClientSearchPM.ChangeSetOp = ChangeSetOperation.Insert;
                    _MyClientSearchPM.Tenant = requestParams.Tenant;
                    _MyClientSearchPM.Code = externalID;
                    _MyClientSearchPM.PassportNumber = requestParams.PassportNumber;
                    _MyClientSearchPM.PassportTypeCode = requestParams.PassportTypeCode;
                    _MyClientSearchPM.PassportCountryCode = requestParams.PassportCountryCode;
                    _MyClientSearchPM.FullName = string.Concat(externalID, " יש לשלוף לקוח");


                    ClientSearchUpdateService.InsertNewClientOnlyByCode(_MyClientSearchPM, true);

                    if (setting != null)
                    {
                        //if (!setting.IsConnectedToUniFreight)
                        if (string.IsNullOrWhiteSpace(setting.UnfConnectionString))
                        {
                            BuildCustomerCard(requestParams.LoggingUserId);
                        }
                    }
                    MyResponseData.UserMessage = "התווסף לקוח " + externalID + " ( " + errorMessage + " )";
                }

                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Client");
                this.MyRequestSheetParam.EntityId1 = clientId;
                this.MyRequestSheetParam.RequestDescription = "הוספת/עדכון/ביטול כתובת לקוח " + externalID;
                return;
            }

            if (customResponse.GeneralCustomerData == null)
            {
                MyResponseData.HasException = true;
                MyResponseData.UserMessage = "Update Client failed: No section 'GeneralCustomerData' in respnse";
                MyResponseData.ResponseStatusXML = GetDummyXml(customResponse.ResponseContentHeader.Remark, requestParams.IsAngularClient);
                return;
            }
            
            if (customResponse.GeneralCustomerData.externalIDSpecified == false && customResponse.GeneralCustomerData.ClientForeignResidentIdentification == null
                && customResponse.GeneralCustomerData.CostomerStatusForCA > 0)
            {
                string customerStatusMessage = "";
                bool isCanContinue = false;
                LogMessagingUtil.Instance.AppendLine("GeneralCustomerData.CostomerStatusForCA" + customResponse.GeneralCustomerData.CostomerStatusForCA);
                switch (customResponse.GeneralCustomerData.CostomerStatusForCA)
                {
                    case 3:
                        customerStatusMessage = "הלקוח איננו קיים במכס וגם לא בשע''ם. כדי להקים את הלקוח, חובה לספק נתוני חובה";
                        isCanContinue = true;
                        break;
                    case 4:
                        customerStatusMessage = "הלקוח איננו קיים במכס ואין תקשורת מול שע''ם. כדי להקים את הלקוח, חובה לספק נתוני חובה";
                        isCanContinue = true;
                        break;
                    case 5:
                        customerStatusMessage = "הלקוח איננו קיים במכס, אך בשע''ם קיים מידע חלקי. כדי להקים את הלקוח, חובה לספק נתוני חובה";
                        isCanContinue = true;
                        break;
                    case 6:
                        customerStatusMessage = "כדי להקים את הלקוח במכס, חובה לספק נתוני חובה, אולם נדרש קודם ייפוי כוח מתאים";
                        break;
                    case 7:
                        customerStatusMessage = "כדי להקים את הלקוח במכס, חובה לפנות למתפ''ש";
                        break;
                    case 8:
                        customerStatusMessage = "לא ניתן להקים לקוח עם מספר ישות זה";
                        break;
                }
                MyResponseData.HasException = true;
                MyResponseData.CanContinue = isCanContinue;
                MyResponseData.UserMessage = customerStatusMessage;
                MyResponseData.ResponseStatusXML = GetDummyXml(customerStatusMessage, requestParams.IsAngularClient);
                return;
            }
            LogMessagingUtil.Instance.AppendLine("customResponse.GeneralCustomerData.externalIDSpecified"+ customResponse.GeneralCustomerData.externalIDSpecified);
            if (customResponse.GeneralCustomerData.externalIDSpecified == true)
            {
                LogMessagingUtil.Instance.AppendLine("customResponse.GeneralCustomerData.externalID.HasValue" + customResponse.GeneralCustomerData.externalID.HasValue);
                if (!customResponse.GeneralCustomerData.externalID.HasValue)
                {
                    MyResponseData.HasException = true;
                    MyResponseData.UserMessage = "Update Client failed: No 'customResponse.GeneralCustomerData.externalID' found in response";
                    MyResponseData.ResponseStatusXML = GetDummyXml(MyResponseData.UserMessage, requestParams.IsAngularClient);
                    return;
                }
                string externalID = GetExternalID(customResponse.GeneralCustomerData.externalID.ToString());
                LogMessagingUtil.Instance.AppendLine("externalID=GetExternalID(customResponse.GeneralCustomerData.externalID.ToString()); " + externalID);
                clientId = ClientSearchQueryService.GetIdByCode(externalID, requestParams.Tenant);
                LogMessagingUtil.Instance.AppendLine("clientId = ClientSearchQueryService.GetIdByCode(externalID, requestParams.Tenant);" + clientId );
            }
            else
            {
                if (customResponse.GeneralCustomerData.ClientForeignResidentIdentification == null)
                {
                    MyResponseData.Succeeded = false;
                    MyResponseData.HasException = true;
                    MyResponseData.UserMessage = "Update Client failed: No 'customResponse.GeneralCustomerData.ClientForeignResidentIdentification' found in response";
                    MyResponseData.ResponseStatusXML = GetDummyXml(MyResponseData.UserMessage, requestParams.IsAngularClient);
                    return;
                }
                if (customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportNumber != null & customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportCountry != null)
                {
                    LogMessagingUtil.Instance.AppendLine("customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportNumber" + customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportNumber);
                    LogMessagingUtil.Instance.AppendLine("customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportCountry" + customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportCountry);

                    clientId = ClientSearchQueryService.GetIdByPassportNumberOrCountry(customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportNumber, customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportCountry, requestParams.Tenant);
                    LogMessagingUtil.Instance.AppendLine("clientId:" + clientId);

                }
            }
            bool isClientNameChanged = false;
            LogMessagingUtil.Instance.AppendLine("clientId:" + clientId);

            if (!String.IsNullOrWhiteSpace(clientId))
            {
                this._MyClientSearchPM = ClientSearchQueryService.GetSingle(clientId, true, false); // Retrieval of existing payment data
                LogMessagingUtil.Instance.AppendLine("_MyClientSearchPM" + _MyClientSearchPM.Code);

                DeleteClientAddress(ClientAddUpdateService, ClientAddCommunicationUpdateService);   // Delete All Client Address Records
                _MyClientSearchPM.ChangeSetOp = ChangeSetOperation.Update;
                userMessage = "עדכון לקוח " + requestParams.ExternalId;
                if (_MyClientSearchPM.FullName != customResponse.GeneralCustomerData.fullName) isClientNameChanged = true;
                SendImporterDeclarationRequest(requestParams);
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("MyClientSearchPM = new ClientPM();");

                this._MyClientSearchPM = new ClientPM();
                _MyClientSearchPM.ChangeSetOp = ChangeSetOperation.Insert;
                _MyClientSearchPM.Tenant = requestParams.Tenant;
                userMessage = "הוספת לקוח " + requestParams.ExternalId;
            }
            
            //Get Client Details
            GetClientDetails(customResponse);
            //Get Client Address Details
            _MyClientSearchPM.ClientAddresses = GetClientAddressDetails(customResponse.GeneralCustomerData.AddressDetails);

            if (setting != null)
            {
                //if (setting.IsConnectedToUniFreight)
                if (!string.IsNullOrWhiteSpace(setting.UnfConnectionString))
                {
                    SendClientToUnifreight(customResponse.GeneralCustomerData, requestParams.Tenant);
                    userMessage = userMessage + "\n" + "נשלח מסר ליוניפרייט";
                }
                else
                {
                    if (CustomerService.DoesCustomerVatNumberExist(_MyClientSearchPM.Code, _MyClientSearchPM.Tenant) == false)
                    {
                        BuildCustomerCard(requestParams.LoggingUserId);
                        userMessage = userMessage + "\n" + "הוקם כרטיס ללקוח";
                    }
                    else // moran 3.11.16 - call 273521
                    {
                        UpdateCustomerCard(isClientNameChanged, requestParams.LoggingUserId);
                        userMessage = userMessage + "\n" + "עודכן כרטיס ללקוח";
                    }
                }
            }

            ClientSearchUpdateService.Update(_MyClientSearchPM, true);
            requestParams.LoggingEntityId = _MyClientSearchPM.Id;
            requestParams.LoggingEntityReference = _MyClientSearchPM.Code;

            var xml = XmlGenericUtil<CL_MSG103_WholeClientForCustomsAgentGeneralCustomerData>.SerializeObject(customResponse.GeneralCustomerData);
            if (requestParams.IsAngularClient)
            {

                MyResponseData.ResponseStatusXML = JsonConvert.SerializeObject(customResponse.GeneralCustomerData);
            }
            else
            {
                MyResponseData.ResponseStatusXML = xml;
            }
           
            MyResponseData.HasException = false;
            MyResponseData.UserMessage = userMessage;
            MyResponseData.Message = "Update client " + _MyClientSearchPM.Code + " Succeeded";           

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Client");
            this.MyRequestSheetParam.EntityId1 = this._MyClientSearchPM.Id;
            this.MyRequestSheetParam.RequestDescription = "הוספת/עדכון/ביטול כתובת לקוח " + _MyClientSearchPM.Code;
        }

        private void UpdateCustomerCard(bool isClientNameChanged
            , string LoggingUserId //Itzik 
            ) // moran 3.11.16 - call 273521
        {
            CustomerQuery CustomerQuery = new CustomerQuery(_MyClientSearchPM.Tenant);
            var TempCustomer = CustomerQuery.GetSinglePMByVatNumberForHybrid(_MyClientSearchPM.Code, _MyClientSearchPM.Tenant, true);
            if (TempCustomer != null)
            {
                TempCustomer.LocalName = _MyClientSearchPM.FullName;
                CustomerService.UpdateByContext(null, TempCustomer, LoggingUserId);
            }
        }

        private void BuildCustomerCard(string LoggingUserId)
        {
            //Build Card- only if not exists and not connected to UniFreight
            CustomerPM customer = new CustomerPM()
            {
                PartnerTypeId = "CS",
                LocalName = _MyClientSearchPM.FullName,
                CountryCode = "IL",
                VatNumber = _MyClientSearchPM.Code,
                EnglishName = "-",
                Code = "new",
                Tenant = _MyClientSearchPM.Tenant,
            };
            CustomerService.CreateNew(null, customer, LoggingUserId);
        }

        //<--- Yuval Chalup 22.06.2014 T-4035
        private void SendClientToUnifreight(CL_MSG103_WholeClientForCustomsAgentGeneralCustomerData cL_MSG103_WholeClientForCustomsAgentGeneralCustomerData, int tanent)
        {
            var myCUSTOMS_TABLE = new CUSTOMS_TABLE();
            myCUSTOMS_TABLE.TABLECODE = new TABLECODE[] { new TABLECODE { TABLECODE_ID = "CTBIMPORT" } };
            var myTABLEDATAList = new List<TABLEDATA>();
            var myTABLEDATA = new TABLEDATA()
            {
                //TABLEDATA_ID = cL_MSG103_WholeClientForCustomsAgentGeneralCustomerData.externalID.ToString(),
                //TABLEDATA_NAME_ENG = cL_MSG103_WholeClientForCustomsAgentGeneralCustomerData.fullName,
                TABLEDATA_ID = _MyClientSearchPM.Code,
                TABLEDATA_NAME_ENG = _MyClientSearchPM.FullName,
                TABLEDATA_BLOCKED = "F"
            };
            if (_MyClientSearchPM.IsActive == false)
            {
                myTABLEDATA.TABLEDATA_BLOCKED = "T";
            }


            myTABLEDATAList.Add(myTABLEDATA);

            myCUSTOMS_TABLE.TABLECODE[0].TABLEDATA = myTABLEDATAList.ToArray();

            LogMessagingUtil.Instance.AppendLine(":יצא ממשק ליוניפרייט");
            LogMessagingUtil.Instance.AppendLine("לקוח חדש: " + myTABLEDATA.TABLEDATA_ID + " - " + myTABLEDATA.TABLEDATA_NAME_ENG);
			var response = Logitude.Customs.BL.Messaging.Amital.UServerCommunication.SendUpdateTableToUnifreight(tanent, "CTBIMPORT", myCUSTOMS_TABLE, true);
			if (response != null)
			{
				var gnrRes = response.GetValueOrDefault().GenericResponseObj;
			}
		}

        private static string IsBlock(bool isActive)
        {
            //if (isActive == true)
            //{
            //    return "F";
            //}
            //return "T";

            if (isActive == null)
            {
                return "F";
            }
            return "T";
        }
        //Yuval Chalup 22.06.2014 T-4035 --->

        private string GetExternalID(string ExternalId)
        {
            ExternalId = ExternalId.Trim();
            while (ExternalId.Length < 9)
            {
                ExternalId = ExternalId.Insert(0, "0");
            }
            return ExternalId;
        }

        private void DeleteClientAddress(ClientAddressUpdateService myClientAddressUpdateService, ClientsAddressCommTypeUpdateService myClientAddCommunicationUpdateService)
        {
            foreach (var addItem in _MyClientSearchPM.ClientAddresses)
            {
                DeleteClientAddressCommunication(addItem.ClientsAddressCommTypes, myClientAddCommunicationUpdateService);
                addItem.ChangeSetOp = ChangeSetOperation.Delete;
                myClientAddressUpdateService.Update(addItem, false);
            }
        }

        private void DeleteClientAddressCommunication(List<ClientsAddressCommTypePM> myAddressCommunicationList, ClientsAddressCommTypeUpdateService myClientAddCommunicationUpdateService)
        {
            foreach (var communicationItem in myAddressCommunicationList)
            {
                communicationItem.ChangeSetOp = ChangeSetOperation.Delete;
                myClientAddCommunicationUpdateService.Update(communicationItem, false);
            }
        }

        private void GetClientDetails(CL_MSG103_WholeClientForCustomsAgent customResponse)
        {
            //myClientSearchPM.Code = customResponse.GeneralCustomerData.externalID.ToString();
            if (customResponse.GeneralCustomerData.externalID != null)
            {
                _MyClientSearchPM.Code = GetExternalID(customResponse.GeneralCustomerData.externalID.ToString());
            }
            _MyClientSearchPM.FullName = customResponse.GeneralCustomerData.fullName;
            if (customResponse.GeneralCustomerData.clientTypeSpecificCodeSpecified == true)
            {
                _MyClientSearchPM.ClientTypeSpecificCode = customResponse.GeneralCustomerData.clientTypeSpecificCode.ToString();
            }
            _MyClientSearchPM.IsActive = customResponse.GeneralCustomerData.isActive.GetValueOrDefault();
            if (customResponse.GeneralCustomerData.GeneralDetails != null)
            {
                _MyClientSearchPM.LocalFirstName = customResponse.GeneralCustomerData.GeneralDetails.localFirstName;
                _MyClientSearchPM.LocalLastName = customResponse.GeneralCustomerData.GeneralDetails.localLastName;
                _MyClientSearchPM.LocalCorporationName = customResponse.GeneralCustomerData.GeneralDetails.localCorporationName;
                _MyClientSearchPM.EnglishFirstName = customResponse.GeneralCustomerData.GeneralDetails.englishFirstName;
                _MyClientSearchPM.EnglishLastName = customResponse.GeneralCustomerData.GeneralDetails.englishLastName;
                _MyClientSearchPM.EnglishCorporationName = customResponse.GeneralCustomerData.GeneralDetails.englishCorporationName;
                _MyClientSearchPM.BirthDate = customResponse.GeneralCustomerData.GeneralDetails.birthDate;
                if (customResponse.GeneralCustomerData.genderSpecified == true)
                {
                    _MyClientSearchPM.GenderCode = customResponse.GeneralCustomerData.gender.ToString();
                }
                _MyClientSearchPM.DunsNumber = customResponse.GeneralCustomerData.GeneralDetails.dunsNumber.ToString();
            }
            if (customResponse.GeneralCustomerData.ClientForeignResidentIdentification != null)
            {
                _MyClientSearchPM.PassportNumber = customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportNumber;
                _MyClientSearchPM.PassportCountryCode = customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportCountry;
                _MyClientSearchPM.PassportTypeCode = customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportType.ToString();
                _MyClientSearchPM.PassportFirstName = customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportFirstName;
                _MyClientSearchPM.PassportLastName = customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportLastName;
                _MyClientSearchPM.EnglishBirthPlace = customResponse.GeneralCustomerData.ClientForeignResidentIdentification.englishBirthPlace;
                _MyClientSearchPM.EnglishFatherName = customResponse.GeneralCustomerData.ClientForeignResidentIdentification.englishFatherName;
                //myClientSearchPM.PassportExpirationDate = customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportExpirationDate;
                //myClientSearchPM.PassportIssueDate = customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportIssueDate;
                //The conversion of a datetime2 data type to a datetime data type resulted in an out-of-range value. !!!
                if (customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportExpirationDate != DateTime.MinValue)
                {
                    _MyClientSearchPM.PassportExpirationDate = customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportExpirationDate;
                }
                if (customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportIssueDate != DateTime.MinValue)
                {
                    _MyClientSearchPM.PassportIssueDate = customResponse.GeneralCustomerData.ClientForeignResidentIdentification.passportIssueDate;
                }
            }
        }

        private List<ClientAddressPM> GetClientAddressDetails(AddressDetails[] addressDetails)
        {
            if (addressDetails == null)
            {
                return null;
            }

            var clientAddressPMList = new List<ClientAddressPM>();
            foreach (var addItem in addressDetails)
            {
                var clientAddressPM = new ClientAddressPM();

                clientAddressPM.ChangeSetOp = ChangeSetOperation.Insert;
                clientAddressPM.ClientId = _MyClientSearchPM.Id;
                clientAddressPM.Tenant = _MyClientSearchPM.Tenant;
                clientAddressPM.CustomAddressCode = addItem.addressId.ToString();
                clientAddressPM.ContactStateCode = addItem.addressContactState.ToString();
                clientAddressPM.AddressTypeCode = addItem.HeaderAddress.addressType.ToString();
                if (addItem.HeaderAddress.addressPurposeSpecified == true) clientAddressPM.AddressPurposeCode = addItem.HeaderAddress.addressPurpose.ToString();
                clientAddressPM.IsPalestinianCity = addItem.HeaderAddress.isPalestinianCity;
                clientAddressPM.IsHebrewAddress = addItem.HeaderAddress.isHebrewAddress;
                clientAddressPM.BranchName = addItem.AddressGeneralDetails.branchName;
                if (addItem.AddressGeneralDetails.contactIdSpecified == true) clientAddressPM.ContactIdentifier = addItem.AddressGeneralDetails.contactId.ToString();
                clientAddressPM.ContactFirstName = addItem.AddressGeneralDetails.contactFirstName;
                clientAddressPM.ContactLastName = addItem.AddressGeneralDetails.contactLastName;
                if (addItem.AddressGeneralDetails.contactRoleTypeSpecified == true) clientAddressPM.ContactRoleTypeCode = addItem.AddressGeneralDetails.contactRoleType.ToString();
                if (addItem.AddressGeneralDetails.AuthorizedSignerPermit != null)
                {
                    int arraySize = addItem.AddressGeneralDetails.AuthorizedSignerPermit.Count();
                    if (arraySize > 0)
                    {
                        clientAddressPM.AuthorizedSignerPermit1 = addItem.AddressGeneralDetails.AuthorizedSignerPermit[0].ToString();
                    }
                    if (arraySize > 1)
                    {
                        clientAddressPM.AuthorizedSignerPermit2 = addItem.AddressGeneralDetails.AuthorizedSignerPermit[1].ToString();
                    }
                    if (arraySize > 2)
                    {
                        clientAddressPM.AuthorizedSignerPermit3 = addItem.AddressGeneralDetails.AuthorizedSignerPermit[2].ToString();
                    }
                }
                ///Itzik  clientAddressPM.LocalCityCode = addItem.LocalAddress.localCity.ToString();  
                addItem.LocalAddress = addItem.LocalAddress ?? new LocalAddress();
                clientAddressPM.LocalCityCode = addItem.LocalAddress.localCity.GetValueOrDefault().ToString();  
                clientAddressPM.LocalSecondLine = addItem.LocalAddress.localSecondLine;
                clientAddressPM.LocalStreetName = addItem.LocalAddress.localStreetName;
                clientAddressPM.LocalHouseNumber = addItem.LocalAddress.localHouseNumber.ToString();
                clientAddressPM.LocalHouseLetter = addItem.LocalAddress.localHouseLetter;
                clientAddressPM.LocalEntrance = addItem.LocalAddress.localEntrance;
                clientAddressPM.LocalApartment = addItem.LocalAddress.localApartment;
                clientAddressPM.LocalPOBox = addItem.LocalAddress.localPOBox.ToString();
                clientAddressPM.LocalPostalCode = addItem.LocalAddress.localPostalCode.ToString();
                if (addItem.EnglishAddress != null)
                {
                    clientAddressPM.EnglishCountryCode = addItem.EnglishAddress.englishCountry;
                    clientAddressPM.EnglishSubCountryCode = addItem.EnglishAddress.englishSubCountry;
                    clientAddressPM.EnglishCityName = addItem.EnglishAddress.englishCityName;
                    clientAddressPM.EnglishMainAddressLine = addItem.EnglishAddress.englishMainAddressLine;
                    clientAddressPM.EnglishPostalCode = addItem.EnglishAddress.englishPostalCode;
                }

                //Get Client Communication Details
                if (addItem.CommunicationDevice != null)
                {
                    clientAddressPM.ClientsAddressCommTypes = GetClientCommunicationDetails(clientAddressPM, addItem.CommunicationDevice);
                }

                clientAddressPMList.Add(clientAddressPM);
            }

            return clientAddressPMList;
        }

        private List<ClientsAddressCommTypePM> GetClientCommunicationDetails(ClientAddressPM clientAddressPM, CommunicationDevice[] communicationDevice)
        {
            var clientAddressCommunicationDetailsPMList = new List<ClientsAddressCommTypePM>();
            int counter = 0;

            foreach (var communicationItem in communicationDevice)
            {
                var clientAddressCommunicationDetailsPM = new ClientsAddressCommTypePM();
                counter++;

                clientAddressCommunicationDetailsPM.ChangeSetOp = ChangeSetOperation.Insert;
                clientAddressCommunicationDetailsPM.ClientId = clientAddressPM.ClientId;
                clientAddressCommunicationDetailsPM.AddressId = clientAddressPM.AddressId;
                clientAddressCommunicationDetailsPM.Line = counter;
                clientAddressCommunicationDetailsPM.Tenant = clientAddressPM.Tenant;
                clientAddressCommunicationDetailsPM.CommunicationTypeCode = communicationItem.communicationType;
                clientAddressCommunicationDetailsPM.CommunicationAddress = communicationItem.communicationAddress;

                clientAddressCommunicationDetailsPMList.Add(clientAddressCommunicationDetailsPM);
            }

            return clientAddressCommunicationDetailsPMList;
        }

        private string GetDummyXml(string message, bool toJson)
        {
            var myDummyXml = new GeneralMessage() { Message = message };
            if (toJson)
            {
                var wraper = new { GeneralMessage = myDummyXml };
                return JsonConvert.SerializeObject(wraper);
            }
            var xml = XmlGenericUtil<GeneralMessage>.SerializeObject(myDummyXml);
            return xml;
        }

        public class GeneralMessage
        {
            public string Message { get; set; }
        }

        void SendImporterDeclarationRequest(ClientSearchRequestParams requestParams)
        {
            DateTime today = DateTime.Today;
            var newImporterDeclarationRequestParams = new ImporterDeclarationRequestParams()
            {
                LoggingEnabled = true,
                LoggingUserId = requestParams.LoggingUserId,
                Tenant = requestParams.Tenant,
                RequestName = "Importer Declaration Request",
                ResponseName = "Importer Declaration Request",
                IsByExpireDate = true,
                IsByType = false,
                ImporterNumber = this._MyClientSearchPM.Code,
                DeclarationExpire = today.AddDays(365),
                RequestVIA = SendRequestVIA.WebServiceBatch
            };

            var service = new VE_8326_ImporterDeclarationMessagingService();
            var responseData = service.Send(newImporterDeclarationRequestParams);
            if (!responseData.Succeeded)
            {
                LogMessagingUtil.Instance.AppendLine("Request Failed " + responseData.CustomsRequestsSheetId + ", Message: " + responseData.UserMessage);
                return;
            }
            LogMessagingUtil.Instance.AppendLine("Request Succeeded " + responseData.CustomsRequestsSheetId);
        }

    }
}
