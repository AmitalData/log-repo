import * as Actions from "../../actions/Actions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { ShipmentSelectors } from "../../selectors/Selectors";
import { PayableDetails } from "cypress/models/PayableDetails"
import { APInvoiceDetails } from "../../../../Accounting/cypress/models/APInvoiceDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import * as AccountingActions from '../../../../Accounting/cypress/actions/Actions';
import * as APPaymentActions from '../../../../Accounting/cypress/actions/APPaymentActions';
import { AccountingSelectors } from "../../../../Accounting/cypress/selectors/Selectors";
import { APPaymentDetails } from "../../../../Accounting/cypress/models/APPaymentDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as MaintenanceActions from '../../../../Maintenance/cypress/actions/Actions';
import { CardDetails } from "../../../../Maintenance/cypress/models/CardDetails";
import { MaintenanceSelectors } from "../../../../Maintenance/cypress/selectors/Selectors";
import { GenerateCurrentDatetimeString } from "../../../../Base/cypress/actions/GenerateRandoms";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";

//#region variables
let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
let invoiceNumber: string;
let CurrentDate = GenerateCurrentDatetimeString("_")
//#endregion

//#region Create new vendor
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
  cy.Login()
  MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.VendorMaintenanceItem)
});

Given("a vendor with the following details", (dataTable) => {
  let vendorDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
  MaintenanceActions.OpenNewWizard("Vendor");
  vendorDetails.CompanyName = CurrentDate;
  MaintenanceActions.FillVendorDetails(vendorDetails)
});

When("create vendor", () => {
  MaintenanceActions.CreateVendor();
});

Then("the vendor should create successfully", () => {
  MaintenanceActions.AssertCreateVendor()
});
//#endregion

//#region Create direct export air shipment
Given("the user navigates to shipments workspace", () => {
  Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
  shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
  Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
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

//#region Add Payables
Given("a payable with the following details", (dataTable) => {
  Actions.OpenShipment(shipmentNumber);
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
Given("an APInvoice with a random invoice number and the following details",
  (dataTable) => {
    const APInvoiceData = Assists.CreateInstance<APInvoiceDetails>(dataTable, true);
    cy.Click(AccountingSelectors.ReceiveInvoiceButton, null);
    APInvoiceData.Vendor = CurrentDate;
    AccountingActions.FillAPInvoiceDetails(APInvoiceData)
  });
When("receive invoice", () => {
  AccountingActions.ReceiveAPInvoice();
});
Then("the invoice should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200);
});
//#endregion

//#region Approve APInvoice
When("approve invoice", () => {
  AccountingActions.APApproveInvoice()
});
Then("the invoice should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200).then((interception) => {
    invoiceNumber = interception.response.body.InvoiceNumber;
  })
});
//#endregion

//#region Create new AP Payment
Given("navigates to Accounting workspace", () => {
  cy.Click(ShipmentSelectors.Backbutton_1, null)
  cy.Click(ShipmentSelectors.Backbutton, null)
  AccountingActions.NavigatesToAccountingMenu()
});

Given("an AP Payment with the following details", (dataTable) => {
  APPaymentActions.NavigatesAPPaymentWorkspace()
  let aPPaymentDetails = Assists.CreateInstance<APPaymentDetails>(dataTable, true);
  aPPaymentDetails.Vendor = CurrentDate;
  APPaymentActions.FillAPPayment(aPPaymentDetails)
});

When("save the AP Payment", () => {
  APPaymentActions.SaveAPPayment()
});

Then("the AP Payment should save successfully", () => {
  APPaymentActions.AssertSaveAPPayment()
});
//#endregion

//#region Connect the invoice to the AP Payment
Given("connect the invoice to the AP Payment", () => {
  APPaymentActions.ConnectAPPaymentToInvoice(invoiceNumber)
});

When("updates the AP Payment", () => {
  APPaymentActions.UpdateAPPayment()
});

Then("the AP Payment should update successfully", () => {
  APPaymentActions.AssertUpdateAPPayment()
  APPaymentActions.OpenApInvoice(invoiceNumber)
  
  
});
//#endregion
Then("the status value should be {string}", (statusValue) => {
    BaseAssertion.AssertElementContain(ShipmentSelectors.APInvoiceStatus, statusValue)
  });
//#region Disconnect the invoice from the AP Payment
Given("disconnect the invoice from the AP Payment", () => {
  APPaymentActions.DisConnectAPPaymentFromInvoice(invoiceNumber)
});
//#endregion