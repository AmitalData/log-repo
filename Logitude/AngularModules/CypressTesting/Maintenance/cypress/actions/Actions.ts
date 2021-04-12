import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { Constants } from "../constants/Constants";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import * as BaseActions from "../../../Base/cypress/actions/Actions";
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';
import * as ShipmentActions from "../../../Shipment/cypress/actions/Actions"
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
import { CountryDetails } from "../models/CountryDetails";
import { EventTypeDetails } from "../../../Base/cypress/models/EventTypeDetails";
import { StateDetails } from "../models/StateDetails";
import { CityDetails } from "../models/CityDetails";
import { CurrencyDetails } from "../models/CurrencyDetails";
import { GlobalZoneDetails } from "../models/GlobalZoneDetails"
import {CommodityDetails} from "../models/CommodityDetails"
import { QuoteSelectors } from "../../../Quote/cypress/selectors/Selectors";
import { ShipmentSelectors } from "../../../Shipment/cypress/selectors/Selectors";
import { ReceivableDetails } from "../../../Shipment/cypress/models/ReceivableDetails";
import { RegionDetails } from "../models/RegionDetails";
//#region variables
let CityName=null;
let StateName=null;
let GlobalZoneName=null;
let CommodityName=null;
let RegionName=null;
let inActiveCountry=false;
let inActiveState=false;
let inActiveCity = false;
let inActiveGlobalZone = false;
let inActiveCommodity=false;
let inActiveRegion=false;
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
function GenerateRandomNumber(NumberLength:number) {
    let NewRandomCode = gr.GenerateRandomNumberAndString(NumberLength)   
    return NewRandomCode;
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
    cy.FillLogLov(MaintenanceSelectors.VendorCountry, vendorDetails.Country,true);
    cy.FillLogLov(MaintenanceSelectors.VendorState, vendorDetails.State,true);
}

export function FillVendorContactDetails(conatactDetails: ContactDetails) {
    FillCheckBoxProcess(MaintenanceSelectors.VendorContactCheckBox,conatactDetails.AddContact)
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
    cy.FillLogTextBox(MaintenanceSelectors.ContactEnglishName , contactDetails.EnglishName)
    cy.FillLogTextBox(MaintenanceSelectors.ContactLocalName , contactDetails.LocalName)
    cy.FillLogTextBox(MaintenanceSelectors.ContactPosition , contactDetails.Position)
    cy.FillLogTextBox(MaintenanceSelectors.ContactBusinessPhone , contactDetails.BusinessPhone)
    cy.FillLogTextBox(MaintenanceSelectors.ContactMobile , contactDetails.Mobile)
    cy.FillLogTextBox(MaintenanceSelectors.ContactFax , contactDetails.Fax)
    cy.FillLogTextBox(MaintenanceSelectors.ContactNotes, contactDetails.Notes)
    FillContactDatepicker(contactDetails.BirthdayDate, Constants.Birthday);
    FillCheckBoxProcess(MaintenanceSelectors.ContactBirthdayReminder ,contactDetails.BirthdayReminder);
    FillContactDatepicker(contactDetails.AnniversaryDate, Constants.Anniversary);
    FillCheckBoxProcess(MaintenanceSelectors.ContactAnniversaryReminder,contactDetails.AnniversaryReminder);
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
    FillCheckBoxProcess(MaintenanceSelectors.IsCustomerTelephoneRequiredCheckBox,customerSettingsDetails.IsCustomerTelphoneRequired)
    FillCheckBoxProcess(MaintenanceSelectors.IsPotentialCustomerTelephoneRequiredCheckBox,customerSettingsDetails.IsPotentialCustomerTelphoneRequired)
    FillCheckBoxProcess(MaintenanceSelectors.IsCustomerFaxRequiredCheckBox,customerSettingsDetails.IsCustomerFaxRequired)
    FillCheckBoxProcess(MaintenanceSelectors.IsPotentialCustomerFaxRequiredCheckBox,customerSettingsDetails.IsPotentialCustomerFaxRequired)
    FillCheckBoxProcess(MaintenanceSelectors.IsCustomerAddress1RequiredCheckBox,customerSettingsDetails.IsCustomerAddress1Required)
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
    cy.FillLogTextBox(MaintenanceSelectors.PotentialCustomerName , customerDetails.CompanyName)
    cy.FillLogTextBox(MaintenanceSelectors.PotentialCustomerCity , customerDetails.City)
    cy.FillLogLov(MaintenanceSelectors.PotentialCustomerCountry , customerDetails.Country,true)
    cy.FillLogLov(MaintenanceSelectors.PotentialCustomerState , customerDetails.State,true)
    cy.FillLogTextBox(MaintenanceSelectors.PotentialCustomerPhoneNumber , customerDetails.PhoneNumber)
    cy.FillLogTextBox(MaintenanceSelectors.PotentialCustomerFaxNumber , customerDetails.FaxNumber)
    cy.FillLogTextBox(MaintenanceSelectors.PotentialCustomerAddress1 , customerDetails.Address1)
    FillCheckBoxProcess(MaintenanceSelectors.PotentialCustomerAddContactCheckBox ,customerDetails.AddContact )
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
    cy.FillLogTextBox(MaintenanceSelectors.CountryCode,countryDetails.CountryCode);
    cy.FillLogTextBox(MaintenanceSelectors.CountryEnglishName,countryDetails.CountryName);
    cy.FillLogTextBox(MaintenanceSelectors.CountryLocalName,countryDetails.CountryLocalName);
    cy.FillLogLov(MaintenanceSelectors.CountryGlobalZone,countryDetails.CountryGlobalZone,true);
    FillCheckBoxProcess(MaintenanceSelectors.InActiveCountryCheckBox,countryDetails.InactiveCountry);
    FillCheckBoxProcess(MaintenanceSelectors.CountryECCheckBox,countryDetails.EC);
    FillCheckBoxProcess(MaintenanceSelectors.CountryIsNorthAmericaCheckBox,countryDetails.NorthAmerica);
    FillCheckBoxProcess(MaintenanceSelectors.CountryIsStateRequiredCheckBox,countryDetails.IsStateRequired);
    FillCheckBoxProcess(MaintenanceSelectors.CountryHasCitiesCheckBox,countryDetails.HasCities);
    cy.FillLogTextBox(MaintenanceSelectors.CountryNotes,countryDetails.Notes);

}

export function FillRandomCountryLocalName(LocalName: string) {
    let randomLocalName = GenerateRandomName(LocalName, 10)
    cy.FillLogTextBox(MaintenanceSelectors.CountryLocalName, randomLocalName)
}

export function FillCountryCode(CountryCode:string){
    cy.FillLogTextBox(MaintenanceSelectors.CountryCode , CountryCode)
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

export function SearchCountry(CountryName: string) {
    DefineCountryViewsGetByFiltersRequest(CountryName);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, CountryName);
    AssertCountryViewsGetByFilters();
}

export function AssertSearchCountry(CountryName: string) {
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(CountryName);
    });
}

export function DefineCountryViewsGetByFiltersRequest(CountryName: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(CountryName + "&GetCount=false"), RequestAliases.GetFilterSearch);
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
    ConversionEventsMapping(eventDetailsList,inActiveCountry)
    return eventDetailsList;
}

export function ValidateErrorPopUpMessage(Message: string) {
    cy.get(BaseSelectors.ErrorPopUp).should("contain.text", Message)

}
//#endregion

//#region State
export function FillStateDetails(stateDetails:StateDetails){
    var RandomStateNumber=GetRandomStateCodeNumber();
    cy.FillLogTextBox(MaintenanceSelectors.StateCode , stateDetails.StateCode.toLowerCase() == "random"?RandomStateNumber:stateDetails.StateCode)
    cy.FillLogTextBox(MaintenanceSelectors.StateEnglishName , stateDetails.StateName.toLowerCase() == "random"?RandomStateNumber: stateDetails.StateName)
    cy.FillLogTextBox(MaintenanceSelectors.StateLocalName , stateDetails.StateLocalName.toLowerCase() == "random"?RandomStateNumber:stateDetails.StateLocalName)
    cy.FillLogLov(MaintenanceSelectors.StateCountry , stateDetails.Country,true)
    FillCheckBoxProcess(MaintenanceSelectors.InActiveStateCheckBox,stateDetails.InactiveState)
    cy.FillLogTextBox(MaintenanceSelectors.StateNotes , stateDetails.Notes)
}
function GetRandomStateCodeNumber(){
    return gr.GenerateRandomNumberAndString(10);
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
        else{
            AssertPostState(interception.response.statusCode, 200,interception.response.body.EnglishName) 
        }        
    }) 
}
function ReCreateState(){
    let stateCode=  GenerateRandomNumber(10);
    cy.FillLogTextBox(MaintenanceSelectors.StateCode , stateCode)
    cy.FillLogTextBox(MaintenanceSelectors.StateEnglishName , stateCode)
    cy.FillLogTextBox(MaintenanceSelectors.StateLocalName , stateCode)
    CreateState();
    AssertCreateState();
}
export function AssertPostState(responseStatusCode: number, expectedStatusCode: number,stateName:string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    StateName=stateName
}

export function SearchState() {
    DefineStateViewsGetByFiltersRequest(StateName);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, StateName);
    AssertStateViewsGetByFilters();
}
export function DefineStateViewsGetByFiltersRequest(StateName: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(StateName + "&GetCount=false"), RequestAliases.GetFilterSearch);
}
export function AssertSearchState() {
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(StateName);
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
    let LocalNameToFill = LocalName.toLowerCase() == "random" ? (gr.GenerateRandomNumberAndString(10)) : LocalName; 
    if (LocalName) {
        cy.FillLogTextBox(MaintenanceSelectors.StateLocalName, LocalNameToFill)
    }
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
function GenerateRandomName(Name:string ,lenght:number){
    return Name.toLowerCase() == "random" ? (gr.GenerateRandomNumberAndString(lenght)) : Name; 
}

export function ChangeInactiveCheckBoxValue(InActivateSelector:string) {
    cy.get(InActivateSelector).then($InActiveStatesCheckBox => {
        if ($InActiveStatesCheckBox.is(':checked')) {
            cy.get(InActivateSelector).uncheck({ force: true })
        }
        else {
            cy.get(InActivateSelector).check({ force: true })
        }
    })
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
function DefineGetByFilterRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetByFilter, RequestAliases.GetByFilter);
}

function AssertGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetByFilter, 200);
}
//#endregion
//#region city
export function FillCityDetails(cityDetails:CityDetails){
    var RandomCityNumber=GetRandomCityCodeNumber();
    cy.FillLogTextBox(MaintenanceSelectors.CityCode , cityDetails.CityCode.toLowerCase() == "random"?RandomCityNumber:cityDetails.CityCode)
    cy.FillLogTextBox(MaintenanceSelectors.CityEnglishName , cityDetails.CityName.toLowerCase() == "random"?RandomCityNumber:cityDetails.CityName)
    cy.FillLogTextBox(MaintenanceSelectors.CityLocalName , cityDetails.CityLocalName.toLowerCase() == "random"?RandomCityNumber:cityDetails.CityLocalName)
    cy.FillLogLov(MaintenanceSelectors.CityCountry , cityDetails.Country,true)
    cy.FillLogLov(MaintenanceSelectors.CityState , cityDetails.Country,true)
    FillCheckBoxProcess(MaintenanceSelectors.InActiveCityCheckBox,cityDetails.InactiveCity)
    cy.FillLogTextBox(MaintenanceSelectors.CityNotes , cityDetails.Notes)
}
function GetRandomCityCodeNumber(){
return gr.GenerateRandomNumberAndString(15);
}
export function CreateCity() {
    DefinePostCityRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePostCityRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.CountryCities, RequestAliases.PostCity);
}
export function AssertCreateCity(){
    let intercept = cy.wait("@" + RequestAliases.PostCity);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            ReCreateCity();
        }
        else{
            AssertPostCity(interception.response.statusCode, 200,interception.response.body.EnglishName) 
        }        
    }) 
}
function ReCreateCity(){
    let cityCode=  GenerateRandomNumber(15);
    cy.FillLogTextBox(MaintenanceSelectors.CityCode , cityCode)
    cy.FillLogTextBox(MaintenanceSelectors.CityEnglishName , cityCode)
    cy.FillLogTextBox(MaintenanceSelectors.CityLocalName , cityCode)
    CreateCity();
    AssertCreateCity();
}
export function AssertPostCity(responseStatusCode: number, expectedStatusCode: number,cityName:string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    CityName=cityName
}
export function SearchCity() {
    DefineCityViewsGetByFiltersRequest(CityName);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, CityName);
    AssertCitiesViewsGetByFilters();
}
export function AssertCitiesViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}
export function AssertSearchCity() {
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(CityName);
    });
}

export function DefineCityViewsGetByFiltersRequest(CityName: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(CityName + "&GetCount=false"), RequestAliases.GetFilterSearch);
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
    let LocalNameToFill = LocalName.toLowerCase() == "random" ? (gr.GenerateRandomNumberAndString(10)) : LocalName; 
    if (LocalName) {
        cy.FillLogTextBox(MaintenanceSelectors.CityLocalName, LocalNameToFill)
    }
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
export function ConversionEventsMapping(eventDetailsList: EventTypeDetails[],inActiveField:boolean){
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
export function FillGlobalZoneCode(GlobalZoneCode:string){
    cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneCode , GlobalZoneCode)
}
export function FillGlobalZoneDetails(globalZoneDetails:GlobalZoneDetails){
    var RandomGlobalZoneNumber=GetRandomGlobalZoneCodeNumber();
    cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneCode, globalZoneDetails.GlobalZoneCode.toLowerCase() == "random"?RandomGlobalZoneNumber:globalZoneDetails.GlobalZoneCode)
    cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneEnglishName, globalZoneDetails.GlobalZoneName.toLowerCase() == "random"?RandomGlobalZoneNumber:globalZoneDetails.GlobalZoneName)
    cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneLocalName, globalZoneDetails.GlobalZoneLocalName.toLowerCase() == "random"?RandomGlobalZoneNumber:globalZoneDetails.GlobalZoneName)
    FillCheckBoxProcess(MaintenanceSelectors.InActiveGlobalZoneCheckBox,globalZoneDetails.InactiveGlobalZone)
}
function GetRandomGlobalZoneCodeNumber(){
    return gr.GenerateRandomNumberAndString(8);
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
        else{
            AssertPostGlobalZone(interception.response.statusCode, 200,interception.response.body.EnglishName) 
        }        
    }) 
}
function ReCreateGlobalZone(){
    let globalZoneCode=  GenerateRandomNumber(8);
    cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneCode , globalZoneCode)
    cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneEnglishName , globalZoneCode)
    cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneLocalName , globalZoneCode)
    CreateGlobalZone();
    AssertCreateGlobalZone();
}
export function AssertPostGlobalZone(responseStatusCode: number, expectedStatusCode: number,globalZoneName:string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    GlobalZoneName=globalZoneName
}
export function SearchGlobalZone() {
    DefineGlobalZoneViewsGetByFiltersRequest(GlobalZoneName);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, GlobalZoneName);
    AsserGlobalZoneViewsGetByFilters();
}
export function DefineGlobalZoneViewsGetByFiltersRequest(GlobalZoneName: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(GlobalZoneName + "&GetCount=false"), RequestAliases.GetFilterSearch);
}
export function AsserGlobalZoneViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}
export function AssertSearchGlobalZone() {
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(GlobalZoneName);
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
    let LocalNameToFill = LocalName.toLowerCase() == "random" ? (gr.GenerateRandomNumberAndString(10)) : LocalName; 
    if (LocalName) {
        cy.FillLogTextBox(MaintenanceSelectors.GlobalZoneLocalName, LocalNameToFill)
    }
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
            inActiveGlobalZone= interception.response.body.InActive;
        });
}

//#endregion
//#region Commodity
export function FillCommodityCode(CommodityCode:string){
    cy.FillLogTextBox(MaintenanceSelectors.CommodityCode , CommodityCode)
}
export function FillCommodityDetails(commodityDetails:CommodityDetails){
    var RandomCommodityNumber=GetRandomCommodityCodeNumber();
    cy.FillLogTextBox(MaintenanceSelectors.CommodityCode, commodityDetails.CommodityCode.toLowerCase() == "random"?RandomCommodityNumber:commodityDetails.CommodityCode)
    cy.FillLogTextBox(MaintenanceSelectors.CommodityName, commodityDetails.CommodityName.toLowerCase() == "random"?RandomCommodityNumber:commodityDetails.CommodityName)
    FillInputCheckBoxProcess(MaintenanceSelectors.InActiveCommodityCheckBox,commodityDetails.InactiveCommodity)
}
function GetRandomCommodityCodeNumber(){
    return gr.GenerateRandomNumberAndString(15);
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
            else{
                AssertPostCommodity(interception.response.statusCode, 200,interception.response.body.Name) 
            }        
        }) 
    }
    function ReCreateCommodity(){
        var RandomCommodityNumber=GetRandomCommodityCodeNumber();
        cy.FillLogTextBox(MaintenanceSelectors.CommodityCode, RandomCommodityNumber)
        cy.FillLogTextBox(MaintenanceSelectors.CommodityName, RandomCommodityNumber)
        CreateCommodity();
        AssertCreateCommodity();
    }
    export function AssertPostCommodity(responseStatusCode: number, expectedStatusCode: number,commodityName:string) {
        assert.equal(responseStatusCode, expectedStatusCode)
        CommodityName=commodityName
    }
    export function SearchCommodity() {
        DefineCommodityViewsGetByFiltersRequest(CommodityName);
        cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, CommodityName);
        AssertCommodityViewsGetByFilters();
    }
    export function DefineCommodityViewsGetByFiltersRequest(CommodityName: string) {
        cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(CommodityName + "&GetCount=false"), RequestAliases.GetFilterSearch);
    }
    export function AssertCommodityViewsGetByFilters() {
        BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
    }
    export function AssertSearchCommodity() {
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
        let NameToFill = Name.toLowerCase() == "random" ? (gr.GenerateRandomNumberAndString(10)) : Name; 
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
                inActiveCommodity= interception.response.body.InActive;
            });
    }
//#endregion
//#region Region
export function FillRegionDetails(regionDetails:RegionDetails){
    var RandomRegionName=GetRandomRegionName();
    cy.FillLogTextBox(MaintenanceSelectors.RegionName, regionDetails.RegionName.toLowerCase() == "random"?RandomRegionName: regionDetails.RegionName)
    cy.FillLogTextBox(MaintenanceSelectors.RegionLocalName, regionDetails.RegionLocalName.toLowerCase() == "random"?RandomRegionName: regionDetails.RegionLocalName)
    FillInputCheckBoxProcess(MaintenanceSelectors.InActiveRegionCheckBox,regionDetails.InactiveRegion)
}
function GetRandomRegionName(){
    return gr.GenerateRandomNumberAndString(15);
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
        else{
            AssertPostRegion(interception.response.statusCode, 200,interception.request.body.name) 
        }        
    }) 
}
function ReCreateRegion(){
    var RandomRegionName=GetRandomCommodityCodeNumber();
    cy.FillLogTextBox(MaintenanceSelectors.RegionName, RandomRegionName)
    cy.FillLogTextBox(MaintenanceSelectors.RegionLocalName, RandomRegionName)
    CreateRegion();
    AssertCreateRegion();
}
export function AssertPostRegion(responseStatusCode: number, expectedStatusCode: number,regionName:string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    RegionName=regionName
}
export function SearchRegion() {
    DefineRegionViewsGetByFiltersRequest(RegionName);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, RegionName);
    AssertRegionViewsGetByFilters();
}
export function DefineRegionViewsGetByFiltersRequest(CommodityName: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(CommodityName + "&GetCount=false"), RequestAliases.GetFilterSearch);
}
export function AssertRegionViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}
export function AssertSearchRegion() {
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
    let NameToFill = Name.toLowerCase() == "random" ? (gr.GenerateRandomNumberAndString(15)) : Name; 
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
            inActiveRegion= interception.response.body.inActive;
        });
}
//#endregion
