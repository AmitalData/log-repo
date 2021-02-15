// @ts-nocheck
import * as CommonActions from '../../../../Common/cypress/actions/Actions';
import * as ShipmentActions from '../../../../Shipment/cypress/actions/Actions';
import * as AccountingActions from '../../../../Accounting/cypress/actions/Actions';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import { ShipmentSelectors } from '../../../../Shipment/cypress/selectors/Selectors';
import { ShipmentDetails } from '../../../../Shipment/cypress/models/ShipmentDetails';
import { PackagesDetails } from '../../../../Shipment/cypress/models/PackagesDetails';
import { PayableDetails } from '../../../../Shipment/cypress/models/PayableDetails';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { CustomerDetails } from '../../../../Common/cypress/models/CustomerDetails';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import { CommonSelectors } from '../../../../Common/cypress/selectors/Selectors';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import { ARInvoiceDetails } from 'cypress/models/ARInvoiceDetails';
import { AccountingSelectors } from "../../selectors/Selectors";

//#region variables
let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
let customerCode: string;
let PayableData: PayableDetails
//#endregion

//#region Create customer
Given("the user logged in and navigates to customers workspace", () => {
  cy.Login();
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
//#region Update customer
Given("the user in the customer's billing tab", () => {
  CommonActions.OpenCustomer(customerCode);
  cy.Click(CommonSelectors.CustomerBillingTab, null, false);
});

When("activate consolidated invoice option", () => {
  cy.Click(CommonSelectors.EnableConsolidationInvoices, null, false);
  CommonActions.UpdateCustomer();
});

Then("the customer should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.Customers, 200)
  cy.Click(BaseSelectors.Backbutton, null, false);
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
//#region Update packages tab
Given("the user add package with the following details", (dataTable) => {
  ShipmentActions.OpenShipment(shipmentNumber);
  let packagesDetails = dataTable.hashes() as PackagesDetails[];
  ShipmentActions.FillPackageTab(shipmentDetails.TransportMode, packagesDetails)
});
//#endregion
//#region update shipment step
When("update shipment", () => {
  ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});
//#endregion
//#region update shipment assert step
Then("the direct should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion
//#region Add Payables
Given("a payable with the following details",
  (dataTable) => {
    PayableData = dataTable.hashes()[0] as PayableDetails;
    ShipmentActions.FillPayablesTab(PayableData)
  });
When("add payables",
  () => {
    ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)

  });
Then("the payables should add successfully",
  () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
  });
//#endregion
//#region generate receivables from payables
When("generate receivables from payables", () => {
  ShipmentActions.GenerateReceivablesFromPayables(true)
});
Then("the receivables should generate successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion
//#region Create ARInvoice
Given("an ARInvoice with a random invoice number and the following details",
  (dataTable) => {
    const ARInvoiceData = dataTable.hashes()[0] as ARInvoiceDetails
    cy.Click(AccountingSelectors.CreateARInvoiceButton, null);
    AccountingActions.FillARInvoiceDetails(ARInvoiceData)
  });
When("create invoice", () => {
  AccountingActions.CreateARInvoice(true)
});
Then("the invoice should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});
//#endregion
//#region Create Consolidation Invoice
Given("a Consolidation Invoice with the following details",
  (dataTable) => {
    cy.BackButton("Shipment: " + shipmentNumber)
    cy.BackButton("Operations")
    const ARInvoiceData = dataTable.hashes()[0] as ARInvoiceDetails
    ARInvoiceData.Partner = customerCode
    AccountingActions.FillconsolidationInvoiceDetails(ARInvoiceData)

  });
When("create consolidation invoice", () => {
  AccountingActions.CreateARInvoice()

});
Then("the consolidation invoice should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200)

});
//#endregion
//#region Approve Consolidation Invoice

When("approve consolidation invoice", () => {
AccountingActions.ApproveConsilidationInvoice()
});
Then("the consolidation invoice should approve successfully", () => {
  BaseAssertion.AssertStatusCode("ConsilidationInvoiceDomain", 200)
});
    //#endregion
