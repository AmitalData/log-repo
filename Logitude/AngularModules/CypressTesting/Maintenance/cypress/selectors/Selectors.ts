export class MaintenanceSelectors {
    //#region Tabs
    public static readonly OthersMaintenanceTab = "#OTH";
    public static readonly ContactsMaintenanceItem = "#MaintenanceItemMTCO";
    public static readonly VendorMaintenanceItem = "#MaintenanceItemMTVD"
    public static readonly VesselMaintenanceItem = "#MaintenanceItemMTVS"
    public static readonly CustomerSettingsMaintenanceItem="#MaintenanceItemCUSA"
    public static readonly InvoiceSettingsMaintenanceItem = "#MaintenanceItemINVS";

    //#endregion

    public static readonly GeneralEditScreen = ".MediaFillAbsolute .CurvedEditArea";

    //#region Contact
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

    public static readonly VendorCompanyName = "#Address_Name";
    public static readonly VendorLocalName = "#Address_LocalName";
    public static readonly VendorAddress1 = "#Address_Address1";
    public static readonly VendorZipCode = "#Address_ZipCode";
    public static readonly VendorCity = "#Address_City";
    public static readonly VendorCountry = "#Address_CountryId";
    public static readonly VendorState = "#Address_StateId";
    public static readonly VendorPhone = "#Address_PhoneNumber";
    public static readonly VendorFax = "#Address_FaxNumber";
    public static readonly VendorWebsite = "#Vendor_Website";
    public static readonly VendorNotes = "#Vendor_Notes";
    public static readonly VendorVatNumber = "#Vendor_VatNumber";
    public static readonly VendorBankName = "#Vendor_BankName";

    public static readonly VendorContactCheckBox = "input[id^='CheckBox_']"
    public static readonly VendorContactEnglishName = "#Address_ContactName";
    public static readonly VendorContactPosition = "#Address_ContactPosition";
    public static readonly VendorContactBusinessPhone = "#Address_ContactBusinessPhone";
    public static readonly VendorContactMobile = "#Address_ContactMobile";
    public static readonly VendorContactFax = "#Address_ContactFax";
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
    //#region Customer settings
    public static readonly IsPotentialCustomerTelephoneRequiredCheckBox="#Tenant_IsPotentialTelRequired"
    public static readonly IsCustomerTelephoneRequiredCheckBox="#Tenant_IsCustomerTelRequired"
    public static readonly IsPotentialCustomerFaxRequiredCheckBox="#Tenant_IsPotentialFaxRequired"
    public static readonly IsCustomerFaxRequiredCheckBox="#Tenant_IsCustomerFaxRequired"
    public static readonly IsCustomerAddress1RequiredCheckBox="#Tenant_IsCustomerAddress1Required"

    //#endregion
    //#region Potential Customer
    public static readonly NewCustomerButton= "#NewCustomer"
    public static readonly PotentialCustomerName = "#Customer_EnglishName"
    public static readonly PotentialCustomerCity = "#Customer_City_Potential"
    public static readonly PotentialCustomerCountry = "#Customer_CountryId_Potential"
    public static readonly PotentialCustomerState = "#Customer_StateId_Potential"
    public static readonly PotentialCustomerAddContactCheckBox = "#AddContactCheckBox"
    public static readonly PotentialCustomerPhoneNumber = "#Customer_PhoneNumber_Potential"
    public static readonly PotentialCustomerFaxNumber = "#Customer_FaxNumber_Potential"
    public static readonly PotentialCustomerAddress1= "#Customer_Address1_Potential"
    public static readonly OkAddPotentialCustomer = "#Ok-AddPotCustomer"
    public static readonly ActivateCustomerButton= "#CustomerBActivate"
    public static readonly OKActivateCustomer="#Ok-activate"
    //#endregion
    //#region Customer workspace
    public static CustomerSearchBar= "#Card_Search"
    public static CustomerSearchParent="quicksearchtextbox"
    public static CustomerSearchParentClass=".LogitudeQuickSearchTextBox"
    //#endregion
//#region Invoice Settings
public static readonly VoidinvoiceCheckBox= "#Tenant_Voidinvoice"
//#endregion
    public static NewWizardButton(name: string): string {
        return "#NewButton_" + name;
    }

    public static SaveButton(itemName: string): string {
        return "#"+itemName+"-Save"
    }

    public static SaveCloseButton(itemName: string): string {
        return "#"+itemName+"-SaveClose"
    }
}