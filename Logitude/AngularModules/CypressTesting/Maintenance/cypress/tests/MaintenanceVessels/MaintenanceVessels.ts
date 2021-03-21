import * as Actions from "../../actions/Actions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { VesselDetails } from "../../models/VesselDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import { MaintenanceSelectors } from "../../selectors/Selectors";

let vesselDetails: VesselDetails;

//#region Create new vessel
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenTabInMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.VesselMaintenanceItem)
});

Given("a vessel with the following details", (dataTable) => {
    vesselDetails = Assists.CreateInstance<VesselDetails>(dataTable, true);
    Actions.OpenNewWizard("Vessel");
    Actions.FillVesselDetails(vesselDetails);
});

When("create vessel", () => {
    Actions.CreateVessel();
});

Then("the vessel should create successfully", () => {
    Actions.AssertCreateVessel();
});
//#endregion

//#region Search for the vessel
When("search vessel", () => {
    Actions.SearchVessel()
});

Then("the vessel should appear successfully", () => {
    Actions.AssertSearchVessel();
});
//#endregion

//#region Open the vessel
When("open vessel", () => {
    Actions.OpenVessel()
});

Then("the vessel should open successfully", () => {
    Actions.AssertOpenVessel()
});
//#endregion

//#region Edit the vessel
Given("the user fill the following vessel details", (dataTable) => {
    let vesselDetails = Assists.CreateInstance<VesselDetails>(dataTable, true);
    Actions.FillVesselGeneralTab(vesselDetails);
});

When("edit vessel", () => {
    Actions.UpdateVessel()
});

Then("the vessel should update successfully", () => {
    Actions.AssertUpdateVessel()
});
//#endregion

//#region Save and close the vessel
When("save and close vessel", () => {
    Actions.CloseSaveVessel()
});

Then("the vessel should close successfully", () => {
    Actions.AssertCloseSaveVessel();
});

//#endregion