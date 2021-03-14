import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
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
import * as Assists from "../../../../Base/cypress/assists/Assists";

let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
let invoiceNumber: string;
let autoCreditInvoiceNumber: string;

Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login();
    ShipmentActions.NavigatesToShipmentsWorkspace();
});

Given("a direct shipment with the following details", (dataTable) => {
    shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    ShipmentActions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
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

Given("a receivable with the following details", (dataTable) => {
    let receivableDetailsList = Assists.CreateSet<ReceivableDetails>(dataTable);
    ShipmentActions.OpenShipment(shipmentNumber);
    ShipmentActions.FillReceivablesTab(receivableDetailsList);
});

When("add receivable", () => {
    ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton);
});

Then("the receivable should add successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});

Given("an AR invoice with the following details", (dataTable) => {
    let arInvoiceDetails = Assists.CreateInstance<ARInvoiceDetails>(dataTable, true);
    cy.Click(AccountingSelectors.CreateARInvoiceButton, null);
    AccountingActions.FillARInvoiceDetails(arInvoiceDetails);
});

When("create invoice", () => {
    AccountingActions.CreateARInvoice();
});

Then("the invoice should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

When("approve invoice", () => {
    AccountingActions.ARApproveInvoice();
});

Then("the invoice should approve successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200).then((interception) => {
        invoiceNumber = interception.response.body.InvoiceNumber;
    });
});

When("approve auto credit invoice", () => {
    AccountingActions.ApproveAutoCreditARInvoice();
});

Then("the auto credit invoice should approve successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200).then((interception) => {
        autoCreditInvoiceNumber = interception.response.body.InvoiceNumber;
    });
});

When("auto credit invoice", () => {
    AccountingActions.AutoCreditARInvoice();
});

Then("new AR invoice with status Auto Credit should appear", () => {
    AccountingActions.AssertARInvoiceStatus(AccountingSelectors.AutoCredit);
    AccountingActions.AssertAutoCreditByInvoiceNumber(invoiceNumber);
});

When("back to the AR invoice", () => {
    cy.Click(BaseSelectors.BackBottonBodyClass, AccountingSelectors.ContainsARInvoice);
});

Then("the status should be Auto Credited", () => {
    AccountingActions.AssertARInvoiceStatus(AccountingSelectors.AutoCredited);
    AccountingActions.AssertAutoCreditByInvoiceNumber(autoCreditInvoiceNumber);
});