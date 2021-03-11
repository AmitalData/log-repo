import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ContactDetails } from "../../models/ContactDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";

Given("the user logged in and navigate to maintenance menu", () => {
    Actions.LoginAndNavigateMaintenanceMenu();
});

Given("open contacts list", () => {
    Actions.OpenContactsList();
});

Given("open new contact wizard", () => {
    Actions.OpenNewContactWizard();
});

Given("fill the following contact details", (dataTable) => {
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

function FillContactDetails(dataTable: any){
    let contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    Actions.FillContactDetails(contactDetails);
}