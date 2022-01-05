import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { MoveTypeDetails } from "../../../cypress/models/MoveTypeDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Constants } from "../../constants/Constants";

//#region variable
let moveTypeDetails: MoveTypeDetails
//#endregion

//#region Add Move Type Code with lenght more than 3
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemMoveTypes)
});

When("add {string} as move type code", (MoveTypeCode) => {
    MaintenanceActions.OpenNewWizard(Constants.MoveType);
    MaintenanceActions.FillMoveTypeCode(MoveTypeCode);
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    MaintenanceActions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region  Create a new move type
Given("a move type with the following details", (dataTable) => {
    moveTypeDetails = Assists.CreateInstance<MoveTypeDetails>(dataTable, true);
    MaintenanceActions.FillMoveTypeDetails(moveTypeDetails)
});

When("create move type", () => {
    MaintenanceActions.CreateMoveType();
});

Then("the move type should create successfully", () => {
    MaintenanceActions.AssertCreateMoveType();
});
//#endregion

//#region Search for the move type
When("search for move type", () => {
    MaintenanceActions.SearchMoveType()
});

Then("the move type should appear successfully", () => {
    MaintenanceActions.AssertSearchMoveType()
});
//#endregion

//#region Open the move type
When("open move type", () => {
    MaintenanceActions.OpenMoveType()
});

Then("the move type should open successfully", () => {
    MaintenanceActions.AssertOpenMoveType()
});
//#endregion

//#region  Edit the move type
Given("fill move type local name", () => {
    MaintenanceActions.FillMoveTypeLocalName()
});

Given("make move type inactivate", () => {
    cy.ClickCheckBox(MaintenanceSelectors.MoveTypeInActive)
});

When("update move type", () => {
    MaintenanceActions.UpdateMoveType()
});

Then("the move type should update successfully", () => {
    MaintenanceActions.AssertUpdateMoveType()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, MaintenanceSelectors.MoveTypeEventsTab);
});
//#endregion