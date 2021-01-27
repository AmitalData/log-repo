import * as Actions from "../../actions/Actions"
import * as Assertions from "../../actions/Assertions"
import { Selectors } from "../../selectors/Selectors"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { PartnersDetails } from "cypress/models/PartnersDetails";

Given("User logged in", () => {
  cy.Login()
});

Given("Go to shipments workspace", () => {
  cy.Click(BaseSelectors.OperationsMenu, null)
  cy.Click(Selectors.ShipmentTab, null)
});

Given("Create a new Direct shipment", () => {
    Actions.CreateAnewShipment("Direct")
    Actions.OpenShipment("CreatedShipmentsData/DEA.json")
});

Given("The user in the general tab", () => {
    cy.Click(Selectors.GeneralTab, null)
});

When("Fill general tab with a random GrossWeight and {string} as a MoveType and click save button", (MoveType) => {
    Actions.FillGeneralTab(MoveType)
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Given("The user in the Orders tab", () => {
    cy.Click(Selectors.OrdersTab, null)
});

When("Fill Orders tab with random number of Packages and click save button", () => {
    Actions.FillOrdersTab()
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Given("The user in the Partners tab", () => {
    cy.Click(Selectors.PartnersTab, null)
});

When("Fill Partners tab with following partners and click save button", (dataTable) => {
    const partnersDetails = dataTable.hashes()[0] as PartnersDetails;
    Actions.FillPartnersTab("E","A", partnersDetails)
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Given("The user in the Packages tab", () => {
    cy.Click(Selectors.PackagesTab, null)
});

When("Fill Packages tab with random number of Packages and click save button", () => {
    Actions.FillPackagesTab("A")
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Given("The user in the Receivables tab", () => {
    cy.Click(Selectors.ReceivablesTab, null)
});

When("Fill Receivables tab  with a random UnitPrice and {string} as a ChargesType and click save button", (ChargesType) => {
    Actions.FillReceivablesTab(ChargesType)
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Given("The user in the Routings tab", () => {
    cy.Click(Selectors.RoutingsTab, null)
});

When("Add new pickup routing and click save button", () => {
    Actions.FillPickupRouting()
    Actions.UpdateShipment(Selectors.SaveClose)
});

When("Add new delivery with {string} as a partner routing and click save button", (partner) => {
    Actions.FillDeliveryRouting(partner)
    Actions.UpdateShipment(Selectors.SaveClose)
});

When("Add carriage routings from port {string} to port {string} and click save button", (fromPort, toPort) => {
    Actions.FillPreCarriageRouting("A", fromPort, toPort)
    Actions.FillOnCarriageRouting("A", fromPort, toPort)
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Given("The user in the Payables tab", () => {
    cy.Click(Selectors.PayablesTab, null)
});

When("Add Payables with {string} as a ChargesType, {string} as a Currency and random UOM and click save button", (chargesType, currency) => {
    Actions.FillPayablesTab(chargesType, currency)
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Then("The save operation complete successfully", () => {
    Assertions.ValidateUpdatedShipment(null);
});