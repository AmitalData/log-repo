import * as Actions from "../../actions/Actions";
import * as BusinessUnitsActions from "../../actions/BusinessUnitsActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { BusinessUnitSelectors } from "../../selectors/BusinessUnitSelectors";
import { BusinessUnitDetails } from "cypress/models/BusinessUnitDetails";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"

//#region Create new business unit
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, BusinessUnitSelectors.MaintenanceItem)
});

Given("a business unit with the following details", (dataTable) => {
    let businessUnitDetails = Assists.CreateInstance<BusinessUnitDetails>(dataTable, true);
    Actions.OpenNewWizard("BusinessUnit");
    BusinessUnitsActions.FillBusinessUnitDetails(businessUnitDetails);
});

When("create business unit", () => {
    BusinessUnitsActions.CreateBusinessUnit();
});

Then("the business unit should create successfully", () => {
    BusinessUnitsActions.AssertCreateBusinessUnit();
});
//#endregion

//#region Search for the business unit
When("search business unit", () => {
    BusinessUnitsActions.SearchBusinessUnit()
});

Then("the business unit should appear successfully", () => {
    BusinessUnitsActions.AssertSearchBusinessUnit();
});
//#endregion

//#region Open the business unit
When("open business unit", () => {
    BusinessUnitsActions.OpenBusinessUnit();
});

Then("the business unit should open successfully", () => {
    BusinessUnitsActions.AssertOpenBusinessUnit();
});
//#endregion

//#region Edit the business unit
Given("the user edit the following business unit details and the parent should be dim", (dataTable) => {
    let businessUnitDetails = Assists.CreateInstance<BusinessUnitDetails>(dataTable, true);
    BusinessUnitsActions.EditBusinessUnitGeneralTab(businessUnitDetails)
    BusinessUnitsActions.AssertParentDisabled()
});

When("save business unit", () => {
    BusinessUnitsActions.UpdateBusinessUnit()
});

Then("the business unit should update successfully", () => {
    BusinessUnitsActions.AssertUpdateBusinessUnit()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, BusinessUnitSelectors.EventsTab);
});
//#endregion

//#region Save and close the business unit
When("save and close business unit", () => {
    BusinessUnitsActions.CloseSaveBusinessUnit();
});

Then("the business unit should close successfully", () => {
    BusinessUnitsActions.AssertCloseSaveBusinessUnit();
});
 //#endregion