import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { Constants } from '../../../cypress/constants/Constants'
import { CommodityDetails } from "../../../cypress/models/CommodityDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"

//#region variable
let commodityDetails:CommodityDetails;
//#endregion
//#region Add CommodityCode with lenght less than 4
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemCommodities)
});
 
When("add {string} as Commodity code", (commodityCode) => {
    MaintenanceActions.OpenNewWizard(Constants.Commodity);
    MaintenanceActions.FillCommodityCode(commodityCode);
});
 
Then("a validation message with {string} error should appear", (ValidationMessage) => {
    MaintenanceActions.ValidateErrorPopUpMessage(ValidationMessage) 
});
 
//#endregion
 
//#region Add Commodity
Given("a commodity with the following details", (dataTable) => {
    commodityDetails = Assists.CreateInstance<CommodityDetails>(dataTable, true);
    MaintenanceActions.FillCommodityDetails(commodityDetails) 
});
 
When("add commodity", () => {
    MaintenanceActions.CreateCommodity();
});
 
Then("the commodity should add successfully", () => {
    MaintenanceActions.AssertCreateCommodity();
});
 
//#endregion
 
//#region Search for the Commodity by name
When("search for commodity", () => {
    MaintenanceActions.SearchCommodity()
});
 
Then("the commodity should appear successfully", () => {
    MaintenanceActions.AssertSearchCommodity() 
});
 
//#endregion
 
//#region Open the commodity
When("open commodity", () => {
    MaintenanceActions.OpenCommodity();
});
 
Then("the commodity should open successfully", () => {
    MaintenanceActions.AssertOpenCommodity(); 
});
 
//#endregion
 
//#region Edit the commodity
Given("a {string} as commodityName", (commoditylName) => {
    MaintenanceActions.FillCommodityName(commoditylName)
});
 
Given("the user activate commodity", () => {
    MaintenanceActions.ChangeInactiveCheckBoxValue(MaintenanceSelectors.InActiveCommodityCheckBox)
});
 
When("edit commodity", () => {
    MaintenanceActions.EditCommodity(); 
});
 
Then("the commodity should update successfully", () => {
    MaintenanceActions.AssertEditCommodity();  
});
 
Then("following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, MaintenanceSelectors.CommodityEventsTab);
});
 
//#endregion