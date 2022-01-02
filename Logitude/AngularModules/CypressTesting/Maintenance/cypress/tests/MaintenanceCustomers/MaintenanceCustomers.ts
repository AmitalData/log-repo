import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { CardDetails } from "../../models/CardDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ContactDetails } from "../../models/ContactDetails";
import { CardBillingTabDetails } from "../../models/CardBillingTabDetails";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Constants } from '../../constants/Constants'
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";



let customerDetails: CardDetails
let contactDetails: ContactDetails
let customerBillingTabDetails: CardBillingTabDetails


//#region Create new customer
Given("the user logged in and open {string} in maintenance menu", (customerTabItem) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(customerTabItem, MaintenanceSelectors.MaintenanceItemCustomer);
});

Given("a customer with the following details", (dataTable) => {
    customerDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    MaintenanceActions.OpenNewWizard("Customer");
    MaintenanceActions.FillCardDetails(customerDetails,null)
});

Given("a customer contact with the following details", (dataTable) => {
    contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    MaintenanceActions.FillCustomerContactDetails(contactDetails)
});

When("create customer", () => {
    MaintenanceActions.CreateCustomer();
});

Then("the customer should create successfully", () => {
    MaintenanceActions.AssertCreateCustomer()
});
//#endregion
When("search customer", () => {
    MaintenanceActions.SearchCustomerCard();
});

Then("the customer should appear successfully", () => {
    MaintenanceActions.AssertSearchCustomer(customerDetails.CompanyName);
});
//#region Open the customer
When("open the customer", () => {
    MaintenanceActions.OpenCard(Constants.Customer)
});

Then("the customer should open successfully", () => {
    MaintenanceActions.AssertOpenCustomer()
});

Then("the customer address should have the following details", (dataTable) => {
    customerDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    MaintenanceActions.AssertCustomerAddress(customerDetails)
});

//#endregion

//#region Edit the customer
Given("{string} as customer StorageFreeDays", (StorageFreeDays) => {
    MaintenanceActions.FillCustomerGeneralTabStorageFreeDays(StorageFreeDays)
});

Given("inactivate the customer", () => {
    cy.Click(BaseSelectors.MenuButtons, null, true);
    cy.Click(MaintenanceSelectors.InActiveCustomer, null, true)
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsConfirm);
});

Given("fill the following customer Billing details", (dataTable) => {
    customerBillingTabDetails = Assists.CreateInstance<CardBillingTabDetails>(dataTable, true);
    MaintenanceActions.FillCustomerBillingTab(customerBillingTabDetails)
});

When("update customer", () => {
    MaintenanceActions.UpdateCustomer()
});

Then("the customer should update successfully", () => {
    MaintenanceActions.AssertUpdateCustomer()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, MaintenanceSelectors.CustomerEventsTab);
});