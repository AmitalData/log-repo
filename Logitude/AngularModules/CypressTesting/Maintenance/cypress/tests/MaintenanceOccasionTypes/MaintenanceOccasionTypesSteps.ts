import * as Actions from "../../actions/Actions";
import * as OccasionTypeActions from "../../actions/OccasionTypeActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { OccasionTypeSelectors } from "../../selectors/OccasionTypeSelectors";
import { OccasionTypeDetails } from "cypress/models/OccasionTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";

//#region Create new occasion type
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, OccasionTypeSelectors.MaintenanceItem)
});

Given("a occasion type with the following details", () => {
    Actions.OpenNewWizard("OccasionType");
    OccasionTypeActions.FillOccasionTypeDetails();
});

When("create occasion type", () => {
    OccasionTypeActions.CreateOccasionType();
});

Then("the occasion type should create successfully", () => {
    OccasionTypeActions.AssertCreateOccasionType();
});
//#endregion

//#region Search for the occasion type
When("search occasion type", () => {
    OccasionTypeActions.SearchOccasionType()
});

Then("the occasion type should appear successfully", () => {
    OccasionTypeActions.AssertSearchOccasionType();
});
//#endregion

//#region Open the occasion type
When("open occasion type", () => {
    OccasionTypeActions.OpenOccasionType();
});

Then("the occasion type should open successfully", () => {
    OccasionTypeActions.AssertOpenOccasionType();
});
//#endregion

//#region Edit the occasion type
Given("the user edit the following occasion type details", () => {
    OccasionTypeActions.EditOccasionTypeGeneralTab()
});

When("save occasion type", () => {
    OccasionTypeActions.UpdateOccasionType()
});

Then("the occasion type should update successfully", () => {
    OccasionTypeActions.AssertUpdateOccasionType()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, OccasionTypeSelectors.EventsTab);
});
//#endregion

//#region Save and close the occasion type
When("save and close occasion type", () => {
    OccasionTypeActions.CloseSaveOccasionType();
});

Then("the occasion type should close successfully", () => {
    OccasionTypeActions.AssertCloseSaveOccasionType();
});
 //#endregion