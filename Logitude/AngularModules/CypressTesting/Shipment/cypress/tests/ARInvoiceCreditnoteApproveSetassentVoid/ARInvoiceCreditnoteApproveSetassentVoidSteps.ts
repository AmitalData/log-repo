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
//#region variables
let invoiceSettingsDetails: InvoiceSettingsDetails;
let ShipmentData: ShipmentDetails;
let shipmentNumber: string;
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

    Given("a receivable with the following details", (dataTable) => {
        const ReceivableData = Assists.CreateSet<ReceivableDetails>(dataTable);
        Actions.OpenShipment(shipmentNumber)
        Actions.FillReceivablesTab(ReceivableData)
        Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
    });
    Given("a credit ARInvoice with a random invoice number and the following details",
        (dataTable) => {
            const ARInvoiceData = Assists.CreateInstance<ARInvoiceDetails>(dataTable, true);
            cy.Click(AccountingSelectors.CreateCreditNoteARInvoiceButton, null);
            AccountingActions.FillARInvoiceDetails(ARInvoiceData)
        });
});
When("create invoice", () => {
    AccountingActions.CreateARInvoice()
});
Then("the invoice should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

When("approve invoice", () => {
    AccountingActions.ARApproveInvoice()
});
Then("the invoice should approve successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

When("set invoice as sent with {string} as a note", (notes) => {
    AccountingActions.SetAsSentARInvoice(notes)
});

Then("the invoice should set as sent successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

When("void invoice", () => {
    AccountingActions.VoidARInvoice()
});
Then("the invoice should void successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

Then("the status value should be {string}", (statusValue) => {
    BaseAssertion.AssertElementContain(ShipmentSelectors.ARInvoiceStatus, statusValue)
});