import * as Actions from "../../actions/Actions"
import * as ShipmentViewActions from "../../actions/ShipmentViewActions"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";

//#region Create New Shipment View
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});

Given("navigates shipment view and fill {string} as view name", (viewName) => {
    ShipmentViewActions.NavigateToShipmentViewWizerd()
    ShipmentViewActions.FillShipmentViewName(viewName)
});

Given("add {string} column to the selected columns", (branch) => {
    ShipmentViewActions.AddColumnToSelectedCoulmns(branch)
});

When("create view", () => {
    ShipmentViewActions.CreateShipmentView()
});

Then("the view should create successfully", () => {
    ShipmentViewActions.AssertCreateShipmentView()
});
//#endregion

//#region Edit the Shipment View
Given("edit the view", () => {
    ShipmentViewActions.EditShipmentView()
});

When("update the view", () => {
    ShipmentViewActions.UpdateShipmentView()
});

Then("the view should update successfully", () => {
    ShipmentViewActions.AssertUpdateShipmentView()
});
//#endregion

//#region Delete the Shipment View
When("delete the view", () => {
    ShipmentViewActions.DeleteShipmentView()
});

Then("the view should delete successfully", () => {
    ShipmentViewActions.AssertDeleteShipmentView()
});
//#endregion