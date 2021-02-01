import * as Actions from "../../actions/Actions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Selectors } from "../../selectors/Selectors";
import { PayableDetails } from "cypress/models/PayableDetails"
import { APInvoiceDetails } from "cypress/models/APInvoiceDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"

let ShipmentData: ShipmentDetails;
let shipmentNumber: string;

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
Then("the direct should create successfully", () => {
  BaseAssertion.AssertStatusCode("WaitPostShipmentRequest", 200).then((interception) => {
    shipmentNumber = interception.response.body.ShipmentNumber;
  });
});

Given("a payable with the following details",
  (dataTable) => {
    const PayableData = dataTable.hashes()[0] as PayableDetails;
    Actions.OpenShipment(shipmentNumber)
    cy.Click(Selectors.PayablesTab, null)
    Actions.FillPayablesTab(PayableData)
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
  });
And("an APInvoice with the following details",
  (dataTable) => {
    const APInvoiceData = dataTable.hashes()[0] as APInvoiceDetails
    cy.Click(Selectors.ReceiveInvoiceButton, null);
    Actions.FillAPInvoiceDetails(APInvoiceData)
  });
When("receive invoice", () => {
  Actions.ReceiveAPInvoice();
});
Then("the invoice should create successfully", () => {
  BaseAssertion.AssertStatusCode("WaitPostAPInvoicesRequest", 200);
});

When("approve invoice", () => {
  Actions.APApproveInvoice()
});
Then("the invoice should approve successfully", () => {
  BaseAssertion.AssertStatusCode("WaitPutAPInvoicesRequest", 200);
});

When("cancel the invoice approvement", () => {
  Actions.APInvoiceCancelApproval()
});
Then("the invoice should cancel successfully", () => {
  BaseAssertion.AssertStatusCode("WaitPutAPInvoicesRequest", 200);
});
