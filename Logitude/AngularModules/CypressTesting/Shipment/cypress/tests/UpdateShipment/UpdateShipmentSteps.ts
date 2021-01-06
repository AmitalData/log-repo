import * as sh from "../../actions/Actions"
import { Selectors } from "../../selectors/Selectors"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";

Given("User logged in successfully", () => {
  cy.Login()
});

Given("Go to shipments workspace", () => {
  cy.Click(BaseSelectors.OperationsMenu, null)
  cy.Click(Selectors.ShipmentTab, null)
});

Given("Open the direct Shipment {string}", (shipmentJsonObject) => {
  sh.OpenShipment(shipmentJsonObject)
});

Given("The user in the general work space", () => {
    cy.Click(Selectors.GeneralTab, null)
});

When("The user fill general tab and click save button", () => {
    sh.FillGeneralTab()
    cy.Click(Selectors.ShipmentSaveButton, null)
});

Given("The user in the Orders work space", () => {
    cy.Click(Selectors.OrdersTab, null)
});

When("The user fill Orders tab and click save button", () => {
    sh.FillOrdersTab()
    cy.Click(Selectors.ShipmentSaveButton, null)
});

Given("The user in the Partners work space", () => {
    cy.Click(Selectors.PartnersTab, null)
});

When("The user fill Partners tab and click save button", () => {
    sh.FillPartnersTab("E","A")
    cy.Click(Selectors.ShipmentSaveButton, null)
});

Given("The user in the Packages work space", () => {
    cy.Click("#ShipmentTHPackages", null)
});

When("The user fill Packages tab and click save button", () => {
    sh.FillPackagesTab("A")
    cy.Click(Selectors.ShipmentSaveButton, null)
});

Then("The save operation complete successfully", () => {
    cy.intercept('PUT', '/test/api/shipment', (req) => {
        req.reply((response) => {
            assert.equal(response.statusCode, 200);
        })
    })
});