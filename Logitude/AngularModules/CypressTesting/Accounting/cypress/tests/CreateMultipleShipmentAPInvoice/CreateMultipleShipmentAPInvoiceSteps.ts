import * as CommonActions from '../../../../Common/cypress/actions/Actions';
import * as ShipmentActions from '../../../../Shipment/cypress/actions/Actions';
import * as AccountingActions from '../../../../Accounting/cypress/actions/Actions';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import { ShipmentSelectors } from '../../../../Shipment/cypress/selectors/Selectors';
import { ShipmentDetails } from '../../../../Shipment/cypress/models/ShipmentDetails';
import { PackagesDetails } from '../../../../Shipment/cypress/models/PackagesDetails';
import { PayableDetails } from '../../../../Shipment/cypress/models/PayableDetails';
import { APInvoiceDetails } from 'cypress/models/APInvoiceDetails';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { CustomerDetails } from '../../../../Common/cypress/models/CustomerDetails';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import * as Assists from "../../../../Base/cypress/assists/Assists";

//#region variables
let shipmentDetails: ShipmentDetails;
let apInvoiceDetails: APInvoiceDetails;
let payableDetails: PayableDetails;
let shipmentNumbers: string[] = [];
let customerCode: string;
let AccountingSystem: string;

//#endregion
//#region Update Accounting System
Given("the user logged in", () => {
  cy.Login();
});
Given("accounting System as {string}", (accountingSystem) => {
  AccountingSystem = accountingSystem;
});

When("change the accounting system", () => {
  AccountingActions.changeAccountingsSystem(AccountingSystem)
});

Then("the accounting system should update successfully", () => {
  BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
});
//#endregion
//#region Create customer
Given("the user navigates to customers workspace", () => {
  CommonActions.NavigatesToCustomersWorkspace();
});

Given("a customer with the following details", (dataTable) => {
  let customerDetails = Assists.CreateInstance<CustomerDetails>(dataTable, true);
  CommonActions.AddNewCustomer(customerDetails);
});

When("create customer", () => {
  CommonActions.CreateCustomer();
});

Then("the customer should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.PartnersDomainRequest, 200).then((interception) => {
    customerCode = interception.response.body.Customer.Code;
  });;
});
//#endregion

//#region Create first/second direct export air shipment
Given("the user in shipments workspace", () => {
  ShipmentActions.NavigatesToShipmentsWorkspace();
});

Given("a direct shipment with the following details", (dataTable) => {
  shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
  ShipmentActions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
  shipmentDetails.Shipper = customerCode;
  ShipmentActions.FillShipmentWizardsFields(shipmentDetails);
});

When("create shipment", () => {
  ShipmentActions.CreateShipment(shipmentDetails.ShipmentLevel);
});

Then("the first direct should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
    shipmentNumbers[0] = interception.response.body.ShipmentNumber;
    shipmentDetails.ShipmentNumber = interception.response.body.ShipmentNumber;
  })
});

Then("the second direct should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
    shipmentNumbers[1] = interception.response.body.ShipmentNumber;
    shipmentDetails.ShipmentNumber = interception.response.body.ShipmentNumber;
  })
});
//#endregion

//#region Update packages/payable tabs
Given("the user add package with the following details", (dataTable) => {
  ShipmentActions.OpenShipment(shipmentDetails.ShipmentNumber);
  let packagesDetails = Assists.CreateSet<PackagesDetails>(dataTable);
  ShipmentActions.FillPackageTab(shipmentDetails.TransportMode, packagesDetails)
});

Given("the user add payable with the following details", (dataTable) => {
  ShipmentActions.OpenShipment(shipmentDetails.ShipmentNumber);
  payableDetails = Assists.CreateInstance<PayableDetails>(dataTable, true);
  ShipmentActions.FillPayablesTab(payableDetails);
});

When("update shipment", () => {
  ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton);
});

Then("the direct should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
  cy.Click(BaseSelectors.Backbutton, null, false);
});
//#endregion

//#region Create multiple shipment APInvoice
Given("the user in Accounts Payable workspace", () => {
  AccountingActions.NavigatesToAccountsPayablesWorkspace();
  cy.Click(BaseSelectors.Button, "New")
});

Given("a multiple AP invoice  with a random invoice number and the following details", (dataTable) => {
  apInvoiceDetails = Assists.CreateInstance<APInvoiceDetails>(dataTable, true)
  AccountingActions.FillAPInvoiceDetails(apInvoiceDetails, true);
});

When("create invoice", () => {
  AccountingActions.ReceiveAPInvoice();
});

Then("the invoice should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200);
});
//#endregion

//#region Add and edit shipment lines
Given("the user add and edit shipment lines", () => {
  AccountingActions.AddTwoShipmentLinesAndEditAmount(shipmentNumbers, apInvoiceDetails.VATType, payableDetails);
  AccountingActions.SaveAPInvoice();
});

Then("the invoice should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.InvoiceDomain, 200);
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