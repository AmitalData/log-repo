import * as Actions from '../../actions/Actions';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import { ShipmentDetails } from '../../models/ShipmentDetails';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { CustomerDetails } from '../../../../Common/cypress/models/CustomerDetails';
import { ShipmentSelectors } from '../../selectors/Selectors';
import { MainCarriageLeg } from 'cypress/models/MainCarriageLeg';
import { PackagesDetails } from 'cypress/models/PackagesDetails';
import { ReceivableDetails } from 'cypress/models/ReceivableDetails';
import { ARInvoiceDetails } from 'cypress/models/ARInvoiceDetails';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import * as CommonActions from '../../../../Common/cypress/actions/Actions';
import * as AccountingActions from '../../../../Accounting/cypress/actions/Actions';

//#region variables
let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
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
    BaseAssertion.AssertStatusCode(RequestAliases.PartnersDomainRequest, 200);
});
//#endregion

//#region Create direct export air shipment
Given("the user navigates to shipments workspace", () => {
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
    shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
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

//#region Update routing tab
Given("the user in the shipment's rounting tab",()=>{
    Actions.OpenShipment(shipmentNumber);
    cy.Click(ShipmentSelectors.RoutingsTab, null);
});

Given("edit main carriage leg with the follwing details",(dataTable)=>{
    let mainCarriageLeg = dataTable.hashes()[0] as MainCarriageLeg;
    Actions.EditMainCarriageLegs(mainCarriageLeg.Airline);
}); 
//#endregion

//#region Update packages tab
Given("the user add package with the following details", (dataTable) => {
    let packagesDetails = dataTable.hashes() as PackagesDetails[];
    Actions.FillPackageTab(shipmentDetails.TransportMode, packagesDetails)
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

//#region Create ARInvoice
Given("a receivable with the following details", (dataTable) => {
    let receivableDetails = dataTable.hashes() as ReceivableDetails[];
    Actions.FillReceivablesTab(receivableDetails);
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton);
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});

Given("an ARInvoice with the following details", (dataTable) => {
    let ARInvoiceDetails = dataTable.hashes()[0] as ARInvoiceDetails;
    Actions.NewCustomsARInvoice()
    AccountingActions.FillARInvoiceDetails(ARInvoiceDetails);
});

When("create invoice", () => {
    AccountingActions.CreateARInvoice()
});

Then("the invoice should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});
//#endregion

//#region Approve customs credit note ARInvoice
When("approve invoice", () => {
    Actions.ARApproveInvoice()
});
Then("the invoice should approve successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});
//#endregion
