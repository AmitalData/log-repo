import * as Actions from "../../actions/Actions";
import * as Assertions from "../../actions/Assertions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Selectors } from "../../selectors/Selectors";
import { PayableDetails } from "cypress/models/PayableDetails"
import { APInvoiceDetails } from "cypress/models/APInvoiceDetails"


let ShipmentData: ShipmentDetails;
let ShipmentFile;
let InvoiceFile;
Given("the user logged in", () => {
  cy.Login();
});
And("navigate to shipments workspace", () => {
  cy.Click(BaseSelectors.OperationsMenu, null);
  cy.Click(Selectors.ShipmentTab, null);
});
Given("a direct shipment with the following details",
  (dataTable) => {
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
Given("a payable with the following details",
  (dataTable) => {
    const PayableData = dataTable.hashes()[0] as PayableDetails;
    Actions.OpenShipment(ShipmentFile)
    cy.Click(Selectors.PayablesTab, null)
    Actions.FillPayablesTab(PayableData)
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
  });
And("an ap invoice with the following details",
  (dataTable) => {
    const APInvoiceData = dataTable.hashes()[0] as APInvoiceDetails
    cy.Click(Selectors.ReceiveInvoiceButton, null);
    Actions.FillAPInvoiceDetails(APInvoiceData)
  });
When("receive invoice", () => {
  Actions.ReceiveAPInvoice();
});
Then("the invoice should create successfully", () => {
  InvoiceFile = "CreatedAPInvoiceData/" + "APDirectEAInvoice" + ".json";
  Assertions.ValidateCreatedAPInvoice(InvoiceFile)

});
When("approve invoice", () => {
  Actions.APApproveInvoice()


});
Then("the invoice should approve successfully", () => {
  Assertions.ValidateUpdatedAPInvoice(InvoiceFile)
});
When("cancel the invoice Approvement", () => {
  Actions.APInvoiceCancelApproval()
});
Then("the invoice should cancel successfully", () => {
  Assertions.ValidateUpdatedAPInvoice(InvoiceFile)

});
