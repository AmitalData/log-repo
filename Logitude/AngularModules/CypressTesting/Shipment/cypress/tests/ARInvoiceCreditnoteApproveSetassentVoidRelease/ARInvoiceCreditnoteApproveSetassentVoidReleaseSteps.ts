import * as Actions from "../../actions/Actions"
import { ShipmentSelectors } from "../../selectors/Selectors"
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ReceivableDetails } from "cypress/models/ReceivableDetails"
import { ARInvoiceDetails } from "../../../../Accounting/cypress/models/ARInvoiceDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import * as AccountingActions from '../../../../Accounting/cypress/actions/Actions';
import { AccountingSelectors } from '../../../../Accounting/cypress/selectors/Selectors'
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as MaintenanceActions from "../../../../Maintenance/cypress/actions/Actions";
import { InvoiceSettingsDetails } from "../../../../Maintenance/cypress/models/InvoiceSettingsDetails";
import { MaintenanceSelectors } from "../../../../Maintenance/cypress/selectors/Selectors";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";

//#region variables
let invoiceSettingsDetails: InvoiceSettingsDetails;
let ShipmentData: ShipmentDetails;
let shipmentNumber: string;
let invoiceNumber: string;
let AccountingSystem: string;
//#endregion

//#region Update Accounting System
Given("the user logged in", () => {
    cy.Login()
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

//#region enable void invoice settings
Given("the user navigates to {string} in maintenance menu", (InvoiceSettings) => {
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
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
    const shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    ShipmentData = shipmentDetails;
    Actions.OpenNewShipmentWizard(ShipmentData.ShipmentLevel);
    Actions.FillShipmentWizardsFields(ShipmentData);
});

When("create shipment", () => {
    Actions.CreateShipment(ShipmentData.ShipmentLevel);
});

Then("the shipment should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentNumber = interception.response.body.ShipmentNumber;
    });
});
//#endregion

//#region Create credit note ARInvoice
Given("a receivable with the following details", (dataTable) => {
    const receivableData = Assists.CreateSet<ReceivableDetails>(dataTable);
    Actions.OpenShipment(shipmentNumber)
    Actions.FillReceivablesTab(receivableData)
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});

Given("a credit ARInvoice with a random invoice number and the following details", (dataTable) => {
    const ARInvoiceData = Assists.CreateInstance<ARInvoiceDetails>(dataTable, true);
    cy.Click(AccountingSelectors.CreateCreditNoteARInvoiceButton, null);
    AccountingActions.FillARInvoiceDetails(ARInvoiceData)
});

When("create invoice", () => {
    AccountingActions.CreateARInvoice()
});
Then("the invoice should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200)
});
//#endregion

//#region Approve credit note ARInvoice
When("approve invoice", () => {
    AccountingActions.ARApproveInvoice()
});

Then("the invoice should approve successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200).then((interception) => {
        invoiceNumber = interception.response.body.InvoiceNumber;
    });
});

Then("the status value should be {string}", (statusValue) => {
    BaseAssertion.AssertElementContain(ShipmentSelectors.ARInvoiceStatus, statusValue)
});
//#endregion

//#region Assert invoice details screen fields after approving the invoice
Given("navigates details tab", () => {
    cy.Click(AccountingSelectors.ARInvoiceDetails, null)
});

Then("the details screen fields should be disabled", () => {
    AccountingActions.AssertARInvoiceDetailsFieldsDisabled()
});
//#endregion

//#region Assert link of the invoice exsit
Given("navigates receivables tab", () => {
    cy.Click(ShipmentSelectors.Backbutton + BaseSelectors.LastElement, null)
});

Then("the link of the invoice should be exsit", () => {
    BaseAssertion.AssertElementContain(BaseSelectors.HyperlinkButtonControl, invoiceNumber)
});
//#endregion

//#region Set credit note ARInvoice as sent
Given("navigates invoice workspace", () => {
    cy.Click(BaseSelectors.HyperlinkButtonControl, invoiceNumber, true)
});

When("set invoice as sent with {string} as a note", (notes) => {
    AccountingActions.SetAsSentARInvoice(notes)
});

Then("the invoice should set as sent successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, AccountingSelectors.ARInvoiceEventsTab);
});
//#endregion

//#region Void credit note ARInvoice
When("void invoice", () => {
    AccountingActions.VoidARInvoice()
});

Then("the invoice should void successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});
//#endregion

//#region Assert link of the invoice not exsit and delete receivable existes
Then("the link of the invoice should not be exsit", () => {
    cy.contains(invoiceNumber).should(BaseSelectors.NotExist)
});

Then("delete receivable button should appear", () => {
    BaseAssertion.AssertElementExist(BaseSelectors.DeleteButton)
});
//#endregion