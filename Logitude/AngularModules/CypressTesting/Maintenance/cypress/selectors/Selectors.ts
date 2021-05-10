import { RegexSelectors } from '../selectors/RegexSelectors';

export class MaintenanceSelectors extends RegexSelectors {
   //#region Tabs
   public static readonly OthersMaintenanceTab = "#OTH";
   public static readonly ContactsMaintenanceItem = "#MaintenanceItemMTCO";
   public static readonly VendorMaintenanceItem = "#MaintenanceItemMTVD"
   public static readonly VesselMaintenanceItem = "#MaintenanceItemMTVS"
   public static readonly LocalSettingsMaintenanceItem = "#MaintenanceItemLOSE"
   public static readonly CustomerSettingsMaintenanceItem = "#MaintenanceItemCUSA"
   public static readonly InvoiceSettingsMaintenanceItem = "#MaintenanceItemINVS";
   public static readonly QuoteTemplatesMaintenanceItem = "#MaintenanceItemMTQT"
   public static readonly MaintenanceItemCountry = "#MaintenanceItemMTCN"
   public static readonly MaintenanceItemState = "#MaintenanceItemMTST"
   public static readonly MaintenanceItemGlobalZone = "#MaintenanceItemMTGZ"
   public static readonly MaintenanceItemCities = "#MaintenanceItemMTCY"
   public static readonly MaintenanceItemCommodities = "#MaintenanceItemMTCM"
   public static readonly MaintenanceItemRegions = "#MaintenanceItemMTRG"
   public static readonly MaintenanceItemShippingAgents = "#MaintenanceItemMTSA"
   public static readonly MaintenanceItemCustomAgents = "#MaintenanceItemMTCU"
   public static readonly MaintenanceItemTrucker = "#MaintenanceItemMTTR"
   public static readonly MaintenanceItemSpecialServicesType = "#MaintenanceItemMTSS"
   public static readonly MaintenanceItemMoveTypes="#MaintenanceItemMTMT"
   //#endregion

   public static readonly GeneralEditScreen = ".MediaFillAbsolute .CurvedEditArea";

   //#region Contact
   public static readonly PersonalSettingsMaintenanceTab = "#PRS"
   public static readonly ChangePasswordMaintenanceItem = "#MaintenanceItemCHPA"
   public static readonly ContactSaveButton = "#Contact-Save";
   public static readonly AnonymizeContactButton = "#ContactBAnonymize";
   public static readonly ContactEmail = "#Contact_Email";
   public static readonly ContactEnglishName = "#Contact_EnglishName";
   public static readonly ContactLocalName = "#Contact_LocalName";
   public static readonly ContactPosition = "#Contact_Position";
   public static readonly ContactBusinessPhone = "#Contact_BusinessPhone";
   public static readonly ContactMobile = "#Contact_Mobile";
   public static readonly ContactFax = "#Contact_Fax";
   public static readonly ContactBirthdayReminder = "#Contact_BirthdayReminder";
   public static readonly ContactAnniversaryReminder = "#Contact_AnniversaryReminder";
   public static readonly ContactNotes = "#Contact_Notes";
   public static readonly ContactDatepicker = "contactdatepicker";
   public static readonly ContainsNewContact = "New Contact";
   public static readonly ContainsContactsRegex = /^Contacts$/;
   //#endregion

   //#region Vendor
   public static readonly VendorBillingTab = "#VendorTHBilling";
   public static readonly VendorSaveButton = "#Vendor-Save";
   public static readonly VendorSaveCloseButton = "#Vendor-SaveClose";
   public static readonly VendorWebsite = "#Vendor_Website";
   public static readonly VendorNotes = "#Vendor_Notes";
   public static readonly VendorVatNumber = "#Vendor_VatNumber";
   public static readonly VendorBankName = "#Vendor_BankName";
   //#endregion

   //#region Vessel
   public static readonly VesselSaveButton = "#Vessel-Save";
   public static readonly VesselSaveCloseButton = "#Vessel-SaveClose";

   public static readonly VesselName = "#Vessel_EnglishName"
   public static readonly VesselLocalName = "#Vessel_LocalName"
   public static readonly VesselIMOCode = "#Vessel_IMOCode"
   public static readonly VesselFlag = "#Vessel_CountryId"
   public static readonly VesselNotes = "#Vessel_Notes"
   public static readonly VesselCode = "#Vessel_Code"
   public static readonly VesselFirstRow = "div[id$='row0']"
   //#endregion

   //#region change password window
   public static readonly CurrentPassword = "#CurrentPassword";
   public static readonly NewPassword = "#NewPassword";
   public static readonly RetypePassword = "#RetypePassword"
   public static readonly PasswordLenghtDiv = "#PasswordLenghtDiv"
   public static readonly PasswordContainsCharactersDiv = "#PasswordContainsCharactersDiv"
   public static readonly PasswordContainsNumberDiv = "#PasswordContainsNumberDiv"
   //#endregion

   //#region Customer settings
   public static readonly IsPotentialCustomerTelephoneRequiredCheckBox = "#Tenant_IsPotentialTelRequired"
   public static readonly IsCustomerTelephoneRequiredCheckBox = "#Tenant_IsCustomerTelRequired"
   public static readonly IsPotentialCustomerFaxRequiredCheckBox = "#Tenant_IsPotentialFaxRequired"
   public static readonly IsCustomerFaxRequiredCheckBox = "#Tenant_IsCustomerFaxRequired"
   public static readonly IsCustomerAddress1RequiredCheckBox = "#Tenant_IsCustomerAddress1Required"
   //#endregion

   //#region Potential Customer
   public static readonly NewCustomerButton = "#NewCustomer"
   public static readonly PotentialCustomerName = "#Customer_EnglishName"
   public static readonly PotentialCustomerCity = "#Customer_City_Potential"
   public static readonly PotentialCustomerCountry = "#Customer_CountryId_Potential"
   public static readonly PotentialCustomerState = "#Customer_StateId_Potential"
   public static readonly PotentialCustomerAddContactCheckBox = "#AddContactCheckBox"
   public static readonly PotentialCustomerPhoneNumber = "#Customer_PhoneNumber_Potential"
   public static readonly PotentialCustomerFaxNumber = "#Customer_FaxNumber_Potential"
   public static readonly PotentialCustomerAddress1 = "#Customer_Address1_Potential"
   public static readonly OkAddPotentialCustomer = "#Ok-AddPotCustomer"
   public static readonly ActivateCustomerButton = "#CustomerBActivate"
   public static readonly OKActivateCustomer = "#Ok-activate"
   //#endregion

   //#region Customer workspace
   public static CustomerSearchBar = "#Card_Search"
   public static CustomerSearchParent = "quicksearchtextbox"
   public static CustomerSearchParentClass = ".LogitudeQuickSearchTextBox"
   //#endregion

   //#region Invoice Settings
   public static readonly VoidinvoiceCheckBox = "#Tenant_Voidinvoice"
   //#endregion

   //#region Quote Template
   public static readonly QuoteTemplateName = "#QuoteTemplate_Name"
   public static readonly ValidationSummary = ".ValidationSummary"
   public static readonly QuoteSettingsLabel = "[data-cy='Labels']"
   public static readonly AddDataField = "#AddDataField"
   //#endregion

   //#region country
   public static readonly CountryCode = "#Country_Code";
   public static readonly CountryEnglishName = "#Country_EnglishName";
   public static readonly CountryLocalName = "#Country_LocalName";
   public static readonly CountryGlobalZone = "#Country_GlobalZoneId";
   public static readonly InActiveCountryCheckBox = "#Country_InActive";
   public static readonly CountryECCheckBox = "#Country_EC";
   public static readonly CountryIsNorthAmericaCheckBox = "#Country_IsNorthAmerica";
   public static readonly CountryIsStateRequiredCheckBox = "#Country_IsStateRequired";
   public static readonly CountryHasCitiesCheckBox = "#Country_HasCitiesList";
   public static readonly CountryNotes = "#Country_Notes";
   public static readonly CountrySaveButton = "#Country-Save"
   public static readonly CountryEventsTab = "#CountryTHEvents"
   public static readonly CardFiltersOpen = "[src='./Images/FiltersOpen.png']";
   public static readonly CardAddFilterBtn = "div[data-cy='AddFilterBtn']";
   public static readonly CountryCodeFilterCheckBox = "input[data-cy='CheckBox_Country.F.Code']"
   public static readonly CardCodeFilterTextValue = "#TextValue"
   public static readonly MoveTypeCodeFilterCheckBox ="input[data-cy='CheckBox_MoveType.F.Code']"
   //#endregion

   //#region State
   public static readonly StateCode = "#State_Code";
   public static readonly StateEnglishName = "#State_EnglishName";
   public static readonly StateLocalName = "#State_LocalName";
   public static readonly StateCountry = "#State_CountryId";
   public static readonly InActiveStateCheckBox = "#State_InActive";
   public static readonly StateNotes = "#State_Notes";
   public static readonly StateEventTab = "#StateTHEvents";
   public static readonly StateSaveButton = "#State-Save"
   //#endregion

   //#region City
   public static readonly CityCode = "#CountryCity_Code"
   public static readonly CityEnglishName = "#CountryCity_EnglishName"
   public static readonly CityLocalName = "#CountryCity_LocalName"
   public static readonly CityCountry = "#CountryCity_CountryId"
   public static readonly CityState = "#CountryCity_StateId";
   public static readonly InActiveCityCheckBox = "#CountryCity_InActive"
   public static readonly CityNotes = "#CountryCity_Notes"
   public static readonly CityEventTab = "#CountryCityTHEvents"
   public static readonly CitySaveButton = "#CountryCity-Save"
   //#endregion


   //#region  Global Zone
   public static readonly GlobalZoneCode = "#GlobalZone_Code"
   public static readonly GlobalZoneEnglishName = "#GlobalZone_EnglishName"
   public static readonly GlobalZoneLocalName = "#GlobalZone_LocalName"
   public static readonly InActiveGlobalZoneCheckBox = "#GlobalZone_InActive"
   public static readonly GlobalZoneSaveButton = "#GlobalZone-Save"
   public static readonly GlobalZoneEventsTab = "#GlobalZoneTHEvents"
   //#endregion
   //#region  Commodity
   public static readonly CommodityCode = "#Commodity_Code"
   public static readonly CommodityName = "#Commodity_Name"
   public static readonly InActiveCommodityCheckBox = "#Commodity_InActive"
   public static readonly CommoditySaveButton = "#Commodity-Save"
   public static readonly CommodityEventsTab = "#CommodityTHEvents"
   //#endregion
   //#region Region
   public static readonly RegionName = "#Region_Name"
   public static readonly RegionLocalName = "#Region_LocalName"
   public static readonly InActiveRegionCheckBox = "#Region_InActive"
   public static readonly RegionSaveButton = "#Region-Save"
   public static readonly RegionEventsTab = "#RegionTHEvents"
   //#endregion
   //#region card details  
   public static readonly CardCode= "#Address_CardCode"
   public static readonly CardCompanyName = "#Address_Name";
   public static readonly CardLocalName = "#Address_LocalName";
   public static readonly CardAddress1 = "#Address_Address1";
   public static readonly CardZipCode = "#Address_ZipCode";
   public static readonly CardCity = "#Address_City";
   public static readonly CardCountry = "#Address_CountryId";
   public static readonly CardState = "#Address_StateId";
   public static readonly CardPhone = "#Address_PhoneNumber";
   public static readonly CardFax = "#Address_FaxNumber";
   public static readonly CardContactCheckBox = "input[id^='CheckBox_']"
   public static readonly CardEmail = "#Address_ContactEmail"
   public static readonly CardContactEnglishName = "#Address_ContactName";
   public static readonly CardContactPosition = "#Address_ContactPosition";
   public static readonly CardContactBusinessPhone = "#Address_ContactBusinessPhone";
   public static readonly CardContactMobile = "#Address_ContactMobile";
   public static readonly CardContactFax = "#Address_ContactFax"
   //#endregion
   //#region shipping agent
   public static readonly ShippingAgentNotes = "#ShippingAgent_Notes"
   public static readonly ShippingAgentBillingTab= "#ShippingAgentTHBilling"
   public static readonly ShippingAgentBankName = "#ShippingAgent_BankName"
   public static readonly ShippingAgentIBANNumber = "#ShippingAgent_IBANNumber"
   public static readonly ShippingAgentAddressesTab = "#ShippingAgentTHAddresses"
   public static readonly ShippingAgentContactsTab = "#ShippingAgentTHContacts"
   public static readonly ShippingAgentGeneralTab="#ShippingAgentTHGeneral"
   public static readonly ShippingAgentEventsTab = "#ShippingAgentTHEvents"
   public static readonly ShippingAgentSaveButton="#ShippingAgent-Save"

   //#endregion
    //#region custom agent
    public static readonly CustomAgentNotes = "#CustomAgent_Notes"
    public static readonly CustomAgentGeneralTab="#CustomAgentTHGeneral"
    public static readonly CustomAgentBillingTab= "#CustomAgentTHBilling"
    public static readonly CustomAgentAddressesTab = "#CustomAgentTHAddresses"
    public static readonly CustomAgentContactsTab = "#CustomAgentTHContacts"
    public static readonly CustomAgentEventsTab = "#CustomAgentTHEvents"
    public static readonly CustomAgentBankName = "#CustomAgent_BankName"
    public static readonly CustomAgentIBANNumber = "#CustomAgent_IBANNumber"
    public static readonly CustomAgentSaveButton="#CustomAgent-Save"
 
    //#endregion
     //#region Trucker
     public static readonly TruckerNotes = "#Trucker_Remark"
     public static readonly TruckerGeneralTab="#TruckerTHGeneral"
     public static readonly TruckerBillingTab= "#TruckerTHBilling"
     public static readonly TruckerAddressesTab = "#TruckerTHAddresses"
     public static readonly TruckerContactsTab = "#TruckerTHContacts"
     public static readonly TruckerEventsTab = "#TruckerTHEvents"
     public static readonly TruckerBankName = "#Trucker_BankName"
     public static readonly TruckerIBANNumber = "#Trucker_IBANNumber"
     public static readonly TruckerSaveButton="#Trucker-Save"
  
     //#endregion
        //#region SpecialServicesType
   public static readonly SpecialServicesTypeCode = "#SpecialServicesType_Code"
   public static readonly SpecialServicesTypeEnglishName = "#SpecialServicesType_EnglishName"
   public static readonly SpecialServicesTypeLocalName = "#SpecialServicesType_LocalName"
   public static readonly SpecialServicesTypeGeneralTab = "#SpecialServicesTypeTHGeneral"
   public static readonly SpecialServicesTypeEventsTab = "#SpecialServicesTypeTHEvents"
   public static readonly SpecialServicesTypeSaveButton = "#SpecialServicesType-Save"
        //#endregion
              //#region SpecialServicesType
   public static readonly MoveTypeCode = "#MoveType_Code"
   public static readonly MoveTypeEnglishName = "#MoveType_MoveTypeEnglishName"
   public static readonly MoveTypeLocalName = "#MoveType_MoveTypeLocalName"
   public static readonly MoveTypeGeneralTab = "#MoveTypeTHGeneral"
   public static readonly MoveTypeEventsTab = "#MoveTypeTHEvents"
   public static readonly MoveTypeSaveButton = "#MoveType-Save"
     
        //#endregion
}