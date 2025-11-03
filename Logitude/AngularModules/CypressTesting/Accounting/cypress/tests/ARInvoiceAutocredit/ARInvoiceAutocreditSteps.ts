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
let invoiceAmount: string;
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

Given("a receivable with the following details", (dataTable) => {
    let receivableDetailsList = Assists.CreateSet<ReceivableDetails>(dataTable);
    ShipmentActions.OpenShipment(shipmentNumber);
    ShipmentActions.FillReceivablesTab(receivableDetailsList);
});

Given("an AR invoice with the following details", (dataTable) => {
    let arInvoiceDetails = Assists.CreateInstance<ARInvoiceDetails>(dataTable, true);
    cy.Click(AccountingSelectors.CreateARInvoiceButton, null);
    AccountingActions.FillARInvoiceDetails(arInvoiceDetails);
});

When("create shipment", () => {
    ShipmentActions.CreateShipment(shipmentDetails.ShipmentLevel);
});

When("add receivable", () => {
    ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton);
});

When("create invoice", () => {
    AccountingActions.CreateARInvoice();
});

When("approve invoice", () => {
    AccountingActions.ARApproveInvoice();
});

When("approve auto credit invoice", () => {
    AccountingActions.ApproveAutoCreditARInvoice();
});

When("auto credit invoice", () => {
    AccountingActions.AutoCreditARInvoice();
});

When("back to the AR invoice", () => {
    cy.Click(BaseSelectors.BackBottonBodyClass, AccountingSelectors.ContainsARInvoice);
});

Then("the shipment should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentNumber = interception.response.body.ShipmentNumber;
    })
});

Then("the receivable should add successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});

Then("the invoice should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

Then("the invoice should approve successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200).then((interception) => {
        invoiceNumber = interception.response.body.InvoiceNumber;
        invoiceAmount = interception.response.body.AmountInInvoiceCurrency;
    });
    // Wait 2 minutes for status to update
    cy.wait(120000);
});

Then("the auto credit invoice should approve successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200).then((interception) => {
        autoCreditInvoiceNumber = interception.response.body.InvoiceNumber;
    });
});

Then("new invoice with status Auto Credit should appear", () => {
    AccountingActions.AssertARInvoiceStatus(AccountingSelectors.AutoCredit);
});

Then("the status should be Auto Credited", () => {
    AccountingActions.AssertARInvoiceStatus(AccountingSelectors.AutoCredited);
});

Then("the AR invoice number should appear next to the auto credit invoice title", () => {
    AccountingActions.AssertAutoCreditByInvoiceNumber(invoiceNumber);
});

Then("the auto credit invoice number should appear next to the AR invoice title", () => {
    AccountingActions.AssertAutoCreditByInvoiceNumber(autoCreditInvoiceNumber);
});

Then("the auto credit invoice should have negative amount of the AR invoice", () => {
    let expectedAutoCreditInvoiceAmount = (Number(invoiceAmount) / -1);
    AccountingActions.AssertARInvoiceAmount(expectedAutoCreditInvoiceAmount);
});

Then("the status value should be {string}", (statusValue) => {
    BaseAssertion.AssertElementContain(ShipmentSelectors.ARInvoiceStatus, statusValue)
  });

Then("the details screen fields should be disabled",()=>{
    AccountingActions.AssertARInvoiceDetailsFieldsDisabled()

})  
Then("the details screen fields shouldn't be dim",()=>{
    AccountingActions.AssertARInvoiceDetailsFieldsNotBeDisabled()

})  