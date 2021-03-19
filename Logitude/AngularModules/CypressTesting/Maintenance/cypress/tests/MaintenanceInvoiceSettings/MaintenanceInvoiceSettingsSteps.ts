import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { InvoiceSettingsDetails } from "../../../cypress/models/InvoiceSettingsDetails";
import { ShipmentDetails } from '../../../../Shipment/cypress/models/ShipmentDetails';
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import { ShipmentSelectors } from '../../../../Shipment/cypress/selectors/Selectors';
import * as MaintenanceActions from "../../actions/Actions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as ShipmentActions from '../../../../Shipment/cypress/actions/Actions';
import { ReceivableDetails } from "../../../../Shipment/cypress/models/ReceivableDetails"
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import { ARInvoiceDetails } from '../../../../Accounting/cypress/models/ARInvoiceDetails';
import { AccountingSelectors } from '../../../../Accounting/cypress/selectors/Selectors'
import * as AccountingActions from '../../../../Accounting/cypress/actions/Actions';
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";

let invoiceSettingsDetails: InvoiceSettingsDetails;
let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
let ARInvoiceNumber: string;
//#region Disable/enable void invoice settings
Given("the user logged in and navigate to {string} in maintenance menu", (InvoiceSettings) => {
    cy.Login()
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(InvoiceSettings, MaintenanceSelectors.InvoiceSettingsMaintenanceItem)
});

Given("the user navigates to {string} in maintenance menu", (InvoiceSettings) => {
    cy.BackButton(BaseSelectors.ContainsShipment + shipmentNumber);
    cy.BackButton(BaseSelectors.ContainsOperations);
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(InvoiceSettings, MaintenanceSelectors.InvoiceSettingsMaintenanceItem)
});

Given("accounting settings with the following details", (dataTable) => {
    invoiceSettingsDetails = Assists.CreateInstance<InvoiceSettingsDetails>(dataTable, true);
    MaintenanceActions.ChangeInvoiceSettings(invoiceSettingsDetails)
});

When("update invoice settings", () => {
    MaintenanceActions.UpdateInvoiceSettings();
});

Then("the invoice setting should update successfully", () => {
    MaintenanceActions.AssertUpdateInvoiceSettings()
});
//#endregion

//#region Create direct export air shipment
Given("the user navigates to shipments workspace", () => {
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

Then("the shipment should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentNumber = interception.response.body.ShipmentNumber;
    })
});
//#endregion
//#region add receivable 
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
//#endregion
//#region Create ARInvoice
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
//#endregion
//#region Approve ARInvoice
When("approve invoice", () => {
    AccountingActions.ARApproveInvoice();
});

Then("the invoice should approve successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200).then((interception) => {
        ARInvoiceNumber = interception.response.body.InvoiceNumber;
    });
});

//#endregion
//#region Void ARInvoice
Given("the user goes back to ARInvoice", () => {
    ShipmentActions.NavigatesToShipmentsWorkspace()
    ShipmentActions.OpenShipment(shipmentNumber);
    cy.Click(ShipmentSelectors.ReceivablesTab, null)
    cy.Click(BaseSelectors.HyperlinkButtonControl, ARInvoiceNumber, true)
});

When("void invoice", () => {
    AccountingActions.VoidARInvoice();
});

Then("the following message {string} should appear", (Message) => {
    MaintenanceActions.AssertVoidInvoiceMessage(Message);
});
Then("the invoice should void successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});
//#endregion

