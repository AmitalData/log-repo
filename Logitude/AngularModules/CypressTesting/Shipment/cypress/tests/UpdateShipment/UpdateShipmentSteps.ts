import * as Actions from "../../actions/Actions"
import { Selectors } from "../../selectors/Selectors"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { PartnersDetails } from "cypress/models/PartnersDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"

let shipmentNumber: string;

Given("the user logged in", () => {
  cy.Login()
});

Given("navigate to shipments workspace", () => {
  cy.Click(BaseSelectors.OperationsMenu, null)
  cy.Click(Selectors.ShipmentTab, null)
});

// Given("create a new Direct shipment", () => {
//     Actions.CreateAnewShipment("Direct")
//     BaseActions.WaitRequestReturnResponse("WaitPostShipmentRequest", 200).then((interception) => {
//         shipmentNumber = interception.response.body.ShipmentNumber;
//     })
//     Actions.OpenShipment("CreatedShipmentsData/DEA.json")
// });

Given("the user in the general tab", () => {
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
    Actions.FillPartnersTab("E","A", partnersDetails)
});

Given("the user in the Packages tab", () => {
    cy.Click(Selectors.PackagesTab, null)
});

Given("fill Packages tab with random number of Packages", () => {
    Actions.FillPackagesTab("A")
});

Given("the user in the Receivables tab", () => {
    cy.Click(Selectors.ReceivablesTab, null)
});

Given("fill Receivables tab  with a random UnitPrice and {string} as a ChargesType", (ChargesType) => {
    Actions.FillReceivablesTab(ChargesType)
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
    Actions.FillPreCarriageRouting("A", fromPort, toPort)
    Actions.FillOnCarriageRouting("A", fromPort, toPort)
});

Given("the user in the Payables tab", () => {
    cy.Click(Selectors.PayablesTab, null)
});

Given("add Payables with {string} as a ChargesType, {string} as a Currency and random UOM", (chargesType, currency) => {
    Actions.FillPayablesTab(chargesType, currency)
});

When("save shipment", () => {
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

When("save shipment window", () => {
    Actions.UpdateShipment(Selectors.SaveClose)
});

Then("the save operation complete successfully", () => {
    BaseActions.WaitRequestReturnResponse("WaitPostShipmentRequest", 200);
});