import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import * as GeneralActions from "../../actions/BaseActions";
import { CardDetails } from "../../models/CardDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ContactDetails } from "../../models/ContactDetails";
import { CardBillingTabDetails } from "../../models/CardBillingTabDetails";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Constants } from '../../constants/Constants'

let truckerDetails: CardDetails
let contactDetails: ContactDetails
let truckerBillingTabDetails: CardBillingTabDetails

//#region Add Trucker Code with lenght more than 7
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemTrucker)
});

When("navigate trucker wizard and add {string} as trucker code", (TruckerCode) => {
    MaintenanceActions.OpenNewWizard(Constants.Trucker);
    MaintenanceActions.FillTruckerCode(TruckerCode);
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    MaintenanceActions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region Add Trucker Code already existes
When("add {string} as trucker code", (truckerCode) => {
    MaintenanceActions.FillTruckerCode(truckerCode);
    cy.get(".WindowHeader").click()
});

Then("a validation code message with {string} should appear", (validationMessage) => {
    cy.get(".CodeMessage").should("contain.text", validationMessage)
});
//#endregion

//#region Assert create trucker without code
Given("the user fill the required fields except the code", () => {
    MaintenanceActions.FillTruckerRequiredFeilds()
});

When("create trucker", () => {
    MaintenanceActions.CreateTruckerMockCreate()
});

Then("a validation single message with {string} error should appear", (validationMessage) => {
    GeneralActions.ValidateSingleErrorMessage(validationMessage)
});
//#endregion

//#region Create new trucker
Given("a trucker with the following details", (dataTable) => {
    truckerDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    MaintenanceActions.FillTruckerDetails(truckerDetails)
});

Given("a trucker contact with the following details", (dataTable) => {
    contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    MaintenanceActions.FillTruckerContactDetails(contactDetails)
});

When("create trucker", () => {
    MaintenanceActions.CreateTruckerMockCreate()
});

Then("the trucker should create successfully", () => {
    MaintenanceActions.AssertCreateTruckerMockCreate()
});
//#endregion

//#region Search for the trucker by code
When("search for {string} trucker", (trucker) => {
    MaintenanceActions.SearchCardByValue(trucker)
});

Then("the {string} trucker should appear successfully", (trucker) => {
    MaintenanceActions.AssertSearchTrucker(trucker)
});
//#endregion

//#region Open the trucker
When("open trucker", () => {
    MaintenanceActions.OpenCard(Constants.Trucker)
});

Then("the trucker should open successfully", () => {
    MaintenanceActions.AssertOpenTrucker()
});
//#endregion

//#region Edit the trucker
Given("{string} as trucker notes", (notes) => {
    MaintenanceActions.FillTruckerGenaralTabNotes(notes)
});

Given("fill the following trucker Billing details", (dataTable) => {
    truckerBillingTabDetails = Assists.CreateInstance<CardBillingTabDetails>(dataTable, true);
    MaintenanceActions.FillTruckerBillingTab(truckerBillingTabDetails)
});

When("update trucker", () => {
    MaintenanceActions.UpdateTrucker()
});

Then("the trucker should update successfully", () => {
    MaintenanceActions.AssertUpdateTrucker()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, MaintenanceSelectors.TruckerEventsTab);
});
//#endregion