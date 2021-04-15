import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as CommonActions from '../../../../Common/cypress/actions/Actions';
import { CustomerDetails } from '../../../../Common/cypress/models/CustomerDetails';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { ShipmentDetails } from '../../../../Shipment/cypress/models/ShipmentDetails';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import * as AccountingActions from '../../actions/Actions';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import * as ShipmentActions from '../../../../Shipment/cypress/actions/Actions';
import { ReceivableDetails } from "../../../../Shipment/cypress/models/ReceivableDetails"
import { ShipmentSelectors } from '../../../../Shipment/cypress/selectors/Selectors';
import { ARInvoiceDetails } from '../../models/ARInvoiceDetails';
import { FTPDetails } from '../../models/FTPDetails';
import { AccountingSelectors } from "../../selectors/Selectors";
import * as BaseActions from '../../../../Base/cypress/actions/Actions';
import { AccountingURLs } from '../../constants/URLs';
import { RestAPI } from '../../../../Base/cypress/constants/RestAPI';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ExternalIDs } from '../../models/ExternalIDs';

//#region variables
let shipmentDetails: ShipmentDetails;
let FTPdetails: FTPDetails;
let shipmentNumber: string;
let customerCode: string;
let AccountingSystem: string;
let ExternalTransmissionType: string
let ARInvoiceNumber: string;
let externalIDs:ExternalIDs
//#endregion

//#region Disable Accounting System
Given("the user logged in and set up accounting system as {string}", (accountingSystem) => {
  cy.Login();
  AccountingSystem = accountingSystem;
});

When("disable the accounting system", () => {
  AccountingActions.changeAccountingsSystem(AccountingSystem)
});

Then("the accounting system should disable successfully", () => {
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

//#region Add receivable and clear external IDs (ChargesType,Currency)
Given("a receivable with the following details", (dataTable) => {
  ShipmentActions.OpenShipment(shipmentNumber);
  const ReceivableData = Assists.CreateSet<ReceivableDetails>(dataTable);
  ShipmentActions.FillReceivablesTab(ReceivableData, true)
});

When("add receivable and clear external IDs", () => {
  ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});

Then("the receivable should add successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});
//#endregion

//#region Create ARInvoice and clear External ID (customer)
Given("an ARInvoice with the following details", (dataTable) => {
  const ARInvoiceData = Assists.CreateInstance<ARInvoiceDetails>(dataTable, true);
  cy.Click(AccountingSelectors.CreateARInvoiceButton, null);
  AccountingActions.FillARInvoiceDetails(ARInvoiceData)
});

When("clear external ID for partner and create invoice", () => {
  BaseActions.ClearExternalIDFromShipmentLevel("Partner");
  AccountingActions.CreateARInvoice()
});

Then("the invoice should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

Then("the transfer status should be Not Ready", () => {
  AccountingActions.AssertTransferStatus(AccountingSelectors.ContainNotReady)
});
//#endregion

//#region Approve ARInvoice
When("approve invoice", () => {
  AccountingActions.ARApproveInvoice()
});

Then("the invoice should approve successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200).then((interception) => {
    ARInvoiceNumber = interception.response.body.InvoiceNumber;
  })
});
//#endregion

//#region Update Accounting System
Given("accounting System as {string} and external transmission as {string}", (accountingSystem, externalTransmissionType) => {
  cy.BackButton(BaseSelectors.ContainsShipment + shipmentNumber);
  cy.BackButton(BaseSelectors.ContainsOperations);
  AccountingSystem = accountingSystem
  ExternalTransmissionType = externalTransmissionType
});

Given("the following FTP details", (dataTable) => {
  FTPdetails = Assists.CreateInstance<FTPDetails>(dataTable, true);
});

When("update the accounting system", () => {
  AccountingActions.changeAccountingsSystem(AccountingSystem, ExternalTransmissionType, FTPdetails);
});

Then("the accounting system should update successfully", () => {
  BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
});

Then("the ARInvoice should appear in AR Not Ready Invoices", () => {
  AccountingActions.NavigatesToNotReadyARInvoices()
  AccountingActions.ARInvoiceSearch(ARInvoiceNumber)
  AccountingActions.AssertTransferError(ARInvoiceNumber, AccountingSelectors.MissingcustomerErrorMessage)
  //AccountingActions.AssertTransferError(ARInvoiceNumber, AccountingSelectors.MissingCurrencyErrorMessage)
  AccountingActions.AssertTransferError(ARInvoiceNumber, AccountingSelectors.MissingAirFreightChargeTypeErrorMessage)
});
//#endregion

//#region Add missing external IDs
Given("an external IDs with the following details", (dataTable) => {
  externalIDs = Assists.CreateInstance<ExternalIDs>(dataTable, true);
  AccountingActions.ClickOnRowDependingOnARInvoiceNumber(ARInvoiceNumber)
  AccountingActions.FillExternalID(BaseSelectors.Partner, "BillTo", externalIDs.BillTo)
  AccountingActions.FillExternalID(BaseSelectors.Currency ,"InvoiceCurrency", externalIDs.Currency)
  AccountingActions.FillExternalID(BaseSelectors.ChargesType ,"ChargeType", externalIDs.ChargesType)
});

When("add the external IDs", () => {
  cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
  cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK)
});

Then("the external IDs should add successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200)
});

Then("the transfer status should be Ready", () => {
  ShipmentActions.NavigatesToShipmentsWorkspace()
  ShipmentActions.OpenShipment(shipmentNumber);
  cy.Click(ShipmentSelectors.ReceivablesTab, null)
  cy.Click(BaseSelectors.HyperlinkButtonControl, ARInvoiceNumber, true)
  AccountingActions.AssertTransferStatus(AccountingSelectors.ContainReady)
});
//#endregion

//#region Transfer ARInvoice
Given("the user in transfer screen", () => {
  cy.BackButton(BaseSelectors.ContainsShipment + shipmentNumber);
  cy.BackButton(BaseSelectors.ContainsOperations);
  AccountingActions.NavigatesToNewTransferARInvoices()
  AccountingActions.ARInvoiceSearchInTransferScreen(ARInvoiceNumber)
});

When("export the ARInvoice", () => {
  AccountingActions.ExportARInvoice(ARInvoiceNumber)
});

Then("the ARInvoice should export successfully", () => {
  AccountingActions.AssertTransferredInvoice()
});

Then("the transfer status should be Transferred", () => {
  ShipmentActions.NavigatesToShipmentsWorkspace()
  ShipmentActions.OpenShipment(shipmentNumber);
  cy.Click(ShipmentSelectors.ReceivablesTab, null)
  cy.Click(BaseSelectors.HyperlinkButtonControl, ARInvoiceNumber, true)
  AccountingActions.AssertTransferStatus(AccountingSelectors.ContainTransferred)
});
//#endregion