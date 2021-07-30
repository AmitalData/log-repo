import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { SpecialServicesTypeDetails } from "../../../cypress/models/SpecialServicesTypeDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Constants } from "../../constants/Constants";

//#region variable
let specialServicesTypeDetails: SpecialServicesTypeDetails
//#endregion

//#region Add Special Services Type Code with lenght more than 8
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemSpecialServicesType)
});

When("add {string} as special services type code", (SpecialServicesTypeCode) => {
    MaintenanceActions.OpenNewWizard(Constants.SpecialServicesType);
    MaintenanceActions.FillSpecialServicesTypeCode(SpecialServicesTypeCode);
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    MaintenanceActions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion
//#region  Create a new special services type
Given("a special services type with the following details", (dataTable) => {
    specialServicesTypeDetails = Assists.CreateInstance<SpecialServicesTypeDetails>(dataTable, true);
    MaintenanceActions.FillSpecialServicesTypeDetails(specialServicesTypeDetails)
});

When("create special services type", () => {
    MaintenanceActions.CreateSpecialServicesType();
});

Then("the special services type should create successfully", () => {
    MaintenanceActions.AssertCreateSpecialServicesType();
});

//#endregion
//#region Search for the special services type by code
When("search for special services type", () => {
    MaintenanceActions.SearchSpecialServicesType()
});

Then("the special services type should appear successfully", () => {
    MaintenanceActions.AssertSearchSpecialServicesType(specialServicesTypeDetails.EnglishName)
});

//#endregion
//#region Open the special services type
When("open special services type", () => {
    MaintenanceActions.OpenSpecialServicesType()
});

Then("the special services type should open successfully", () => {
    MaintenanceActions.AssertOpenSpecialServicesType()
});
//#endregion
//#region  Edit the special services type
Given("edit special services type local name", () => {
    MaintenanceActions.FillSpecialServicesTypeLocalName()
});

When("update special services type", () => {
    MaintenanceActions.UpdateSpecialServicesType()
});

Then("the special services type should update successfully", () => {
    MaintenanceActions.AssertUpdateSpecialServicesType()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, MaintenanceSelectors.SpecialServicesTypeEventsTab);
});
//#endregion