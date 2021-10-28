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
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as MaintenanceActions from "../../../../Maintenance/cypress/actions/Actions";
import { InvoiceSettingsDetails } from "../../../../Maintenance/cypress/models/InvoiceSettingsDetails";
import { MaintenanceSelectors } from "../../../../Maintenance/cypress/selectors/Selectors";

//#region variables
let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
let customerCode: string;
let AccountingSystem: string;
let invoiceSettingsDetails: InvoiceSettingsDetails;
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
  Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
  shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
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
Given("the user in the shipment's rounting tab", () => {
  Actions.OpenShipment(shipmentNumber);
  cy.Click(ShipmentSelectors.RoutingsTab, null);
});
Given("edit main carriage leg with the following details", (dataTable) => {
  let mainCarriageLeg = Assists.CreateInstance<MainCarriageLeg>(dataTable, true);
  Actions.EditMainCarriageLegs(mainCarriageLeg.Airline);
});
//#endregion

//#region Update packages tab
Given("the user add package with the following details", (dataTable) => {
  let packagesDetails = Assists.CreateSet<PackagesDetails>(dataTable);
  Actions.FillPackageTab(shipmentDetails.TransportMode, packagesDetails)
});
//#endregion

//#region Add Payables
Given("a payable with the following details",
  (dataTable) => {
    const PayableData = Assists.CreateInstance<PayableDetails>(dataTable, true);
    Actions.FillPayablesTab(PayableData)
  });
When("add payables", () => {
  Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)

});
Then("the payables should add successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});
//#endregion

//#region Create APInvoice
Given("an APInvoice with a random invoice number and the following details", (dataTable) => {
  const APInvoiceData = Assists.CreateInstance<APInvoiceDetails>(dataTable, true);
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

//#region Assert all buttons in menu are/aren't dim except void button
When("press on menu button", () => {
  cy.Click(BaseSelectors.ToggleButtonClass + BaseSelectors.LastElement, null)
});

Then("all buttons in menu should be dim except void button", () => {
  AccountingActions.AssertAPInvoiceMenuButtonsDimExceptReTransfer()
});

Then("all buttons in menu should not be dim except void button", () => {
  AccountingActions.AssertAPInvoiceMenuButtonsNotDimExceptReTransfer()
});
//#endregion

//#region Approve APInvoice
When("approve invoice", () => {
  AccountingActions.APApproveInvoice()
});
Then("the invoice should approve successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200);
});

Then("the status value should be {string}", (statusValue) => {
  BaseAssertion.AssertElementContain(ShipmentSelectors.APInvoiceStatus, statusValue)
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