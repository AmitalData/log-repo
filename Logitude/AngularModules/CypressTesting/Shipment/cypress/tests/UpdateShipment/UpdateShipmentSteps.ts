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

Given("Open the direct Shipment {string}", (shipmentJsonObject) => {
  Actions.OpenShipment(shipmentJsonObject)
});

Given("The user in the general work space", () => {
    cy.Click(Selectors.GeneralTab, null)
});

When("Fill general tab and click save button", () => {
    Actions.FillGeneralTab()
    cy.Click(Selectors.ShipmentSaveButton, null)
});

Given("The user in the Orders work space", () => {
    cy.Click(Selectors.OrdersTab, null)
});

When("Fill Orders tab and click save button", () => {
    Actions.FillOrdersTab()
    cy.Click(Selectors.ShipmentSaveButton, null)
});

Given("The user in the Partners work space", () => {
    cy.Click(Selectors.PartnersTab, null)
});

When("Fill Partners tab and click save button", () => {
    Actions.FillPartnersTab("E","A")
    cy.Click(Selectors.ShipmentSaveButton, null)
});

Given("The user in the Packages work space", () => {
    cy.Click(Selectors.PackagesTab, null)
});

When("Fill Packages tab and click save button", () => {
    Actions.FillPackagesTab("A")
    cy.Click(Selectors.ShipmentSaveButton, null)
});

Given("The user in the Receivables work space", () => {
    cy.Click(Selectors.ReceivablesTab, null)
});

When("Fill Receivables tab and click save button", () => {
    Actions.FillReceivablesTab()
    cy.Click(Selectors.ShipmentSaveButton, null)
});

Then("The save operation complete successfully", () => {
    Assertions.SaveOperationCompletedSuccessfully();
});