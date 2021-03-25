import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { StateDetails } from "../../../cypress/models/StateDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"

//#region variable
let stateDetails: StateDetails
//#endregion

//#region Add StateCode with lenght more than 10
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemState)
});

When("add {string} as state code", (stateCode) => {
    MaintenanceActions.OpenNewWizard("State");
    cy.FillLogTextBox(MaintenanceSelectors.StateCode, stateCode);
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    MaintenanceActions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region Add State
Given("a state with the following details", (dataTable) => {
    stateDetails = Assists.CreateInstance<StateDetails>(dataTable, true);
    MaintenanceActions.FillStateDetails(stateDetails)
});

When("add state", () => {
    MaintenanceActions.CreateState();
});

Then("the state should add successfully", () => {
    MaintenanceActions.AssertCreateState();
});
//#endregion

//#region Search for the State by name
When("search for {string} state", (State) => {
    MaintenanceActions.SearchState(State)
});

Then("the {string} state should appear successfully", (State) => {
    MaintenanceActions.AssertSearchState(State);
});
//#endregion

//#region Open the State
When("open state", () => {
    MaintenanceActions.OpenState();
});

Then("the state should open successfully", () => {
    MaintenanceActions.AssertOpenState()
});
//#endregion

//#region Edit the State
Given("a {string} as stateLocalName", (NewStateLocalName) => {
    MaintenanceActions.FillStateLocalName(NewStateLocalName)
});

Given("the user change Inactivestate check box", () => {
    MaintenanceActions.ChangeInactiveCheckBoxValue(MaintenanceSelectors.InActiveStateCheckBox)
});

When("edit state", () => {
    MaintenanceActions.EditState();
});

Then("the state should update successfully", () => {
    MaintenanceActions.AssertPutState();
});

Then("following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    eventDetailsList = MaintenanceActions.StateConversionEventsMapping(eventDetailsList)
    BaseActions.ValidateEventsTab(eventDetailsList, MaintenanceSelectors.StateEventTab);
});
//#endregion