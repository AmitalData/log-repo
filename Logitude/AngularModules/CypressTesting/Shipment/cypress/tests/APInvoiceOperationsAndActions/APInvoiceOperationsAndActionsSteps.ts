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

Given("the user logged in and navigates to shipments workspace", () => {
  cy.Login()
  Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details",
  (dataTable) => {
    const shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    ShipmentData = shipmentDetails;
    Actions.OpenNewShipmentWizard(ShipmentData.ShipmentLevel);
    Actions.FillShipmentWizardsFields(ShipmentData);
  });
When("create shipment", () => {
  Actions.CreateShipment(ShipmentData.ShipmentLevel);
});
Then("the direct shipment should create successfully", () => {
  BaseAssertion.AssertStatusCode("WaitPostShipmentRequest", 200).then((interception) => {
    shipmentNumber = interception.response.body.ShipmentNumber;
  });
});

Given("a payable with the following details",
  (dataTable) => {
    const PayableData = dataTable.hashes()[0] as PayableDetails;
    Actions.OpenShipment(shipmentNumber)
    Actions.FillPayablesTab(PayableData)
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
  });
And("an APInvoice with a random invoice number and the following details",
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
When("void invoice", () => {
  Actions.VoidAPInvoice()
});
Then("the invoice should void successfully", () => {
  BaseAssertion.AssertStatusCode("WaitPutAPInvoicesRequest", 200);
});