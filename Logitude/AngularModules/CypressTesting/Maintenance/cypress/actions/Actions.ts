import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { Terms } from "../constants/Terms";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import * as gr from '../../../Base/cypress/actions/GenerateRandoms'
import { ContactDetails } from "../models/ContactDetails";
import { ContactContext } from "../models/ContactContext";
import { BaseURLs } from "../../../Base/cypress/constants/URLs";
import { Datepicker } from "../models/Datepicker";

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
        let contactDatepicker: Datepicker = GetContactDatepicker(contactDate);
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

function GetContactDatepicker(contactDate: string): Datepicker{
    let today = new Date();
    let contactDateDay: number;
    let contactDateMonth: number;
    let contactDateYear: number;

    if (contactDate.toLowerCase() == "today") {
        contactDateDay = today.getDate();
        contactDateMonth = today.getMonth() + 1;
        contactDateYear = today.getFullYear();
    }
    else if (contactDate.toLowerCase() == "random") {
        contactDateMonth = gr.GenerateRandomNumber(1, 12);
        contactDateYear = gr.GenerateRandomNumber(1950, today.getFullYear());
        contactDateDay = GetRandomDay(contactDateMonth, contactDateYear);
    }
    else{
        let date = new Date(contactDate);
        contactDateDay = date.getDate();
        contactDateMonth = date.getMonth() + 1;
        contactDateYear = date.getFullYear();
    }

    let datepicker = new Datepicker();
    datepicker.Day = contactDateDay;
    datepicker.Month = contactDateMonth;
    datepicker.Year = contactDateYear;
    return datepicker;
}

function GetRandomDay(month: number, year: number){
    let maxDay = 0;
    if(month == 2){
        if(year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)){
            maxDay = 29;
        }
        else{
            maxDay = 28;
        }
    }
    else if([1, 3, 5, 7, 8, 10, 12].indexOf(month) != -1){
        maxDay = 31;
    }
    else if([4, 6, 9, 11].indexOf(month) != -1){
        maxDay = 30;
    }

    return gr.GenerateRandomNumber(1, maxDay);
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
    cy.get(inputSelector).should("have.value", value);
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
            cy.get(BaseSelectors.SelectedComboboxItem).should("not.exist");
        });
        cy.get(BaseSelectors.ComboBox).eq(1).within(() => {
            cy.get(BaseSelectors.SelectedComboboxItem).should("not.exist");
        });
        cy.get(BaseSelectors.ComboBox).eq(2).within(() => {
            cy.get(BaseSelectors.SelectedComboboxItem).should("not.exist");
        });
    });
}
//////////