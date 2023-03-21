import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as CommonActions from '../../../../Common/cypress/actions/Actions';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { ShipmentDetails } from '../../../../Shipment/cypress/models/ShipmentDetails';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import * as AccountingActions from '../../actions/Actions';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import { ReceivableDetails } from "../../../../Shipment/cypress/models/ReceivableDetails"
import { ShipmentSelectors } from '../../../../Shipment/cypress/selectors/Selectors';
import { ARInvoiceDetails } from '../../models/ARInvoiceDetails';
import { AccountingSelectors } from "../../selectors/Selectors";
import * as BaseActions from '../../../../Base/cypress/actions/Actions';
import { AccountingURLs } from '../../constants/URLs';
import { RestAPI } from '../../../../Base/cypress/constants/RestAPI';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as ShipmentActions from '../../../../Shipment/cypress/actions/Actions';
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
//#region variables
let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
let AccountingSystem: string;
let ARInvoiceNumber: string;
let invoiceNumber: number;
//#endregion

//#region Update Accounting System
Given("the user logged in and navigate to SAT Interface settings", () => {
  cy.Login();
  AccountingActions.NavigatesToAccountingSettings()
});

Given("SAT Interface Settings as{string}", (accountingSystem) => {
 AccountingSystem = accountingSystem;
});

When("change the SAT Interface Settings", () => {
  AccountingActions.changeSATInterfaceSettings(AccountingSystem)
  cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
});

Then("SAT Interface Settings should update successfully", () => {
 BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
});
//#endregion

//#region Create direct export air shipment
Given("the user navigates to shipments workspace", () => {
  ShipmentActions.NavigatesToShipmentsWorkspace()
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
//#endregion

//#region add receivable
Given("a Receivable with the following details", (dataTable) => {
  const ReceivableData = Assists.CreateSet<ReceivableDetails>(dataTable);
  ShipmentActions.OpenShipment(shipmentNumber);
  ShipmentActions.FillReceivablesTabSAT(ReceivableData, false)
});

When("add Receivable", () => {
  ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});

Then("the Receivable should add successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});
//#endregion

//#region Create ARInvoice
Given("an ARInvoice with the following details", (dataTable) => {
  const ARInvoiceData = Assists.CreateInstance<ARInvoiceDetails>(dataTable, true);
  const yesterday = new Date(new Date().getTime() - 24*60*60*1000)
  console.log('yesterday ', yesterday);
  const formatted =  `${yesterday.getDate()}/${yesterday.getMonth()+1}/${yesterday.getFullYear()}`;
  ARInvoiceData.InvoiceDate= formatted;
  cy.Click(AccountingSelectors.CreateARInvoiceButton, null);
  AccountingActions.FillARInvoiceDetailsSAT(ARInvoiceData)
});

When("create invoice", () => {
  AccountingActions.CreateARInvoice()
});

Then("the invoice should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

Then("the status value should be Draft", () => {
  AccountingActions.AssertTransferStatus(AccountingSelectors.ContainNotReady)
});
//#endregion

//#region Approve ARInvoice
When("approve invoice", () => {
  invoiceNumber = (new Date()).getMilliseconds();
  AccountingActions.SATARApproveInvoice(invoiceNumber)
});

Then("the invoice should approve successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200).then((interception) => {
      ARInvoiceNumber = interception.response.body.InvoiceNumber;
  })
});

Then("And SAT status should be Transferred",(dataTable) => {
  let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
  BaseActions.ValidateEventsTab(eventDetailsList, AccountingSelectors.ARInvoiceEventsTab)
});
//#endregion
