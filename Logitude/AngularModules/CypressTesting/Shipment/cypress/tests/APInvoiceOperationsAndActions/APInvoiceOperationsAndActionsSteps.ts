import * as Actions from "../../actions/Actions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { ShipmentSelectors } from "../../selectors/Selectors";
import { PayableDetails } from "cypress/models/PayableDetails"
import { APInvoiceDetails } from "../../../../Accounting/cypress/models/APInvoiceDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { CustomerDetails } from '../../../../Common/cypress/models/CustomerDetails';
import { MainCarriageLeg } from 'cypress/models/MainCarriageLeg';
import { PackagesDetails } from 'cypress/models/PackagesDetails';
import * as CommonActions from '../../../../Common/cypress/actions/Actions';
import * as AccountingActions from '../../../../Accounting/cypress/actions/Actions';
import { AccountingSelectors } from "../../../../Accounting/cypress/selectors/Selectors";

//#region variables
let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
let customerCode: string;
//#endregion
//#region Create customer
Given("the user logged in and navigates to customers workspace", () => {
  cy.Login();
  CommonActions.NavigatesToCustomersWorkspace();
});
Given("a customer with the following details", (dataTable) => {
  let customerDetails = dataTable.hashes()[0] as CustomerDetails;
  CommonActions.AddNewCustomer(customerDetails);
});
When("create customer", () => {
  CommonActions.CreateCustomer();
});

Then("the customer should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.PartnersDomainRequest, 200).then((interception) => {
    customerCode = interception.response.body.Customer.Code;
  });
});
//#endregion

//#region Create direct export air shipment
Given("the user navigates to shipments workspace", () => {
  Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
  shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
  Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
  shipmentDetails.Shipper = customerCode;
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
  cy.Click(ShipmentSelectors.RoutingsTab, null);
});
Given("edit main carriage leg with the following details",(dataTable)=>{
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
    Actions.FillPayablesTab(PayableData)
  });
  When("add payables",
  () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)

  });
  Then("the payables should add successfully",
  () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
  });
  //#endregion
//#region Create APInvoice
Given("an APInvoice with a random invoice number and the following details",
  (dataTable) => {
    const APInvoiceData = dataTable.hashes()[0] as APInvoiceDetails
    cy.Click(AccountingSelectors.ReceiveInvoiceButton, null);
    AccountingActions.FillAPInvoiceDetails(APInvoiceData)
  });
When("receive invoice", () => {
  AccountingActions.ReceiveAPInvoice();
});
Then("the invoice should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200);
});
  //#endregion
//#region Approve APInvoice
When("approve invoice", () => {
  AccountingActions.APApproveInvoice()
});
Then("the invoice should approve successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200);
});
  //#endregion
//#region Cancel the APInvoice approvement
When("cancel the invoice approvement", () => {
  AccountingActions.APInvoiceCancelApproval()
});
Then("the invoice should cancel successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200);
});
  //#endregion
//#region Void APInvoice
When("void invoice", () => {
  AccountingActions.VoidAPInvoice()
});
Then("the invoice should void successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200);
});
  //#endregion

//#region update shipment step
When("update shipment", () => {
  Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});
//#endregion

//#region update shipment assert step
Then("the direct should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion