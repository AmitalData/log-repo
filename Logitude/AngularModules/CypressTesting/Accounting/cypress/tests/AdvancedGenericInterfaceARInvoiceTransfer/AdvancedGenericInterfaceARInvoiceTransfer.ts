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
import { AccountingSelectors } from "../../selectors/Selectors";
import * as BaseActions from '../../../../Base/cypress/actions/Actions';
import { AccountingURLs } from '../../constants/URLs';
import { RestAPI } from '../../../../Base/cypress/constants/RestAPI'

//#region variables
let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
let customerCode: string;
let AccountingSystem: string;
let ExternalTransmissionType: string
let ARInvoiceNumber: string;
//#endregion

//#region Update Accounting System
Given("the user logged in and navigate to accounting settings", () => {
    cy.Login();
    AccountingActions.NavigatesToAccountingSettings()
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
    ShipmentActions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
    shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
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

//#region add receivable
Given("a receivable with the following details", (dataTable) => {
    const ReceivableData = dataTable.hashes() as ReceivableDetails[];
    ShipmentActions.OpenShipment(shipmentNumber);
    ShipmentActions.FillReceivablesTab(ReceivableData, true)
});

When("add receivable", () => {
    ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});

Then("the receivable should add successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});
//#endregion

//#region Create ARInvoice
Given("an ARInvoice with the following details", (dataTable) => {
    const ARInvoiceData = dataTable.hashes()[0] as ARInvoiceDetails
    cy.Click(AccountingSelectors.CreateARInvoiceButton, null);
    AccountingActions.FillARInvoiceDetails(ARInvoiceData)
});

Given("clear external ID for partner", () => {
    BaseActions.ClearExternalIDFromShipmentLevel("Partner");
});

When("create invoice", () => {
    AccountingActions.CreateARInvoice()
});

Then("the invoice should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

Then("the transfer status should be not ready", () => {
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
    AccountingActions.NavigatesToAccountingSettings()
    AccountingSystem = accountingSystem
    ExternalTransmissionType = externalTransmissionType
});

When("update the accounting system", () => {
    AccountingActions.changeAccountingsSystem(AccountingSystem, ExternalTransmissionType)
});

Then("the accounting system should update successfully", () => {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
});

Then("the ARInvoice should appear in AR Not Ready Invoices", () => {
    AccountingActions.NavigatesToNotReadyARInvoices()
    AccountingActions.ARInvoiceSearch(ARInvoiceNumber)
    AccountingActions.AssertTransferError(ARInvoiceNumber, AccountingSelectors.MissingcustomerErrorMessage)
    AccountingActions.AssertTransferError(ARInvoiceNumber, AccountingSelectors.MissingAirFreightChargeTypeErrorMessage)
});
//#endregion

//#region Add missing external IDs
Given("an external IDs with the following details", (dataTable) => {
    const ExternalIDs = dataTable.hashes()[0];
    AccountingActions.ClickOnRowDependingOnARInvoiceNumber(ARInvoiceNumber)
    AccountingActions.FillExternalID(BaseSelectors.Partner, "BillTo", ExternalIDs.BillTo)
    AccountingActions.FillExternalID(BaseSelectors.Currency ,"InvoiceCurrency", ExternalIDs.Currency)
    AccountingActions.FillExternalID(BaseSelectors.ChargesType ,"ChargeType", ExternalIDs.ChargesType)
});

When("add the external IDs", () => {
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK)
});

Then("the external IDs should add successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200)
});

Then("the transfer status should be ready", () => {
    ShipmentActions.NavigatesToShipmentsWorkspace()
    ShipmentActions.OpenShipment(shipmentNumber);
    cy.Click(ShipmentSelectors.ReceivablesTab, null)
    cy.Click(BaseSelectors.HyperlinkButtonControl, ARInvoiceNumber, true)
    AccountingActions.AssertTransferStatus(AccountingSelectors.ContainReady)
});
//#endregion

//#region Transfer ARInvoice from transfer screen
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
    AccountingActions.CloseExportingInvoiceTransferWindow()
    BaseActions.CloseWindow()
});

Then("the transfer status should be transferred", () => {
    ShipmentActions.NavigatesToShipmentsWorkspace()
    ShipmentActions.OpenShipment(shipmentNumber);
    cy.Click(ShipmentSelectors.ReceivablesTab, null)
    cy.Click(BaseSelectors.HyperlinkButtonControl, ARInvoiceNumber, true)
    AccountingActions.AssertTransferStatus(AccountingSelectors.ContainTransferred)
});
//#endregion