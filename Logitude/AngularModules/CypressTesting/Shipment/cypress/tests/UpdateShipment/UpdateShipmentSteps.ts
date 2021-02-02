import * as Actions from "../../actions/Actions"
import { Selectors } from "../../selectors/Selectors"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { PartnersDetails } from "cypress/models/PartnersDetails";
import {PayableDetails} from "cypress/models/PayableDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { ShipmentDetails } from "cypress/models/ShipmentDetails";
import {ReceivableDetails}from"cypress/models/ReceivableDetails"

let shipmentNumber: string;
let shipmentDetails: ShipmentDetails;
let payableDetails: PayableDetails;

Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(Selectors.ShipmentTab, null)
});

Given("a direct shipment with the following details",
  (dataTable) => {
   shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
   Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
   Actions.FillShipmentWizardsFields(shipmentDetails);
});

When("create shipment", () => {
  Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});

Then("the direct should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostShipmentRequest", 200).then((interception) => {
        shipmentNumber = interception.response.body.ShipmentNumber;
    })
});

Given("the user in the general tab", () => {
    Actions.OpenShipment(shipmentNumber);
    cy.Click(Selectors.GeneralTab, null)
});

Given("fill random GrossWeight and {string} as a MoveType",(MoveType) => {
    Actions.FillGeneralTab(MoveType)
});

Given("the user in the Orders tab", () => {
    cy.Click(Selectors.OrdersTab, null)
});

Given("fill Orders tab with random number of Packages", () => {
    Actions.FillOrdersTab()
});

Given("the user in the Partners tab", () => {
    cy.Click(Selectors.PartnersTab, null)
});

Given("fill Partners tab with following details", (dataTable) => {
    const partnersDetails = dataTable.hashes()[0] as PartnersDetails;
    Actions.FillPartnersTab("Export", "Air", partnersDetails)
});

Given("the user in the Packages tab", () => {
    cy.Click(Selectors.PackagesTab, null)
});

Given("fill Packages tab with random number of Packages", () => {
    Actions.FillPackagesTab("Air")
});

Given("the user in the Receivables tab",  (dataTable) => {
    cy.Click(Selectors.ReceivablesTab, null)
 
});

Given("fill Receivables with the following details", (dataTable) => {
    const ReceivableData = dataTable.hashes()[0] as ReceivableDetails;
    Actions.FillReceivablesTab(ReceivableData)
});

Given("the user in the Routings tab", () => {
    cy.Click(Selectors.RoutingsTab, null)
});

Given("add new pickup routing", () => {
    Actions.FillPickupRouting()
});

Given("add new delivery with {string} as a partner routing", (partner) => {
    Actions.FillDeliveryRouting(partner)
});

Given("add carriage routings from port {string} to port {string}", (fromPort, toPort) => {
    Actions.FillPreCarriageRouting("Air", fromPort, toPort)
    Actions.FillOnCarriageRouting("Air", fromPort, toPort)
});

Given("the user in the Payables tab", () => {
    cy.Click(Selectors.PayablesTab, null)
});

Given("add Payables with {string} as a ChargesType, {string} as a Currency and {string} as UOM", (chargesType, currency,uom) => {
    payableDetails = {
     ChargesType:chargesType,
     Currency:currency,
     UOM:uom
      } as PayableDetails;
      Actions.FillPayablesTab(payableDetails)
});

When("save shipment", () => {
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

When("save shipment window", () => {
    Actions.UpdateShipment(Selectors.SaveClose)
});

Then("the save operation complete successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutShipmentRequest", 200);
});