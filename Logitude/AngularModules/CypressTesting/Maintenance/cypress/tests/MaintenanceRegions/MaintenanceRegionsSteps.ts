import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { Constants } from '../../../cypress/constants/Constants'
import { RegionDetails } from "../../../cypress/models/RegionDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"

//#region variable
let regionDetails:RegionDetails;
//#endregion
 
//#region Add region
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemRegions)
});
Given("a region with the following details", (dataTable) => {
    regionDetails = Assists.CreateInstance<RegionDetails>(dataTable, true);
    MaintenanceActions.OpenNewWizard(Constants.Region);
    MaintenanceActions.FillRegionDetails(regionDetails) 
});
 
When("add region", () => {
    MaintenanceActions.CreateRegion();
});
 
Then("the region should add successfully", () => {
    MaintenanceActions.AssertCreateRegion();
});
 
//#endregion
 
//#region Search for the region by name
When("search for region", () => {
    MaintenanceActions.SearchRegion()
});
 
Then("the region should appear successfully", () => {
    MaintenanceActions.AssertSearchRegion() 
});
 
//#endregion
 
//#region Open the region
When("open region", () => {
    MaintenanceActions.OpenRegion();
});
 
Then("the region should open successfully", () => {
    MaintenanceActions.AssertOpenRegion(); 
});
 
//#endregion
 
//#region Edit the region
Given("a {string} as regionLocalName", (commoditylName) => {
    MaintenanceActions.FillRegionLocalName(commoditylName)
});
 
Given("the user activate region", () => {
    MaintenanceActions.ChangeInactiveCheckBoxValue(MaintenanceSelectors.InActiveRegionCheckBox)
});
 
When("edit region", () => {
    MaintenanceActions.EditRegion(); 
});
 
Then("the region should update successfully", () => {
    MaintenanceActions.AssertEditRegion();  
});
 
Then("following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, MaintenanceSelectors.RegionEventsTab);
});
 
//#endregion