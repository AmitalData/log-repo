import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { Constants } from '../../../cypress/constants/Constants'
import { GlobalZoneDetails } from "../../../cypress/models/GlobalZoneDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"

//#region variable
let globalZoneDetails:GlobalZoneDetails;
//#endregion
//#region Add GlobalZoneCode with lenght more than 8
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemGlobalZone)
});
 
When("add {string} as Global Zone code", (globalZoneCode) => {
    MaintenanceActions.OpenNewWizard(Constants.GlobalZone);
    MaintenanceActions.FillGlobalZoneCode(globalZoneCode);
});
 
Then("a validation message with {string} error should appear", (ValidationMessage) => {
    MaintenanceActions.ValidateErrorPopUpMessage(ValidationMessage) 
});
 
//#endregion
 
//#region Add Global Zone
Given("a global zone with the following details", (dataTable) => {
    globalZoneDetails = Assists.CreateInstance<GlobalZoneDetails>(dataTable, true);
    MaintenanceActions.FillGlobalZoneDetails(globalZoneDetails) 
});
 
When("add global zone", () => {
    MaintenanceActions.CreateGlobalZone();
});
 
Then("the global zone should add successfully", () => {
    MaintenanceActions.AssertCreateGlobalZone();
});
 
//#endregion
 
//#region Search for the global zone by name
When("search for {string} global zone", (globalZoneName) => {
    MaintenanceActions.SearchGlobalZone(globalZoneName)
});
 
Then("the {string} global zone should appear successfully", (globalZoneName) => {
    MaintenanceActions.AssertSearchGlobalZone(globalZoneName) 
});
 
//#endregion
 
//#region Open the global zone
When("open global zone", () => {
    MaintenanceActions.OpenGlobalZone();
});
 
Then("the global zone should open successfully", () => {
    MaintenanceActions.AssertOpenGlobalZone(); 
});
 
//#endregion
 
//#region Edit the global zone
Given("a {string} as globalZoneLocalName", (globalZoneLocalName) => {
    MaintenanceActions.FillGlobalZoneLocalName(globalZoneLocalName)
});
 
Given("the user change InactiveGlobalZone check box", () => {
    MaintenanceActions.ChangeInactiveCheckBoxValue(MaintenanceSelectors.InActiveGlobalZoneCheckBox)
});
 
When("edit global zone", () => {
    MaintenanceActions.EditGlobalZone(); 
});
 
Then("the global zone should update successfully", () => {
    MaintenanceActions.AssertEditGlobalZone();  
});
 
Then("following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    eventDetailsList = MaintenanceActions.GlobalZoneConversionEventsMapping(eventDetailsList)
    BaseActions.ValidateEventsTab(eventDetailsList, MaintenanceSelectors.GlobalZoneEventsTab);
});
 
//#endregion