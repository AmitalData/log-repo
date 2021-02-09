import * as Actions from "../../actions/Actions"
import { Selectors } from "../../selectors/Selectors"
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { Given, When, Then,And } from "cypress-cucumber-preprocessor/steps";
import {ReceivableDetails}from"cypress/models/ReceivableDetails"
import { ARInvoiceDetails } from "cypress/models/ARInvoiceDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";

let ShipmentData: ShipmentDetails;
let shipmentNumber: string;

Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});
Given("a direct shipment with the following details",(dataTable) => {
    const shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    ShipmentData = shipmentDetails;
    Actions.OpenNewShipmentWizard(ShipmentData.ShipmentLevel);
    Actions.FillShipmentWizardsFields(ShipmentData);
});
When("create shipment", () => {
    Actions.CreateShipment(ShipmentData.ShipmentLevel);
});

Then("the shipment should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentNumber = interception.response.body.ShipmentNumber;
});

Given("a receivable with the following details", (dataTable) => {
    const ReceivableData = dataTable.hashes() as ReceivableDetails[];
    Actions.OpenShipment(shipmentNumber)
    Actions.FillReceivablesTab(ReceivableData)
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});
Given("a credit ARInvoice with a random invoice number and the following details", 
(dataTable) => {
    const ARInvoiceData = dataTable.hashes()[0] as ARInvoiceDetails
    cy.Click(Selectors.CreateCreditNoteARInvoiceButton, null);
    Actions.FillARInvoiceDetails(ARInvoiceData)
  });
});
When("create invoice", () => {
    Actions.CreateARInvoice()
});
Then("the invoice should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

When("cancel draft", () => {
    Actions.CancelDraftARInvoice()
});
Then("the invoice should cancel successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});