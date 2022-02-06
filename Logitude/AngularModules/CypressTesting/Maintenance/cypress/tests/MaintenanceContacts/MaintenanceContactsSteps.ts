import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ContactDetails } from "../../models/ContactDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";

Given("the user logged in and open contacts menu", () => {
    cy.Login();
    cy.Click(BaseSelectors.ContactsMenu, null)
});

Given("a contact with the following details", (dataTable) => {
    //Actions.OpenContactsList();
    Actions.OpenNewContactWizard();
    FillContactDetails(dataTable);
});

Given("the user fill the following contact details", (dataTable) => {
    FillContactDetails(dataTable);
});

When("create contact", () => {
    Actions.CreateContact();
});

Then("the contact should create successfully", () => {
    Actions.AssertCreateContact();
});

When("search contact", () => {
    Actions.SearchContact();
});

Then("the contact should appear successfully", () => {
    Actions.AssertSearchContact();
});

When("open contact", () => {
    Actions.OpenContact();
});

Then("the contact should open successfully", () => {
    Actions.AssertOpenContact();
});

When("edit contact", () => {
    Actions.EditContact();
});

Then("the contact should edit successfully", () => {
    Actions.AssertEditContact();
});

When("anonymize contact", () => {
    Actions.AnonymizeContact();
});

Then("the contact should anonymize successfully", () => {
    Actions.AssertAnonymizeContact();
});

Then("contact details should change successfully", () => {
    Actions.AssertContactDetailsValues();
});

function FillContactDetails(dataTable: any) {
    let contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    Actions.FillContactDetails(contactDetails);
}