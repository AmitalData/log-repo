import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { Constants } from "../constants/Constants";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import * as BaseActions from "../../../Base/cypress/actions/Actions";
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';
import { BaseURLs } from "../../../Base/cypress/constants/URLs";
import { Datepicker } from "../../../Base/cypress/models/Datepicker";
import { ContactDetails } from "../models/ContactDetails";
import { ContactContext } from "../models/ContactContext";
import { VesselDetails } from "../models/VesselDetails";
import { VesselContext } from "../models/VesselContext";
import { CustomerSettingsDetails } from "../models/CustomerSettingsDetails"
import { CustomerDetails } from "../../../Common/cypress/models/CustomerDetails";
import { URLs } from "../../../Common/cypress/constants/URLs"
import { QuickSearchDetails } from "../../../Base/cypress/models/QuickSearchDetails";
import { constants } from "../../../Base/cypress/constants/constants"
import { InvoiceSettingsDetails } from "../models/InvoiceSettingsDetails";
import { QuoteTemplateContext } from "../models/QuoteTemplateContext";
import { QuoteTemplateDetails } from "cypress/models/QuoteTemplateDetails";
import { CountryDetails } from "../models/CountryDetails";
import { EventTypeDetails } from "../../../Base/cypress/models/EventTypeDetails";
import { StateDetails } from "../models/StateDetails";
import { CityDetails } from "../models/CityDetails";
import { GlobalZoneDetails } from "../models/GlobalZoneDetails"
import { CommodityDetails } from "../models/CommodityDetails"
import { QuoteSelectors } from "../../../Quote/cypress/selectors/Selectors";
import { ShipmentSelectors } from "../../../Shipment/cypress/selectors/Selectors";
import { ReceivableDetails } from "../../../Shipment/cypress/models/ReceivableDetails";
import { RegionDetails } from "../models/RegionDetails";
import { ChangePasswordsDetails } from '../models/ChangePasswordsDetails'
import { PasswordValidationMessagesDetails } from '../models/PasswordValidationMessagesDetails'
import { CardDetails } from "../models/CardDetails";
import { CardGeneralTabDetails } from "../models/CardGeneralTabDetails";
import { CardBillingTabDetails } from "../models/CardBillingTabDetails";
import { SpecialServicesTypeDetails } from "../models/SpecialServicesTypeDetails";
import { MoveTypeDetails } from "../models/MoveTypeDetails";
import { ShipmentSubTypeDetails } from "../models/ShipmentSubTypeDetails";
import { CreditCardTypeDetails } from "../models/CreditCardTypeDetails";


//#region variables
let CityCode = null;
let StateCode = null;
let GlobalZoneCode = null;
let SpecialServicesTypeCode = null;
let CardCode = null;
let TruckerCode = null;
let MoveTypeCode = null;
let ShipmentSubTypeCode = null;
let CreditCardTypeSearchValue = null;
let CommodityName = null;
let RegionName = null;
let inActiveCountry = false;
let inActiveCreditCardType = false;
let inActiveState = false;
let inActiveCity = false;
let inActiveGlobalZone = false;
let inActiveCommodity = false;
let inActiveRegion = false;
let WarehouseCode = null;
//#endregion

//#region General Actions
export function OpenMaintenanceMenu() {
    cy.Click(BaseSelectors.MaintenanceMenu, null)
}

export function SearchMaintenanceItemInMaintenanceMenu(MaintenanceItem: string) {
    OpenMaintenanceMenu();
    cy.FillLogTextBox(BaseSelectors.NullSearch, MaintenanceItem);
}

export function OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemNameToSearch: string, maintenanceItemSelector: string) {
    SearchMaintenanceItemInMaintenanceMenu(maintenanceItemNameToSearch);
    cy.Click(maintenanceItemSelector, null);
}

export function FillChangePasswordWindow(changePasswordsDetails: ChangePasswordsDetails) {
    if (changePasswordsDetails.CurrentPassword == "LoggedInUserPassword") {
        cy.GetCurrentPassword().then(CurrentPassword => {
            cy.FillLogTextBox(MaintenanceSelectors.CurrentPassword, CurrentPassword.toString())
        })
    }
    else {
        cy.FillLogTextBox(MaintenanceSelectors.CurrentPassword, changePasswordsDetails.CurrentPassword)
    }
    cy.FillLogTextBox(MaintenanceSelectors.NewPassword, changePasswordsDetails.NewPassword)
    cy.FillLogTextBox(MaintenanceSelectors.RetypePassword, changePasswordsDetails.RetypePassword)

}

export function ChangePasswordMockChange() {
    cy.intercept(Urls.PostChangePassword, [true])
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}
export function ChangePassword() {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}
export function ValidatePasswordValidationMessagesColors(passwordValidationMessagesDetails: PasswordValidationMessagesDetails) {
    ValidateMessageColor(MaintenanceSelectors.PasswordLenghtDiv, passwordValidationMessagesDetails.PasswordLengh)
    ValidateMessageColor(MaintenanceSelectors.PasswordContainsCharactersDiv, passwordValidationMessagesDetails.PasswordContainsUpperLowercase)
    ValidateMessageColor(MaintenanceSelectors.PasswordContainsNumberDiv, passwordValidationMessagesDetails.PasswordContainsNumber)
}
export function ValidateMessageColor(ValidationMessageSelector: string, ValidationMessage: string) {
    if (ValidationMessage.toLowerCase() == constants.green) {
        cy.ValidateElementColor(ValidationMessageSelector, "rgb(0, 128, 0)");
    }
    else if (ValidationMessage.toLowerCase() == constants.gray) {
        cy.ValidateElementColor(ValidationMessageSelector, "rgb(128, 128, 128)");
    }
}
export function OpenTabInMaintenanceMenu(maintenanceItemNameToSearch: string, maintenanceItemSelector: string) {
    cy.Click(BaseSelectors.MaintenanceMenu, null);
    cy.FillLogTextBox(BaseSelectors.NullSearch, maintenanceItemNameToSearch);
    cy.Click(maintenanceItemSelector, null);
}

export function OpenNewWizard(tabName: string) {
    cy.Click(MaintenanceSelectors.NewWizardButton(tabName), null);
}

export function GenerateRandomNumber(NumberLength: number) {
    let NewRandomCode = gr.GenerateRandomNumberAndString(NumberLength)
    return NewRandomCode;
}

function GenerateOnlyRandomNumber(NumberMin: number, NumberMax: number) {
    let NewRandomCode = gr.GenerateRandomNumber(NumberMin, NumberMax)
    return NewRandomCode;
}
//#endregion

//#region Vendor
export function FillVendorDetails(vendorDetails: CardDetails) {
    FillCardDetails(vendorDetails, null)
}

export function FillVendorContactDetails(conatactDetails: ContactDetails) {
    FillCardContactDetails(conatactDetails)
}

export function FillVendorGeneralTab(vendorGeneralTabDetails: CardGeneralTabDetails) {
    cy.FillLogTextBox(MaintenanceSelectors.VendorWebsite, vendorGeneralTabDetails.Website)
    cy.FillLogTextBox(MaintenanceSelectors.VendorNotes, vendorGeneralTabDetails.Notes)
}

export function FillVendorBillingTab(vendorBillingTabDetails: CardBillingTabDetails) {
    cy.FillLogTextBox(MaintenanceSelectors.VendorVatNumber, vendorBillingTabDetails.VatNumber.toString())
    cy.FillLogTextBox(MaintenanceSelectors.VendorBankName, vendorBillingTabDetails.BankName)
}

export function CreateVendor() {
    CreateCard()
}

export function UpdateVendor() {
    DefinePutVendorRequest()
    cy.Click(MaintenanceSelectors.VendorSaveButton, null)
}

export function AssertCreateVendor() {
    AssertCreateCard(Constants.Vendor)
}

export function AssertUpdateVendor() {
    AssertPutVendor()
}

export function SearchVendor() {
    SearchCard()
}

export function AssertSearchVendor(companyName: string) {
    AssertSearchCard(companyName)
}

export function AssertOpenVendor() {
    AssertOpenCard()
}

export function CloseSaveVendor() {
    DefineVendorViewGetSingleRequest()
    cy.Click(MaintenanceSelectors.VendorSaveCloseButton, null);
}

export function AssertCloseSaveVendor() {
    AssertVendorGetSingle();
}

function DefinePutVendorRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Vendor, RequestAliases.PutVendor);
}
function AssertPutVendor() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutVendor, 200);
}
function DefineVendorViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.VendorsviewGetSingle, RequestAliases.GetSignle);
}
function AssertVendorGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}
function DefineVendorsGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.VendorsGetSingle, RequestAliases.GetSignle);
}
//#endregion

//#region Vessel
export function FillVesselDetails(vesselDetails: VesselDetails) {
    FillVesselName(vesselDetails.Name)
    cy.FillLogTextBox(MaintenanceSelectors.VesselIMOCode, vesselDetails.IMO);
    cy.FillLogTextBox(MaintenanceSelectors.VesselLocalName, vesselDetails.LocalName);
    cy.FillLogLov(MaintenanceSelectors.VesselFlag, vesselDetails.Flag, true);
    cy.FillLogTextBox(MaintenanceSelectors.VesselNotes, vesselDetails.Notes);
}

export function FillVesselGeneralTab(vesselDetails: VesselDetails) {
    cy.FillLogTextBox(MaintenanceSelectors.VesselIMOCode, vesselDetails.IMO)
    FillVesselCode(vesselDetails.Code)
}

export function CreateVessel() {
    DefinePostVesselRequest()
    DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function UpdateVessel() {
    DefinePutVesselRequest()
    cy.Click(MaintenanceSelectors.VesselSaveButton, null)
}

export function AssertCreateVessel() {
    AssertPostVessel()
    AssertGetByFilters()
}

export function AssertUpdateVessel() {
    let intercept = cy.wait("@" + RequestAliases.PutVessel);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            CheckError(interception);
        }
        else {
            AssertPutVessel(interception.response.statusCode, 200)
        }
    })
}

export function SearchVessel() {
    let vesselName = VesselContext.Name;
    if (vesselName) {
        DefineVesselViewsGetByFiltersRequest(vesselName);
        cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, vesselName);
        AssertVesselViewsGetByFilters();
    }
}

export function AssertSearchVessel() {
    let vesselName = VesselContext.Name;
    if (vesselName) {
        cy.get(BaseSelectors.ListDataLoaded)
        cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
            expect(text).to.contain(vesselName);
        });
    }
}

export function OpenVessel() {
    DefineVesselsGetSingleRequest();
    cy.get(MaintenanceSelectors.VesselFirstRow).click();
}

export function AssertOpenVessel() {
    AssertVesselGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

export function CloseSaveVessel() {
    DefineVesselViewGetSingleRequest()
    cy.Click(MaintenanceSelectors.VesselSaveCloseButton, null);
}

export function AssertCloseSaveVessel() {
    AssertVesselGetSingle();
}

function FillVesselName(vesselName: string) {
    if (vesselName) {
        VesselContext.Name = vesselName.toLowerCase() == "random" ? (GenerateRandomNumber(5)) : vesselName;
        cy.FillLogTextBox(MaintenanceSelectors.VesselName, VesselContext.Name);
    }
}

function FillVesselCode(vesselCode: string) {
    let vesselName = VesselContext.Name
    if (vesselCode) {
        VesselContext.Code = vesselCode.toLowerCase() == "random" ? vesselName : vesselCode;
        cy.FillLogTextBox(MaintenanceSelectors.VesselCode, VesselContext.Name);
    }
}

function FillNewRandomCode() {
    let NewRandomCode = GenerateRandomNumber(5)
    VesselContext.Name = NewRandomCode;
    VesselContext.Code = NewRandomCode;
    cy.FillLogTextBox(MaintenanceSelectors.VesselName, VesselContext.Name);
    cy.FillLogTextBox(MaintenanceSelectors.VesselCode, VesselContext.Code);
}

function DefinePostVesselRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Vessels, RequestAliases.PostVessel);
}

function DefinePutVesselRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Vessels, RequestAliases.PutVessel);
}

function AssertPostVessel() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostVessel, 200);
}

function AssertPutVessel(responseStatusCode: number, expectedStatusCode: number) {
    assert.equal(responseStatusCode, expectedStatusCode)
}

function CheckError(interception) {
    if (interception.response.body.ErrorMessage.indexOf("This vessel already exists") !== -1) {
        GenerateNewRandomCode();
    } else {
        throw new Error(interception.response.body.ErrorMessage);
    }
}
function GenerateNewRandomCode() {
    FillNewRandomCode();
    UpdateVessel();
}

function DefineVesselViewsGetByFiltersRequest(VesselCode: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(VesselCode), RequestAliases.GetFilterSearch);
}

function AssertVesselViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

function DefineVesselsGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.VesselsGetSingle, RequestAliases.GetSignle);
}

function DefineVesselViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.VesselviewGetSingle, RequestAliases.GetSignle);
}

function AssertVesselGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}
//#endregion

//#region Contacts
export function OpenContactsList() {
    cy.Click(MaintenanceSelectors.OthersMaintenanceTab, null);
    DefineContactViewsGetByFiltersRequest();
    cy.Click(MaintenanceSelectors.ContactsMaintenanceItem, null);
    AssertContactViewsGetByFilters();
    DefineContactViewsGetByFiltersRequest();
    cy.Click(BaseSelectors.QueryListToggleButton, null);
    cy.get(BaseSelectors.QueryListToggleButtonItem).contains(MaintenanceSelectors.ContainsContactsRegex).click();
    AssertContactViewsGetByFilters();
}

export function OpenNewContactWizard() {
    cy.Click(BaseSelectors.Button, MaintenanceSelectors.ContainsNewContact);
}

export function FillContactDetails(contactDetails: ContactDetails) {
    FillContactEmail(contactDetails.Email);
    cy.FillLogTextBox(MaintenanceSelectors.ContactEnglishName, contactDetails.EnglishName)
    cy.FillLogTextBox(MaintenanceSelectors.ContactLocalName, contactDetails.LocalName)
    cy.FillLogTextBox(MaintenanceSelectors.ContactPosition, contactDetails.Position)
    cy.FillLogTextBox(MaintenanceSelectors.ContactBusinessPhone, contactDetails.BusinessPhone)
    cy.FillLogTextBox(MaintenanceSelectors.ContactMobile, contactDetails.Mobile)
    cy.FillLogTextBox(MaintenanceSelectors.ContactFax, contactDetails.Fax)
    cy.FillLogTextBox(MaintenanceSelectors.ContactNotes, contactDetails.Notes)
    FillContactDatepicker(contactDetails.BirthdayDate, Constants.Birthday);
    FillCheckBoxProcess(MaintenanceSelectors.ContactBirthdayReminder, contactDetails.BirthdayReminder);
    FillContactDatepicker(contactDetails.AnniversaryDate, Constants.Anniversary);
    FillCheckBoxProcess(MaintenanceSelectors.ContactAnniversaryReminder, contactDetails.AnniversaryReminder);
}

export function CreateContact() {
    DefinePostContactRequest();
    DefineContactViewsGetByFiltersRequest();
    cy.Click(BaseSelectors.RedButton + ":last", null);
}

export function AssertCreateContact() {
    AssertPostContact();
    AssertContactViewsGetByFilters();
}

export function SearchContact() {
    let contactEmail = ContactContext.Email;
    if (contactEmail) {
        DefineContactViewsGetByFiltersRequest(contactEmail);
        cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, contactEmail);
        AssertContactViewsGetByFilters();
    }
}

export function AssertSearchContact() {
    let contactEmail = ContactContext.Email;
    if (contactEmail) {
        cy.get(BaseSelectors.ListDataLoaded)
        cy.get(BaseSelectors.RowClass).eq(0).invoke("text").then((text) => {
            expect(text).to.contain(contactEmail);
        });
    }
}

export function OpenContact() {
    DefineContactsGetSingleRequest();
    DefineGetMenuButtonGroupsRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

export function AssertOpenContact() {
    AssertContactsGetSingle();
    AssertGetMenuButtonGroups();
}

export function EditContact() {
    DefinePutContactRequest();
    cy.Click(MaintenanceSelectors.ContactSaveButton, null);
}

export function AssertEditContact() {
    AssertPutContact();
}

export function AnonymizeContact() {
    DefineContactsGetSingleRequest();
    cy.Click(BaseSelectors.MenuButtons, null, true);
    cy.Click(MaintenanceSelectors.AnonymizeContactButton, null);
    cy.Click(BaseSelectors.ConfirmWindowButton, null);
}

export function AssertAnonymizeContact() {
    AssertContactsGetSingle();
}

export function AssertContactDetailsValues() {
    AssertContactInputHaveValue(MaintenanceSelectors.ContactEmail, "xxx@" + ContactContext.Id + ".com");
    AssertContactInputHaveValue(MaintenanceSelectors.ContactEnglishName, "xxx");
    AssertContactInputHaveValue(MaintenanceSelectors.ContactLocalName, "xxx");
    AssertContactInputHaveValue(MaintenanceSelectors.ContactPosition, "");
    AssertContactInputHaveValue(MaintenanceSelectors.ContactBusinessPhone, "");
    AssertContactInputHaveValue(MaintenanceSelectors.ContactMobile, "");
    AssertContactInputHaveValue(MaintenanceSelectors.ContactFax, "");
    AssertContactInputHaveValue(MaintenanceSelectors.ContactNotes, "");
    AssertContactDatepickerNotSelected(Constants.Birthday);
    AssertContactDatepickerNotSelected(Constants.Anniversary);
}

function FillContactEmail(contactEmail: string) {
    if (contactEmail) {
        let emailToFill = contactEmail.toLowerCase() == "random" ? (gr.GenerateCurrentDatetimeString("_") + "@test.com") : contactEmail;
        cy.FillLogTextBox(MaintenanceSelectors.ContactEmail, emailToFill);
    }
}

function FillContactDatepicker(contactDate: string, dateType: string) {
    if (contactDate && dateType) {
        let contactDatepicker: Datepicker = BaseActions.GetDatepicker(contactDate);
        let indexOfDatepicker: number;
        if (dateType.toLowerCase() == Constants.Birthday) {
            indexOfDatepicker = 0;
        }
        else if (dateType.toLowerCase() == Constants.Anniversary) {
            indexOfDatepicker = 1;
        }
        cy.get(MaintenanceSelectors.ContactDatepicker).eq(indexOfDatepicker).within(() => {
            cy.get(BaseSelectors.ComboBox).eq(0).within(() => {
                SelectComboBoxToggleItem(contactDatepicker.Day.toString());
            });
            cy.get(BaseSelectors.ComboBox).eq(1).within(() => {
                SelectComboBoxToggleItem(contactDatepicker.Month.toString());
            });
            cy.get(BaseSelectors.ComboBox).eq(2).within(() => {
                SelectComboBoxToggleItem(contactDatepicker.Year.toString());
            });
        });
    }
}

function SelectComboBoxToggleItem(value: string) {
    cy.get(BaseSelectors.ToggleIconImage).click();
    cy.get(BaseSelectors.SpanTitle(value)).parent().click();
}

function DefineContactViewsGetByFiltersRequest(contactEmail: string = null) {
    cy.DefineRequestWait(RestAPI.GET, Urls.ContactViewsGetByFilters + (contactEmail == null ? "" : encodeURIComponent(contactEmail) + "**"), RequestAliases.ContactViewsGetByFilters);
}

function DefinePostContactRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Contacts, RequestAliases.PostContact);
}

function DefineContactsGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.ContactsGetSingle, RequestAliases.ContactsGetSingle);
}

function DefineGetMenuButtonGroupsRequest() {
    cy.DefineRequestWait(RestAPI.GET, BaseURLs.GetMenuButtonGroups, RequestAliases.GetContactMenuButtonGroups);
}

function DefinePutContactRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Contacts, RequestAliases.PutContact);
}

function AssertPostContact() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostContact, 200).then((interception) => {
        let responseBody = interception.response.body;
        ContactContext.Id = responseBody.Id;
        ContactContext.Email = responseBody.Email;
    });
}

function AssertContactViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.ContactViewsGetByFilters, 200);
}

function AssertContactsGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.ContactsGetSingle, 200);
}

function AssertPutContact() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutContact, 200);
}

function AssertGetMenuButtonGroups() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetContactMenuButtonGroups, 200);
}

function AssertContactInputHaveValue(inputSelector: string, value: string) {
    BaseAssertion.AssertElementHaveValue(inputSelector, value);
}

function AssertContactDatepickerNotSelected(dateType: string) {
    let indexOfDatepicker: number;
    if (dateType.toLowerCase() == Constants.Birthday) {
        indexOfDatepicker = 0;
    }
    else if (dateType.toLowerCase() == Constants.Anniversary) {
        indexOfDatepicker = 1;
    }
    cy.get(MaintenanceSelectors.ContactDatepicker).eq(indexOfDatepicker).within(() => {
        cy.get(BaseSelectors.ComboBox).eq(0).within(() => {
            BaseAssertion.AssertElementNotExist(BaseSelectors.SelectedComboboxItem);
        });
        cy.get(BaseSelectors.ComboBox).eq(1).within(() => {
            BaseAssertion.AssertElementNotExist(BaseSelectors.SelectedComboboxItem);
        });
        cy.get(BaseSelectors.ComboBox).eq(2).within(() => {
            BaseAssertion.AssertElementNotExist(BaseSelectors.SelectedComboboxItem);
        });
    });
}
//#endregion

//#region Quote Template
export function FillQuoteTemplateName(quoteTemplateName: string) {
    let NameToFill = quoteTemplateName.toLowerCase() == "random" ? ("Quote_" + GenerateRandomNumber(4)) : quoteTemplateName;
    QuoteTemplateContext.Name = NameToFill
    cy.FillLogTextBox(MaintenanceSelectors.QuoteTemplateName, QuoteTemplateContext.Name);
}

export function CreateQuoteTemplate() {
    DefinePostQuoteTemplateRequest();
    DefineQuoteTemplatetGetSingleRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function OpenQuoteTemplateSection(section: string) {
    cy.Click(MaintenanceSelectors.QuoteTemplateSectionsButton(section), null)
}

export function OpenQuoteHeaderSection() {
    DefineGetQuoteTemplateHeaderFieldByQuoteTemplateId()
    DefineGetQuoteTemplateTextDesignPMListByIds()
    cy.Click(MaintenanceSelectors.QuoteTemplateSectionsButton("Quote Header"), null)
}

function DefineGetQuoteTemplateHeaderFieldByQuoteTemplateId() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetQuoteTemplateHeaderFieldByQuoteTemplateId, RequestAliases.GetQuoteTemplateHeaderFieldByQuoteTemplateId);
}

function DefineGetQuoteTemplateTextDesignPMListByIds() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetQuoteTemplateTextDesignPMListByIds, RequestAliases.GetQuoteTemplateTextDesignPMListByIds);
}

export function AssertGetQuoteHeader() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetQuoteTemplateHeaderFieldByQuoteTemplateId, 200);
    BaseAssertion.AssertStatusCode(RequestAliases.GetQuoteTemplateTextDesignPMListByIds, 200);
}

export function OpenQuoteDetailsSection() {
    DefineGetQuoteTemplateDetailsFieldByQuoteTemplateId()
    DefineGetQuoteTemplateTextDesignPMListByIds()
    cy.Click(MaintenanceSelectors.QuoteTemplateSectionsButton("Quote Details"), null)
}

function DefineGetQuoteTemplateDetailsFieldByQuoteTemplateId() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetQuoteTemplateDetailsFieldByQuoteTemplateId, RequestAliases.GetQuoteTemplateDetailsFieldByQuoteTemplateId);
}

export function AssertGetQuoteDetails() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetQuoteTemplateDetailsFieldByQuoteTemplateId, 200);
    BaseAssertion.AssertStatusCode(RequestAliases.GetQuoteTemplateTextDesignPMListByIds, 200);
}

export function FillQuoteHeaderFooterColumnWidth(coulmnWidthDetails: QuoteTemplateDetails) {
    cy.FillLogTextBox(MaintenanceSelectors.HeaderFooterColumnWidth("1"), coulmnWidthDetails.Width1)
    cy.FillLogTextBox(MaintenanceSelectors.HeaderFooterColumnWidth("2"), coulmnWidthDetails.Width2)
    cy.FillLogTextBox(MaintenanceSelectors.HeaderFooterColumnWidth("3"), coulmnWidthDetails.Width3)
}

export function ValidateWidthErrorMesseage() {
    BaseAssertion.AssertElementNotExist(MaintenanceSelectors.ValidationSummary)
}

export function DragAndDropFields(fieldDetails: QuoteTemplateDetails[]) {
    for (let i = 0; i < fieldDetails.length; i++) {
        cy.get(MaintenanceSelectors.AvaliableColumnsFields(fieldDetails[i].Field)).drag(MaintenanceSelectors.ColumnDropArea(fieldDetails[i].Column))
    }
}

export function EditLabelField(labelToEdit: string, newFieldValue: string) {
    if (labelToEdit == "Shipper Name") {
        EditShipperNameLabelField(labelToEdit, newFieldValue)
    }
    else if (labelToEdit == "Customer") {
        EditCustomerLabelField(labelToEdit, newFieldValue)
    }
}

export function EditCustomerLabelField(labelToEdit: string, newFieldValue: string) {
    cy.Navigate(MaintenanceSelectors.QuoteSettingsLabel);
    cy.Click(MaintenanceSelectors.LabelDiv(labelToEdit), null);
    cy.get(MaintenanceSelectors.LabelTextBox(labelToEdit)).type(newFieldValue);
}

export function EditShipperNameLabelField(labelToEdit: string, newFieldValue: string) {
    cy.Navigate(MaintenanceSelectors.QuoteSettingsLabel);
    cy.get(BaseSelectors.TextTrimming).contains(labelToEdit).type(newFieldValue);
}
export function AddDataFieldToIntroduction(fieldToBeAdd: string) {
    cy.Click(MaintenanceSelectors.AddDataField, null)
    cy.Click(MaintenanceSelectors.IntroductionDataField(fieldToBeAdd), null)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AddColumnsToPricingTable(coulmnsList: QuoteTemplateDetails[]) {
    for (let i = 0; i < coulmnsList.length; i++) {
        cy.SelectCheckBox(MaintenanceSelectors.PricingCheckBox(coulmnsList[i].Column))
    }
}

export function OpenQuoteTemplate() {
    SearchQuoteTemplate()
    DefineQuoteTemplatetGetSingleRequest()
    cy.get(BaseSelectors.ListDataLoaded)
    cy.get(BaseSelectors.RowClass).eq(0).click({ force: true });
    AssertOpenQuoteTemplate();
}

export function ReopenQuoteTemplate() {
    DefineQuoteTemplatetGetSingleRequest()
    cy.get(BaseSelectors.RowClass).eq(0).click()
    AssertOpenQuoteTemplate();
}

export function UpdateQuoteTemplate() {
    DefinePutQuoteTemplateRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePutQuoteTemplateRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.QuoteTemplatetextdesigns, RequestAliases.PutQuoteTemplate);
}

export function UpdateQuoteHeaderTemplate() {
    DefineQuoteTemplatetPutTextDesignRequest();
    DefineQuoteTemplatetPutHeaderFieldsRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefineQuoteTemplatetPutTextDesignRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.PutQuoteTemplateTextDesignPMs, RequestAliases.PutQuoteTemplateTextDesignPMs);
}

function DefineQuoteTemplatetPutHeaderFieldsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.PutQuoteTemplateHeaderFields, RequestAliases.PutQuoteTemplateHeaderFields);
}

export function UpdateQuoteDetailsTemplate() {
    DefineQuoteTemplatetPutTextDesignRequest();
    DefinePutQuoteTemplateDetailsFieldsRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePutQuoteTemplateDetailsFieldsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.PutQuoteTemplateDetailsFields, RequestAliases.PutQuoteTemplateDetailsFields);
}

export function UpdateQuotePricingTemplate() {
    DefineQuoteTemplatetPutSettingsRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefineQuoteTemplatetPutSettingsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.QuotetemplateSettings, RequestAliases.PutQuoteTemplate);
}

export function UpdateQuoteIntroductionTemplate() {
    DefineQuoteTemplatetPutSectionsRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefineQuoteTemplatetPutSectionsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Quotetemplatesections, RequestAliases.PutQuoteTemplate);
}

export function AssertCreateQuoteTemplate() {
    AssertPostQuoteTemplate();
    AssertQuoteTemplatetGetSingle();
}

export function AssertUpdateQuoteTemplate() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutQuoteTemplate, 200);
}

export function AssertUpdateQuoteHeaderTemplate() {
    AssertQuoteTemplatetPutTextDesign();
    AssertQuoteTemplatetPutHeaderFields();
}

function AssertQuoteTemplatetPutTextDesign() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutQuoteTemplateTextDesignPMs, 200);
}

function AssertQuoteTemplatetPutHeaderFields() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutQuoteTemplateHeaderFields, 200);
}

export function AssertUpdateQuoteDetailsTemplate() {
    AssertQuoteTemplatetPutTextDesign();
    AssertPutQuoteTemplateDetailsFields();
}

function AssertPutQuoteTemplateDetailsFields() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutQuoteTemplateDetailsFields, 200);
}

function SearchQuoteTemplate() {
    let quoteTemplateName = QuoteTemplateContext.Name;
    DefineQuoteTemplateViewsGetByFiltersRequest(quoteTemplateName);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, quoteTemplateName);
    AssertQuoteTemplateViewsGetByFilters();
}

function AssertOpenQuoteTemplate() {
    AssertQuoteTemplatetGetSingle();
    BaseAssertion.AssertElementExist(".SectionBody")
}

function DefinePostQuoteTemplateRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.QuoteTemplateExtended, RequestAliases.PostQuoteTemplate);
}

function DefineQuoteTemplatetGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.QuoteTemplateGetSingle, RequestAliases.GetSignle);
}

function DefineQuoteTemplateViewsGetByFiltersRequest(quoteTemplateName: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(quoteTemplateName), RequestAliases.GetFilterSearch);
}

function AssertPostQuoteTemplate() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostQuoteTemplate, 200).then((interception) => {
        let responseBody = interception.response.body;
        QuoteTemplateContext.QuoteTemplateSettingId = responseBody.QuoteTemplateSettingId;
    });
}

function AssertQuoteTemplatetGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

function AssertQuoteTemplateViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}
//#endregion

//#region Invoice Settings
export function ChangeInvoiceSettings(invoiceSettings: InvoiceSettingsDetails) {
    if (invoiceSettings.VoidInvoice.toUpperCase() == Constants.Allowed) {
        cy.get(MaintenanceSelectors.VoidinvoiceCheckBox).check({ force: true })
    }
    else if (invoiceSettings.VoidInvoice.toUpperCase() == Constants.NotAllowed) {
        cy.get(MaintenanceSelectors.VoidinvoiceCheckBox).uncheck({ force: true })
    }
}

export function UpdateInvoiceSettings() {
    DefinePutAccountingSettingsRequest();
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePutAccountingSettingsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.AccountingSettings, RequestAliases.AccountingSettings);
}

export function AssertUpdateInvoiceSettings() {
    AssertPutAccountingSettings();
}

function AssertPutAccountingSettings() {
    BaseAssertion.AssertStatusCode(RequestAliases.AccountingSettings, 200).then((interception) => {
        InvoiceSettingsDetails.AllowVoidARI = interception.request.body.allowVoidARI
    });
}

export function AssertVoidInvoiceMessage(Message: string) {
    cy.get(BaseSelectors.MessageWindow).should('contain.text', Message)
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsOK)
}
//#endregion
//#region Customer Settings
export function FillCustomerSettingsDetails(customerSettingsDetails: CustomerSettingsDetails) {
    FillCheckBoxProcess(MaintenanceSelectors.IsCustomerTelephoneRequiredCheckBox, customerSettingsDetails.IsCustomerTelphoneRequired)
    FillCheckBoxProcess(MaintenanceSelectors.IsPotentialCustomerTelephoneRequiredCheckBox, customerSettingsDetails.IsPotentialCustomerTelphoneRequired)
    FillCheckBoxProcess(MaintenanceSelectors.IsCustomerFaxRequiredCheckBox, customerSettingsDetails.IsCustomerFaxRequired)
    FillCheckBoxProcess(MaintenanceSelectors.IsPotentialCustomerFaxRequiredCheckBox, customerSettingsDetails.IsPotentialCustomerFaxRequired)
    FillCheckBoxProcess(MaintenanceSelectors.IsCustomerAddress1RequiredCheckBox, customerSettingsDetails.IsCustomerAddress1Required)
}

export function UpdateCustomerSettings() {
    DefinePutCustomerSettings()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK)
}

function DefinePutCustomerSettings() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Tenants, RequestAliases.Tenants);
}

export function AssertPutCustomerSettings() {
    BaseAssertion.AssertStatusCode(RequestAliases.Tenants, 200);
}
//#endregion

//#region Customer in CRM
export function NavigatesToCustomerCRMWorkspace() {
    cy.Click(BaseSelectors.CRMMenu, null)
    cy.Click(BaseSelectors.CRMCustomers, null)
}
export function OpenNewPotentialCustomerWizard() {
    cy.Click(MaintenanceSelectors.NewCustomerButton, null);
}
export function FillPotentialCustomerDetails(customerDetails: CustomerDetails) {
    cy.FillLogTextBox(MaintenanceSelectors.PotentialCustomerName, customerDetails.CompanyName)
    cy.FillLogTextBox(MaintenanceSelectors.PotentialCustomerCity, customerDetails.City)
    cy.FillLogLov(MaintenanceSelectors.PotentialCustomerCountry, customerDetails.Country, true)
    cy.FillLogLov(MaintenanceSelectors.PotentialCustomerState, customerDetails.State, true)
    cy.FillLogTextBox(MaintenanceSelectors.PotentialCustomerPhoneNumber, customerDetails.PhoneNumber)
    cy.FillLogTextBox(MaintenanceSelectors.PotentialCustomerFaxNumber, customerDetails.FaxNumber)
    cy.FillLogTextBox(MaintenanceSelectors.PotentialCustomerAddress1, customerDetails.Address1)
    FillCheckBoxProcess(MaintenanceSelectors.PotentialCustomerAddContactCheckBox, customerDetails.AddContact)
}

export function AddPotentialCustomer() {
    DefinePostPotentialCustomer()
    cy.Click(MaintenanceSelectors.OkAddPotentialCustomer, null);
}
function DefinePostPotentialCustomer() {
    cy.DefineRequestWait(RestAPI.POST, Urls.PartnersDomain, RequestAliases.PartnersDomainRequest)
}
export function AssertAddPotentialCustomer() {
    BaseAssertion.AssertStatusCode(RequestAliases.PartnersDomainRequest, 200).then((interception) => {
        CustomerDetails.Code = interception.response.body.Customer.Code;
    });
}
//#endregion

//#region Customer
export function SearchCustomer() {
    var quickSearchDetails = {
        Selector: MaintenanceSelectors.CustomerSearchBar,
        Parent: MaintenanceSelectors.CustomerSearchParent,
        ParentClass: MaintenanceSelectors.CustomerSearchParentClass,
        WaitURL: Urls.GetCustomersQuickSearch(CustomerDetails.Code),
        Value: CustomerDetails.Code,
        RequestAliase: RequestAliases.GetCustomersQuickSearch
    } as QuickSearchDetails;
    cy.SelectQuickSearchFirstElement(quickSearchDetails);
}

export function AssertCustomerStatus(CustomerStatus: string) {
    BaseAssertion.AssertElementContain(BaseSelectors.HeaderScreen, CustomerStatus)
}

export function FillCustomerActivationWindow(customerDetails: CustomerDetails) {
    FillPotentialCustomerDetails(customerDetails);
}

export function ActivateCustomer() {
    DefinePutCustomer()
    cy.Click(MaintenanceSelectors.OKActivateCustomer, null);
}

export function AssertActivateCustomer() {
    BaseAssertion.AssertStatusCode(RequestAliases.Customers, 200);
}

function DefinePutCustomer() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Customers, RequestAliases.Customers)
}
//#endregion

//#region Change password
export function OpenChangeUserPasswordWindow() {
    OpenPersonalSettingsTab();
    cy.Click(MaintenanceSelectors.ChangePasswordMaintenanceItem, null)
}

function OpenPersonalSettingsTab() {
    OpenMaintenanceMenu();
    cy.Click(MaintenanceSelectors.PersonalSettingsMaintenanceTab, null)
}
//#endregion

//#region Country
export function FillCountryDetails(countryDetails: CountryDetails) {
    cy.FillLogTextBox(MaintenanceSelectors.CountryCode, countryDetails.CountryCode);
    cy.FillLogTextBox(MaintenanceSelectors.CountryEnglishName, countryDetails.CountryName);
    cy.FillLogTextBox(MaintenanceSelectors.CountryLocalName, countryDetails.CountryLocalName);
    cy.FillLogLov(MaintenanceSelectors.CountryGlobalZone, countryDetails.CountryGlobalZone, true);
    FillCheckBoxProcess(MaintenanceSelectors.InActiveCountryCheckBox, countryDetails.InactiveCountry);
    FillCheckBoxProcess(MaintenanceSelectors.CountryECCheckBox, countryDetails.EC);
    FillCheckBoxProcess(MaintenanceSelectors.CountryIsNorthAmericaCheckBox, countryDetails.NorthAmerica);
    FillCheckBoxProcess(MaintenanceSelectors.CountryIsStateRequiredCheckBox, countryDetails.IsStateRequired);
    FillCheckBoxProcess(MaintenanceSelectors.CountryHasCitiesCheckBox, countryDetails.HasCities);
    cy.FillLogTextBox(MaintenanceSelectors.CountryNotes, countryDetails.Notes);

}

export function FillRandomCountryLocalName(LocalName: string) {
    let randomLocalName = LocalName.toLowerCase() == "random" ? (GenerateRandomNumber(10)) : LocalName;
    cy.FillLogTextBox(MaintenanceSelectors.CountryLocalName, randomLocalName)
}

export function FillCountryCode(CountryCode: string) {
    cy.FillLogTextBox(MaintenanceSelectors.CountryCode, CountryCode)
}

export function CreateCountry() {
    DefinePostCountryMockRequest()
    DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePostCountryMockRequest() {
    cy.intercept(RestAPI.POST, Urls.Countries, [true])
}

export function AssertCreateCountry() {
    AssertMockPostCountry();
    AssertGetByFilters();
}

export function AssertMockPostCountry() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow)
}

export function SearchCountryByCode(CountryCode: string) {
    SearchCardByFilter(CountryCode, MaintenanceSelectors.CountryCodeFilterCheckBox)
}
export function AssertSearchCountry(CountryCode: string) {
    cy.wait(1000)
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(CountryCode);
    });
}

export function DefineCountryViewsGetByFiltersRequest(CountryCode: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(CountryCode), RequestAliases.GetFilterSearch);
}

export function AssertCountryViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function OpenCountry() {
    DefineCountriesGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

export function AssertOpenCountry() {
    AssertCountrieGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function DefineCountriesGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.CountriesGetSingle, RequestAliases.GetSignle);
}

function AssertCountrieGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function EditCountry() {
    DefinePutCountryRequest();
    cy.Click(MaintenanceSelectors.CountrySaveButton, null);
}

function DefinePutCountryRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Countries, RequestAliases.PutCountry);
}

export function AssertEditCountry() {
    AssertPutCountry();
}
export function AssertPutCountry() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutCountry, 200).
        then((interception) => {
            inActiveCountry = interception.response.body.InActive;
        });
}

export function CountryConversionEventsMapping(eventDetailsList: EventTypeDetails[]): EventTypeDetails[] {
    ConversionEventsMapping(eventDetailsList, inActiveCountry)
    return eventDetailsList;
}

export function ValidateErrorPopUpMessage(Message: string) {
    cy.get(BaseSelectors.ErrorPopUp).should("contain.text", Message)

}
//#endregion

//#region State
export function FillStateDetails(stateDetails: StateDetails) {
    var RandomStateNumber = GenerateRandomNumber(10);
    cy.FillLogTextBox(MaintenanceSelectors.StateCode, stateDetails.StateCode.toLowerCase() == "random" ? RandomStateNumber : stateDetails.StateCode)
    cy.FillLogTextBox(MaintenanceSelectors.StateEnglishName, stateDetails.StateName)
    cy.FillLogTextBox(MaintenanceSelectors.StateLocalName, stateDetails.StateLocalName)
    cy.FillLogLov(MaintenanceSelectors.StateCountry, stateDetails.Country, true)
    FillCheckBoxProcess(MaintenanceSelectors.InActiveStateCheckBox, stateDetails.InactiveState)
    cy.FillLogTextBox(MaintenanceSelectors.StateNotes, stateDetails.Notes)
}

export function CreateState() {
    DefinePostStateRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePostStateRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.States, RequestAliases.PostState)
}

export function AssertCreateState() {
    let intercept = cy.wait("@" + RequestAliases.PostState);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            ReCreateState();
        }
        else {
            AssertPostState(interception.response.statusCode, 200, interception.response.body.Code)
        }
    })
}

function ReCreateState() {
    let stateCode = GenerateRandomNumber(10);
    cy.FillLogTextBox(MaintenanceSelectors.StateCode, stateCode)
    cy.FillLogTextBox(MaintenanceSelectors.StateEnglishName, stateCode)
    cy.FillLogTextBox(MaintenanceSelectors.StateLocalName, stateCode)
    CreateState();
    AssertCreateState();
}

export function AssertPostState(responseStatusCode: number, expectedStatusCode: number, stateCode: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    StateCode = stateCode
}

export function SearchState() {
    DefineStateViewsGetByFiltersRequest(StateCode);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, StateCode);
    AssertStateViewsGetByFilters();
}

export function DefineStateViewsGetByFiltersRequest(StateName: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(StateName), RequestAliases.GetFilterSearch);
}

export function AssertSearchState() {
    cy.get(BaseSelectors.ListDataLoaded)
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(StateCode);
    });
}

export function AssertStateViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function OpenState() {
    DefineStatesGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

export function AssertOpenState() {
    AssertStatesGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function DefineStatesGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.StatesGetSingle, RequestAliases.GetSignle);
}

function AssertStatesGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function FillStateLocalName(LocalName: string) {
    cy.FillLogTextBox(MaintenanceSelectors.StateLocalName, LocalName)
}

export function EditState() {
    DefinePutStateRequest();
    cy.Click(MaintenanceSelectors.StateSaveButton, null);
}

function DefinePutStateRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.States, RequestAliases.PutState);
}

export function AssertEditState() {
    AssertPutState();
}

export function AssertPutState() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutState, 200).
        then((interception) => {
            inActiveState = interception.response.body.InActive;
        });
}
//#endregion

//#region Common 
export function ChangeInactiveCheckBoxValue(InActivateSelector: string) {
    cy.get(InActivateSelector).then($InActiveStatesCheckBox => {
        if ($InActiveStatesCheckBox.is(':checked')) {
            cy.get(InActivateSelector).uncheck({ force: true })
        }
        else {
            cy.get(InActivateSelector).check({ force: true })
        }
    })
}
export function SearchCardByFilter(FilterValue: string, FilterTypeSelector: string) {
    cy.Click(MaintenanceSelectors.CardFiltersOpen, null);
    cy.Click(MaintenanceSelectors.CardAddFilterBtn, null);
    cy.get(FilterTypeSelector).then($InActiveStatesCheckBox => {
        if (!($InActiveStatesCheckBox.is(':checked'))) {
            cy.get(FilterTypeSelector).check({ force: true });
        }
        FillFilterValue(FilterValue);
    })
}
export function DefineCardGetByFiltersRequest(ShipmentSubTypeCode: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(ShipmentSubTypeCode), RequestAliases.GetFilterSearch);
}
function FillFilterValue(FilterValue: string) {
    DefineCountryViewsGetByFiltersRequest(FilterValue);
    cy.FillLogTextBox(MaintenanceSelectors.CardCodeFilterTextValue, FilterValue);
    AssertListViewsGetByFilters();
}
export function AssertListViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}
export function FillCheckBoxProcess(CheckBoxSelector: string, IsCheck: string) {
    if (IsCheck) {
        if (IsCheck.toUpperCase() == constants.YES) {
            cy.get(CheckBoxSelector).check({ force: true })
        }
        else {
            cy.get(CheckBoxSelector).find(BaseSelectors.input).uncheck({ force: true })
        }
    }
}
export function FillInputCheckBoxProcess(CheckBoxSelector: string, IsCheck: string) {
    if (IsCheck) {
        if (IsCheck.toUpperCase() == constants.YES) {
            cy.get(CheckBoxSelector).check({ force: true })
        }
        else {
            cy.get(CheckBoxSelector).uncheck({ force: true })
        }
    }
}

export function DefineGetByFilterRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetByFilter, RequestAliases.GetByFilter);
}

export function AssertGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetByFilter, 200);
}
function FillCodeFilterValue(Code: string) {
    DefineCountryViewsGetByFiltersRequest(Code);
    cy.FillLogTextBox(MaintenanceSelectors.CardCodeFilterTextValue, Code);
    AssertCountryViewsGetByFilters();
}
//#endregion

//#region city
export function FillCityDetails(cityDetails: CityDetails) {
    var RandomCityNumber = GenerateRandomNumber(15);
    cy.FillLogTextBox(MaintenanceSelectors.CityCode, cityDetails.CityCode.toLowerCase() == "random" ? RandomCityNumber : cityDetails.CityCode)
    cy.FillLogTextBox(MaintenanceSelectors.CityEnglishName, cityDetails.CityName)
    cy.FillLogTextBox(MaintenanceSelectors.CityLocalName, cityDetails.CityLocalName)
    cy.FillLogLov(MaintenanceSelectors.CityCountry, cityDetails.Country, true)
    cy.FillLogLov(MaintenanceSelectors.CityState, cityDetails.Country, true)
    FillCheckBoxProcess(MaintenanceSelectors.InActiveCityCheckBox, cityDetails.InactiveCity)
    cy.FillLogTextBox(MaintenanceSelectors.CityNotes, cityDetails.Notes)
}

export function CreateCity() {
    DefinePostCityRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePostCityRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.CountryCities, RequestAliases.PostCity);
}
export function AssertCreateCity() {
    let intercept = cy.wait("@" + RequestAliases.PostCity);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            ReCreateCity();
        }
        else {
            AssertPostCity(interception.response.statusCode, 200, interception.response.body.Code)
        }
    })
}
function ReCreateCity() {
    let cityCode = GenerateRandomNumber(15);
    cy.FillLogTextBox(MaintenanceSelectors.CityCode, cityCode)
    cy.FillLogTextBox(MaintenanceSelectors.CityEnglishName, cityCode)
    cy.FillLogTextBox(MaintenanceSelectors.CityLocalName, cityCode)
    CreateCity();
    AssertCreateCity();
}
export function AssertPostCity(responseStatusCode: number, expectedStatusCode: number, cityCode: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    CityCode = cityCode
}
export function SearchCity() {
    DefineCityViewsGetByFiltersRequest(CityCode);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, CityCode);
    AssertCitiesViewsGetByFilters();
}
export function AssertCitiesViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}
export function AssertSearchCity() {
    cy.get(BaseSelectors.ListDataLoaded)
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(CityCode);
    });
}

export function DefineCityViewsGetByFiltersRequest(CityName: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(CityName), RequestAliases.GetFilterSearch);
}
export function OpenCity() {
    DefineCountryCitiesGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}
function DefineCountryCitiesGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.CountryCitiesGetSingle, RequestAliases.GetSignle);
}
export function AssertOpenCity() {
    AssertCountryCitiesGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function AssertCountryCitiesGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}
export function FillCityLocalName(LocalName: string) {
    cy.FillLogTextBox(MaintenanceSelectors.CityLocalName, LocalName)
}
export function EditCity() {
    DefinePutCityRequest();
    cy.Click(MaintenanceSelectors.CitySaveButton, null);
}

function DefinePutCityRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.CountryCities, RequestAliases.PutCity);
}
export function AssertPutCity() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutCity, 200).
        then((interception) => {
            inActiveCity = interception.response.body.InActive;
        });
}
export function ConversionEventsMapping(eventDetailsList: EventTypeDetails[], inActiveField: boolean) {
    for (let i = 0; i < eventDetailsList.length; i++) {
        if (inActiveField) {
            eventDetailsList[i].Notes = eventDetailsList[i].Notes.replace(/\"status\"/gi, "Inactivated");
        }
        else {
            eventDetailsList[i].Notes = eventDetailsList[i].Notes.replace(/\"status\"/gi, "Activated");
        }
    }
}
//#endregion

//#region Global Zone
export function FillGlobalZoneCode(GlobalZoneCode: string) {
    cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneCode, GlobalZoneCode)
}

export function FillGlobalZoneDetails(globalZoneDetails: GlobalZoneDetails) {
    var RandomGlobalZoneNumber = GenerateRandomNumber(8);
    cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneCode, globalZoneDetails.GlobalZoneCode.toLowerCase() == "random" ? RandomGlobalZoneNumber : globalZoneDetails.GlobalZoneCode)
    cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneEnglishName, globalZoneDetails.GlobalZoneName)
    cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneLocalName, globalZoneDetails.GlobalZoneName)
    FillCheckBoxProcess(MaintenanceSelectors.InActiveGlobalZoneCheckBox, globalZoneDetails.InactiveGlobalZone)
}

export function CreateGlobalZone() {
    DefinePostGlobalZoneRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePostGlobalZoneRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.GlobalZones, RequestAliases.PostGlobalZone)
}

export function AssertCreateGlobalZone() {
    let intercept = cy.wait("@" + RequestAliases.PostGlobalZone);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            ReCreateGlobalZone();
        }
        else {
            AssertPostGlobalZone(interception.response.statusCode, 200, interception.response.body.Code)
        }
    })
}

function ReCreateGlobalZone() {
    let globalZoneCode = GenerateRandomNumber(8);
    cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneCode, globalZoneCode)
    cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneEnglishName, globalZoneCode)
    cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneLocalName, globalZoneCode)
    CreateGlobalZone();
    AssertCreateGlobalZone();
}

export function AssertPostGlobalZone(responseStatusCode: number, expectedStatusCode: number, globalZoneCode: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    GlobalZoneCode = globalZoneCode
}

export function SearchGlobalZone() {
    DefineGlobalZoneViewsGetByFiltersRequest(GlobalZoneCode);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, GlobalZoneCode);
    AsserGlobalZoneViewsGetByFilters();
}

export function DefineGlobalZoneViewsGetByFiltersRequest(GlobalZoneName: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(GlobalZoneName), RequestAliases.GetFilterSearch);
}
export function AsserGlobalZoneViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function AssertSearchGlobalZone() {
    cy.get(BaseSelectors.ListDataLoaded)
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(GlobalZoneCode);
    });
}

export function OpenGlobalZone() {
    DefineGlobalZonesGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

function DefineGlobalZonesGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GlobalZonesGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenGlobalZone() {
    AssertGlobalZoneGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function AssertGlobalZoneGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function FillGlobalZoneLocalName(LocalName: string) {
    cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneLocalName, LocalName)
}

export function EditGlobalZone() {
    DefinePutGlobalZoneRequest();
    cy.Click(MaintenanceSelectors.GlobalZoneSaveButton, null);
}

function DefinePutGlobalZoneRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.GlobalZones, RequestAliases.PutGlobalZone);
}

export function AssertEditGlobalZone() {
    AssertPutGlobalZone();
}

export function AssertPutGlobalZone() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutGlobalZone, 200).
        then((interception) => {
            inActiveGlobalZone = interception.response.body.InActive;
        });
}

//#endregion


//#region Commodity
export function FillCommodityCode(CommodityCode: string) {
    cy.FillLogTextBox(MaintenanceSelectors.CommodityCode, CommodityCode)
}
export function FillCommodityDetails(commodityDetails: CommodityDetails) {
    var RandomCommodityNumber = GenerateRandomNumber(15);
    cy.FillLogTextBox(MaintenanceSelectors.CommodityCode, commodityDetails.CommodityCode.toLowerCase() == "random" ? RandomCommodityNumber : commodityDetails.CommodityCode)
    cy.FillLogTextBox(MaintenanceSelectors.CommodityName, commodityDetails.CommodityName.toLowerCase() == "random" ? RandomCommodityNumber : commodityDetails.CommodityName)
    FillInputCheckBoxProcess(MaintenanceSelectors.InActiveCommodityCheckBox, commodityDetails.InactiveCommodity)
}

export function CreateCommodity() {
    DefinePostCommodityRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePostCommodityRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Commodities, RequestAliases.PostCommodity)
}

export function AssertCreateCommodity() {
    let intercept = cy.wait("@" + RequestAliases.PostCommodity);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            ReCreateCommodity();
        }
        else {
            AssertPostCommodity(interception.response.statusCode, 200, interception.response.body.Name)
        }
    })
}
function ReCreateCommodity() {
    var RandomCommodityNumber = GenerateRandomNumber(15);
    cy.FillLogTextBox(MaintenanceSelectors.CommodityCode, RandomCommodityNumber)
    cy.FillLogTextBox(MaintenanceSelectors.CommodityName, RandomCommodityNumber)
    CreateCommodity();
    AssertCreateCommodity();
}
export function AssertPostCommodity(responseStatusCode: number, expectedStatusCode: number, commodityName: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    CommodityName = commodityName
}
export function SearchCommodity() {
    DefineCommodityViewsGetByFiltersRequest(CommodityName);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, CommodityName);
    AssertCommodityViewsGetByFilters();
}
export function DefineCommodityViewsGetByFiltersRequest(CommodityName: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(CommodityName), RequestAliases.GetFilterSearch);
}
export function AssertCommodityViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}
export function AssertSearchCommodity() {
    cy.get(BaseSelectors.ListDataLoaded)
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(CommodityName);
    });
}
export function OpenCommodity() {
    DefineCommoditiesGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}
function DefineCommoditiesGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.CommoditiesGetSingle, RequestAliases.GetSignle);
}
export function AssertOpenCommodity() {
    AssertCommoditiesGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function AssertCommoditiesGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}
export function FillCommodityName(Name: string) {
    let NameToFill = Name.toLowerCase() == "random" ? (GenerateRandomNumber(10)) : Name;
    if (Name) {
        cy.FillLogTextBox(MaintenanceSelectors.CommodityName, NameToFill)
    }
}
export function EditCommodity() {
    DefinePutCommodityRequest();
    cy.Click(MaintenanceSelectors.CommoditySaveButton, null);
}

function DefinePutCommodityRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Commodities, RequestAliases.PutCommodity);
}

export function AssertEditCommodity() {
    AssertPutCommodity();
}

export function AssertPutCommodity() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutCommodity, 200).
        then((interception) => {
            inActiveCommodity = interception.response.body.InActive;
        });
}
//#endregion

//#region Region
export function FillRegionDetails(regionDetails: RegionDetails) {
    var RandomRegionName = GenerateRandomNumber(15);
    cy.FillLogTextBox(MaintenanceSelectors.RegionName, regionDetails.RegionName.toLowerCase() == "random" ? RandomRegionName : regionDetails.RegionName)
    cy.FillLogTextBox(MaintenanceSelectors.RegionLocalName, regionDetails.RegionLocalName.toLowerCase() == "random" ? RandomRegionName : regionDetails.RegionLocalName)
    FillInputCheckBoxProcess(MaintenanceSelectors.InActiveRegionCheckBox, regionDetails.InactiveRegion)
}

export function CreateRegion() {
    DefinePostRegionRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePostRegionRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Regions, RequestAliases.PostRegions)
}

export function AssertCreateRegion() {
    let intercept = cy.wait("@" + RequestAliases.PostRegions);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            ReCreateRegion();
        }
        else {
            AssertPostRegion(interception.response.statusCode, 200, interception.request.body.name)
        }
    })
}
function ReCreateRegion() {
    var RandomRegionName = GenerateRandomNumber(15);
    cy.FillLogTextBox(MaintenanceSelectors.RegionName, RandomRegionName)
    cy.FillLogTextBox(MaintenanceSelectors.RegionLocalName, RandomRegionName)
    CreateRegion();
    AssertCreateRegion();
}
export function AssertPostRegion(responseStatusCode: number, expectedStatusCode: number, regionName: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    RegionName = regionName
}
export function SearchRegion() {
    DefineRegionViewsGetByFiltersRequest(RegionName);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, RegionName);
    AssertRegionViewsGetByFilters();
}
export function DefineRegionViewsGetByFiltersRequest(CommodityName: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(CommodityName), RequestAliases.GetFilterSearch);
}
export function AssertRegionViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}
export function AssertSearchRegion() {
    cy.get(BaseSelectors.ListDataLoaded)
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(RegionName);
    });
}
export function OpenRegion() {
    DefineRegionsGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}
function DefineRegionsGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.RegionsGetSingle, RequestAliases.GetSignle);
}
export function AssertOpenRegion() {
    AssertRegionsGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function AssertRegionsGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}
export function FillRegionLocalName(Name: string) {
    let NameToFill = Name.toLowerCase() == "random" ? (GenerateRandomNumber(15)) : Name;
    if (Name) {
        cy.FillLogTextBox(MaintenanceSelectors.RegionLocalName, NameToFill)
    }
}
export function EditRegion() {
    DefinePutRegionRequest();
    cy.Click(MaintenanceSelectors.RegionSaveButton, null);
}

function DefinePutRegionRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Regions, RequestAliases.PutRegions);
}

export function AssertEditRegion() {
    AssertPutRegion();
}

export function AssertPutRegion() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutRegions, 200).
        then((interception) => {
            inActiveRegion = interception.response.body.inActive;
        });
}
//#endregion

//#region card
export function getCardCode() {
    return CardCode;
}
export function FillCardDetails(cardDetails: CardDetails, codeDigits: number) {
    var RandomCardCode = GenerateRandomNumber(codeDigits);
    if (cardDetails.Code) {
        cy.FillLogTextBox(MaintenanceSelectors.CardCode, cardDetails.Code.toLowerCase() == "random" ? RandomCardCode : cardDetails.Code)
    }
    cy.FillLogTextBox(MaintenanceSelectors.CardCompanyName, cardDetails.CompanyName);
    cy.FillLogTextBox(MaintenanceSelectors.CardPhone, cardDetails.Phone);
    cy.FillLogTextBox(MaintenanceSelectors.CardLocalName, cardDetails.LocalName);
    cy.FillLogTextBox(MaintenanceSelectors.CardFax, cardDetails.Fax);
    cy.FillLogTextBox(MaintenanceSelectors.CardAddress1, cardDetails.Address1);
    cy.FillLogTextBox(MaintenanceSelectors.CardZipCode, cardDetails.Zip);
    cy.FillLogTextBox(MaintenanceSelectors.CardCity, cardDetails.City);
    cy.FillLogLov(MaintenanceSelectors.CardCountry, cardDetails.Country, true);
    cy.FillLogLov(MaintenanceSelectors.CardState, cardDetails.State, true);
}
export function FillCardContactDetails(cardConatactDetails: ContactDetails) {
    FillCheckBoxProcess(MaintenanceSelectors.CardContactCheckBox + BaseSelectors.LastElement, cardConatactDetails.AddContact)
    cy.FillLogTextBox(MaintenanceSelectors.CardContactEnglishName, cardConatactDetails.EnglishName);
    cy.FillLogTextBox(MaintenanceSelectors.CardContactPosition, cardConatactDetails.Position);
    cy.FillLogTextBox(MaintenanceSelectors.CardContactBusinessPhone, cardConatactDetails.BusinessPhone);
    cy.FillLogTextBox(MaintenanceSelectors.CardContactMobile, cardConatactDetails.Mobile);
    cy.FillLogTextBox(MaintenanceSelectors.CardContactFax, cardConatactDetails.Fax);
}
export function CreateCard() {
    DefinePostCardRequest()
    DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePostCardRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.PartnersDomain, RequestAliases.PostCard);
}

export function AssertCreateCard(CardType: string) {
    AssertPostCard(CardType)
    AssertGetByFilters()
}

export function AssertPostCard(CardType: string) {
    BaseAssertion.AssertStatusCode(RequestAliases.PostCard, 200).then((interception) => {
        let responseBody = interception.response.body;
        if (CardType == Constants.ShippingAgent) {
            CardCode = responseBody.ShippingAgent.Code;
        }
        else if (CardType == Constants.CustomAgent) {
            CardCode = responseBody.CustomAgent.Code;
        }
        else if (CardType == Constants.Vendor) {
            CardCode = responseBody.Vendor.Code;
        }
        else if (CardType == Constants.Agent) {
            CardCode = responseBody.Agent.Code;
        }
        else if (CardType == Constants.Customer) {
            CardCode = responseBody.Customer.Code;
        }
    });
}

export function CreateCardMockCreate() {
    DefinePostCardMockRequest()
    DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function DefinePostCardMockRequest() {
    cy.intercept(RestAPI.POST, Urls.PartnersDomain, [true])
}
export function AssertCreateCardMockCreate() {
    AssertMockPostCard();
    AssertGetByFilters();
}
export function AssertMockPostCard() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow)
}

export function SearchCard() {
    let cardCode = CardCode
    SearchCardByValue(cardCode)
}

export function SearchCardByValue(Card: string) {
    DefineCardViewsGetByFiltersRequest(Card);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, Card);
    AssertCardViewsGetByFilters();
}
function DefineCardViewsGetByFiltersRequest(CardCode: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(CardCode), RequestAliases.GetFilterSearch);
}
function AssertCardViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function AssertSearchCard(companyName: string) {
    cy.get(BaseSelectors.ListDataLoaded)
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(companyName);
    });
}

export function OpenCard(CardType: string) {
    if (CardType == Constants.ShippingAgent) {
        DefineShippingAgentsGetSingleRequest();
    }
    else if (CardType == Constants.Agent) {
        cy.DefineRequestWait(RestAPI.GET, Urls.AgentsGetSingle, RequestAliases.GetSignle);
    }
    else if (CardType == Constants.CustomAgent) {
        DefineCustomAgentsGetSingleRequest()
    }
    else if (CardType == Constants.Vendor) {
        DefineVendorsGetSingleRequest();
    }
    else if (CardType == Constants.Trucker) {
        DefineTruckersGetSingleRequest();
    }
    else if (CardType == Constants.Customer) {
        cy.DefineRequestWait(RestAPI.GET, Urls.CustomerGetSingle, RequestAliases.GetSignle);
    }
    DefineGetMenuButtonGroupsRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}
export function AssertOpenCard() {
    AssertCardGetSingle();
    AssertGetMenuButtonGroups();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}
export function AssertCardGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}
export function AssertCardAddress(cardDetails: CardDetails) {
    BaseAssertion.AssertElementContain(BaseSelectors.TemplateBoxItem, cardDetails.Address1)
    BaseAssertion.AssertElementContain(BaseSelectors.TemplateBoxItem, cardDetails.City)
    BaseAssertion.AssertElementContain(BaseSelectors.TemplateBoxItem, cardDetails.State)
    BaseAssertion.AssertElementContain(BaseSelectors.TemplateBoxItem, cardDetails.Zip)
    BaseAssertion.AssertElementContain(BaseSelectors.TemplateBoxItem, cardDetails.Country)
    BaseAssertion.AssertElementContain(BaseSelectors.TemplateBoxItem, cardDetails.Phone)
    BaseAssertion.AssertElementContain(BaseSelectors.TemplateBoxItem, cardDetails.Fax)
}
export function AssertCardContact(conatactDetails: ContactDetails) {
    BaseAssertion.AssertElementContain(BaseSelectors.TemplateBoxItem, conatactDetails.EnglishName)
    BaseAssertion.AssertElementContain(BaseSelectors.TemplateBoxItem, conatactDetails.BusinessPhone)
    BaseAssertion.AssertElementContain(BaseSelectors.TemplateBoxItem, conatactDetails.Mobile)
    BaseAssertion.AssertElementContain(BaseSelectors.TemplateBoxItem, conatactDetails.Fax)
    BaseAssertion.AssertElementContain(BaseSelectors.TemplateBoxItem, conatactDetails.Position)
}
//#endregion

//#region  ShippingAgents
export function FillShippingAgentsDetails(shippingAgentDetails: CardDetails) {
    FillCardDetails(shippingAgentDetails, null)
}
export function FillShippingAgentsContactDetails(shippingAgentConatactDetails: ContactDetails) {
    FillCardContactDetails(shippingAgentConatactDetails)
}
export function CreateShippingAgent() {
    CreateCard()
}
export function AssertCreateShippingAgent() {
    AssertCreateCard(Constants.ShippingAgent)
}
export function CreateShippingAgentMockCreate() {
    CreateCardMockCreate()
}

export function AssertCreateShippingAgentMockCreate() {
    AssertCreateCardMockCreate()
}

export function UpdateShippingAgent() {
    DefinePutShippingAgentRequest()
    cy.Click(MaintenanceSelectors.ShippingAgentSaveButton, null)
}
function DefinePutShippingAgentRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Shippingagents, RequestAliases.PutShippingAgent);
}

export function SearchShippingAgent() {
    SearchCard()
}

export function AssertSearchShippingAgent(companyName: string) {
    AssertSearchCard(companyName)
}

function DefineShippingAgentsGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.ShippingAgentGetSingle, RequestAliases.GetSignle);
}
export function AssertOpenShippingAgent() {
    AssertOpenCard()
}

export function AssertShippingAgentAddress(shippingAgentDetails: CardDetails) {
    cy.Click(MaintenanceSelectors.ShippingAgentAddressesTab, null, true)
    AssertCardAddress(shippingAgentDetails)
}
export function AssertShippingAgentContact(conatactDetails: ContactDetails) {
    cy.Click(MaintenanceSelectors.ShippingAgentContactsTab, null, true)
    AssertCardContact(conatactDetails)
}
export function FillShippingAgentGenaralTabNotes(Notes: string) {
    cy.Click(MaintenanceSelectors.ShippingAgentGeneralTab, null, true)
    cy.FillLogTextBox(MaintenanceSelectors.ShippingAgentNotes, " ")
    cy.FillLogTextBox(MaintenanceSelectors.ShippingAgentNotes, Notes)
}
export function FillShippingAgentBillingTab(shippingAgentBillingTabDetails: CardBillingTabDetails) {
    cy.Click(MaintenanceSelectors.ShippingAgentBillingTab, null, true)
    cy.FillLogTextBox(MaintenanceSelectors.ShippingAgentBankName, shippingAgentBillingTabDetails.BankName)
    cy.FillLogTextBox(MaintenanceSelectors.ShippingAgentIBANNumber, shippingAgentBillingTabDetails.IBANNo)
}
export function AssertUpdateShippingAgent() {
    AssertPutShippingAgent()
}
function AssertPutShippingAgent() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutShippingAgent, 200);
}
//#endregion
//#region CustomAgents 
export function FillCustomAgentsDetails(customAgentDetails: CardDetails) {
    FillCardDetails(customAgentDetails, null)
}
export function FillCustomAgentsContactDetails(customAgentConatactDetails: ContactDetails) {
    FillCardContactDetails(customAgentConatactDetails)
}
export function CreateCustomAgent() {
    CreateCard()
}
export function AssertCreateCustomAgent() {
    AssertCreateCard(Constants.CustomAgent)
}
export function CreateCustomAgentMockCreate() {
    CreateCardMockCreate()
}

export function AssertCreateCustomAgentMockCreate() {
    AssertCreateCardMockCreate()
}

export function UpdateCustomAgent() {
    DefinePutCustomAgentRequest()
    cy.Click(MaintenanceSelectors.CustomAgentSaveButton, null)
}
function DefinePutCustomAgentRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.CustomAgents, RequestAliases.PutCustomAgent);
}

export function SearchCustomAgent() {
    SearchCard()
}

export function AssertSearchCustomAgent(companyName: string) {
    AssertSearchCard(companyName)
}

function DefineCustomAgentsGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.CustomAgentsGetSingle, RequestAliases.GetSignle);
}
export function AssertOpenCustomAgent() {
    AssertOpenCard()
}

export function AssertCustomAgentAddress(customAgentDetails: CardDetails) {
    cy.Click(MaintenanceSelectors.CustomAgentAddressesTab, null, true)
    AssertCardAddress(customAgentDetails)
}

export function AssertCustomAgentContact(conatactDetails: ContactDetails) {
    cy.Click(MaintenanceSelectors.CustomAgentContactsTab, null, true)
    AssertCardContact(conatactDetails)
}

export function FillCustomAgentGeneralTabNotes(Notes: string) {
    cy.Click(MaintenanceSelectors.CustomAgentGeneralTab, null, true)
    cy.FillLogTextBox(MaintenanceSelectors.CustomAgentNotes, " ")
    cy.FillLogTextBox(MaintenanceSelectors.CustomAgentNotes, Notes)
}

export function FillCustomAgentBillingTab(customAgentBillingTabDetails: CardBillingTabDetails) {
    cy.Click(MaintenanceSelectors.CustomAgentBillingTab, null, true)
    cy.FillLogTextBox(MaintenanceSelectors.CustomAgentBankName, customAgentBillingTabDetails.BankName)
    cy.FillLogTextBox(MaintenanceSelectors.CustomAgentIBANNumber, customAgentBillingTabDetails.IBANNo)
}

export function AssertUpdateCustomAgent() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutCustomAgent, 200);
}

export function FillCardsCityAndCountryFeilds() {
    cy.FillLogTextBox(MaintenanceSelectors.CardCity, "city");
    cy.FillLogLov(MaintenanceSelectors.CardCountry, "AE", true);
}

export function FillTruckerRequiredFeilds() {
    cy.get(MaintenanceSelectors.CardCode).clear()
    cy.FillLogTextBox(MaintenanceSelectors.CardCompanyName, "company");
    cy.FillLogTextBox(MaintenanceSelectors.CardCity, "city");
    cy.FillLogLov(MaintenanceSelectors.CardCountry, "AE", true);
}
//#endregion

//#region  trucker
export function FillTruckerDetails(truckerDetails: CardDetails) {
    FillCardDetails(truckerDetails, 6)
}
export function FillTruckerCode(TruckerCode: string) {
    cy.FillLogTextBox(MaintenanceSelectors.CardCode, TruckerCode)
}
export function FillTruckerContactDetails(truckerConatactDetails: ContactDetails) {
    FillCardContactDetails(truckerConatactDetails)
}
export function CreateTrucker() {
    CreateCard()
}

export function AssertCreateTrucker() {
    let intercept = cy.wait("@" + RequestAliases.PostCard);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            ReCreateTrucker();
        }
        else {
            AssertPostTrucker(interception.response.statusCode, 200, interception.response.body.Trucker.Code)
        }
    })
}
function ReCreateTrucker() {
    var RandomCardCode = GenerateRandomNumber(6);
    cy.FillLogTextBox(MaintenanceSelectors.CardCode, RandomCardCode)
    CreateTrucker()
    AssertCreateTrucker()
}
export function AssertPostTrucker(truckerStatusCode: number, expectedStatusCode: number, truckerCode: string) {
    assert.equal(truckerStatusCode, expectedStatusCode)
    TruckerCode = truckerCode
}
export function CreateTruckerMockCreate() {
    CreateCardMockCreate()
}

export function AssertCreateTruckerMockCreate() {
    AssertCreateCardMockCreate()
}

export function UpdateTrucker() {
    DefinePutTruckerRequest()
    cy.Click(MaintenanceSelectors.TruckerSaveButton, null)
}
function DefinePutTruckerRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Truckers, RequestAliases.PutTrucker);
}

export function SearchTrucker() {
    SearchCardByValue(TruckerCode)
}

export function AssertSearchTrucker(companyName: string) {
    AssertSearchCard(companyName)
}

function DefineTruckersGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.TruckersGetSingle, RequestAliases.GetSignle);
}
export function AssertOpenTrucker() {
    AssertOpenCard()
}

export function AssertTruckerAddress(truckerDetails: CardDetails) {
    cy.Click(MaintenanceSelectors.TruckerAddressesTab, null, true)
    AssertCardAddress(truckerDetails)
}
export function AssertTruckerContact(conatactDetails: ContactDetails) {
    cy.Click(MaintenanceSelectors.TruckerContactsTab, null, true)
    AssertCardContact(conatactDetails)
}
export function FillTruckerGenaralTabNotes(Notes: string) {
    cy.Click(MaintenanceSelectors.TruckerGeneralTab, null, true)
    cy.FillLogTextBox(MaintenanceSelectors.TruckerNotes, " ")
    cy.FillLogTextBox(MaintenanceSelectors.TruckerNotes, Notes)
}
export function FillTruckerBillingTab(truckerBillingTabDetails: CardBillingTabDetails) {
    cy.Click(MaintenanceSelectors.TruckerBillingTab, null, true)
    cy.FillLogTextBox(MaintenanceSelectors.TruckerBankName, truckerBillingTabDetails.BankName)
    cy.FillLogTextBox(MaintenanceSelectors.TruckerIBANNumber, truckerBillingTabDetails.IBANNo)
}
export function AssertUpdateTrucker() {
    AssertPutTrucker()
}
function AssertPutTrucker() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutTrucker, 200);
}
//#endregion
//#region special services type
export function FillSpecialServicesTypeCode(SpecialServicesTypeCode: string) {
    cy.FillLogTextBox(MaintenanceSelectors.SpecialServicesTypeCode, SpecialServicesTypeCode)
}
export function FillSpecialServicesTypeDetails(specialServicesTypeDetails: SpecialServicesTypeDetails) {
    var RandomSpecialServicesTypeCode = gr.GenerateRandomNumberAndString(8);
    cy.FillLogTextBox(MaintenanceSelectors.SpecialServicesTypeCode, specialServicesTypeDetails.Code.toLowerCase() == "random" ? RandomSpecialServicesTypeCode : specialServicesTypeDetails.Code)
    cy.FillLogTextBox(MaintenanceSelectors.SpecialServicesTypeEnglishName, specialServicesTypeDetails.EnglishName)
    cy.FillLogTextBox(MaintenanceSelectors.SpecialServicesTypeLocalName, specialServicesTypeDetails.LocalName)
}
export function CreateSpecialServicesType() {
    DefinePostSpecialServicesTypeRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}
export function CreateSpecialServicesTypeMockCreate() {
    cy.intercept(RestAPI.POST, Urls.SpecialServicesTypes, [true])
    DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null)
}

export function AssertCreateSpecialServicesTypeMockCreate() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow)
    AssertGetByFilters();
}
function DefinePostSpecialServicesTypeRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.SpecialServicesTypes, RequestAliases.PostSpecialServicesType)
}
export function AssertCreateSpecialServicesType() {
    let intercept = cy.wait("@" + RequestAliases.PostSpecialServicesType);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            ReCreateSpecialServicesType();
        }
        else {
            AssertPostSpecialServicesType(interception.response.statusCode, 200, interception.response.body.Code)
        }
    })
}

function ReCreateSpecialServicesType() {
    var RandomSpecialServicesTypeCode = gr.GenerateRandomNumberAndString(8);
    cy.FillLogTextBox(MaintenanceSelectors.SpecialServicesTypeCode, RandomSpecialServicesTypeCode)
    CreateSpecialServicesType();
    AssertCreateSpecialServicesType();
}

export function AssertPostSpecialServicesType(responseStatusCode: number, expectedStatusCode: number, specialServicesTypeCode: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    SpecialServicesTypeCode = specialServicesTypeCode
}
export function SearchSpecialServicesType() {
    DefineSpecialServicesTypeGetByFiltersRequest(SpecialServicesTypeCode);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, SpecialServicesTypeCode);
    AsserSpecialServicesTypeViewsGetByFilters();
}
export function DefineSpecialServicesTypeGetByFiltersRequest(SpecialServicesTypeCode: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(SpecialServicesTypeCode), RequestAliases.GetFilterSearch);
}
export function AsserSpecialServicesTypeViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}
export function AssertSearchSpecialServicesType(companyName: string) {
    AssertSearchCard(companyName)
}
export function OpenSpecialServicesType() {
    DefineSpecialServicesTypesGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}
export function AssertOpenSpecialServicesType() {
    AssertCardGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}
function DefineSpecialServicesTypesGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.SpecialServicesTypesGetSingle, RequestAliases.GetSignle);
}

export function FillSpecialServicesTypeLocalName() {
    cy.FillLogTextBox(MaintenanceSelectors.SpecialServicesTypeLocalName, gr.GenerateRandomNumberAndString(5))
}

export function UpdateSpecialServicesType() {
    DefinePutSpecialServicesTypeRequest()
    cy.Click(MaintenanceSelectors.SpecialServicesTypeSaveButton, null)
}
function DefinePutSpecialServicesTypeRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.SpecialServicesTypes, RequestAliases.PutSpecialServicesType);
}
export function AssertUpdateSpecialServicesType() {
    AssertPutSpecialServicesType()
}
function AssertPutSpecialServicesType() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutSpecialServicesType, 200);
}
//#endregion
//#region move types
export function FillMoveTypeCode(MoveTypeTypeCode: string) {
    cy.FillLogTextBox(MaintenanceSelectors.MoveTypeCode, MoveTypeTypeCode)
}
export function FillMoveTypeDetails(moveTypeDetails: MoveTypeDetails) {
    var RandomMoveTypeCode = gr.GenerateRandomNumberAndString(3);
    cy.FillLogTextBox(MaintenanceSelectors.MoveTypeCode, moveTypeDetails.Code.toLowerCase() == "random" ? RandomMoveTypeCode : moveTypeDetails.Code)
    cy.FillLogTextBox(MaintenanceSelectors.MoveTypeEnglishName, moveTypeDetails.EnglishName)
    cy.FillLogTextBox(MaintenanceSelectors.MoveTypeLocalName, moveTypeDetails.LocalName)
    FillMoveTypeTransportMode(moveTypeDetails.TransportMode)
}

export function FillMoveTypeLocalName() {
    cy.FillLogTextBox(MaintenanceSelectors.MoveTypeLocalName, gr.GenerateRandomNumberAndString(5))
}

function FillMoveTypeTransportMode(TransportMode: string) {
    if (TransportMode.toLocaleUpperCase() == constants.Air) {
        cy.get(BaseSelectors.IsAir).click({ force: true })
    }
    if (TransportMode.toLocaleUpperCase() == constants.Ocean) {
        cy.get(BaseSelectors.IsOcean).click({ force: true })
    }
    if (TransportMode.toLocaleUpperCase() == constants.Inland) {
        cy.get(BaseSelectors.IsInland).click({ force: true })
    }
}
export function CreateMoveType() {
    DefinePostMoveTypeRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}
function DefinePostMoveTypeRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.MoveTypes, RequestAliases.PostMoveType)
}
export function CreateMoveTypeMockCreate() {
    cy.intercept(RestAPI.POST, Urls.MoveTypes, [true])
    DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null)
}

export function AssertCreateMoveTypeMockCreate() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow)
    AssertGetByFilters();
}

export function AssertCreateMoveType() {
    let intercept = cy.wait("@" + RequestAliases.PostMoveType);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            ReCreateMoveType();
        }
        else {
            AssertPostMoveType(interception.response.statusCode, 200, interception.response.body.Code)
        }
    })
}

function ReCreateMoveType() {
    var RandomMoveTypeCode = gr.GenerateRandomNumberAndString(3);
    cy.FillLogTextBox(MaintenanceSelectors.MoveTypeCode, RandomMoveTypeCode)
    CreateMoveType();
    AssertCreateMoveType();
}

export function AssertPostMoveType(responseStatusCode: number, expectedStatusCode: number, moveTypeCode: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    MoveTypeCode = moveTypeCode
}
export function SearchMoveTypeByCode(MoveTypeCode: string) {
    DefineMoveTypeCodeGetByFiltersRequest(MoveTypeCode)
    SearchCardByFilter(MoveTypeCode, MaintenanceSelectors.MoveTypeCodeFilterCheckBox)
}

function DefineMoveTypeCodeGetByFiltersRequest(MoveTypeCode: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(MoveTypeCode), RequestAliases.GetFilterSearch);
}

export function AssertSearchMoveTypeByCode(code: string) {
    AssertMoveTypeViewsGetByFilters()
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(code);
    });
}

function AssertMoveTypeViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function SearchMoveType() {
    SearchMoveTypeByCode(MoveTypeCode)
}

export function AssertSearchMoveType() {
    AssertSearchMoveTypeByCode(MoveTypeCode)
}
export function OpenMoveType() {
    DefineMoveTypesGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

function DefineMoveTypesGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.MoveTypeGetSingle, RequestAliases.GetSignle);
}
export function AssertOpenMoveType() {
    AssertCardGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

export function UpdateMoveType() {
    DefinePutMoveTypeRequest()
    cy.Click(MaintenanceSelectors.MoveTypeSaveButton, null)
}
function DefinePutMoveTypeRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.MoveTypes, RequestAliases.PutMoveType);
}
export function AssertUpdateMoveType() {
    AssertPutMoveType()
}
function AssertPutMoveType() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutMoveType, 200);
}
//#endregion
//#region Shipment Sub Type
export function FillShipmentSubTypeCode(ShipmentSubTypeCode: string) {
    cy.FillLogTextBox(MaintenanceSelectors.ShipmentSubTypeCode, ShipmentSubTypeCode)
}
export function FillShipmentSubTypeDetails(shipmentSubTypeDetails: ShipmentSubTypeDetails) {
    var RandomShipmentSubTypeCode = gr.GenerateRandomNumberAndString(5);
    cy.FillLogTextBox(MaintenanceSelectors.ShipmentSubTypeCode, shipmentSubTypeDetails.Code.toLowerCase() == "random" ? RandomShipmentSubTypeCode : shipmentSubTypeDetails.Code)
    FillShipmentSubTypeName(shipmentSubTypeDetails.Name)
    FillShipmentType(shipmentSubTypeDetails.ShipmentType)
}

export function FillShipmentSubTypeName(ShipmentSubTypeName: string) {
    cy.FillLogTextBox(MaintenanceSelectors.ShipmentSubTypeName, " ")
    cy.wait(1000)
    cy.FillLogTextBox(MaintenanceSelectors.ShipmentSubTypeName, ShipmentSubTypeName)
}

export function FillShipmentType(ShipmentType: string) {
    cy.SelectDropDownListItem(MaintenanceSelectors.LogLovShipmentSubType, ShipmentType)
}
export function SelectShipmentType() {
    cy.SelectDropDownListItemNumber(MaintenanceSelectors.LogLovShipmentSubType, 3)
}
export function CreateShipmentSubType() {
    DefinePostShipmentSubTypeRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}
function DefinePostShipmentSubTypeRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.ShipmentSubTypes, RequestAliases.PostShipmentSubType)
}
export function CreateShipmentSubTypeMockCreate() {
    cy.intercept(RestAPI.POST, Urls.ShipmentSubTypes, [true])
    DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null)
}

export function AssertCreateShipmentSubTypeMockCreate() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow)
    AssertGetByFilters();
}

export function AssertCreateShipmentSubType() {
    let intercept = cy.wait("@" + RequestAliases.PostShipmentSubType);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            ReCreateShipmentSubType();
        }
        else {
            AssertPostShipmentSubType(interception.response.statusCode, 200, interception.response.body.Code)
        }
    })
}

function ReCreateShipmentSubType() {
    var RandomShipmentSubTypeCode = gr.GenerateRandomNumberAndString(3);
    cy.FillLogTextBox(MaintenanceSelectors.MoveTypeCode, RandomShipmentSubTypeCode)
    CreateShipmentSubType();
    AssertCreateShipmentSubType();
}

export function AssertPostShipmentSubType(responseStatusCode: number, expectedStatusCode: number, shipmentSubTypeCode: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    ShipmentSubTypeCode = shipmentSubTypeCode
}
export function SearchShipmentSubTypeByCode(ShipmentSubTypeCode: string) {
    SearchCardByFilter(ShipmentSubTypeCode, MaintenanceSelectors.ShipmentSubTypeCodeFilterCheckBox)
}
export function SearchShipmentSubType() {
    SearchCardByValue(ShipmentSubTypeCode)
}
export function AssertSearchShipmentSubType() {
    AssertSearchCard(ShipmentSubTypeCode)
}
export function AssertSearchShipmentSubTypeByCode(code: string) {
    cy.wait(1000)
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(code);
    });
}
export function OpenShipmentSubType() {
    DefineShipmentSubTypeGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}
function DefineShipmentSubTypeGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.ShipmentSubTypesGetSingle, RequestAliases.GetSignle);
}
export function AssertOpenShipmentSubType() {
    AssertCardGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

export function UpdateShipmentSubType() {
    DefinePutShipmentSubTypeRequest()
    cy.Click(MaintenanceSelectors.ShipmentSubTypeSaveButton, null)
}
function DefinePutShipmentSubTypeRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.ShipmentSubTypes, RequestAliases.PutShipmentSubType);
}
export function AssertUpdateShipmentSubType() {
    AssertPutShipmentSubType()
}
function AssertPutShipmentSubType() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutShipmentSubType, 200);
}
//#endregion
////#region Credit CardT ype 
export function FillCreditCardTypeCode(CreditCardTypeCode: string) {
    cy.FillLogTextBox(MaintenanceSelectors.CreditCardTypeCode, CreditCardTypeCode)
}
export function FillCreditCardTypeDetails(creditCardTypeDetails: CreditCardTypeDetails) {
    cy.FillLogTextBox(MaintenanceSelectors.CreditCardTypeCode, creditCardTypeDetails.Code.toLowerCase() == "random" ? gr.GenerateRandomNumberAndString(2) : creditCardTypeDetails.Code)
    FilllCreditCardTypeName(gr.GenerateCurrentDatetimeString("_"))
}
export function FilllCreditCardTypeName(Name: string) {
    cy.FillLogTextBox(MaintenanceSelectors.CreditCardTypeName, Name)
}
export function CreateCreditCardType() {
    DefinePostCreditCardTypeRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}
function DefinePostCreditCardTypeRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.CreditCardTypes, RequestAliases.PostCreditCardType)
}
export function CreateCreditCardTypeMockCreate() {
    cy.intercept(RestAPI.POST, Urls.CreditCardTypes, [true])
    DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null)
}

export function AssertCreateCreditCardTypeMockCreate() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow)
    AssertGetByFilters();
}

export function AssertCreateCreditCardType() {
    let intercept = cy.wait("@" + RequestAliases.PostCreditCardType);
    intercept.then((interception) => {
        assert.equal(interception.response.statusCode, 200)
        CreditCardTypeSearchValue = interception.response.body.Name
    })
}

export function getCreditCardTypeSearchValue() {
    return CreditCardTypeSearchValue
}

export function AssertSearchCreditCardTypeByFilter() {
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(CreditCardTypeSearchValue);
    });
}

export function OpenCreditCardType() {
    DefineCreditCardTypeGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}
function DefineCreditCardTypeGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.CreditCardTypesGetSingle, RequestAliases.GetSignle);
}
export function AssertOpenCreditCardType() {
    AssertCardGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}
export function UpdateCreditCardType() {
    DefinePutCreditCardTypeRequest()
    cy.Click(MaintenanceSelectors.CreditCardTypeSaveButton, null)
}
function DefinePutCreditCardTypeRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.CreditCardTypes, RequestAliases.PutCreditCardType);
}
export function AssertUpdateCreditCardType() {
    AssertPutCreditCardType()
}
function AssertPutCreditCardType() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutCreditCardType, 200).
        then((interception) => {
            inActiveCreditCardType = interception.response.body.InActive;
        });
}
export function CreditCardTypeConversionEventsMapping(eventDetailsList: EventTypeDetails[]): EventTypeDetails[] {
    ConversionEventsMapping(eventDetailsList, inActiveCreditCardType)
    return eventDetailsList;
}
//#endregion


//#region Company Address Settings
export function FillCompanyAddressSettingsDetails(address2, zipCode) {
    cy.FillLogTextBox(MaintenanceSelectors.CompanyAddressSettingsAddress2, address2)
    cy.FillLogTextBox(MaintenanceSelectors.CompanyAddressSettingsZipCode, zipCode)
}

export function UpdateCompanyAddressSettings() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Addresses, RequestAliases.PutCompanyAddressSettings);
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertUpdateCompanyAddressSettings() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutCompanyAddressSettings, 200)
}
//#endregion

//#region Shipper-Consignee
export function FillCustomerLocalName(localName) {
    cy.Click(MaintenanceSelectors.CustomerGeneralTab, null)
    cy.FillLogTextBox(MaintenanceSelectors.CustomerLocalName, localName)
}

export function UpdateCustomer() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Customers, RequestAliases.PutShipperConsignee);
    cy.Click(MaintenanceSelectors.CustomerSaveButton, null);
}

export function AssertUpdateCustomer() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutShipperConsignee, 200)
}
//#endregion

//#region Company Address Settings
export function UpdateAgent() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Agents, RequestAliases.PutAgent);
    cy.Click(MaintenanceSelectors.AgentSaveButton, null)
}

export function AssertUpdateAgent() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutAgent, 200);
}
//#endregion