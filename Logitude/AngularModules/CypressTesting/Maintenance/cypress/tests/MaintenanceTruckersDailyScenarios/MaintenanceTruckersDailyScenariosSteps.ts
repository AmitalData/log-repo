import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { CardDetails } from "../../models/CardDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ContactDetails } from "../../models/ContactDetails";
import {CardGeneralTabDetails} from "../../models/CardGeneralTabDetails";
import { CardBillingTabDetails } from "../../models/CardBillingTabDetails";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Constants } from '../../constants/Constants'

let truckerDetails: CardDetails
let contactDetails: ContactDetails
let truckerGeneralTabDetails:CardGeneralTabDetails
let truckerBillingTabDetails:CardBillingTabDetails
//#region Create new trucker
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemTrucker)
});

Given("a trucker with the following details", (dataTable) => {
    truckerDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    MaintenanceActions.OpenNewWizard(Constants.Trucker);
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
Given("the user fill the following trucker general details", (dataTable) => {
    truckerGeneralTabDetails = Assists.CreateInstance<CardGeneralTabDetails>(dataTable, true);
    MaintenanceActions.FillTruckerGeneralTab(truckerGeneralTabDetails)
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