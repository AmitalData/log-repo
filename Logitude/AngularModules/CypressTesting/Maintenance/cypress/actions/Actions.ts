import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { Terms } from "../constants/Terms";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import * as BaseActions from "../../../Base/cypress/actions/Actions";
import * as gr from '../../../Base/cypress/actions/GenerateRandoms'
import { ContactDetails } from "../models/ContactDetails";
import { ContactContext } from "../models/ContactContext";
import { BaseURLs } from "../../../Base/cypress/constants/URLs";
import { Datepicker } from "../../../Base/cypress/models/Datepicker";

export function OpenMaintenanceMenu() {
    cy.Click(BaseSelectors.MaintenanceMenu, null)
}

//////////Contacts
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
    FillContactDatepicker(contactDetails.BirthdayDate, Terms.Birthday);
    FillContactDateReminder(contactDetails.BirthdayReminder, Terms.Birthday);
    FillContactDatepicker(contactDetails.AnniversaryDate, Terms.Anniversary);
    FillContactDateReminder(contactDetails.AnniversaryReminder, Terms.Anniversary);
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
    AssertContactDatepickerNotSelected(Terms.Birthday);
    AssertContactDatepickerNotSelected(Terms.Anniversary);
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

function FillContactNotes(notes: string){
    if(notes){
        cy.FillLogTextBox(MaintenanceSelectors.ContactNotes, notes);
    }
}

function FillContactDatepicker(contactDate: string, dateType:string) {
    if (contactDate && dateType) {
        let contactDatepicker: Datepicker = BaseActions.GetDatepicker(contactDate);
        let indexOfDatepicker: number;
        if(dateType.toLowerCase() == Terms.Birthday){
            indexOfDatepicker = 0;
        }
        else if(dateType.toLowerCase() == Terms.Anniversary){
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

function SelectComboBoxToggleItem(value: string){
    cy.get(BaseSelectors.ToggleIconImage).click();
    cy.get(BaseSelectors.SpanTitle(value)).parent().click();
}

function FillContactDateReminder(reminder: string, dateType:string){
    if(reminder && dateType){
        let reminderCheckboxSelector = GetContactDateReminderSelector(dateType);
        if(reminder.toLowerCase() == "yes"){
            cy.get(reminderCheckboxSelector).check({ force: true });
        }
        else{
            cy.get(reminderCheckboxSelector).uncheck({ force: true });
        }
    }
}

function GetContactDateReminderSelector(dateType:string){
    let reminderCheckboxSelector: string;
    if(dateType.toLowerCase() == Terms.Birthday){
        reminderCheckboxSelector = MaintenanceSelectors.ContactBirthdayReminder;
    }
    else if(dateType.toLowerCase() == Terms.Anniversary){
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

function AssertContactInputHaveValue(inputSelector: string, value: string){
    BaseAssertion.AssertElementHaveValue(inputSelector, value);
}

function AssertContactDatepickerNotSelected(dateType:string){
    let indexOfDatepicker: number;
    if(dateType.toLowerCase() == Terms.Birthday){
        indexOfDatepicker = 0;
    }
    else if(dateType.toLowerCase() == Terms.Anniversary){
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
//////////