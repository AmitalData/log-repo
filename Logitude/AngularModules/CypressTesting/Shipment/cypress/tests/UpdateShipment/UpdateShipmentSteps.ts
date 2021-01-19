import * as Actions from "../../actions/Actions"
import * as Assertions from "../../actions/Assertions"
import { Selectors } from "../../selectors/Selectors"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";

Given("User logged in", () => {
  cy.Login()
});

Given("Go to shipments workspace", () => {
  cy.Click(BaseSelectors.OperationsMenu, null)
  cy.Click(Selectors.ShipmentTab, null)
});

Given("Open shipment {string}", (shipmentJsonObject) => {
  Actions.OpenShipment(shipmentJsonObject)
});

Given("The user in the general tab", () => {
    cy.Click(Selectors.GeneralTab, null)
});

When("Fill general tab and click save button", () => {
    Actions.FillGeneralTab()
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Given("The user in the Orders tab", () => {
    cy.Click(Selectors.OrdersTab, null)
});

When("Fill Orders tab and click save button", () => {
    Actions.FillOrdersTab()
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Given("The user in the Partners tab", () => {
    cy.Click(Selectors.PartnersTab, null)
});

When("Fill Partners tab and click save button", () => {
    Actions.FillPartnersTab("E","A")
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Given("The user in the Packages tab", () => {
    cy.Click(Selectors.PackagesTab, null)
});

When("Fill Packages tab and click save button", () => {
    Actions.FillPackagesTab("A")
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Given("The user in the Receivables tab", () => {
    cy.Click(Selectors.ReceivablesTab, null)
});

When("Fill Receivables tab and click save button", () => {
    Actions.FillReceivablesTab()
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Given("The user in the Routings tab", () => {
    cy.Click(Selectors.RoutingsTab, null)
});

When("Add new pickup routing and click save button", () => {
    Actions.FillPickupRouting()
    Actions.UpdateShipment(Selectors.SaveClose)
});

When("Add new delivery routing and click save button", () => {
    Actions.FillDeliveryRouting()
    Actions.UpdateShipment(Selectors.SaveClose)
});

When("Add carriage routings and click save button", () => {
    Actions.FillPreCarriageRouting("A")
    Actions.FillOnCarriageRouting("A")
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Given("The user in the Payables tab", () => {
    cy.Click(Selectors.PayablesTab, null)
});

When("Add Payables and click save button", () => {
    Actions.FillPayablesTab()
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Then("The save operation complete successfully", () => {
    Assertions.ValidateUpdatedShipment(null);
});