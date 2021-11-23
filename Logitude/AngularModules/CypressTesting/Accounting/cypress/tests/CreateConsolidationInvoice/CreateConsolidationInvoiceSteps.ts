import * as CommonActions from '../../../../Common/cypress/actions/Actions';
import * as ShipmentActions from '../../../../Shipment/cypress/actions/Actions';
import * as AccountingActions from '../../../../Accounting/cypress/actions/Actions';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import { ShipmentSelectors } from '../../../../Shipment/cypress/selectors/Selectors';
import { ShipmentDetails } from '../../../../Shipment/cypress/models/ShipmentDetails';
import { PackagesDetails } from '../../../../Shipment/cypress/models/PackagesDetails';
import { PayableDetails } from '../../../../Shipment/cypress/models/PayableDetails';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { CustomerDetails } from '../../../../Common/cypress/models/CustomerDetails';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import { CommonSelectors } from '../../../../Common/cypress/selectors/Selectors';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import { ARInvoiceDetails } from '../../models/ARInvoiceDetails';
import { AccountingSelectors } from "../../selectors/Selectors";
import { ARPaymentDetails } from '../../models/ARPaymentDetails';
import { ReceivableDetails } from "../../../../Shipment/cypress/models/ReceivableDetails"
import { AccountingURLs } from '../../constants/URLs';
import { RestAPI } from '../../../../Base/cypress/constants/RestAPI';
import * as Assists from "../../../../Base/cypress/assists/Assists";

//#region variables
let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
let consolidationInvoiceNumber: string;
let draftConsolidationInvoiceNumber: string
let customerCode: string;
let PayableData: PayableDetails
let AccountingSystem: string;
let ARInvoiceNumber:string
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
  });
});
//#endregion

//#region Update customer
Given("the user in the customer's billing tab", () => {
  CommonActions.OpenCustomer(customerCode);
  cy.Click(CommonSelectors.CustomerBillingTab, null, false);
});

When("activate consolidated invoice option", () => {
  cy.Click(CommonSelectors.EnableConsolidationInvoices, null, false);
  CommonActions.UpdateCustomer();
});

Then("the customer should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.Customers, 200)
  cy.Click(BaseSelectors.Backbutton, null, false);
});
//#endregion
//#region Create direct export air shipment
Given("the user navigates to shipments workspace", () => {
  ShipmentActions.NavigatesToShipmentsWorkspace()
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

Then("the direct should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
    shipmentNumber = interception.response.body.ShipmentNumber;
  })
});
//#endregion

//#region Update packages tab
Given("the user add package with the following details", (dataTable) => {
  ShipmentActions.OpenShipment(shipmentNumber);
  let packagesDetails = Assists.CreateSet<PackagesDetails>(dataTable);
  ShipmentActions.FillPackageTab(shipmentDetails.TransportMode, packagesDetails)
});
//#endregion

//#region update shipment step
When("update shipment", () => {
  ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});
//#endregion

//#region update shipment assert step
Then("the direct should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

//#region Add Payables
Given("a payable with the following details", (dataTable) => {
  PayableData = Assists.CreateInstance<PayableDetails>(dataTable, true);
  ShipmentActions.FillPayablesTab(PayableData)
});

When("add payables", () => {
  ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)

});

Then("the payables should add successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});
//#endregion

//#region generate receivables from payables
When("generate receivables from payables", () => {
  ShipmentActions.GenerateReceivablesFromPayables()
});

Then("the receivables should generate successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

//#region Create ARInvoice
Given("an ARInvoice with a random invoice number and the following details", (dataTable) => {
  const ARInvoiceData = Assists.CreateInstance<ARInvoiceDetails>(dataTable, true)
  cy.Click(AccountingSelectors.CreateARInvoiceButton, null);
  AccountingActions.FillARInvoiceDetails(ARInvoiceData)
});

When("create invoice", () => {
  AccountingActions.CreateARInvoice(true)
});

Then("the invoice should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200).then((interception) => {
    ARInvoiceNumber = interception.response.body.InvoiceNumber;
  })
});
//#endregion

//#region Create Consolidation Invoice
Given("a consolidation invoice with the following details", (dataTable) => {
  cy.BackButton(BaseSelectors.ContainsShipment + shipmentNumber)
  cy.BackButton(BaseSelectors.ContainsOperations)
  const ARInvoiceData = Assists.CreateInstance<ARInvoiceDetails>(dataTable, true)
  ARInvoiceData.Partner = customerCode
  AccountingActions.FillconsolidationInvoiceDetails(ARInvoiceData)

});

When("create consolidation invoice", () => {
  AccountingActions.CreateARInvoice()
});

Then("the consolidation invoice should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200).then((interception) => {
    draftConsolidationInvoiceNumber = interception.response.body.DraftNumber;
    cy.log(draftConsolidationInvoiceNumber)
  })
});
//#endregion
//#region back to Accounting workspace
Given("the user back to Accounting workspace", () => {
  cy.BackButton(BaseSelectors.ContainsAccounting)
});
//#endregion
//#region add receivable
Given("a receivable with the following details", (dataTable) => {
  ShipmentActions.OpenShipment(shipmentNumber);
  const ReceivableData = Assists.CreateSet<ReceivableDetails>(dataTable);
  ShipmentActions.FillReceivablesTab(ReceivableData)
});
When("add receivable", () => {
  ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)

});

Then("the receivable should add successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});
//#endregion
//#region Edit Consolidation Invoice
Given("the user navigates to draft consolidation invoice", () => {
  cy.BackButton(BaseSelectors.ContainsShipment + shipmentNumber)
  cy.BackButton(BaseSelectors.ContainsOperations)
  AccountingActions.NavigatesToDraftInvoice(draftConsolidationInvoiceNumber)
});
When("edit the invoice", () => {
  AccountingActions.AddSecondInvoiceToConsolidation(ARInvoiceNumber)
});
Then("the invoice should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesPutRequest, 200);

});
//#endregion
//#region Approve Consolidation Invoice
When("approve consolidation invoice", () => {
  AccountingActions.ApproveConsilidationInvoice()
});

Then("the consolidation invoice should approve successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200).then((interception) => {
    consolidationInvoiceNumber = interception.response.body.InvoiceNumber;
  })
  //cy.BackButton("Draft Invoices")
  //cy.BackButton(BaseSelectors.ContainsAccounting)
});

Then("the status value should be {string}", (statusValue) => {
  BaseAssertion.AssertElementContain(ShipmentSelectors.ARInvoiceStatus, statusValue)
});

Then("the status of AR Payment value should be {string}", (statusValue) => {
  BaseAssertion.AssertElementContain(ShipmentSelectors.ARPaymentStatus, statusValue)
});

//#endregion

//#region Connect to Payment
Given("a payment with the following details", (dataTable) => {
  cy.BackButton("Draft Invoices")
  cy.BackButton(BaseSelectors.ContainsAccounting)
  const arPaymentDetails = Assists.CreateInstance<ARPaymentDetails>(dataTable, true);
  arPaymentDetails.Partner = customerCode;
  AccountingActions.NewARPaymentFromAccounting(arPaymentDetails, consolidationInvoiceNumber);
});

When("pay the consolidation invoice", () => {
  AccountingActions.SaveARPayment()
});

Then("the consolidation invoice should pay successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ARPayments, 200)
});
//#endregion

//#region Connect to Payment
When("approve the payment", () => {
  AccountingActions.ApproveARPayment()
});

Then("the payment should approve successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ARPayments, 200)
});
//#endregion