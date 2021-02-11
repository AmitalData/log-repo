import * as Actions from "../../actions/Actions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { ShipmentSelector } from "../../selectors/Selectors";
import { PayableDetails } from "cypress/models/PayableDetails"
import { APInvoiceDetails } from "cypress/models/APInvoiceDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { CustomerDetails } from "cypress/models/CustomerDetails";
import { MainCarriageLeg } from 'cypress/models/MainCarriageLeg';
import { PackagesDetails } from 'cypress/models/PackagesDetails';
//#region variables
let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
//#endregion
//#region Create customer
Given("the user logged in and navigates to customers workspace", () => {
  cy.Login();
  Actions.NavigatesToCustomersWorkspace();
});
Given("a customer with the following details", (dataTable) => {
  let customerDetails = dataTable.hashes()[0] as CustomerDetails;
  Actions.AddNewCustomer(customerDetails);
});
When("create customer", () => {
  Actions.CreateCustomer();
});

Then("the customer should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.PartnersDomainRequest, 200);
});
//#endregion

//#region Create direct export air shipment
Given("the user navigates to shipments workspace", () => {
  Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
  shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
  Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
  Actions.FillShipmentWizardsFields(shipmentDetails);
});

When("create shipment", () => {
  Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});

Then("the direct should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
      shipmentNumber = interception.response.body.ShipmentNumber;
  })
});
//#endregion

//#region Update routing tab
Given("the user in the shipment's rounting tab",()=>{
  Actions.OpenShipment(shipmentNumber);
  cy.Click(ShipmentSelector.RoutingsTab, null);
});
Given("edit main carriage leg with the follwing details",(dataTable)=>{
  let mainCarriageLeg = dataTable.hashes()[0] as MainCarriageLeg;
  Actions.EditMainCarriageLegs(mainCarriageLeg.Airline);
}); 
//#endregion

//#region Update packages tab
Given("the user add package with the following details", (dataTable) => {
  let packagesDetails = dataTable.hashes() as PackagesDetails[];
  Actions.FillPackageTab(shipmentDetails.TransportMode, packagesDetails)
});
//#endregion
//#region Add Payables
Given("a payable with the following details",
  (dataTable) => {
    const PayableData = dataTable.hashes()[0] as PayableDetails;
    Actions.OpenShipment(shipmentNumber)
    Actions.FillPayablesTab(PayableData)
  });
  When("add payables",
  () => {
    Actions.UpdateShipment(ShipmentSelector.ShipmentSaveButton)

  });
  Then("the payables should add successfully",
  () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
  });
  //#endregion
//#region Create APInvoice
And("an APInvoice with a random invoice number and the following details",
  (dataTable) => {
    const APInvoiceData = dataTable.hashes()[0] as APInvoiceDetails
    cy.Click(ShipmentSelector.ReceiveInvoiceButton, null);
    Actions.FillAPInvoiceDetails(APInvoiceData)
  });
When("receive invoice", () => {
  Actions.ReceiveAPInvoice();
});
Then("the invoice should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200);
});
  //#endregion
//#region Approve APInvoice
When("approve invoice", () => {
  Actions.APApproveInvoice()
});
Then("the invoice should approve successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200);
});
  //#endregion
//#region Cancel the APInvoice approvement
When("cancel the invoice approvement", () => {
  Actions.APInvoiceCancelApproval()
});
Then("the invoice should cancel successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200);
});
  //#endregion
//#region Void APInvoice
When("void invoice", () => {
  Actions.VoidAPInvoice()
});
Then("the invoice should void successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200);
});
  //#endregion

//#region update shipment step
When("update shipment", () => {
  Actions.UpdateShipment(ShipmentSelector.ShipmentSaveButton)
});
//#endregion

//#region update shipment assert step
Then("the direct should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion