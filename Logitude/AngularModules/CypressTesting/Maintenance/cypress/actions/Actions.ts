import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { Constants } from "../constants/Constants";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import * as BaseActions from "../../../Base/cypress/actions/Actions";
import * as gr from '../../../Base/cypress/actions/GenerateRandoms'
import { BaseURLs } from "../../../Base/cypress/constants/URLs";
import { Datepicker } from "../../../Base/cypress/models/Datepicker";
import { ContactDetails } from "../models/ContactDetails";
import { ContactContext } from "../models/ContactContext";
import { VendorDetails } from "../models/VendorDetails";
import { VendorContext } from "../models/VendorContext";
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

export function FillChangePasswordWindow(CurrentPassword: string, NewPassword: string, RetypePassword: string) {
    cy.FillLogTextBox(MaintenanceSelectors.CurrentPassword, CurrentPassword)
    cy.FillLogTextBox(MaintenanceSelectors.NewPassword, NewPassword)
    if (RetypePassword != null) {
        cy.FillLogTextBox(MaintenanceSelectors.RetypePassword, RetypePassword)
    }
}

export function ChangePasswordMockChange() {
    cy.intercept(Urls.PostChangePassword, [true])

    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

export function OpenTabInMaintenanceMenu(maintenanceItemNameToSearch: string, maintenanceItemSelector: string) {
    cy.Click(BaseSelectors.MaintenanceMenu, null);
    cy.FillLogTextBox(BaseSelectors.NullSearch, maintenanceItemNameToSearch);
    cy.Click(maintenanceItemSelector, null);
}

export function OpenNewWizard(tabName: string) {
    cy.Click(MaintenanceSelectors.NewWizardButton(tabName), null);
}

function DefineGetByFilterRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetByFilter, RequestAliases.GetByFilter);
}

function AssertGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetByFilter, 200);
}
//#endregion

//#region Vendor
export function FillVendorDetails(vendorDetails: VendorDetails) {
    cy.FillLogTextBox(MaintenanceSelectors.VendorCompanyName, vendorDetails.CompanyName);
    cy.FillLogTextBox(MaintenanceSelectors.VendorPhone, vendorDetails.Phone);
    cy.FillLogTextBox(MaintenanceSelectors.VendorLocalName, vendorDetails.LocalName);
    cy.FillLogTextBox(MaintenanceSelectors.VendorFax, vendorDetails.Fax);
    cy.FillLogTextBox(MaintenanceSelectors.VendorAddress1, vendorDetails.Address1);
    cy.FillLogTextBox(MaintenanceSelectors.VendorZipCode, vendorDetails.Zip);
    cy.FillLogTextBox(MaintenanceSelectors.VendorCity, vendorDetails.City);
    cy.FillLogLov(MaintenanceSelectors.VendorCountry, vendorDetails.Country, true);
    cy.FillLogLov(MaintenanceSelectors.VendorState, vendorDetails.State, true);
}

export function FillVendorContactDetails(conatactDetails: ContactDetails) {
    cy.SelectCheckBox(MaintenanceSelectors.VendorContactCheckBox)
    cy.FillLogTextBox(MaintenanceSelectors.VendorContactEnglishName, conatactDetails.EnglishName);
    cy.FillLogTextBox(MaintenanceSelectors.VendorContactPosition, conatactDetails.Position);
    cy.FillLogTextBox(MaintenanceSelectors.VendorContactBusinessPhone, conatactDetails.BusinessPhone);
    cy.FillLogTextBox(MaintenanceSelectors.VendorContactMobile, conatactDetails.Mobile);
    cy.FillLogTextBox(MaintenanceSelectors.VendorContactFax, conatactDetails.Fax);
}

export function FillVendorGeneralTab(vendorDetails: VendorDetails) {
    if (vendorDetails.Website) {
        cy.FillLogTextBox(MaintenanceSelectors.VendorWebsite, vendorDetails.Website)
    }
    if (vendorDetails.Notes) {
        cy.FillLogTextBox(MaintenanceSelectors.VendorNotes, vendorDetails.Notes)
    }
}

export function FillVendorBillingTab(vendorDetails: VendorDetails) {
    if (vendorDetails.VatNumber) {
        cy.FillLogTextBox(MaintenanceSelectors.VendorVatNumber, vendorDetails.VatNumber)
    }
    if (vendorDetails.BankName) {
        cy.FillLogTextBox(MaintenanceSelectors.VendorBankName, vendorDetails.BankName)
    }
}

export function CreateVendor() {
    DefinePostVendorRequest()
    DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function UpdateVendor() {
    DefinePutVendorRequest()
    cy.Click(MaintenanceSelectors.VendorSaveButton, null)
}

export function AssertCreateVendor() {
    AssertPostVendor()
    AssertGetByFilters()
}

export function AssertUpdateVendor() {
    AssertPutVendor()
}

export function SearchVendor() {
    let VendorCode = VendorContext.Code;
    DefineVendorViewsGetByFiltersRequest(VendorCode);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, VendorCode);
    AssertVendorViewsGetByFilters();
}

export function AssertSearchVendor(companyName: string) {
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(companyName);
    });
}

export function OpenVendor() {
    DefineVendorsGetSingleRequest();
    DefineGetMenuButtonGroupsRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

export function AssertOpenVendor() {
    AssertVendorGetSingle();
    AssertGetMenuButtonGroups();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

export function CloseSaveVendor() {
    DefineVendorViewGetSingleRequest()
    cy.Click(MaintenanceSelectors.VendorSaveCloseButton, null);
}

export function AssertCloseSaveVendor() {
    AssertVendorGetSingle();
}

function DefinePostVendorRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.PartnersDomain, RequestAliases.PostVendor);
}

function DefinePutVendorRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Vendor, RequestAliases.PutVendor);
}

function AssertPostVendor() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostVendor, 200).then((interception) => {
        let responseBody = interception.response.body;
        VendorContext.Code = responseBody.Vendor.Code;
    });
}
function AssertPutVendor() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutVendor, 200);
}

function DefineVendorViewsGetByFiltersRequest(vendorCode: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(vendorCode), RequestAliases.GetFilterSearch);
}

function AssertVendorViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

function DefineVendorsGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.VendorsGetSingle, RequestAliases.GetSignle);
}

function DefineVendorViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.VendorsviewGetSingle, RequestAliases.GetSignle);
}

function AssertVendorGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
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
        VesselContext.Name = vesselName.toLowerCase() == "random" ? (gr.GenerateRandomNumberAndString(5)) : vesselName;
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
    let NewRandomCode = gr.GenerateRandomNumberAndString(5)
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
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(VesselCode + "&GetCount=false"), RequestAliases.GetFilterSearch);
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
    FillEnglishContactName(contactDetails.EnglishName);
    FillLocalContactName(contactDetails.LocalName);
    FillContactPosition(contactDetails.Position);
    FillContactBusinessPhone(contactDetails.BusinessPhone);
    FillContactMobile(contactDetails.Mobile);
    FillContactFax(contactDetails.Fax);
    FillContactDatepicker(contactDetails.BirthdayDate, Constants.Birthday);
    FillContactDateReminder(contactDetails.BirthdayReminder, Constants.Birthday);
    FillContactDatepicker(contactDetails.AnniversaryDate, Constants.Anniversary);
    FillContactDateReminder(contactDetails.AnniversaryReminder, Constants.Anniversary);
    FillContactNotes(contactDetails.Notes);
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

function FillEnglishContactName(contactEnglishName: string) {
    if (contactEnglishName) {
        cy.FillLogTextBox(MaintenanceSelectors.ContactEnglishName, contactEnglishName);
    }
}

function FillLocalContactName(contactLocalName: string) {
    if (contactLocalName) {
        cy.FillLogTextBox(MaintenanceSelectors.ContactLocalName, contactLocalName);
    }
}

function FillContactPosition(contactPosition: string) {
    if (contactPosition) {
        cy.FillLogTextBox(MaintenanceSelectors.ContactPosition, contactPosition);
    }
}

function FillContactBusinessPhone(contactBusinessPhone: string) {
    if (contactBusinessPhone) {
        cy.FillLogTextBox(MaintenanceSelectors.ContactBusinessPhone, contactBusinessPhone);
    }
}

function FillContactMobile(contactMobile: string) {
    if (contactMobile) {
        cy.FillLogTextBox(MaintenanceSelectors.ContactMobile, contactMobile);
    }
}

function FillContactFax(contactFax: string) {
    if (contactFax) {
        cy.FillLogTextBox(MaintenanceSelectors.ContactFax, contactFax);
    }
}

function FillContactNotes(notes: string) {
    if (notes) {
        cy.FillLogTextBox(MaintenanceSelectors.ContactNotes, notes);
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

function FillContactDateReminder(reminder: string, dateType: string) {
    if (reminder && dateType) {
        let reminderCheckboxSelector = GetContactDateReminderSelector(dateType);
        if (reminder.toLowerCase() == "yes") {
            cy.get(reminderCheckboxSelector).check({ force: true });
        }
        else {
            cy.get(reminderCheckboxSelector).uncheck({ force: true });
        }
    }
}

function GetContactDateReminderSelector(dateType: string) {
    let reminderCheckboxSelector: string;
    if (dateType.toLowerCase() == Constants.Birthday) {
        reminderCheckboxSelector = MaintenanceSelectors.ContactBirthdayReminder;
    }
    else if (dateType.toLowerCase() == Constants.Anniversary) {
        reminderCheckboxSelector = MaintenanceSelectors.ContactAnniversaryReminder;
    }
    return reminderCheckboxSelector;
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
    let NameToFill = quoteTemplateName.toLowerCase() == "random" ? ("Quote_" + gr.GenerateRandomNumberAndString(4)) : quoteTemplateName;
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
    cy.Navigate(MaintenanceSelectors.QuoteSettingsLabel);
    cy.Click(MaintenanceSelectors.LabelDiv(labelToEdit), null);
    cy.get(MaintenanceSelectors.LabelTextBox(labelToEdit)).type(newFieldValue);
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
    cy.get(BaseSelectors.RowClass).last().click();
    AssertOpenQuoteTemplate();
}

export function ReopenQuoteTemplate() {
    DefineQuoteTemplatetGetSingleRequest()
    cy.get(BaseSelectors.RowClass).last().click({ force: true });
    AssertOpenQuoteTemplate();
}

export function UpdateQuoteTemplate() {
    DefinePutQuoteTemplateRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function UpdateQuoteHeaderTemplate() {
    DefineQuoteTemplatetPutTextDesignRequest();
    DefineQuoteTemplatetPutHeaderFieldsRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function UpdateQuotePricingTemplate() {
    DefineQuoteTemplatetPutSettingsRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function UpdateQuoteIntroductionTemplate() {
    DefineQuoteTemplatetPutSectionsRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCreateQuoteTemplate() {
    AssertPostQuoteTemplate();
    AssertQuoteTemplatetGetSingle();
}

export function AssertUpdateQuoteTemplate() {
    AssertPutQuoteTemplate();
}

export function AssertUpdateQuoteHeaderTemplate() {
    AssertQuoteTemplatetPutHeaderFields();
    AssertQuoteTemplatetPutTextDesign();
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

function DefinePutQuoteTemplateRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.QuoteTemplatetextdesigns, RequestAliases.PutQuoteTemplate);
}

function DefineQuoteTemplatetGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.QuoteTemplateGetSingle, RequestAliases.GetSignle);
}

function DefineQuoteTemplateViewsGetByFiltersRequest(quoteTemplateName: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(quoteTemplateName), RequestAliases.GetFilterSearch);
}

function DefineQuoteTemplatetPutHeaderFieldsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.PutQuoteTemplateHeaderFields, RequestAliases.PutQuoteTemplateHeaderFields);
}

function DefineQuoteTemplatetPutTextDesignRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.PutQuoteTemplateTextDesignPMs, RequestAliases.PutQuoteTemplateTextDesignPMs);
}

function DefineQuoteTemplatetPutSettingsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.QuotetemplateSettings, RequestAliases.PutQuoteTemplate);
}

function DefineQuoteTemplatetPutSectionsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Quotetemplatesections, RequestAliases.PutQuoteTemplate);
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

function AssertPutQuoteTemplate() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutQuoteTemplate, 200);
}

function AssertQuoteTemplatetPutHeaderFields() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.PutQuoteTemplateHeaderFields, RequestAliases.PutQuoteTemplateHeaderFields);
}

function AssertQuoteTemplatetPutTextDesign() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.PutQuoteTemplateTextDesignPMs, RequestAliases.PutQuoteTemplateTextDesignPMs);
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
    FillIsCustomerTelphoneRequiredCheckBox(customerSettingsDetails.IsCustomerTelphoneRequired);
    FillIsPotentialTelphoneRequiredCheckBox(customerSettingsDetails.IsPotentialCustomerTelphoneRequired);
    FillIsCustomerFaxRequiredCheckBox(customerSettingsDetails.IsCustomerFaxRequired);
    FillIsPotentialCustomerFaxRequiredCheckBox(customerSettingsDetails.IsPotentialCustomerFaxRequired);
    FillIsCustomerAddress1RequiredCheckBox(customerSettingsDetails.IsCustomerAddress1Required);
}
function FillIsCustomerTelphoneRequiredCheckBox(IsCustomerTelphoneRequired: string) {
    if (IsCustomerTelphoneRequired.toUpperCase() == constants.YES) {
        cy.get(MaintenanceSelectors.IsCustomerTelephoneRequiredCheckBox).check({ force: true });
    }
}
function FillIsPotentialTelphoneRequiredCheckBox(IsPotentialCustomerTelphoneRequired: string) {
    if (IsPotentialCustomerTelphoneRequired.toUpperCase() == constants.YES) {
        cy.get(MaintenanceSelectors.IsPotentialCustomerTelephoneRequiredCheckBox).check({ force: true });
    }
}
function FillIsCustomerFaxRequiredCheckBox(IsCustomerFaxRequired: string) {
    if (IsCustomerFaxRequired.toUpperCase() == constants.YES) {
        cy.get(MaintenanceSelectors.IsCustomerFaxRequiredCheckBox).check({ force: true });
    }
}
function FillIsPotentialCustomerFaxRequiredCheckBox(IsPotentialCustomerFaxRequired: string) {
    if (IsPotentialCustomerFaxRequired.toUpperCase() == constants.YES) {
        cy.get(MaintenanceSelectors.IsPotentialCustomerFaxRequiredCheckBox).check({ force: true });
    }
}
function FillIsCustomerAddress1RequiredCheckBox(IsCustomerAddress1Required) {
    if (IsCustomerAddress1Required.toUpperCase() == constants.YES) {
        cy.get(MaintenanceSelectors.IsCustomerAddress1RequiredCheckBox).check({ force: true });
    }
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
    FillPotentialCustomerName(customerDetails.CompanyName)
    FillPotentialCustomerCity(customerDetails.City)
    FillPotentialCustomerCountry(customerDetails.Country)
    FillPotentialCustomerState(customerDetails.State)
    FillPotentialCustomerPhoneNumber(customerDetails.PhoneNumber)
    FillPotentialCustomerFaxNumber(customerDetails.FaxNumber)
    fillPotentialCustomerAddress1(customerDetails.Address1);
    if (customerDetails.AddContact) {
        FillPotentialCustomerAddContactCheckBox(customerDetails.AddContact);
    }
}
function FillPotentialCustomerName(CustomerName: string) {
    cy.FillLogTextBox(MaintenanceSelectors.PotentialCustomerName, CustomerName)
}
function FillPotentialCustomerCity(City: string) {
    cy.FillLogTextBox(MaintenanceSelectors.PotentialCustomerCity, City)
}
function FillPotentialCustomerCountry(Country: string) {
    cy.FillLogLov(MaintenanceSelectors.PotentialCustomerCountry, Country, true)
}
function FillPotentialCustomerState(State: string) {
    if (State) {
        cy.FillLogLov(MaintenanceSelectors.PotentialCustomerState, State, true)
    }
}
function FillPotentialCustomerPhoneNumber(PhoneNumber: string) {
    if (PhoneNumber) {
        cy.FillLogTextBox(MaintenanceSelectors.PotentialCustomerPhoneNumber, PhoneNumber)
    }
}
function FillPotentialCustomerFaxNumber(FaxNumber: string) {
    if (FaxNumber) {
        cy.FillLogTextBox(MaintenanceSelectors.PotentialCustomerFaxNumber, FaxNumber)
    }
}
function fillPotentialCustomerAddress1(Address1: string) {
    if (Address1) {
        cy.FillLogTextBox(MaintenanceSelectors.PotentialCustomerAddress1, Address1)
    }
}
function FillPotentialCustomerAddContactCheckBox(AddContact: string) {
    if (AddContact.toUpperCase() == constants.YES) {
        cy.get(MaintenanceSelectors.PotentialCustomerAddContactCheckBox).check({ force: true })
    }
    else {
        cy.get(MaintenanceSelectors.PotentialCustomerAddContactCheckBox).find(BaseSelectors.input).uncheck({ force: true })
    }
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
        WaitURL: Urls.GetQuickSearch(CustomerDetails.Code),
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

//#region change password
export function OpenChangeUserPasswordWindow() {
    OpenPersonalSettingsTab();
    cy.Click(MaintenanceSelectors.ChangePasswordMaintenanceItem, null)
}
function OpenPersonalSettingsTab() {
    OpenMaintenanceMenu();
    cy.Click(MaintenanceSelectors.PersonalSettingsMaintenanceTab, null)
}
//#endregion