import * as Actions from "../../actions/Actions"
import * as Assertions from "../../actions/Assertions"
import { Selectors } from "../../selectors/Selectors"
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Given, When, Then,And } from "cypress-cucumber-preprocessor/steps";
import { PartnersDetails } from "cypress/models/PartnersDetails";
let ShipmentData: ShipmentDetails;
let ShipmentFile;
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(Selectors.ShipmentTab, null)
});

Given("a direct shipment with the following details",(dataTable) => {
    const shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    ShipmentData = shipmentDetails;
    Actions.OpenNewShipmentWizard(ShipmentData.ShipmentLevel);
    Actions.FillShipmentDefaultFields(ShipmentData);
});

When("create shipment", () => {
    Actions.CreateShipment(ShipmentData.ShipmentLevel);
});

Then("the shipment should create successfully", () => {
    ShipmentFile = "CreatedShipmentsData/" + ShipmentData.ShipmentLevel + ShipmentData.Direction +
    ShipmentData.TransportMode +
    ((typeof ShipmentData.ShipmentType) === "undefined" || ShipmentData.ShipmentType === null ? "" : ShipmentData.ShipmentType) + ".json";
  Assertions.ValidateCreatedShipment(ShipmentFile);
});

Given("a receivable with the following details", (dataTable) => {
    const ReceivableData = dataTable.hashes()[0] as ReceivableDetails;
    Actions.OpenShipment(ShipmentFile)
    cy.Click(Selectors.ReceivablesTab, null)
    Actions.FillReceivableTab(ReceivableData)
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Given("a credit ARInvoice with the following details", () => {
});

When("create invoice", () => {
});

Then("the invoice should create successfully", () => {
});


When("approve invoice", () => {
});

Then("the invoice should approve successfully", () => {
});

When("set invoice as sent", () => {
});

Then("the invoice should set as sent successfully", () => {
});


When("void invoice", () => {
});

Then("the invoice should void successfully", () => {
});
